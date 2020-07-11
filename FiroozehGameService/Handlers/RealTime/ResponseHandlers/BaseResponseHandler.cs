﻿using System.Text;
using FiroozehGameService.Models.Enums;
using FiroozehGameService.Models.GSLive.RT;

namespace FiroozehGameService.Handlers.RealTime.ResponseHandlers
{
    internal abstract class BaseResponseHandler : IResponseHandler
    {
        public virtual void HandlePacket(Packet packet, GProtocolSendType type)
        {
            HandleResponse(packet, type);
        }

        protected abstract void HandleResponse(Packet packet, GProtocolSendType type);

        protected static string GetStringFromBuffer(byte[] buffer)
        {
            return Encoding.UTF8.GetString(buffer);
        }
    }
}