// <copyright file="GSLiveRT.cs" company="Firoozeh Technology LTD">
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
using System.Text;
using System.Threading.Tasks;
using FiroozehGameService.Handlers.Command.RequestHandlers;
using FiroozehGameService.Handlers.RealTime;
using FiroozehGameService.Handlers.RealTime.RequestHandlers;
using FiroozehGameService.Models;
using FiroozehGameService.Models.Enums;
using FiroozehGameService.Models.Enums.GSLive;
using FiroozehGameService.Models.GSLive.Command;
using FiroozehGameService.Models.GSLive.RT;
using FiroozehGameService.Utils;

namespace FiroozehGameService.Core.GSLive
{
    /// <summary>
    ///     Represents Game Service Realtime MultiPlayer System
    /// </summary>
    public class GSLiveRT
    {
        private const string Tag = "GSLive-RealTime";

        internal GSLiveRT()
        {
        }

        /// <summary>
        ///     Create Room With Option Like : Name , Min , Max , Role , IsPrivate
        /// </summary>
        /// <param name="option">(NOTNULL)Create Room Option</param>
        public async Task CreateRoom(GSLiveOption.CreateRoomOption option)
        {
            if (GameService.IsGuest) throw new GameServiceException("This Function Not Working In Guest Mode").LogException<GSLiveRT>(DebugLocation.RealTime,"CreateRoom");
            if (option == null) throw new GameServiceException("option Cant Be Null").LogException<GSLiveRT>(DebugLocation.RealTime,"CreateRoom");
            option.GsLiveType = GSLiveType.RealTime;
            await GSLive.Handler.CommandHandler.RequestAsync(CreateRoomHandler.Signature, option);
        }


        /// <summary>
        ///     Create AutoMatch With Option Like :  Min , Max , Role
        /// </summary>
        /// <param name="option">(NOTNULL)AutoMatch Option</param>
        public async Task AutoMatch(GSLiveOption.AutoMatchOption option)
        {
            if (GameService.IsGuest) throw new GameServiceException("This Function Not Working In Guest Mode").LogException<GSLiveRT>(DebugLocation.RealTime,"AutoMatch");
            if (option == null) throw new GameServiceException("option Cant Be Null").LogException<GSLiveRT>(DebugLocation.RealTime,"AutoMatch");
            option.GsLiveType = GSLiveType.RealTime;
            await GSLive.Handler.CommandHandler.RequestAsync(AutoMatchHandler.Signature, option);
        }


        /// <summary>
        ///     Cancel Current AutoMatch
        /// </summary>
        public async Task CancelAutoMatch()
        {
            if (GameService.IsGuest) throw new GameServiceException("This Function Not Working In Guest Mode").LogException<GSLiveRT>(DebugLocation.RealTime,"CancelAutoMatch");
            await GSLive.Handler.CommandHandler.RequestAsync(CancelAutoMatchHandler.Signature);
        }

        /// <summary>
        ///     Join In Room With RoomID
        /// </summary>
        /// <param name="roomId">(NOTNULL)Room's id You Want To Join</param>
        /// <param name="extra">Specifies the Extra Data To Send to Other Clients</param>
        /// <param name="password">(NULLABLE)Specifies the Password if Room is Private</param>
        public async Task JoinRoom(string roomId, string extra = null,string password = null)
        {
            if (GameService.IsGuest) throw new GameServiceException("This Function Not Working In Guest Mode").LogException<GSLiveRT>(DebugLocation.RealTime,"JoinRoom");
            if (string.IsNullOrEmpty(roomId)) throw new GameServiceException("roomId Cant Be EmptyOrNull").LogException<GSLiveRT>(DebugLocation.RealTime,"JoinRoom");
            await GSLive.Handler.CommandHandler.RequestAsync(JoinRoomHandler.Signature,
                new RoomDetail {Id = roomId, Extra = extra , RoomPassword = password});
        }

