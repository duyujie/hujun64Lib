using System;
using com.hujun64.type;

namespace com.hujun64.po
{
    /// <summary>
    ///Client 的摘要说明
    /// </summary>
    public class Client 
    {
        public string id;
        public string client_name;
        public string mobile_code;
        public string mobile_province;
        public ClientStatus status;
        public DateTime addtime;

        public Client()
        {
        }

        public Client(string client_name,string mobile_code)
        {
            this.client_name = client_name;
            this.mobile_code = mobile_code;
            this.status = ClientStatus.SMS_ENABLED;

        }
    }
}