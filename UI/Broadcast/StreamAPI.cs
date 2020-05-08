using NAudio.Wave;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WCFClient;

namespace UI.Broadcast
{
    class StreamAPI
    {
        private static readonly Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private static StreamAPI Instance;

        public static StreamAPI GetInstance()
        {
            if (Instance == null)
                Instance = new StreamAPI();
            return Instance;
        }

        private StreamEngine engine = new StreamEngine();
        public bool IsSessionStarted { get; private set; }
        public bool IsSessionPaused { get; private set; }

        public async Task StartStream(string name, int groupId)
        {
            IsSessionStarted = true;
            await API.proxy.StartSessionAsync(name, groupId);
            engine.StartStream(433);
        }

        public void PauseStream()
        {
            IsSessionPaused = true;
        }
        public void ResumeStream()
        {
            IsSessionPaused = false;
        }

        public void StopStream()
        {
            IsSessionStarted = false;
        }
    }

    
}
