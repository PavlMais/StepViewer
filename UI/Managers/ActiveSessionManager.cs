using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCFClient;

namespace UI.Managers
{
    using Models;


    public class ActiveSessionManager : BaseManager<ActiveSessionManager, ActiveSession>
    {
        protected override ICollection<ActiveSession> GetFromService(ICollection<int> ids)
        {
            throw new NotImplementedException();
        }

        protected override void Init()
        {
            //Update(Mpr.Map<List<ActiveSession>>(API.proxy.GetActiveSessions()?.Data));

            //API.SessionStarted += (s) => AddItem(Mpr.Map<ActiveSession>(s));
        }
    }
}
