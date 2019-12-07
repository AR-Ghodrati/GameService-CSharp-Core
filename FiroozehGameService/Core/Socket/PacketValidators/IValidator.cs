﻿using System.Collections.Generic;
using System.Text;

namespace FiroozehGameService.Core.Socket.PacketValidators
{
    internal interface IValidator
    {
        IEnumerable<string> ValidateDataAndReturn(string data);

        bool ValidateBinaryData(byte[] buffer, int offset, int length);
    }
}
