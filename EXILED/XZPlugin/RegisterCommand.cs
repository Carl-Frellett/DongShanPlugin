using CommandSystem;

namespace XZPlugin
{
    public class RegisterCommandHandler : CommandHandler
    {
        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new XZPlugin.HelpCommand());
            RegisterCommand(new XZPlugin.TX());
        }
    }
}
