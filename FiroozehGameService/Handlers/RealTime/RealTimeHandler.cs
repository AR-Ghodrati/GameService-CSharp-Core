using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using FiroozehGameService.Core;
using FiroozehGameService.Core.Socket;
using FiroozehGameService.Handlers.RealTime.RequestHandlers;
using FiroozehGameService.Handlers.RealTime.ResponseHandlers;
using FiroozehGameService.Models.Command;
using FiroozehGameService.Models.Enums;
using FiroozehGameService.Models.Enums.GSLive;
using FiroozehGameService.Models.EventArgs;
using FiroozehGameService.Models.GSLive;
using FiroozehGameService.Utils;
using Newtonsoft.Json;

namespace FiroozehGameService.Handlers.RealTime
{
    internal class RealTimeHandler : IDisposable
    {
        #region RTHandlerRegion
        private static GsUdpClient _udpClient;
        public static Room CurrentRoom;
        
        private readonly GsLiveSystemObserver _observer;
        private readonly CancellationTokenSource _cancellationToken;
        public static string PlayerHash { private set; get; }
        public static string UserToken => GameService.UserToken;
        public static bool IsAvailable => _udpClient?.IsAvailable ?? false;
        
        private readonly Dictionary<int, IResponseHandler> _responseHandlers =
            new Dictionary<int, IResponseHandler>();
        private readonly Dictionary<string, IRequestHandler> _requestHandlers =
            new Dictionary<string, IRequestHandler>();
        
        #endregion
        
        internal RealTimeHandler(StartPayload payload)
        {
            CurrentRoom = payload.Room;
            _udpClient = new GsUdpClient(payload.Area);
            _udpClient.DataReceived += OnDataReceived;
            _udpClient.Error += OnError;
            _cancellationToken = new CancellationTokenSource();
            _observer = new GsLiveSystemObserver(GSLiveType.RealTime);

            
            // Set Internal Event Handlers
            CoreEventHandlers.Authorized += OnAuth;
            
            InitRequestMessageHandlers();
            InitResponseMessageHandlers();
        }

        
        private static void OnAuth(object sender, string playerHash)
        {
            if (sender.GetType() == typeof(AuthResponseHandler))
                PlayerHash = playerHash;
        }

        private void InitRequestMessageHandlers()
        {
            var baseInterface = typeof(IRequestHandler);
            var subclassTypes = Assembly
                .GetAssembly(baseInterface)
                .GetTypes()
                .Where(t => t.GetInterfaces().Contains(baseInterface) && !t.IsInterface && !t.IsAbstract);

            foreach (var type in subclassTypes)
            {
                var p = (string)type.GetProperty("Signature", BindingFlags.Public | BindingFlags.Static).GetValue(null);
                _requestHandlers.Add(p, (IRequestHandler)Activator.CreateInstance(type));
            }
        }
        private void InitResponseMessageHandlers()
        {
            var baseInterface = typeof(IResponseHandler);
            var subclassTypes = Assembly
                .GetAssembly(baseInterface)
                .GetTypes()
                .Where(t => t.GetInterfaces().Contains(baseInterface) && !t.IsInterface && !t.IsAbstract);

            foreach (var type in subclassTypes)
            {
                var p = (int)type.GetProperty("ActionCommand", BindingFlags.Public | BindingFlags.Static).GetValue(null);
                _responseHandlers.Add(p, (IResponseHandler)Activator.CreateInstance(type));
            }
        }
     

        internal void Request(string handlerName, GProtocolSendType type, object payload = null)
            => Send(_requestHandlers[handlerName]?.HandleAction(payload),type);
        
       
        internal void Init()
        {
            _udpClient.Init();
            Request(AuthorizationHandler.Signature,GProtocolSendType.Reliable);
        }
        
        
        private void Send(Packet packet,GProtocolSendType type)
        {
            if (!_observer.Increase()) return;
            var json = JsonConvert.SerializeObject(packet , new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            var data = Encoding.UTF8.GetBytes(json);          
            _udpClient.Send(data,type);
        }
        
              
        private static void OnError(object sender, ErrorArg e)
        {
            // TODO Connect Again??
        }

        private void OnDataReceived(object sender, SocketDataReceived e)
        {
            var packet = JsonConvert.DeserializeObject<Packet>(e.Data);
            _responseHandlers.GetValue(packet.Action)?.HandlePacket(packet,e.Type);           
        }
        
        public void Dispose()
        {
            _udpClient?.StopReceiving();
            _observer.Dispose();
            _cancellationToken.Cancel(true);
            CoreEventHandlers.Dispose?.Invoke(this,null);
        }
        
    }
}