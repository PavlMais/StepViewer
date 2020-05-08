using NAudio.Wave;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace UI.Broadcast
{
    class ReceiverAPI : BindableBase
    {
        private static Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        private static ReceiverAPI Instance;
        private UdpClient client;
        private Thread thread;
        private BufferedWaveProvider bwp;
        public float Volume = 1f;
        private WaveOut waveOut = new WaveOut();


        public event Action<int, BitmapImage> ImgUpdated;

        public static ReceiverAPI GetInstance()
        {
            if (Instance == null)
                Instance = new ReceiverAPI();
            return Instance;
        }

        public ReceiverAPI()
        {
            bwp = new BufferedWaveProvider(new WaveFormat(8000, 1));
            bwp.DiscardOnBufferOverflow = true;
            waveOut.Init(bwp);

            thread = new Thread(ReceiveThread);
        }



        public async Task JoinSession()
        {
            client = new UdpClient(8888);
            client.JoinMulticastGroup(IPAddress.Parse("239.255.42.99"));
            thread.Start();
            waveOut.Play();
            Logger.Debug($"Local end point: {client.Client.LocalEndPoint}");
            Logger.Debug("Join multicast group... 239.255.42.99");
        }
        public void Mute()
        {
            waveOut.Volume = 0;
        }
        public void UnMute()
        {
            waveOut.Volume = Volume;
        }

        private void ReceiveThread()
        {

            byte[] buffer = null;
            IPEndPoint point = null;
            Logger.Debug("Wait service udp stream...");

            while (true)
            {
                buffer = client.Receive(ref point);

                BinaryReader reader = new BinaryReader(new MemoryStream(buffer));

                int sessionId = reader.ReadInt32(); // Session id
                byte type = reader.ReadByte(); // type 1-videp 2-audio

                if (type == 1)
                {
                    byte fId = reader.ReadByte();   // frame id
                    int fSize = reader.ReadInt32(); // frame size
                    byte[] fBuffer = reader.ReadBytes(fSize);
                    UpdateVideo(fId, fBuffer);
                }
                else if (type == 2)
                {
                    int sampleSize = reader.ReadInt32();
                    byte[] sampleBuffer = reader.ReadBytes(sampleSize);
                    UpdateAudio(sampleBuffer);
                }
            }
        }


        private void UpdateAudio(byte[] buffer)
        {
            //Logger.Debug("Update audio");
            bwp.AddSamples(buffer, 0, buffer.Length);
        }


        private void UpdateVideo(int id, byte[] buffer)
        {
            Logger.Debug("Update video");
            var image = new BitmapImage();
            using (var mem = new MemoryStream(buffer))
            {
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();

            Application.Current.Dispatcher.Invoke(() => ImgUpdated?.Invoke(id, image));

        }
    }
}
