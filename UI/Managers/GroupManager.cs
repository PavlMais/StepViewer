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

    public class GroupManager : BaseManager<GroupManager, Group>
    {
        protected override void Init()
        {
            //Update(Mpr.Map<List<Group>>(API.proxy.GetGroups().Data));

            API.GroupListUpdated += (gl) => Update(gl);
        }

        
        protected override ICollection<Group> GetFromService(ICollection<int> ids)
        {
            var r = API.proxy.GetGroups(ids.ToArray());

            var gr = r.Data;

            List<Group> groups = Mpr.Map<List<Group>>(gr);

            for (int i = 0; i < gr.Length; i++)
            {
                //Logger.Debug("user id");
                //Logger.Debug(gr[i].MembersIds[0]);
                groups[i].ActiveSession = Mpr.Map<ActiveSession>(API.proxy.GetActiveSessionById(groups[i].Id).Data);
                groups[i].IsOwner = (API.CurrentUser.Id == groups[i].Owner.Id);
                groups[i].Members = new ObservableCollection<User>(
                    UserManager.GetInstance().GetItems(gr[i].MembersIds).ToList());
            }

            return groups;
        }

        public List<Group> GetUserGroups()
        {
            return GetItems(API.proxy.GetUserGroupsIds().Data.ToList()).ToList();
        }

    }


    


}

