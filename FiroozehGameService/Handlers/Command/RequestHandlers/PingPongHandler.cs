﻿using System;
using FiroozehGameService.Models.GSLive.Command;

namespace FiroozehGameService.Handlers.Command.RequestHandlers
{
    internal class PingPongHandler : BaseRequestHandler
    {
        public static string Signature
            => "PING_PONG";

        private static Packet DoAction()
        {
            return new Packet(
                CommandHandler.PlayerHash,
                Models.Consts.Command.ActionPing);
        }

        protected override bool CheckAction(object payload)
        {
            return true;
        }

        protected override Packet DoAction(object payload)
        {
            if (!CheckAction(payload)) throw new ArgumentException();
            return DoAction();
        }
    }
}