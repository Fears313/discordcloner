namespace DiscordChatExporter.Messages
{
    public class ShowCloneDoneMessage
    {
        public string FilePath { get; }

        public ShowCloneDoneMessage(string filePath)
        {
            FilePath = filePath;
        }
    }
}