using QuestRoom.DAL.Entities;
using QuestRoom.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appz4
{
    public class GiftCertificateService
    {
        private readonly GiftCertificateRepository _certificateRepository;

        public GiftCertificateService(GiftCertificateRepository certificateRepository)
        {
            _certificateRepository = certificateRepository;
        }

        public GiftCertificate GetCertificateById(int id)
        {
            return _certificateRepository.GetById(id);
        }

        public List<GiftCertificate> GetActiveCertificatesForClient(int clientId)
        {
            return new List<GiftCertificate>(_certificateRepository.GetActiveCertificatesForClient(clientId));
        }

        public bool IsValidCertificate(string code)
        {
            return _certificateRepository.IsValidCertificate(code);
        }

        // Створення нового сертифіката
        public GiftCertificate CreateCertificate(int? clientId, int? questId, int validityDays = 180)
        {
            var certificate = new GiftCertificate
            {
                ClientId = clientId,
                QuestId = questId,
                Code = GenerateUniqueCode(),
                IssueDate = DateTime.Now,
                ExpiryDate = DateTime.Now.AddDays(validityDays),
                IsUsed = false
            };

            _certificateRepository.Add(certificate);
            _certificateRepository.SaveChanges();
            return certificate;
        }

        // Генерація унікального коду сертифіката
        private string GenerateUniqueCode()
        {
            string code;
            do
            {
                code = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            } while (_certificateRepository.GetByCode(code) != null);

            return code;
        }
    }
}
