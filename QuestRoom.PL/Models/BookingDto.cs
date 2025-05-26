namespace QuestRoom.PL.Models
{
    public class BookingDto
    {
        public int Id { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int ParticipantsCount { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
        public bool IsPaid { get; set; }

        // Інформація про квест
        public int QuestId { get; set; }
        public string QuestName { get; set; }

        // Інформація про клієнта
        public int ClientId { get; set; }
        public string ClientName { get; set; }

        // Інформація про сертифікат (якщо є)
        public int? GiftCertificateId { get; set; }
        public string GiftCertificateCode { get; set; }
    }
}
