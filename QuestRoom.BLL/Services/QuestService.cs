using QuestRoom.DAL.Entities;
using QuestRoom.DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuestRoom.BLL.Services
{
    public class QuestService : IQuestService
    {
        private readonly IUnitOfWork _unitOfWork;

        public QuestService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public List<Quest> GetAllQuests()
        {
            return new List<Quest>(_unitOfWork.Quests.GetAll());
        }

        public Quest GetQuestById(int id)
        {
            return _unitOfWork.QuestRepository.GetById(id);
        }

        public Quest GetQuestWithBookings(int id)
        {
            return _unitOfWork.QuestRepository.GetQuestWithBookings(id);
        }

        public void AddQuest(Quest quest)
        {
            _unitOfWork.Quests.Add(quest);
            _unitOfWork.Complete();
        }

        public void UpdateQuest(Quest quest)
        {
            _unitOfWork.Quests.Update(quest);
            _unitOfWork.Complete();
        }

        public void DeleteQuest(int id)
        {
            var quest = _unitOfWork.Quests.GetById(id);
            if (quest != null)
            {
                _unitOfWork.Quests.Remove(quest);
                _unitOfWork.Complete();
            }
        }
    }
}