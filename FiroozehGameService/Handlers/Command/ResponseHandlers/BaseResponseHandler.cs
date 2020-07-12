﻿using System.Text;
using FiroozehGameService.Models.GSLive.Command;

namespace FiroozehGameService.Handlers.Command.ResponseHandlers
{
    internal abstract class BaseResponseHandler : IResponseHandler
    {
       
        public virtual void HandlePacket(Packet packet)
        {
            HandleResponse(packet);
        }

        protected abstract void HandleResponse(Packet packet); 
    }
}
