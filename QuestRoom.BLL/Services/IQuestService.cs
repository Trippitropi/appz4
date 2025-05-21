using QuestRoom.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestRoom.BLL.Services
{
    public interface IQuestService
    {
        List<Quest> GetAllQuests();
        Quest GetQuestById(int id);
        Quest GetQuestWithBookings(int id);
        void AddQuest(Quest quest);
        void UpdateQuest(Quest quest);
        void DeleteQuest(int id);
    }
}