        /// <summary>
        ///     Leave The Current Room
        /// </summary>
        public void LeaveRoom()
        {
            if (GameService.IsGuest) throw new GameServiceException("This Function Not Working In Guest Mode").LogException<GSLiveRT>(DebugLocation.RealTime,"LeaveRoom");
            if (GSLive.Handler.RealTimeHandler == null) throw new GameServiceException("You Must Create or Join Room First").LogException<GSLiveRT>(DebugLocation.RealTime,"LeaveRoom");
            
            GSLive.Handler.RealTimeHandler?.Request(LeaveRoomHandler.Signature, GProtocolSendType.Reliable,
                isCritical: true);
            GSLive.Handler.RealTimeHandler?.Dispose();
        }


        /// <summary>
        ///     Get Available Rooms According To Room's Role
        /// </summary>
        /// <param name="role">(NOTNULL)Room's Role </param>
        public async Task GetAvailableRooms(string role)
        {
            if (GameService.IsGuest) throw new GameServiceException("This Function Not Working In Guest Mode").LogException<GSLiveRT>(DebugLocation.RealTime,"GetAvailableRooms");
            if (string.IsNullOrEmpty(role)) throw new GameServiceException("role Cant Be EmptyOrNull").LogException<GSLiveRT>(DebugLocation.RealTime,"GetAvailableRooms");
           
            await GSLive.Handler.CommandHandler.RequestAsync(GetRoomsHandler.Signature,
                new RoomDetail {Role = role, GsLiveType = (int) GSLiveType.RealTime});
        }


        /// <summary>
        ///     Send A Data To All Players in Room.
        /// </summary>
        /// <param name="data">(NOTNULL) Data To BroadCast </param>
        /// <param name="sendType">Send Type </param>
        [Obsolete("This Method is Deprecated,Use SendPublicMessage(byte[] data, GProtocolSendType sendType) Instead")]
        public void SendPublicMessage(string data, GProtocolSendType sendType)
        {
            if (GameService.IsGuest) throw new GameServiceException("This Function Not Working In Guest Mode").LogException<GSLiveRT>(DebugLocation.RealTime,"SendPublicMessage");
            if (string.IsNullOrEmpty(data)) throw new GameServiceException("data Cant Be EmptyOrNull").LogException<GSLiveRT>(DebugLocation.RealTime,"SendPublicMessage");
            if (GSLive.Handler.RealTimeHandler == null) throw new GameServiceException("You Must Create or Join Room First").LogException<GSLiveRT>(DebugLocation.RealTime,"SendPublicMessage");
            
            GSLive.Handler.RealTimeHandler.Request(SendPublicMessageHandler.Signature, sendType,
                new DataPayload {Payload = Encoding.UTF8.GetBytes(data)});
        }


        /// <summary>
        ///     Send A Data To Specific Player in Room.
        /// </summary>
        /// <param name="receiverId">(NOTNULL) (Type : MemberID)Player's ID</param>
        /// <param name="data">(NOTNULL) Data for Send</param>
        [Obsolete("This Method is Deprecated,Use SendPrivateMessage(string receiverId, byte[] data) Instead")]
        public void SendPrivateMessage(string receiverId, string data)
        {
            if (GameService.IsGuest) throw new GameServiceException("This Function Not Working In Guest Mode").LogException<GSLiveRT>(DebugLocation.RealTime,"SendPrivateMessage");
            if (string.IsNullOrEmpty(receiverId) && string.IsNullOrEmpty(data)) throw new GameServiceException("data Or receiverId Cant Be EmptyOrNull").LogException<GSLiveRT>(DebugLocation.RealTime,"SendPrivateMessage");
            if (GSLive.Handler.RealTimeHandler == null) throw new GameServiceException("You Must Create or Join Room First").LogException<GSLiveRT>(DebugLocation.RealTime,"SendPrivateMessage");
           
            GSLive.Handler.RealTimeHandler.Request(SendPrivateMessageHandler.Signature, GProtocolSendType.Reliable,
                new DataPayload {ReceiverId = receiverId, Payload = Encoding.UTF8.GetBytes(data)});
        }


