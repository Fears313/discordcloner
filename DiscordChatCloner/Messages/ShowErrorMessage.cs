namespace DiscordChatCloner.Messages
{
    public class ShowErrorMessage
    {
        public string Message { get; }

        public ShowErrorMessage(string message)
        {
            Message = message;
        }
    }
}