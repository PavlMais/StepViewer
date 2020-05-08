using MaterialDesignThemes.Wpf;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using UI.Broadcast;
using UI.Models;
using WCFClient;

namespace UI.ViewModels
{
    class BaseViewModel : BindableBase
    {
        public bool IsLeftMenuOpen { get => _isLeftMenuOpen; set => SetProperty(ref _isLeftMenuOpen, value); }

        public ObservableCollection<BindableBase> Contents { get; set; }

        public BindableBase SelectedContent { get => _selectedContent; set => SetProperty(ref _selectedContent, value); }
        public BindableBase CurrentContent { get => _currentContent; set => SetProperty(ref _currentContent, value); }
        public Command LeftMenuClickCommand { get; set; }
        public bool IsStreamVisible { get => _isStreamJoined; set => SetProperty(ref _isStreamJoined, value); }
        public bool IsAppVisible { get => _isAppOpened; set => SetProperty(ref _isAppOpened, value); }
        public User CurrentUser { get => _currentUser; set => SetProperty(ref _currentUser, value); }
        public ISnackbarMessageQueue MainSnackBar { get; set; }



        public StackPanel VideoStackPanel { get => _videoStackPanel; set => SetProperty(ref _videoStackPanel, value); }
        public ObservableCollection<System.Windows.Controls.Image> Images { get; set; } 
            = new ObservableCollection<System.Windows.Controls.Image>();


        private static Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private BindableBase activeSessionView;
        private GroupsViewModel groupsViewModel;
        private NotifViewModel notifViewModel;
        private ViewsManager viewsManager;

        private BindableBase _selectedContent;
        private bool _isLeftMenuOpen = false;
        private BindableBase _currentContent;
        private bool _isStreamJoined = false;
        private bool _isAppOpened = true;
        private User _currentUser;
        private StackPanel _videoStackPanel;

        public BaseViewModel(ViewsManager _viewsManager)
        {
            VideoStackPanel = new StackPanel { Orientation = Orientation.Horizontal };


            for (int i = 0; i < 20; i++)
            {
                Images.Add(new System.Windows.Controls.Image());
                VideoStackPanel.Children.Add(Images[i]);
            }

            ReceiverAPI.GetInstance().ImgUpdated += ImgUpdated;

            viewsManager = _viewsManager;
            groupsViewModel = new GroupsViewModel(this);
            notifViewModel = new NotifViewModel(this);
            LeftMenuClickCommand = new Command(OnLeftMenuClick);
            CurrentUser = Mpr.Map<User>(API.CurrentUser);
            Contents = new ObservableCollection<BindableBase>();
            Contents.Add(groupsViewModel);
            Contents.Add(notifViewModel);
            MainSnackBar = new SnackbarMessageQueue(TimeSpan.FromSeconds(3));

            API.SessionStarted += OnSessionStarted;



            CurrentContent = groupsViewModel;
        }

        private void ImgUpdated(int id, BitmapImage image)
        {
            Images[id].Source = image;
        }

        private void OnSessionStarted(WCFClient.ServiceReference1.ActiveSession ac)
        {

            MainSnackBar.Enqueue($"Session '{ac.Name}' started");
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

        public void OpenActiveSessionView(Group group)
        {
            CurrentContent = new ActiveSessionViewModel(this, group);
        }

        public void StreamView(Group group)
        {
            IsStreamVisible = true;
            viewsManager.FullMode();


            CurrentContent = new JoinSessionViewModel(this, group);
        }
    }
}
