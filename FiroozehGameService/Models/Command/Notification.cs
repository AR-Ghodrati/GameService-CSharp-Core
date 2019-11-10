// <copyright file="Notification.cs" company="Firoozeh Technology LTD">
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




/**
* @author Alireza Ghodrati
*/

using FiroozehGameService.Models.Enums.Command;
using Newtonsoft.Json;

namespace FiroozehGameService.Models.Command
{
    public class Notification : System.EventArgs
    {
        [JsonProperty("1")]
        public string Title;

        [JsonProperty("2")]
        public string Description;

        [JsonProperty("4")]
        public TapActionType TapActionType = TapActionType.CloseNotification;

        [JsonProperty("5")]
        public NotificationTapAction TapAction;
        
        [JsonProperty("6")]
        public string JsonData;

    }
}