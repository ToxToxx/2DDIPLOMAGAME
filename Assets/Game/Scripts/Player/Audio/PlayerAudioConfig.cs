using UnityEngine;

namespace PlayerAudio
{

    [CreateAssetMenu(menuName = "Game/Player/Audio Config", fileName = "PlayerAudioConfig")]
    public class PlayerAudioConfig : ScriptableObject
    {
        [Header("Clips")]
        public AudioClip Jump;
        public AudioClip Land;
        public AudioClip Dash;
        public AudioClip Attack;

        [Range(0f, 1f)]
        public float Volume = 1f;
    }

}