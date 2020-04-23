using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UI.Models;

namespace UI.ViewModels
{
    class GroupsViewModel : BindableBase
    {
        private bool _isDialogCreateGroupOpen;

        public string ViewModelName { get; set; } = "Groups";


        public Command OpenGroupCommand { get; set; }
        public Group SelectedGroup { get; set; }
        public BaseViewModel BaseViewModel { get; set; }
        public ObservableCollection<Group> Groups { get; set; }

        public bool IsDialogCreateGroupOpen { get => _isDialogCreateGroupOpen; set => SetProperty(ref _isDialogCreateGroupOpen, value); }

        public IAsyncCommand<string> CreateGroupCommand { get; set; }


        public GroupsViewModel(BaseViewModel i)
        {
            BaseViewModel = i;
            OpenGroupCommand = new Command(OnOpenGroup);
            CreateGroupCommand = new AsyncCommand<string>(OnCreateGroup);

            Groups = new ObservableCollection<Group>(APIProxy.GetGroups());


            
        }

        private void OnOpenGroup(object o)
        {
            BaseViewModel.OpenGroupView(o as Group);
        }

        private async Task OnCreateGroup(string groupName)
        {
            await APIProxy.api.CreateGroupAsync(groupName);
            Groups = null;
            Groups = new ObservableCollection<Group>(APIProxy.GetGroups());
            IsDialogCreateGroupOpen = false;

        }



    }
}
