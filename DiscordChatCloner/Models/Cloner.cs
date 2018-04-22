namespace DiscordChatCloner.Models
{
    public class Cloner
    {
        public string Id { get; }
        public string Name { get; }

        public Guild FromGuild { get; }
        public Channel FromChannel { get; }

        public Guild ToGuild { get; }
        public Channel ToChannel { get; }

        public int PollingFrequency { get; }
        public bool Running { get; private set; }

        public Cloner(string id, string name, Guild fromGuild, Channel fromChannel, Guild toGuild, Channel toChannel, int pollingFrequency) {
            Id = id;
            Name = name;
            FromGuild = fromGuild;
            FromChannel = fromChannel;
            ToGuild = toGuild;
            ToChannel = toChannel;
            PollingFrequency = pollingFrequency;
        }

        public void Start() {
            Running = true;
        }

        public void Stop()
        {
            Running = false;
        }

        public override string ToString() {
            return Name;
        }
    }
}
