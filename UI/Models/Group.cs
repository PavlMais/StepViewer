using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCFClient;

namespace UI.Models
{
    public abstract class IdBindable : BindableBase
    {
        public int Id { get; set; }
    }


    public class Group : IdBindable
    {
        public string Name { get; set; }

        public bool IsOwner { get; set; }
        public User Owner { get; set; }

        public ActiveSession ActiveSession { get => _activeSession; set => SetProperty(ref _activeSession, value); }
        public ObservableCollection<User> Members { get => _members; set => SetProperty(ref _members, value); }

        private ObservableCollection<User> _members;
        private ActiveSession _activeSession;

        public Group()
        {



            API.SessionStarted += API_SessionStarted;
            API.ActiveSessionUpdated += API_SessionStarted;
        }

        private void API_SessionStarted(WCFClient.ServiceReference1.ActiveSession session)
        {
            if (session.GroupId == Id) ActiveSession = Mpr.Map<ActiveSession>(session);
        }
    }




}
