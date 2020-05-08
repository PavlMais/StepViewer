using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WCFClient.ServiceReference1;


namespace WCFClient
{
    public class API : IServiceCallback
    {
        static InstanceContext site = new InstanceContext(new API());
        public static ServiceClient proxy = new ServiceClient(site);
        public static User CurrentUser { get; set; }

        public static event Action<ActiveSession> SessionStarted;
        public static event Action<ActiveSession> ActiveSessionUpdated;
        public static event Action<List<GroupInvite>> GroupInvitesUpdated;
        public static event Action<List<Group>> GroupListUpdated;
    


        public void OnSessionStarted(ActiveSession s)
        {
            SessionStarted?.Invoke(s);
        }


        public void OnGroupListUpdated(Group[] groups)
        {
            GroupListUpdated.Invoke(groups.ToList());
        }

        public void OnActiveSessionUpdated(ActiveSession activeSession)
        {
            ActiveSessionUpdated?.Invoke(activeSession);
        }

        public void OnGroupInvitesUpdated(GroupInvite[] groupInvites)
        {
            GroupInvitesUpdated?.Invoke(groupInvites.ToList());
        }
    }

    
}
