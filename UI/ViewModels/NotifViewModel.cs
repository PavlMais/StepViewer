using AsyncAwaitBestPractices.MVVM;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UI.Managers;
using UI.Models;
using WCFClient;


namespace UI.ViewModels
{
    class NotifViewModel : BindableBase
    {
        public string ViewModelName { get; set; } = "Notification";

        public BaseViewModel BaseViewModel { get; set; }

        public InvitesManager InvitesManager { get => _invitesManager; set => SetProperty(ref _invitesManager, value); }
        public IAsyncCommand<GroupInvite> ICancelInviteCommand { get; set; }
        public IAsyncCommand<GroupInvite> IAcceptInviteCommand { get; set; }

        private static readonly Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private InvitesManager _invitesManager = InvitesManager.GetInstance();

        public NotifViewModel(BaseViewModel baseViewModel)
        {
            BaseViewModel = baseViewModel;

            ICancelInviteCommand = new AsyncCommand<GroupInvite>(OnCancelInvite);
            IAcceptInviteCommand = new AsyncCommand<GroupInvite>(OnAcceptlInvite);



        }

        private async Task OnCancelInvite(GroupInvite groupInvite)
        {

            InvitesManager.Collection.Remove(groupInvite);
            await API.proxy.CancelGroupInviteAsync(groupInvite.Id);
        }

        private async Task OnAcceptlInvite(GroupInvite groupInvite)
        {
            InvitesManager.Collection.Remove(groupInvite);
            await API.proxy.AcceptGroupInviteAsync(groupInvite.Id);

        }
    }
}
