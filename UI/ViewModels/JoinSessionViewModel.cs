using AsyncAwaitBestPractices.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Broadcast;
using UI.Models;

namespace UI.ViewModels
{
    class JoinSessionViewModel : BindableBase
    {
        public BaseViewModel BaseViewModel { get; set; }

        public ActiveSession Session { get; set; }

        public string IsMuteContent { get => _isMuteContent; set => SetProperty(ref _isMuteContent,  value); }
        public bool IsMute
        {
            get { return _isMute; }
            set
            {
                _isMute = value;
                if (_isMute)
                {
                    receiverAPI.Mute();
                    IsMuteContent = "UnMute";
                }
                else
                {
                    receiverAPI.UnMute();
                    IsMuteContent = "Mute";
                }
            }
        }
        public AsyncCommand LeaveSession { get; set; }

        private ReceiverAPI receiverAPI = ReceiverAPI.GetInstance();
        private bool _isMute;
        private string _isMuteContent;

        public JoinSessionViewModel(BaseViewModel bvm, Group group)
        {
            BaseViewModel = bvm;
            Session = group.ActiveSession;
        }


    }
}
