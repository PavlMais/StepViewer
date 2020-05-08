using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using UI.Models;
using UI.Managers;
using AsyncAwaitBestPractices.MVVM;
using UI.Broadcast;

namespace UI.ViewModels
{
    class ActiveSessionViewModel : BindableBase
    {
        public BaseViewModel BaseViewModel { get; set; }
        public ActiveSession Session { get => _session; set => SetProperty(ref _session, value); }
        private static readonly Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private static readonly DispatcherTimer _timer = new DispatcherTimer();
        private ActiveSession _session;

        public string IsPausedContent { get => _isPausedContent; set => SetProperty(ref _isPausedContent, value); }
        public bool IsPaused
        {
            get { return _isPaused; }
            set
            {
                _isPaused = value;
                if (_isPaused)
                {
                    OnPauseSession();
                    IsPausedContent = "Resume";
                }
                else
                {
                    OnResumeSession();
                    IsPausedContent = "Pause";
                }
            }
        }


        private StreamAPI streamAPI = StreamAPI.GetInstance();
        private bool _isPaused;
        private string _isPausedContent;

        public AsyncCommand StopCommand { get; set; }

        public ActiveSessionViewModel(BaseViewModel bvm, Group group)
        {
            BaseViewModel = bvm;
            Session = group.ActiveSession;

            StopCommand = new AsyncCommand(OnStopSession);

            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Start();
            _timer.Tick += (s, e) => OnPropertyChanged("Session");

        }
        private async Task OnStopSession()
        {
            streamAPI.StopStream();
        }

        public void OnPauseSession()
        {
            streamAPI.PauseStream();
        }
        public void OnResumeSession()
        {
            streamAPI.ResumeStream();

        }

    }
}
