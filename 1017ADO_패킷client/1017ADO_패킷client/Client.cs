using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _1017ADO_패킷client
{
    class Client
    {
        //9000
        private Socket client;

        public void ClientRun(string IP, int Port)
        {
            try
            {
                IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(IP), Port);

                Socket server = new Socket(AddressFamily.InterNetwork,
                                         SocketType.Stream, ProtocolType.Tcp);

                server.Connect(ipep);  // 127.0.0.1 서버 7000번 포트에 접속시도

                Console.WriteLine("서버에 접속...");  // 만약 서버 접속이 실패하면 예외 발생
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
