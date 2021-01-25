using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace wt_tool.Terminal
{
    public class Terminal
    {
        private readonly string settingsPath;

        public JObject Settings { get; }

        public Terminal(string path)
        {
            settingsPath = path;

            var content = System.IO.File.ReadAllText(settingsPath);

            Settings = JObject.Parse(content);
        }

        public void Save()
        {
            string jsonText = JsonConvert.SerializeObject(Settings, Formatting.Indented);

            System.IO.File.WriteAllText(settingsPath, jsonText);
        }
    } 
}
