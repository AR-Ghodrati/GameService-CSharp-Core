﻿using System;
using FiroozehGameService.Models.GSLive.Command;
using Newtonsoft.Json;

namespace FiroozehGameService.Handlers.Command.RequestHandlers
{
    internal class JoinRoomHandler : BaseRequestHandler
    {
        public static string Signature
            => "JOIN_ROOM";

        private static Packet DoAction(RoomDetail room)
        {
            return new Packet(
                CommandHandler.PlayerHash,
                Models.Consts.Command.ActionJoinRoom,
                GetBuffer(JsonConvert.SerializeObject(room
                    , new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        DefaultValueHandling = DefaultValueHandling.Ignore
                    }))
            );
        }

        protected override bool CheckAction(object payload)
        {
            return payload.GetType() == typeof(RoomDetail);
        }

        protected override Packet DoAction(object payload)
        {
            if (!CheckAction(payload)) throw new ArgumentException();
            return DoAction(payload as RoomDetail);
        }
    }
}