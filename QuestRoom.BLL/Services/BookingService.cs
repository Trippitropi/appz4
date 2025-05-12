using QuestRoom.DAL.Entities;
using QuestRoom.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestRoom.BLL.Services
{
    public class BookingService
    {
        private readonly BookingRepository _bookingRepository;
        private readonly QuestRepository _questRepository;
        private readonly GiftCertificateRepository _certificateRepository;

        public BookingService(
            BookingRepository bookingRepository,
            QuestRepository questRepository,
            GiftCertificateRepository certificateRepository)
        {
            _bookingRepository = bookingRepository;
            _questRepository = questRepository;
            _certificateRepository = certificateRepository;
        }

        public Booking GetBookingById(int id)
        {
            return _bookingRepository.GetById(id);
        }

        public List<Booking> GetAllBookings()
        {
            return new List<Booking>(_bookingRepository.GetAll());
        }

        public List<Booking> GetBookingsByQuestId(int questId)
        {
            return new List<Booking>(_bookingRepository.GetBookingsByQuestId(questId));
        }


        public bool CreateBooking(int questId, int clientId, DateTime startTime, int participantsCount, string certificateCode = null)
        {
            Quest quest = _questRepository.GetById(questId);
            if (quest == null)
                return false;


            if (participantsCount > quest.MaxParticipants)
                return false;


            DateTime endTime = startTime.AddMinutes(quest.DurationMinutes);


            if (!_bookingRepository.IsTimeSlotAvailable(questId, startTime, endTime))
                return false;

            decimal totalPrice = quest.Price;
            GiftCertificate certificate = null;


            if (!string.IsNullOrEmpty(certificateCode))
            {
                certificate = _certificateRepository.GetByCode(certificateCode);
                if (certificate != null && !certificate.IsUsed && certificate.ExpiryDate > DateTime.Now)
                {
                    if (certificate.QuestId == null || certificate.QuestId == questId)
                    {
                        totalPrice = 0;
                    }
                }
                else
                {
                    certificate = null;
                }
            }


            var booking = new Booking
            {
                QuestId = questId,
                ClientId = clientId,
                BookingDate = DateTime.Now,
                StartTime = startTime,
                EndTime = endTime,
                ParticipantsCount = participantsCount,
                TotalPrice = totalPrice,
                Status = "Підтверджено",
                GiftCertificateId = certificate?.Id
            };

            _bookingRepository.Add(booking);


            if (certificate != null)
            {
                certificate.IsUsed = true;
                _certificateRepository.Update(certificate);
            }

            _bookingRepository.SaveChanges();
            return true;
        }


        public bool CancelBooking(int bookingId)
        {
            var booking = _bookingRepository.GetById(bookingId);
            if (booking == null)
                return false;

            booking.Status = "Скасовано";
            _bookingRepository.Update(booking);
            _bookingRepository.SaveChanges();
            return true;
        }
    }
}
