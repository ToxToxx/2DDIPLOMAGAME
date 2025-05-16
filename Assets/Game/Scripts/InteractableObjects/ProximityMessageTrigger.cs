using System;
using UnityEngine;
using InGameUI;
using Player; 

namespace PlayerInteractionLogic
{
    public class ProximityMessageTrigger
    {
        private readonly FloatingMessageUI _floatingMessage;
        private readonly string _message;
        private bool _hasShown;

        public ProximityMessageTrigger(FloatingMessageUI floatingMessage, string message)
        {
            _floatingMessage = floatingMessage
                ?? throw new ArgumentNullException(nameof(floatingMessage));
            _message = message;
        }

        public void HandleEnter(Collider2D other)
        {
            if (_hasShown) return;
            if (other.TryGetComponent<PlayerMarker>(out _))
            {
                _hasShown = true;
                _floatingMessage.Show(_message);
            }
        }

        public void HandleExit(Collider2D other)
        {
            if (other.TryGetComponent<PlayerMarker>(out _))
            {
                _hasShown = false;
            }
        }
    }
}
