﻿
using FiroozehGameService.Core;
using FiroozehGameService.Models;
using FiroozehGameService.Models.Consts;
using FiroozehGameService.Models.GSLive.RT;
using Newtonsoft.Json;

namespace FiroozehGameService.Handlers.RealTime.RequestHandlers
{
    internal class PingPongHandler : BaseRequestHandler
    {
        public static string Signature
            => "PING_PONG";

        public PingPongHandler(RealTimeHandler handler)
            => RealTimeHandler = handler;

        private static Packet DoAction()
        {
            if (!RealTimeHandler.IsAvailable) throw new GameServiceException("GSLiveRealTime Not Available yet");
            return new Packet(RT.ActionPingPong
                ,JsonConvert.SerializeObject(new PingPongPayload(GameService.CurrentGame?._Id,RealTimeHandler.PlayerHash)));         
        }

        protected override bool CheckAction(object payload)
           => true;

        protected override Packet DoAction(object payload)
        {
            if (!CheckAction(payload)) throw new System.ArgumentException();
            return DoAction();
        }
    }

}