        /// <summary>
        ///     Send A Data To All Players in Room.
        /// </summary>
        /// <param name="data">(NOTNULL) Data To BroadCast </param>
        /// <param name="sendType">Send Type </param>
        public void SendPublicMessage(byte[] data, GProtocolSendType sendType)
        {
            if (GameService.IsGuest) throw new GameServiceException("This Function Not Working In Guest Mode").LogException<GSLiveRT>(DebugLocation.RealTime,"SendPublicMessage");
            if (data == null) throw new GameServiceException("data Cant Be Null").LogException<GSLiveRT>(DebugLocation.RealTime,"SendPublicMessage");
            if (GSLive.Handler.RealTimeHandler == null) throw new GameServiceException("You Must Create or Join Room First").LogException<GSLiveRT>(DebugLocation.RealTime,"SendPublicMessage");
          
            GSLive.Handler.RealTimeHandler.Request(SendPublicMessageHandler.Signature, sendType,
                new DataPayload {Payload = data});
        }


        /// <summary>
        ///     Send A Data To Specific Player in Room.
        /// </summary>
        /// <param name="receiverId">(NOTNULL) (Type : MemberID)Player's ID</param>
        /// <param name="data">(NOTNULL) Data for Send</param>
        public void SendPrivateMessage(string receiverId, byte[] data)
        {
            if (GameService.IsGuest) throw new GameServiceException("This Function Not Working In Guest Mode").LogException<GSLiveRT>(DebugLocation.RealTime,"SendPrivateMessage");
            if (string.IsNullOrEmpty(receiverId) && data == null) throw new GameServiceException("data Or receiverId Cant Be EmptyOrNull").LogException<GSLiveRT>(DebugLocation.RealTime,"SendPrivateMessage");
            if (GSLive.Handler.RealTimeHandler == null) throw new GameServiceException("You Must Create or Join Room First").LogException<GSLiveRT>(DebugLocation.RealTime,"SendPrivateMessage");
           
            GSLive.Handler.RealTimeHandler.Request(SendPrivateMessageHandler.Signature, GProtocolSendType.Reliable,
                new DataPayload {ReceiverId = receiverId, Payload = data});
        }


        /// <summary>
        ///     Get Room Members Details
        /// </summary>
        public void GetRoomMembersDetail()
        {
            if (GameService.IsGuest) throw new GameServiceException("This Function Not Working In Guest Mode").LogException<GSLiveRT>(DebugLocation.RealTime,"GetRoomMembersDetail");
            if (GSLive.Handler.RealTimeHandler == null) throw new GameServiceException("You Must Create or Join Room First").LogException<GSLiveRT>(DebugLocation.RealTime,"GetRoomMembersDetail");
          
            GSLive.Handler.RealTimeHandler.Request(GetMemberHandler.Signature, GProtocolSendType.Reliable,
                isCritical: true);
        }


        /// <summary>
        ///     Get Your Invite Inbox
        /// </summary>
        public async Task GetInviteInbox()
        {
            if (GameService.IsGuest) throw new GameServiceException("This Function Not Working In Guest Mode").LogException<GSLiveRT>(DebugLocation.RealTime,"GetInviteInbox");
           
            await GSLive.Handler.CommandHandler.RequestAsync(InviteListHandler.Signature,
                new RoomDetail {GsLiveType = (int) GSLiveType.RealTime});
        }


        /// <summary>
        ///     Invite a Specific Player To Specific Room
        /// </summary>
        /// <param name="roomId">(NOTNULL) (Type : RoomID)Room's ID</param>
        /// <param name="userId">(NOTNULL) (Type : UserID)User's ID</param>
        public async Task InviteUser(string roomId, string userId)
        {
            if (GameService.IsGuest) throw new GameServiceException("This Function Not Working In Guest Mode").LogException<GSLiveRT>(DebugLocation.RealTime,"InviteUser");
            if (string.IsNullOrEmpty(roomId) && string.IsNullOrEmpty(userId)) throw new GameServiceException("roomId Or userId Cant Be EmptyOrNull").LogException<GSLiveRT>(DebugLocation.RealTime,"InviteUser");
         
            await GSLive.Handler.CommandHandler.RequestAsync(InviteUserHandler.Signature,
                new RoomDetail {UserOrMemberId = userId, Id = roomId, GsLiveType = (int) GSLiveType.RealTime});
        }


