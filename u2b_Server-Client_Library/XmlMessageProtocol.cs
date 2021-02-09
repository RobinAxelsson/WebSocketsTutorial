using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace u2b_Client_Server_Library
{
    public class XmlMessageProtocol : Protocol<XDocument>
    {
        protected override XDocument Decode(byte[] message)
        {
            var xmlData = Encoding.UTF8.GetString(message);
            var xmlReader = XmlReader.Create(new StringReader(xmlData), new XmlReaderSettings { DtdProcessing = DtdProcessing.Ignore });
            return XDocument.Load(xmlReader);
        }

        protected override byte[] EncodeBody<T>(T Message)
        {
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            var xs = new XmlSerializer(typeof(T));
            xs.Serialize(sw, Message);
            return Encoding.UTF8.GetBytes(sb.ToString());
        }
    }
}
