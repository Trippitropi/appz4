using QuestRoom.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestRoom.BLL.Services
{
    public interface IGiftCertificateService
    {
        GiftCertificate GetCertificateById(int id);
        List<GiftCertificate> GetActiveCertificatesForClient(int clientId);
        bool IsValidCertificate(string code);
        GiftCertificate CreateCertificate(int? clientId, int? questId, int validityDays = 180);
    }
}
