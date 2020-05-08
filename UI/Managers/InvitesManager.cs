using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCFClient;
using WCFClient.ServiceReference1;

namespace UI.Managers
{
    using Models;
    class InvitesManager : BaseManager<InvitesManager, GroupInvite>
    {
        protected override void Init()
        {
            
            Update(API.proxy.GetGroupsInvites().Data);

            API.GroupInvitesUpdated += (gi) => Update(gi.Cast<object>());
        }

        protected override ICollection<GroupInvite> GetFromService(ICollection<int> ids)
        {
            return Mpr.Map<ICollection<GroupInvite>>(API.proxy.GetGroupsInvites().Data);
        }

    }
}

