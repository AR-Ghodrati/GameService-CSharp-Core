﻿using FiroozehGameService.Models.Consts;
using FiroozehGameService.Models.Enums.GSLive;
using FiroozehGameService.Models.GSLive;
using FiroozehGameService.Models.GSLive.Command;
using Newtonsoft.Json;

namespace FiroozehGameService.Handlers.TurnBased.ResponseHandlers
{
    internal class JoinRoomResponseHandler : BaseResponseHandler
    {
        public static int ActionCommand => TB.OnJoin;

        protected override void HandleResponse(Packet packet)
        {
            var joinData = JsonConvert.DeserializeObject<JoinData>(GetStringFromBuffer(packet.Data));
            TurnBasedEventHandlers.JoinedRoom?.Invoke(this, new JoinEvent
            {
                Type = GSLiveType.TurnBased,
                JoinData = joinData
            });
        }
    }
}