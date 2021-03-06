// <copyright file="Game.cs" company="Firoozeh Technology LTD">
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

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FiroozehGameService.Models.Internal
{
    [Serializable]
    internal class InternalGame
    {
        [JsonProperty("_id")] public string _Id;

        [JsonProperty("category")] public string Category;

        [JsonProperty("coverURL")] public string CoverUrl;

        [JsonProperty("created")] public int Created;

        [JsonProperty("explane")] public string Explane;
        
        [JsonProperty("id")] public string Id;

        [JsonProperty("installed")] public int Installed;

        [JsonProperty("link")] public string Link;

        [JsonProperty("logoURL")] public string LogoUrl;

        [JsonProperty("name")] public string Name;

        [JsonProperty("package")] public string Package;

        [JsonProperty("pictures")] public List<string> Pictures;

        [JsonProperty("platforms")] public List<Platform> Platforms;

        [JsonProperty("publisher")] public Publisher Publisher;

        [JsonProperty("status")] public int Status;
    }
}