using System;
using System.Collections;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;

namespace CommonTypes
{
    public class Helper
    {
        public static IChannel GetChannel(int tcpPort)
        {
            BinaryServerFormatterSinkProvider serverProv =
                new BinaryServerFormatterSinkProvider();

            IDictionary propBag = new Hashtable();
            propBag["port"] = tcpPort;
            propBag["typeFilterLevel"] = TypeFilterLevel.Full;
            propBag["name"] = Guid.NewGuid().ToString();

            return new TcpChannel(
                propBag, null, serverProv);
        }
    }
}
