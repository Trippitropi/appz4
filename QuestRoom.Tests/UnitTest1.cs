using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using QuestRoom.BLL.Services;
using QuestRoom.DAL.Entities;
using QuestRoom.DAL.UnitOfWork;
using QuestRoom.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuestRoom.Tests
{
    [TestClass]
    public class BookingServiceTests
    {
        private Mock<IUnitOfWork> _mockUnitOfWork;
        private BookingService _bookingService;

        [TestInitialize]
        public void Setup()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _bookingService = new BookingService(_mockUnitOfWork.Object);
        }

        [TestMethod]
        public void GetAllBookings_ShouldReturnAllBookings()
        {
            // Arrange
            var mockRepo = new Mock<IRepository<Booking>>();
            var bookings = new List<Booking>
            {
                new Booking { Id = 1, QuestId = 1, ClientId = 1, StartTime = DateTime.Now, Status = "Підтверджено" },
                new Booking { Id = 2, QuestId = 2, ClientId = 2, StartTime = DateTime.Now.AddHours(2), Status = "Підтверджено" }
            };

            mockRepo.Setup(repo => repo.GetAll()).Returns(bookings);
            _mockUnitOfWork.Setup(uow => uow.Bookings).Returns(mockRepo.Object);

            // Act
            var result = _bookingService.GetAllBookings();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(1, result[0].Id);
            Assert.AreEqual(2, result[1].Id);
            mockRepo.Verify(repo => repo.GetAll(), Times.Once);
        }

        [TestMethod]
        public void GetBookingById_ShouldReturnCorrectBooking()
        {
            // Arrange
            int testId = 1;
            var booking = new Booking
            {
                Id = testId,
                QuestId = 1,
                ClientId = 1,
                StartTime = DateTime.Now,
                Status = "Підтверджено"
            };

            var mockBookingRepo = new Mock<IBookingRepository>();
            mockBookingRepo.Setup(repo => repo.GetById(testId)).Returns(booking);
            _mockUnitOfWork.Setup(uow => uow.BookingRepository).Returns(mockBookingRepo.Object);

            // Act
            var result = _bookingService.GetBookingById(testId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(testId, result.Id);
            mockBookingRepo.Verify(repo => repo.GetById(testId), Times.Once);
        }

        [TestMethod]
        public void GetBookingsByQuestId_ShouldReturnCorrectBookings()
        {
            // Arrange
            int questId = 1;
            var bookings = new List<Booking>
            {
                new Booking { Id = 1, QuestId = questId, ClientId = 1, StartTime = DateTime.Now, Status = "Підтверджено" },
                new Booking { Id = 2, QuestId = questId, ClientId = 2, StartTime = DateTime.Now.AddHours(2), Status = "Очікує" }
            };

            var mockBookingRepo = new Mock<IBookingRepository>();
            mockBookingRepo.Setup(repo => repo.GetBookingsByQuestId(questId)).Returns(bookings);
            _mockUnitOfWork.Setup(uow => uow.BookingRepository).Returns(mockBookingRepo.Object);

            // Act
            var result = _bookingService.GetBookingsByQuestId(questId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.All(b => b.QuestId == questId));
            mockBookingRepo.Verify(repo => repo.GetBookingsByQuestId(questId), Times.Once);
        }

        [TestMethod]
        public void CreateBooking_WithValidData_ShouldReturnTrue()
        {
            // Arrange
            int questId = 1;
            int clientId = 1;
            DateTime startTime = DateTime.Now.AddDays(1);
            int participantsCount = 4;

            var quest = new Quest
            {
                Id = questId,
                MaxParticipants = 6,
                DurationMinutes = 60,
                Price = 1000M
            };

            var mockQuestRepo = new Mock<IQuestRepository>();
            mockQuestRepo.Setup(repo => repo.GetById(questId)).Returns(quest);

            var mockBookingRepo = new Mock<IBookingRepository>();
            mockBookingRepo.Setup(repo => repo.IsTimeSlotAvailable(
                questId, It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(true);

            var mockBookingsRepo = new Mock<IRepository<Booking>>();

            _mockUnitOfWork.Setup(uow => uow.QuestRepository).Returns(mockQuestRepo.Object);
            _mockUnitOfWork.Setup(uow => uow.BookingRepository).Returns(mockBookingRepo.Object);
            _mockUnitOfWork.Setup(uow => uow.Bookings).Returns(mockBookingsRepo.Object);

            // Act
            bool result = _bookingService.CreateBooking(questId, clientId, startTime, participantsCount);

            // Assert
            Assert.IsTrue(result);
            mockQuestRepo.Verify(repo => repo.GetById(questId), Times.Once);
            mockBookingRepo.Verify(repo => repo.IsTimeSlotAvailable(
                questId, startTime, It.IsAny<DateTime>()), Times.Once);
            mockBookingsRepo.Verify(repo => repo.Add(It.IsAny<Booking>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [TestMethod]
        public void CreateBooking_WithInvalidParticipantsCount_ShouldReturnFalse()
        {
            // Arrange
            int questId = 1;
            int clientId = 1;
            DateTime startTime = DateTime.Now.AddDays(1);
            int participantsCount = 10; // Більше ніж MaxParticipants

            var quest = new Quest
            {
                Id = questId,
                MaxParticipants = 5, // Менше ніж participantsCount
                DurationMinutes = 60,
                Price = 1000M
            };

            var mockQuestRepo = new Mock<IQuestRepository>();
            mockQuestRepo.Setup(repo => repo.GetById(questId)).Returns(quest);

            var mockBookingsRepo = new Mock<IRepository<Booking>>();

            _mockUnitOfWork.Setup(uow => uow.QuestRepository).Returns(mockQuestRepo.Object);
            _mockUnitOfWork.Setup(uow => uow.Bookings).Returns(mockBookingsRepo.Object);

            // Act
            bool result = _bookingService.CreateBooking(questId, clientId, startTime, participantsCount);

            // Assert
            Assert.IsFalse(result);
            mockQuestRepo.Verify(repo => repo.GetById(questId), Times.Once);
            mockBookingsRepo.Verify(repo => repo.Add(It.IsAny<Booking>()), Times.Never);
            _mockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [TestMethod]
        public void CreateBooking_WithUnavailableTimeSlot_ShouldReturnFalse()
        {
            // Arrange
            int questId = 1;
            int clientId = 1;
            DateTime startTime = DateTime.Now.AddDays(1);
            int participantsCount = 4;

            var quest = new Quest
            {
                Id = questId,
                MaxParticipants = 6,
                DurationMinutes = 60,
                Price = 1000M
            };

            var mockQuestRepo = new Mock<IQuestRepository>();
            mockQuestRepo.Setup(repo => repo.GetById(questId)).Returns(quest);

            var mockBookingRepo = new Mock<IBookingRepository>();
            mockBookingRepo.Setup(repo => repo.IsTimeSlotAvailable(
                questId, It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(false); // Слот недоступний

            var mockBookingsRepo = new Mock<IRepository<Booking>>();

            _mockUnitOfWork.Setup(uow => uow.QuestRepository).Returns(mockQuestRepo.Object);
            _mockUnitOfWork.Setup(uow => uow.BookingRepository).Returns(mockBookingRepo.Object);
            _mockUnitOfWork.Setup(uow => uow.Bookings).Returns(mockBookingsRepo.Object);

            // Act
            bool result = _bookingService.CreateBooking(questId, clientId, startTime, participantsCount);

            // Assert
            Assert.IsFalse(result);
            mockQuestRepo.Verify(repo => repo.GetById(questId), Times.Once);
            mockBookingRepo.Verify(repo => repo.IsTimeSlotAvailable(
                questId, startTime, It.IsAny<DateTime>()), Times.Once);
            mockBookingsRepo.Verify(repo => repo.Add(It.IsAny<Booking>()), Times.Never);
            _mockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }

        [TestMethod]
        public void CreateBooking_WithValidGiftCertificate_ShouldApplyDiscountAndMarkCertificateAsUsed()
        {
            // Arrange
            int questId = 1;
            int clientId = 1;
            DateTime startTime = DateTime.Now.AddDays(1);
            int participantsCount = 4;
            string certificateCode = "TEST123";

            var quest = new Quest
            {
                Id = questId,
                MaxParticipants = 6,
                DurationMinutes = 60,
                Price = 1000M
            };

            var certificate = new GiftCertificate
            {
                Id = 1,
                Code = certificateCode,
                IsUsed = false,
                ExpiryDate = DateTime.Now.AddMonths(1),
                QuestId = null
            };

            var mockQuestRepo = new Mock<IQuestRepository>();
            mockQuestRepo.Setup(repo => repo.GetById(questId)).Returns(quest);

            var mockBookingRepo = new Mock<IBookingRepository>();
            mockBookingRepo.Setup(repo => repo.IsTimeSlotAvailable(
                questId, It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(true);

            var mockCertificateRepo = new Mock<IGiftCertificateRepository>();
            mockCertificateRepo.Setup(repo => repo.GetByCode(certificateCode)).Returns(certificate);
            mockCertificateRepo.Setup(repo => repo.IsValidCertificate(certificateCode)).Returns(true);

            var mockBookingsRepo = new Mock<IRepository<Booking>>();
            var mockCertificatesRepo = new Mock<IRepository<GiftCertificate>>();

            _mockUnitOfWork.Setup(uow => uow.QuestRepository).Returns(mockQuestRepo.Object);
            _mockUnitOfWork.Setup(uow => uow.BookingRepository).Returns(mockBookingRepo.Object);
            _mockUnitOfWork.Setup(uow => uow.GiftCertificateRepository).Returns(mockCertificateRepo.Object);
            _mockUnitOfWork.Setup(uow => uow.Bookings).Returns(mockBookingsRepo.Object);
            _mockUnitOfWork.Setup(uow => uow.GiftCertificates).Returns(mockCertificatesRepo.Object);

            // Act
            bool result = _bookingService.CreateBooking(questId, clientId, startTime, participantsCount, certificateCode);

            // Assert
            Assert.IsTrue(result);
            mockBookingsRepo.Verify(repo => repo.Add(It.Is<Booking>(b => b.TotalPrice == 0)), Times.Once);
            mockCertificatesRepo.Verify(repo => repo.Update(It.Is<GiftCertificate>(c => c.IsUsed == true)), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [TestMethod]
        public void CancelBooking_WithValidId_ShouldReturnTrue()
        {
            // Arrange
            int bookingId = 1;
            var booking = new Booking
            {
                Id = bookingId,
                QuestId = 1,
                ClientId = 1,
                StartTime = DateTime.Now.AddDays(1),
                Status = "Підтверджено"
            };

            var mockBookingRepo = new Mock<IBookingRepository>();
            mockBookingRepo.Setup(repo => repo.GetById(bookingId)).Returns(booking);

            var mockBookingsRepo = new Mock<IRepository<Booking>>();

            _mockUnitOfWork.Setup(uow => uow.BookingRepository).Returns(mockBookingRepo.Object);
            _mockUnitOfWork.Setup(uow => uow.Bookings).Returns(mockBookingsRepo.Object);

            // Act
            bool result = _bookingService.CancelBooking(bookingId);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual("Скасовано", booking.Status);
            mockBookingsRepo.Verify(repo => repo.Update(booking), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.Complete(), Times.Once);
        }

        [TestMethod]
        public void CancelBooking_WithInvalidId_ShouldReturnFalse()
        {
            // Arrange
            int bookingId = 999; 

            var mockBookingRepo = new Mock<IBookingRepository>();
            mockBookingRepo.Setup(repo => repo.GetById(bookingId)).Returns((Booking)null);

            var mockBookingsRepo = new Mock<IRepository<Booking>>();

            _mockUnitOfWork.Setup(uow => uow.BookingRepository).Returns(mockBookingRepo.Object);
            _mockUnitOfWork.Setup(uow => uow.Bookings).Returns(mockBookingsRepo.Object);

            // Act
            bool result = _bookingService.CancelBooking(bookingId);

            // Assert
            Assert.IsFalse(result);
            mockBookingsRepo.Verify(repo => repo.Update(It.IsAny<Booking>()), Times.Never);
            _mockUnitOfWork.Verify(uow => uow.Complete(), Times.Never);
        }
    }
}