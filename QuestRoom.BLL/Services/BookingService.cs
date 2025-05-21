using QuestRoom.DAL.Entities;
using QuestRoom.DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuestRoom.BLL.Services
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public Booking GetBookingById(int id)
        {
            return _unitOfWork.BookingRepository.GetById(id);
        }

        public List<Booking> GetAllBookings()
        {
            return new List<Booking>(_unitOfWork.Bookings.GetAll());
        }

        public List<Booking> GetBookingsByQuestId(int questId)
        {
            return new List<Booking>(_unitOfWork.BookingRepository.GetBookingsByQuestId(questId));
        }

        public bool CreateBooking(int questId, int clientId, DateTime startTime, int participantsCount, string certificateCode = null)
        {
            Quest quest = _unitOfWork.QuestRepository.GetById(questId);
            if (quest == null)
                return false;

            if (participantsCount > quest.MaxParticipants)
                return false;

            DateTime endTime = startTime.AddMinutes(quest.DurationMinutes);

            if (!_unitOfWork.BookingRepository.IsTimeSlotAvailable(questId, startTime, endTime))
                return false;

            decimal totalPrice = quest.Price;
            GiftCertificate certificate = null;

            if (!string.IsNullOrEmpty(certificateCode))
            {
                certificate = _unitOfWork.GiftCertificateRepository.GetByCode(certificateCode);
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

            _unitOfWork.Bookings.Add(booking);

            if (certificate != null)
            {
                certificate.IsUsed = true;
                _unitOfWork.GiftCertificates.Update(certificate);
            }

            _unitOfWork.Complete();
            return true;
        }

        public bool CancelBooking(int bookingId)
        {
            var booking = _unitOfWork.BookingRepository.GetById(bookingId);
            if (booking == null)
                return false;

            booking.Status = "Скасовано";
            _unitOfWork.Bookings.Update(booking);
            _unitOfWork.Complete();
            return true;
        }
    }
}