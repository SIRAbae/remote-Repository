using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1017ADO_패킷client
{
    class Packet
    {

        //싱글톤
        public static Packet Intance { get; private set; }


        static Packet()
        {
            Intance = new Packet();
        }
        private Packet()
        {

        }

        public string GetNewMember( string id, string pw, string name, int age)
        {
            string msg = null;
            msg += "NEWMEMBER@";         // 회원 가입 요청 메시지
            msg += id + "#";                  // 아이디
            msg += pw + "#";                 // 암호
            msg += name + "#";                // 이름
            msg += age;               // 나이

            return msg;
        }

        public string GetDelMember(string id)
        {
            string msg = null;
            msg += "DELMEMBER@";         // 회원 가입 요청 메시지
            msg += id;                  // 아이디
        
            return msg;
        }

        public string GetLogin(string id, string pw)
        {
            string msg = null;
            msg += "LOGIN@";         // 회원 가입 요청 메시지
            msg += id + "#";                  // 아이디
            msg += pw;                 // 암호
           
            return msg;
        }

        public string GetLogout(string id)
        {
            string msg = null;
            msg += "LOGOUT@";         // 회원 가입 요청 메시지
            msg += id;                  // 아이디
           

            return msg;
        }

        public string GetMessage(string data)
        {
            string msg = null;
            msg += "MESSAGE@";         // 회원 가입 요청 메시지
            msg += data;
            return msg;
        }
    }
}
