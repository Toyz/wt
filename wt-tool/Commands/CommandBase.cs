using System.Threading.Tasks;

namespace wt_tool.Commands
{
    public abstract class CommandBase
    {
        public CommandBase()
        {
        }

        public abstract int Run(Terminal.Terminal config);
    }
}
