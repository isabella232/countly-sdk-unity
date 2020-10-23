using Plugins.Countly.Helpers;
using Plugins.Countly.Models;
using Plugins.Countly.Services;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Notifications.Impls.iOs
{
    public class IOsNotificationsService : INotificationsService
    {
        private readonly CountlyConfigModel _config;
        private readonly Action<IEnumerator> _startCoroutine;
        private readonly IEventCountlyService _eventCountlyService;

        private readonly IOSBridage _bridge;
        private const string BridgeName = "[iOS] Bridge";

        private Action<string> _OnNotificationReceiveResult;
        private Action<string, int> _OnNotificationClickResult;


        public IOsNotificationsService(CountlyConfigModel config, Action<IEnumerator> startCoroutine, IEventCountlyService eventCountlyService)
        {
            _config = config;
            _startCoroutine = startCoroutine;
            _eventCountlyService = eventCountlyService;

            var gameObject = new GameObject(BridgeName);
            _bridge = gameObject.AddComponent<IOSBridage>();
            _bridge.Config = _config;

        }

        public void GetToken(Action<string> result)
        {
            // _startCoroutine.Invoke(RequestAuthorization(result));
            _bridge.ListenTokenResult(result);
            _bridge.GetToken();
        }

        public async Task<CountlyResponse> ReportPushActionAsync()
        {
            string mesageId = _bridge.MessageId;
            string identifier = _bridge.ButtonIndex;

            if (_bridge.MessageId != null)
            {
                var segment =
                    new Plugins.Countly.Services.Impls.Actual.PushCountlyService.PushActionSegment
                    {
                        MessageID = mesageId,
                        Identifier = identifier
                    };

                if (_config.EnableConsoleErrorLogging)
                {
                    Debug.Log("[Countly] ReportPushActionAsync key: " + CountlyEventModel.PushActionEvent + ", segments: " + segment);
                }

                await _eventCountlyService.ReportCustomEventAsync(
                    CountlyEventModel.PushActionEvent, segment.ToDictionary());
            }

            _bridge.MessageId = null;
            _bridge.ButtonIndex = null;

            return new CountlyResponse
            {
                IsSuccess = true,
            };
        }

        public void OnNotificationClicked(Action<string, int> result)
        {
            if (_config.EnableConsoleErrorLogging)
            {
                Debug.Log("[Countly] OnNotificationClicked register");
            }
            
            _bridge.ListenClickResult(result);

        }

        public void OnNotificationReceived(Action<string> result)
        {
            if (_config.EnableConsoleErrorLogging)
            {
                Debug.Log("[Countly] OnNotificationReceived register");
            }

            _bridge.ListenReceiveResult(result);
           
        }

       
    }
}