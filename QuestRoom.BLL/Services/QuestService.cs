using QuestRoom.DAL.Entities;
using QuestRoom.DAL.Repositories;
using System;
using System.Collections.Generic;

namespace QuestRoom.BLL.Services
{
    public class QuestService
    {
        private readonly QuestRepository _questRepository;

        public QuestService(QuestRepository questRepository)
        {
            _questRepository = questRepository;
        }

        public List<Quest> GetAllQuests()
        {
            return new List<Quest>(_questRepository.GetAll());
        }

        public Quest GetQuestById(int id)
        {
            return _questRepository.GetById(id);
        }

        public Quest GetQuestWithBookings(int id)
        {
            return _questRepository.GetQuestWithBookings(id);
        }

        public void AddQuest(Quest quest)
        {
            _questRepository.Add(quest);
            _questRepository.SaveChanges();
        }

        public void UpdateQuest(Quest quest)
        {
            _questRepository.Update(quest);
            _questRepository.SaveChanges();
        }

        public void DeleteQuest(int id)
        {
            var quest = _questRepository.GetById(id);
            if (quest != null)
            {
                _questRepository.Remove(quest);
                _questRepository.SaveChanges();
            }
        }
    }
}
