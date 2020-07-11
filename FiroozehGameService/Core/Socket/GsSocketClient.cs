﻿using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FiroozehGameService.Core.Socket.PacketHelper;
using FiroozehGameService.Models.EventArgs;
using FiroozehGameService.Models.GSLive.Command;

namespace FiroozehGameService.Core.Socket
{
    internal abstract class GsSocketClient
    {
        public event EventHandler<SocketDataReceived> DataReceived;
        public event EventHandler<ErrorArg> Error;


        protected void OnDataReceived(SocketDataReceived arg)
        {
            DataReceived?.Invoke(this, arg);
        }

        protected void OnClosed(ErrorArg errorArg)
        {
            IsAvailable = false;
            DataBuilder?.Clear();
            Error?.Invoke(this, errorArg);
        }

        internal abstract bool Init();

        internal abstract void Send(Packet packet);

        internal abstract Task SendAsync(Packet packet);

        internal abstract Task StartReceiving();

        internal abstract void StopReceiving();

        #region Fields

        private const int BufferCapacity = 1024 * 128;
        protected Area Endpoint;
        protected readonly StringBuilder DataBuilder = new StringBuilder();
        protected CancellationTokenSource OperationCancellationToken;
        public bool IsAvailable;


        protected readonly byte[] Buffer = new byte[BufferCapacity];
        protected const int BufferOffset = 0;
        protected int BufferReceivedBytes = 0;
        protected readonly IValidator PacketValidator = new JsonDataValidator();
        protected readonly IDeserializer PacketDeserializer = new PacketDeserializer();
        protected readonly ISerializer PacketSerializer = new PacketSerializer();

        #endregion
    }
}