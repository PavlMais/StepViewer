using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Models;

namespace UI.ViewModels
{
    class GroupViewModel : BindableBase
    {
        public string ViewModelName { get; set; } = "Group name";
        

        public Group Group { get; set; }

        public bool IsOwner { get; set; }

        public AsyncCommand StartSessionCommand { get; set; }
        public BaseViewModel BaseViewModel { get; set; }


        public GroupViewModel(BaseViewModel i, Group group)
        {
            BaseViewModel = i;
            Group = group;
            StartSessionCommand = new AsyncCommand(OnStartSession);
        }

        private async Task OnStartSession()
        {
            await APIProxy.api.StartSessionAsync("Session test 1", Group.Id);
        }


    }
}
