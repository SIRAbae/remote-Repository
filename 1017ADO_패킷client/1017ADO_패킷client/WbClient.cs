using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1017ADO_패킷client
{
    class WbClient
    {
        #region 맴버 필드 및 프로퍼티
        //9000
        private Socket client;
        private Form1 form;
        private List<Socket> slist = new List<Socket>();
        public string ClientIp { get; private set; }
        public int ClientPort { get; private set; }
        #endregion
        public void ParentInfo(Form1 f)
        {
            form = f;
        }

        #region 기능(외부 접근) Form1에서 접근
        public bool ClientRun(string IP, int Port)
        {
            try
            {
                IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(IP), Port);

                client = new Socket(AddressFamily.InterNetwork,
                                         SocketType.Stream, ProtocolType.Tcp);

                client.Connect(ipep);  // 127.0.0.1 서버 7000번 포트에 접속시도

                Thread th = new Thread(new ThreadStart(RecvThread));
                th.Start();
                th.IsBackground = true;

                return true;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public void ClientStop()
        {
            try
            {
                client.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region 기능(내부 사용) WbServer -> Form1으로 전달하는 함수

        private void ShortMessage(byte[] recvdata)
        {
            form.ShortMessage(Encoding.Default.GetString(recvdata));
        }

        #endregion

        #region(내부 사용) 자체 호출 기능

        public void Send(string msg)
        {
            try
            {
                if (client.Connected)
                {
                    byte[] data = Encoding.Default.GetBytes(msg);
                    this.SendData(data);
                }
                else
                {
                   MessageBox.Show("메시지 전송 실패");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void SendData(byte[] data)
        {
            try
            {
                int total = 0;
                int size = data.Length;
                int left_data = size;
                int send_data = 0;

                // 전송할 데이터의 크기 전달
                byte[] data_size = new byte[4];
                data_size = BitConverter.GetBytes(size);
                send_data = this.client.Send(data_size);

                // 실제 데이터 전송
                while (total < size)
                {
                    send_data = this.client.Send(data, total, left_data, SocketFlags.None);
                    total += send_data;
                    left_data -= send_data;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void ReceiveData(ref byte[] data)
        {
            try
            {
                int total = 0;
                int size = 0;
                int left_data = 0;
                int recv_data = 0;

                // 수신할 데이터 크기 알아내기 
                byte[] data_size = new byte[4];
                recv_data = this.client.Receive(data_size, 0, 4, SocketFlags.None);
                size = BitConverter.ToInt32(data_size, 0);
                left_data = size;

                data = new byte[size];

                // 실제 데이터 수신
                while (total < size)
                {
                    recv_data = this.client.Receive(data, total, left_data, 0);
                    if (recv_data == 0) break;
                    total += recv_data;
                    left_data -= recv_data;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }

        #endregion

        #region thread

        public void SendMessage(string msg)
        {
            client.Send(Encoding.Default.GetBytes(msg));
        }

        public void RecvThread()
        {
            byte[] data = new byte[1024];
            ReceiveData(ref data);
            string str = Encoding.Default.GetString(data);
            form.RecvPrint(str);
        }

        #endregion
    }
}
