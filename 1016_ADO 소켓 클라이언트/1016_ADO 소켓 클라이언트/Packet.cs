using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1016_ADO_소켓_클라이언트
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

        public string GetNewMember(string id, string pw, string name, string age)
        {
            string msg = null;
            msg += "NEW_MEMBER@";         // 회원 가입 요청 메시지
            msg += id + "#";                  // 아이디
            msg += pw + "#";                 // 암호
            msg += name + "#";                // 이름
            msg += age + "#";               // 별칭


            return msg;
        }
    }
}
