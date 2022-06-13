namespace LegendaryBot.Entities
{
    public class Guild
    {
        public int Id { get; set; }
        public ulong GuildId { get; set; }
        public DateTime JoinedTime { get; set; }
        public virtual List<MessageResponse>? MessageResponses { get; set; }
        public virtual List<MessageReaction>? MessageReactions { get; set; }
    }
}
