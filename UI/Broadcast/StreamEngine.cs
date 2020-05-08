using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

using System.Net.Sockets;
using System.Text;
using System.IO;

using System.Threading;
using System.Threading.Tasks;
using System.Net;
using NLog;
using NAudio.Wave;

namespace UI.Broadcast
{
    class StreamEngine
    {
        private static Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public IPEndPoint ServerIP { get; set; } = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 10000);
        public int SessionId { get; private set; }
        private WaveIn waveIn = new WaveIn();
        private VideoStream videoStream;
        private UdpClient sender;
        private static StreamEngine Instance;

        public StreamEngine()
        {
            videoStream = new VideoStream();
            videoStream.SendFrame += SendBuffer;

            waveIn.WaveFormat = new WaveFormat(8000, 16, 1);
            waveIn.BufferMilliseconds = 25;
            waveIn.DataAvailable += WaveIn_DataAvailable;
        }

        public static StreamEngine GetInstance()
        {
            if (Instance == null)
                Instance = new StreamEngine();
            return Instance;
        }
        public void Pause()
        {
            waveIn.StopRecording();
            videoStream.PauseStream();
        }
        public void Resume()
        {
            waveIn.StartRecording();
            videoStream.ResumeStream();
        }

        public void StartStream(int sessionId)
        {
            sender = new UdpClient(8888);
            SessionId = sessionId;

            videoStream.StartStreaming(sessionId);
            waveIn.StartRecording();
            
        }
        private void WaveIn_DataAvailable(object _, WaveInEventArgs e)
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write((Int32)SessionId);
            writer.Write((byte)2); // 2 = Audio
            writer.Write((Int32)e.BytesRecorded); // buffer size
            writer.Write(e.Buffer); // buffer sample

            stream.Position = 0;
            SendBuffer(stream.ToArray());
        }

        public void SendBuffer(byte[] buffer)
        {
            sender.Send(buffer, buffer.Length, ServerIP);
        }
    }


    

}
