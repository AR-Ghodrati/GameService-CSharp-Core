// <copyright file="DataPayload.cs" company="Firoozeh Technology LTD">
// Copyright (C) 2019 Firoozeh Technology LTD. All Rights Reserved.
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//    limitations under the License.
// </copyright>


/**
* @author Alireza Ghodrati
*/


using System;
using System.Text;
using FiroozehGameService.Utils.Serializer.Utils.IO;

namespace FiroozehGameService.Models.GSLive.RT
{
    [Serializable]
    internal class DataPayload : Payload
    {
        private int _extraLen;

        private int _payloadLen;
        private int _receiverLen;
        private int _senderLen;
        internal byte[] ExtraData;
        internal byte[] Payload;
        internal string ReceiverId;
        internal string SenderId;

        public DataPayload(string senderId = null, string receiverId = null, byte[] payload = null,
            byte[] extraData = null)
        {
            ExtraData = extraData;
            SenderId = senderId;
            ReceiverId = receiverId;
            Payload = payload;
        }

        public DataPayload(byte[] buffer)
        {
            Deserialize(buffer);
        }


        internal void Deserialize(byte[] buff)
        {
            using (var packetWriter = ByteArrayReaderWriter.Get(buff))
            {
                var haveSender = packetWriter.ReadByte();
                var haveReceiver = packetWriter.ReadByte();
                var havePayload = packetWriter.ReadByte();
                var haveExtra = packetWriter.ReadByte();


                if (haveSender == 0x1) _senderLen = packetWriter.ReadByte();
                if (haveReceiver == 0x1) _receiverLen = packetWriter.ReadByte();
                if (havePayload == 0x1) _payloadLen = packetWriter.ReadUInt16();
                if (haveExtra == 0x1) _extraLen = packetWriter.ReadUInt16();


                if (haveSender == 0x1) SenderId = ConvertToString(packetWriter.ReadBytes(_senderLen));
                if (haveReceiver == 0x1) ReceiverId = ConvertToString(packetWriter.ReadBytes(_receiverLen));
                if (havePayload == 0x1) Payload = packetWriter.ReadBytes(_payloadLen);
                if (haveExtra == 0x1) ExtraData = packetWriter.ReadBytes(_extraLen);
            }
        }

        internal byte[] Serialize()
        {
            byte havePayload = 0x0, haveExtra = 0x0, haveSender = 0x0, haveReceiver = 0x0;
            short prefixLen = 4 * sizeof(byte);


            if (Payload != null)
            {
                havePayload = 0x1;
                _payloadLen = Payload.Length;
                prefixLen += sizeof(ushort);
            }

            if (ExtraData != null)
            {
                haveExtra = 0x1;
                _extraLen = ExtraData.Length;
                prefixLen += sizeof(ushort);
            }


            if (SenderId != null)
            {
                haveSender = 0x1;
                _senderLen = SenderId.Length;
                prefixLen += sizeof(byte);
            }


            if (ReceiverId != null)
            {
                haveReceiver = 0x1;
                _receiverLen = ReceiverId.Length;
                prefixLen += sizeof(byte);
            }


            var packetBuffer = BufferPool.GetBuffer(BufferSize(prefixLen));
            using (var packetWriter = ByteArrayReaderWriter.Get(packetBuffer))
            {
                // header Segment
                packetWriter.Write(haveSender);
                packetWriter.Write(haveReceiver);
                packetWriter.Write(havePayload);
                packetWriter.Write(haveExtra);

                if (haveSender == 0x1) packetWriter.Write((byte) _senderLen);
                if (haveReceiver == 0x1) packetWriter.Write((byte) _receiverLen);
                if (havePayload == 0x1) packetWriter.Write((ushort) _payloadLen);
                if (haveExtra == 0x1) packetWriter.Write((ushort) _extraLen);

                // data Segment
                if (haveSender == 0x1) packetWriter.Write(ConvertToBytes(SenderId));
                if (haveReceiver == 0x1) packetWriter.Write(ConvertToBytes(ReceiverId));
                if (havePayload == 0x1) packetWriter.Write(Payload);
                if (haveExtra == 0x1) packetWriter.Write(ExtraData);
            }

            return packetBuffer;
        }


        internal int BufferSize(short prefixLen)
        {
            return prefixLen + _senderLen + _receiverLen + _payloadLen + _extraLen;
        }

        public override string ToString()
        {
            return "DataPayload{ReceiverID='" + ReceiverId + '\'' +
                   ", SenderID='" + SenderId + '\'' +
                   ", Payload='" + Payload?.Length + '\'' +
                   '}';
        }

        internal string ConvertToString(byte[] data)
        {
            return Encoding.UTF8.GetString(data);
        }

        internal byte[] ConvertToBytes(string data)
        {
            return Encoding.UTF8.GetBytes(data);
        }
    }
}