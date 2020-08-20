using System;
using FiroozehGameService.Models.GSLive.Command;

namespace FiroozehGameService.Handlers.Command.RequestHandlers
{
    internal class CancelAutoMatchHandler : BaseRequestHandler
    {
        public static string Signature =>
            "CANCEL_AUTO_MATCH";


        private static Packet DoAction()
        {
            return new Packet(
                CommandHandler.PlayerHash,
                Models.Consts.Command.ActionCancelAutoMatch);
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