using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UI.Broadcast
{
    class VideoStream
    {
        private ManualResetEvent resetEvent;
        private Thread thread;
        private bool isStopped;
        private int sessionId;

        public event Action<byte[]> SendFrame;

        public VideoStream()
        {
            thread = new Thread(Loop);
        }

        public void StartStreaming(int _sessionId)
        {
            sessionId = _sessionId;
            isStopped = false;
            resetEvent = new ManualResetEvent(true);

            thread.Start();

        }
        public void PauseStream()
        {
            resetEvent.Set();
        }
        public void ResumeStream()
        {
            resetEvent.Reset();
        }
        public void StopStream()
        {
            isStopped = true;
            thread.Join(50);
        }

        private void Loop()
        {
            while (!isStopped)
            {
                resetEvent.WaitOne();

                Bitmap[] bitmaps = Screenshoter.Screenshot(20);

                for (int i = 0; i < 20; i++)
                    SendFragment(i, bitmaps[i]);

            }
        }
        private void SendFragment(int id, Bitmap fragment)
        {
            MemoryStream fStream = new MemoryStream();
            fragment.Save(fStream, ImageFormat.Jpeg);
            fStream.Position = 0;

            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write((Int32)sessionId);
            writer.Write((byte)1); // 1 = Video
            writer.Write((byte)id); // id fragment
            writer.Write((Int32)fStream.Length); // size fragment
            writer.Write(fStream.ToArray());
            stream.Position = 0;


            SendFrame?.Invoke(stream.ToArray());
        }
    }
}
