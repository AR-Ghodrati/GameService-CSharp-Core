// <copyright file="Vote.cs" company="Firoozeh Technology LTD">
// Copyright (C) 2019 Firoozeh Technology LTD. All Rights Reserved.
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//    limitations under the License.
// </copyright>

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

/**
* @author Alireza Ghodrati
*/


namespace FiroozehGameService.Models.GSLive.TB
{
    /// <summary>
    ///     Represents Finish Data Model In GameService TurnBased MultiPlayer System
    /// </summary>
    [Serializable]
    public class Vote
    {
        /// <summary>
        ///     Gets the Outcomes sent from the player.
        ///     Call From Other Player With this Function <see cref="Vote" />
        ///     (Type : Dictionary(MemberID,Outcome))
        /// </summary>
        /// <value>the Outcomes sent from the player</value>
        [JsonProperty("1")] public Dictionary<string, Outcome> Outcomes;

        /// <summary>
        ///     Gets Member Data That Submit This Vote
        /// </summary>
        /// <value>Member Data That Submit This Vote</value>
        [JsonProperty("0")] public Member Submitter;
    }
}