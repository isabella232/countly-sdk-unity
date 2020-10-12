using System.Collections.Generic;
using Plugins.Countly.Models;
using UnityEngine;

namespace Notifications
{
    public class NotificationsCallbackService 
    {
        CountlyConfigModel _config;
        private List<INotificationListener> _listeners;
        internal NotificationsCallbackService(CountlyConfigModel config)
        {
            _config = config;
            _listeners = new List<INotificationListener>();
        }

        public void AddListener(INotificationListener listener)
        {
            if (_listeners.Contains(listener)) {
                return;
            }

            _listeners.Add(listener);

            if (_config.EnableConsoleErrorLogging)
            {
                Debug.Log("[Countly NotificationsCallbackService] AddListener: " + listener);
            }
        }

        public void RemoveListener(INotificationListener listener)
        {
            _listeners.Remove(listener);

            if (_config.EnableConsoleErrorLogging)
            {
                Debug.Log("[Countly NotificationsCallbackService] RemoveListener: " + listener);
            }
        }

        public void NotifyOnNotificationReceived(string data)
        {
            foreach (INotificationListener listener in _listeners)
            {
                if (listener != null)
                {
                    listener.OnNotificationReceived(data);
                }
            }

            if (_config.EnableConsoleErrorLogging)
            {
                Debug.Log("[Countly NotificationsCallbackService] SendMessageToListeners: " + data);
            }
        }

        public void NotifyOnNotificationClicked(string data, int index)
        {
            foreach (INotificationListener listener in _listeners)
            {
                if (listener != null)
                {
                    listener.OnNotificationClicked(data, index);
                }
            }

            if (_config.EnableConsoleErrorLogging)
            {
                Debug.Log("[Countly NotificationsCallbackService] SendMessageToListeners: " + data);
            }
        }
    }

    public interface INotificationListener
    {
        void OnNotificationReceived(string message);
        void OnNotificationClicked(string message, int index);
    }
}