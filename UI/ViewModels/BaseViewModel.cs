using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UI.Models;

namespace UI.ViewModels
{
    class BaseViewModel : BindableBase
    {
        public bool IsLeftMenuOpen { get => _isLeftMenuOpen; set => SetProperty(ref _isLeftMenuOpen, value); }

        public ObservableCollection<BindableBase> Contents { get; set; }

        public BindableBase SelectedContent { get => _selectedContent; set => SetProperty(ref _selectedContent, value); }
        public BindableBase CurrentContent { get => _currentContent; set => SetProperty(ref _currentContent, value); }
        public Command LeftMenuClickCommand { get; set; }

        private GroupsViewModel groupsViewModel;
        private BindableBase _selectedContent;
        private bool _isLeftMenuOpen = false;
        private BindableBase _currentContent;

        public BaseViewModel()
        {
            groupsViewModel = new GroupsViewModel(this);
            LeftMenuClickCommand = new Command(OnLeftMenuClick);
            Contents = new ObservableCollection<BindableBase>();
            Contents.Add(groupsViewModel);
            CurrentContent = groupsViewModel;
        }

        private void OnLeftMenuClick(object o)
        {
            CurrentContent = SelectedContent;
            IsLeftMenuOpen = false;

        }

        public void OpenGroupView(Group groups)
        {
            CurrentContent = new GroupViewModel(this, groups);
        }
    }
}
