using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WCFClient.ServiceReference1;


namespace WCFClient
{
    public class API : ServiceClient
    {
        public APICallback APICallback = new APICallback();

        public API() : base(APICallback.site) {}
    }

    

    public class APICallback : IServiceCallback
    {
        public static InstanceContext site = new InstanceContext(new APICallback());

        public event Action<int, int> CInviteReceived;
        public event Action<ActiveSession> CSessionStarted;


        public void InviteReceived(int fromUserId, int groupId)
        {
            CInviteReceived?.Invoke(fromUserId, groupId);
        }

        public void SessionStarted(ActiveSession session)
        {
            CSessionStarted?.Invoke(session);
        }
    }
}
