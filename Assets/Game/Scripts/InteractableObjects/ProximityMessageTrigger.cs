// Assets/Game/Scripts/Interaction/ProximityMessageTrigger.cs
using System;
using UnityEngine;
using InGameUI;
using Player; // для PlayerMarker

namespace PlayerInteractionLogic
{
    /// <summary>
    /// Чистый класс — хранит логику показа одного proximity-сообщения.
    /// Вызывается из MonoBehaviour-скриптов IInteractable.
    /// </summary>
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

        /// <summary>Вызывать из OnTriggerEnter2D</summary>
        public void HandleEnter(Collider2D other)
        {
            if (_hasShown) return;
            if (other.TryGetComponent<PlayerMarker>(out _))
            {
                _hasShown = true;
                _floatingMessage.Show(_message);
            }
        }

        /// <summary>Вызывать из OnTriggerExit2D</summary>
        public void HandleExit(Collider2D other)
        {
            if (other.TryGetComponent<PlayerMarker>(out _))
            {
                _hasShown = false;
            }
        }
    }
}
