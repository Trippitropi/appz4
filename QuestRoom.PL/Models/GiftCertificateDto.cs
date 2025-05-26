namespace QuestRoom.PL.Models
{
    public class GiftCertificateDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsUsed { get; set; }
        public int? ClientId { get; set; }
        public string ClientName { get; set; }
        public int? QuestId { get; set; }
        public string QuestName { get; set; }
    }
}
