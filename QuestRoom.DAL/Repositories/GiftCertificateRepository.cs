using QuestRoom.DAL.Entities;
using QuestRoom.DAL.QuestRoom.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestRoom.DAL.Repositories
{
    public class GiftCertificateRepository : Repository<GiftCertificate>
    {
        public GiftCertificateRepository(QuestRoomDbContext context) : base(context)
        {
        }

        
        public IEnumerable<GiftCertificate> GetActiveCertificatesForClient(int clientId)
        {
            return _context.GiftCertificates
                .Where(g => g.ClientId == clientId && !g.IsUsed && g.ExpiryDate > DateTime.Now)
                .ToList();
        }

       
        public bool IsValidCertificate(string code)
        {
            return _context.GiftCertificates
                .Any(g => g.Code == code && !g.IsUsed && g.ExpiryDate > DateTime.Now);
        }

        
        public GiftCertificate GetByCode(string code)
        {
            return _context.GiftCertificates
                .FirstOrDefault(g => g.Code == code);
        }
    }
}
