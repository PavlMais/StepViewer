using AsyncAwaitBestPractices.MVVM;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UI.Managers;
using UI.Models;
using WCFClient;

namespace UI.ViewModels
{
    class GroupsViewModel : BindableBase
    {
        private bool _isDialogCreateGroupOpen;
        private bool _isDialogJoinGroupOpen;

        public string ViewModelName { get; set; } = "Groups";
        ///private static API api = API.GetInstance();

        public Command OpenGroupCommand { get; set; }
        public Group SelectedGroup { get; set; }
        public BaseViewModel BaseViewModel { get; set; }

        public ObservableCollection<Group> Groups { get; set; } 
            = new ObservableCollection<Group>(GroupManager.GetInstance().GetUserGroups());

        public bool IsDialogCreateGroupOpen { get => _isDialogCreateGroupOpen; set => SetProperty(ref _isDialogCreateGroupOpen, value); }
        public bool IsDialogJoinGroupOpen { get => _isDialogJoinGroupOpen; set => SetProperty(ref _isDialogJoinGroupOpen, value); }

        public IAsyncCommand<string> CreateGroupCommand { get; set; }
        public IAsyncCommand<string> SendInviteCommand { get; set; }


        public GroupsViewModel(BaseViewModel i)
        {
            BaseViewModel = i;
            OpenGroupCommand = new Command(OnOpenGroup);
            CreateGroupCommand = new AsyncCommand<string>(OnCreateGroup);
            SendInviteCommand = new AsyncCommand<string>(OnSendInviteCommand);
        }

        private void OnOpenGroup(object o)
        {
            if (o == null) return;
            BaseViewModel.OpenGroupView(o as Group);
        }
        private async Task OnSendInviteCommand(string groupName)
        {
            await API.proxy.SendGroupInviteAsync(groupName);
            IsDialogJoinGroupOpen = false;

        }

        private async Task OnCreateGroup(string groupName)
        {
            await API.proxy.CreateGroupAsync(groupName);
            IsDialogCreateGroupOpen = false;
        }
    }
}
