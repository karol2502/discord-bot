namespace LegendaryBot.Entities
{
    public class MessageResponse
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Response { get; set; }
        public int GuildId { get; set; }
        public virtual Guild Guild { get; set; }
    }
}
