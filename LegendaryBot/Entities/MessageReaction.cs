namespace LegendaryBot.Entities
{
    public class MessageReaction
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Reaction { get; set; }
        public int GuildId { get; set; }
        public virtual Guild Guild { get; set; }
    }
}