        /// <summary>
        ///     Accept a Specific Invite With Invite ID
        ///     Note: After accepting the invitation, you will be automatically entered into the game room
        /// </summary>
        /// <param name="inviteId">(NOTNULL) (Type : InviteID) Invite's ID</param>
        /// <param name="extra">Specifies the Extra Data To Send to Other Clients</param>
        public async Task AcceptInvite(string inviteId, string extra = null)
        {
            if (GameService.IsGuest) throw new GameServiceException("This Function Not Working In Guest Mode").LogException<GSLiveRT>(DebugLocation.RealTime,"AcceptInvite");
            if (string.IsNullOrEmpty(inviteId)) throw new GameServiceException("inviteId Cant Be EmptyOrNull").LogException<GSLiveRT>(DebugLocation.RealTime,"AcceptInvite");
            
            await GSLive.Handler.CommandHandler.RequestAsync(AcceptInviteHandler.Signature,
                new RoomDetail {Invite = inviteId, Extra = extra, GsLiveType = (int) GSLiveType.RealTime});
        }

        /// <summary>
        ///     Find All Member With Specific Query
        /// </summary>
        /// <param name="query">(NOTNULL) Query </param>
        /// <param name="limit">(Max = 15) The Result Limits</param>
        public async Task FindMember(string query, int limit = 10)
        {
            if (GameService.IsGuest) throw new GameServiceException("This Function Not Working In Guest Mode").LogException<GSLiveRT>(DebugLocation.RealTime,"FindMember");
            if (string.IsNullOrEmpty(query)) throw new GameServiceException("query Cant Be EmptyOrNull").LogException<GSLiveRT>(DebugLocation.RealTime,"FindMember");
            if (limit <= 0 || limit > 15) throw new GameServiceException("invalid Limit Value").LogException<GSLiveRT>(DebugLocation.RealTime,"FindMember");
          
            await GSLive.Handler.CommandHandler.RequestAsync(FindMemberHandler.Signature,
                new RoomDetail {Max = limit, UserOrMemberId = query});
        }

        /// <summary>
        ///     Get The Ping
        /// </summary>
        public short GetPing()
        {
            if (GameService.IsGuest) throw new GameServiceException("This Function Not Working In Guest Mode").LogException<GSLiveRT>(DebugLocation.RealTime,"GetPing");
            if (GSLive.Handler.RealTimeHandler == null) throw new GameServiceException("You Must Create or Join Room First").LogException<GSLiveRT>(DebugLocation.RealTime,"GetPing");
           
            return RealTimeHandler.GetPing();
        }


        internal static void SendEvent(byte[] caller, byte[] data, GProtocolSendType sendType)
        {
            if (GameService.IsGuest) throw new GameServiceException("This Function Not Working In Guest Mode").LogException<GSLiveRT>(DebugLocation.RealTime,"SendEvent");
            if (caller == null || data == null) throw new GameServiceException("caller or data Cant Be Null").LogException<GSLiveRT>(DebugLocation.RealTime,"SendEvent");
            if (GSLive.Handler.RealTimeHandler == null) throw new GameServiceException("You Must Create or Join Room First").LogException<GSLiveRT>(DebugLocation.RealTime,"SendEvent");

            GSLive.Handler.RealTimeHandler.Request(NewEventHandler.Signature, sendType,
                new DataPayload {Payload = data, ExtraData = caller}, true);
        }


        internal static void SendObserver(byte[] caller, byte[] data)
        {
            if (GameService.IsGuest) throw new GameServiceException("This Function Not Working In Guest Mode").LogException<GSLiveRT>(DebugLocation.RealTime,"SendObserver");
            if (data == null) throw new GameServiceException("caller or data Cant Be Null").LogException<GSLiveRT>(DebugLocation.RealTime,"SendObserver");
            if (GSLive.Handler.RealTimeHandler == null) throw new GameServiceException("You Must Create or Join Room First").LogException<GSLiveRT>(DebugLocation.RealTime,"SendObserver");

            ObserverCompacterUtil.AddToQueue(new DataPayload {Payload = data, ExtraData = caller});
        }
    }
}