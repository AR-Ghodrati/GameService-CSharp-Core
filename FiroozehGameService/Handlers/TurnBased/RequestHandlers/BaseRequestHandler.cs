﻿using System;
using FiroozehGameService.Models.Command;

namespace FiroozehGameService.Handlers.TurnBased.RequestHandlers
{
    internal abstract class BaseRequestHandler : IRequestHandler
    {
        protected TurnBasedHandler TurnBasedHandler;

        public virtual Packet HandleAction(object payload)
        {
            if (CheckAction(payload))
                return DoAction(payload);
            throw new ArgumentException();
        }

        protected abstract bool CheckAction(object payload);

        protected abstract Packet DoAction(object payload);
    }
}