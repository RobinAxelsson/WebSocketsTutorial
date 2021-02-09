using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace u2b_Client_Server_Library
{
    public class JsonMessageProtocol : Protocol<JObject>
{
        protected override JObject Decode(byte[] message)
        {
            throw new NotImplementedException();
        }

        protected override byte[] EncodeBody<T>(T Message)
        {
            throw new NotImplementedException();
        }
    }
}
