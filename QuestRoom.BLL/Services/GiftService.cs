using QuestRoom.DAL.Entities;
using QuestRoom.DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestRoom.BLL.Services
{
    public class GiftCertificateService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GiftCertificateService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public GiftCertificate GetCertificateById(int id)
        {
            return _unitOfWork.GiftCertificates.GetById(id);
        }

        public List<GiftCertificate> GetActiveCertificatesForClient(int clientId)
        {
            return new List<GiftCertificate>(_unitOfWork.GiftCertificateRepository.GetActiveCertificatesForClient(clientId));
        }

        public bool IsValidCertificate(string code)
        {
            return _unitOfWork.GiftCertificateRepository.IsValidCertificate(code);
        }

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

            _unitOfWork.GiftCertificates.Add(certificate);
            _unitOfWork.Complete();
            return certificate;
        }

        private string GenerateUniqueCode()
        {
            string code;
            do
            {
                code = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            } while (_unitOfWork.GiftCertificateRepository.GetByCode(code) != null);

            return code;
        }
    }
}