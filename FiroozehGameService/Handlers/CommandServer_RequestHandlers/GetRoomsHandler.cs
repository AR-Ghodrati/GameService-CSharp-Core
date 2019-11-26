﻿using FiroozehGameService.Models.Command;
using FiroozehGameService.Models.Consts;
using Newtonsoft.Json;


namespace FiroozehGameService.Handlers.CommandHandlers
{
    internal class GetRoomsHandler : BaseHandler<GetRoomsHandler>
    {
        public static new string Signature
            => "AVAILABLEROOMS";

        public GetRoomsHandler(CommandHandler _handler)
            => this._commandHander = _handler;

        protected Packet DoAction(RoomDetail roomOptions)
            => new Packet(
                _commandHander.PlayerHash,
                Command.ActionGetRooms,
                JsonConvert.SerializeObject(roomOptions));

        protected override bool CheckAction(object payload)
            => payload.GetType() == typeof(RoomDetail);

        protected override Packet DoAction(object payload)
        {
            if (!CheckAction(payload)) throw new System.ArgumentException();
            return DoAction(payload as RoomDetail);
        }

    }
}
