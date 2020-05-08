using AsyncAwaitBestPractices.MVVM;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UI.Broadcast;
using UI.Managers;
using UI.Models;
using WCFClient;

namespace UI.ViewModels
{
    class GroupViewModel : BindableBase
    {
        public string ViewModelName { get; set; } = "Group name";
        private static Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private StreamAPI streamAPI = StreamAPI.GetInstance();
        private Group _group;

        public Group Group { get => _group; set => SetProperty(ref _group, value); }



        public AsyncCommand JoinSessionCommand { get; set; }
        public AsyncCommand StartSessionCommand { get; set; }
        public BaseViewModel BaseViewModel { get; set; }


        public GroupViewModel(BaseViewModel i, Group group)
        {
            BaseViewModel = i;
            Group = group;
            //MessageBox.Show($"{Group.Members.Count}");

            

            StartSessionCommand = new AsyncCommand(OnStartSession);
            JoinSessionCommand = new AsyncCommand(OnJoinSession);
        }


        private async Task OnStartSession()
        {
            Logger.Debug("Start session...");
            await streamAPI.StartStream("Name-session", Group.Id);

            BaseViewModel.OpenActiveSessionView(Group);
        }

        private async Task OnJoinSession()
        {
            Logger.Debug("Join session...");
            await ReceiverAPI.GetInstance().JoinSession();

            BaseViewModel.StreamView(Group);

        }


    }
}
