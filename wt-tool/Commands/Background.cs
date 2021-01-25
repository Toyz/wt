﻿using CommandLine;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace wt_tool.Commands
{
    [Verb("background", HelpText = "Modify windows terminal background options")]
    class Background : CommandBase
    {
        internal enum BackgroundImageAlignment
        {
            bottom,
            bottomLeft,
            bottomRight,
            center,
            left,
            right,
            top,
            topLeft,
            topRight,
            nothing
        }

        internal enum BackgrounStrechMode
        {
            fill,
            none,
            uniform,
            uniformToFill
        }

        [Option('i', "image", Required = false, HelpText = "Set background image")]
        public string Image { get; set; }

        [Option('a', "alignment", Required = false, HelpText = "Sets how the background image aligns to the boundaries of the window.\nPossible values: \"center\", \"left\", \"top\", \"right\", \"bottom\", \"topLeft\", \"topRight\", \"bottomLeft\", \"bottomRight\"")]
        public BackgroundImageAlignment? ImageAlignment { get; set; }

        [Option('s', "strech", Required = false, HelpText = "Sets how the background image is resized to fill the window.")]
        public BackgrounStrechMode? ImageStrechMode { get; set; }

        [Option('o', "opacity", Required = false, HelpText = "Sets the transparency of the background image. Accepts floating point values from 0-1.")]
        public float? ImageOpacity { get; set; }

        [Option('c', "color", Required = false, HelpText = "Sets the background color of the text. Overrides \"background\" from the color scheme. Uses hex color format: \"#rrggbb\".")]
        public string BackgroundColor { get; set; }

        public override int Run(Terminal.Terminal config)
        {
            var defaultProfile = config.Settings["profiles"]["defaults"] as JObject;

            if (!string.IsNullOrEmpty(Image))
            {
                defaultProfile["backgroundImage"] = Image;
            }

            if (ImageAlignment != null)
            {
                defaultProfile["backgroundImageAlignment"] = ImageAlignment.ToString();
            }
            
            if(ImageStrechMode != null)
            {
                defaultProfile["backgroundImageStretchMode"] = ImageStrechMode.ToString();
            }

            if (ImageOpacity != null)
            {
                var op = ImageOpacity;

                if (op > 1.0) op = 1.0f;
                if (op < 0.0) op = 0.0f;

                defaultProfile["backgroundImageOpacity"] = op;
            }

            if(BackgroundColor != null && CheckValidFormatHtmlColor(BackgroundColor))
            {
                defaultProfile["background"] = BackgroundColor;

            }

            config.Save();

            return 1;
        }

        protected static bool CheckValidFormatHtmlColor(string inputColor)
        {
            //regex from http://stackoverflow.com/a/1636354/2343
            if (Regex.Match(inputColor, "^#(?:[0-9a-fA-F]{3}){1,2}$").Success)
                return true;

            var result = System.Drawing.Color.FromName(inputColor);
            return result.IsKnownColor;
        }
    }
}
