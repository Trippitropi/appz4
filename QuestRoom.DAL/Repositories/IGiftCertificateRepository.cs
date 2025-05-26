using QuestRoom.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestRoom.DAL.Repositories
{
    public interface IGiftCertificateRepository : IRepository<GiftCertificate>
    {
        IEnumerable<GiftCertificate> GetActiveCertificatesForClient(int clientId);
        bool IsValidCertificate(string code);
        GiftCertificate GetByCode(string code);
    }
}
