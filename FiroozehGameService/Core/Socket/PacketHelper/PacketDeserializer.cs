using System;
using System.Linq;
using FiroozehGameService.Utils;
using GameServiceHelper.Utils;

namespace FiroozehGameService.Core.Socket.PacketHelper
{
    internal class PacketDeserializer : IDeserializer
    {
        public string Deserialize(byte[] buffer, int offset, int receivedBytes, string k)
        {
            try
            {
                LogUtil.Log(this,"PacketDeserializer Rec :" + receivedBytes +" Bytes");
                var seg = new ArraySegment<byte>(buffer,offset,receivedBytes);
                return Serializer.Deserialize(seg.ToArray(), k);
            }
            catch (Exception e)
            {
                LogUtil.LogError(this,"PacketDeserializer Err : " + e.Message);
                return null;
            }
           
        }
    }
}