// <copyright file="ChatEventHandlers.cs" company="Firoozeh Technology LTD">
// Copyright (C) 2020 Firoozeh Technology LTD. All Rights Reserved.
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
using FiroozehGameService.Models.GSLive;
using FiroozehGameService.Models.GSLive.Chat;
using FiroozehGameService.Models.GSLive.Providers;

/**
* @author Alireza Ghodrati
*/

namespace FiroozehGameService.Handlers
{
    /// <summary>
    ///     Represents ChatEventHandlers
    /// </summary>
    public class ChatEventHandlers
    {
        /// <summary>
        ///     Calls When New Chat Received
        ///     <see cref="GsLiveChatProvider.SendChannelMessage" />
        /// </summary>
        public static EventHandler<Chat> OnChatReceived;

        /// <summary>
        ///     Calls When Current Player Subscribe Channel
        ///     This Event Handler Called By Following Function :
        ///     <see cref="GsLiveChatProvider.SubscribeChannel" />
        /// </summary>
        public static EventHandler<string> OnSubscribeChannel;


        /// <summary>
        ///     Calls When Current Player UnSubscribe Channel
        ///     This Event Handler Called By Following Function :
        ///     <see cref="GsLiveChatProvider.UnSubscribeChannel" />
        /// </summary>
        public static EventHandler<string> OnUnSubscribeChannel;


        /// <summary>
        ///     Calls When a Chat Removed
        ///     This Event Handler Called By Following Functions :
        ///     <see cref="GsLiveChatProvider.RemoveChannelMessage" />
        ///     <see cref="GsLiveChatProvider.RemovePrivateMessage" />
        /// </summary>
        public static EventHandler<Chat> OnChatRemoved;


        /// <summary>
        ///     Calls When all Chats belong a Member Removed
        ///     NOTE : Call With Member Id that removed all chats belong to it
        ///     This Event Handler Called By Following Function :
        ///     <see cref="GsLiveChatProvider.RemoveMemberMessages" />
        /// </summary>
        public static EventHandler<string> OnMemberChatsRemoved;


        /// <summary>
        ///     Calls When Current Player Get Channels Subscribed List
        ///     This Event Handler Called By Following Function :
        ///     <see cref="GsLiveChatProvider.GetChannelsSubscribed" />
        /// </summary>
        public static EventHandler<List<string>> ChannelsSubscribed;


        /// <summary>
        ///     Calls When Current Player Get Channel Members List
        ///     This Event Handler Called By Following Function :
        ///     <see cref="GsLiveChatProvider.GetChannelMembers" />
        /// </summary>
        public static EventHandler<List<Member>> ChannelMembers;


        /// <summary>
        ///     Calls When Current Player Get Channel Recent Messages
        ///     This Event Handler Called By Following Function :
        ///     <see cref="GsLiveChatProvider.GetChannelRecentMessages" />
        /// </summary>
        public static EventHandler<List<Chat>> ChannelRecentMessages;


        /// <summary>
        ///     Calls When Current Player Get Pending Messages
        ///     Pending Messages Saved when the Current Player is offline
        ///     You Can Get Them With this Function
        ///     This Event Handler Called By Following Function :
        ///     <see cref="GsLiveChatProvider.GetChannelRecentMessages" />
        /// </summary>
        public static EventHandler<List<Chat>> PendingMessages;
    }
}