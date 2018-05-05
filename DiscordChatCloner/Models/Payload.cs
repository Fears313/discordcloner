using Newtonsoft.Json;

namespace DiscordChatCloner.Models
{

    public class Image
    {
        [JsonProperty("url")]
        public string Url { get; }

        public Image(string url)
        {
            Url = url;
        }
    }

    public class Embed
    {
        [JsonProperty("image")]
        public Image Image { get; }

        public Embed(Attachment attachment)
        {
            if (attachment != null)
            {
                Image = new Image(attachment.Url);
            }
        }
    }

    public class Payload
    {
        [JsonProperty("content")]
        public string Content { get; }
        [JsonProperty("embed")]
        public Embed Embed { get; }


        public Payload(string content, Attachment attachment)
        {
            Content = content;
            Embed = new Embed(attachment);
        }

        public override string ToString()
        {
            return Content;
        }
    }
}