using System.Collections.Generic;
using System.Linq;
using Commons.Extensions;
using Commons.Utility;
using UnityEngine;

namespace Commons.Audio
{
    public class AudioManagerComponent : MonoBehaviour
    {
        /// <summary>
        /// BGMのAudioSource
        /// </summary>
        [SerializeField] private AudioSource _bgmSource;

        /// <summary>
        /// SEのAudioSource
        /// </summary>
        [SerializeField] private AudioSource _seSource;

        /// <summary>
        /// BGMのAudioClip
        /// </summary>
        [SerializeField] private AudioClip _bgmAudioClip;

        /// <summary>
        /// SEのAudioClip群
        /// </summary>
        [SerializeField] private List<AudioClip> _seAudioClipList = new List<AudioClip>();

        public void Start()
        {
            _bgmSource = InitializeAudioSource(_bgmSource, true);
            _seSource = InitializeAudioSource(_seSource, false);
        }

        /// <summary>
        /// 初期化処理
        /// </summary>
        private AudioSource InitializeAudioSource(AudioSource audioSource, bool isLoop = false)
        {
            audioSource.loop = isLoop;
            audioSource.playOnAwake = false;

            return audioSource;
        }

        /// <summary>
        /// BGMを流す
        /// </summary>
        public void PlayBGM()
        {
            _bgmSource.Play(_bgmAudioClip);
        }

        /// <summary>
        /// SEを流す
        /// </summary>
        /// <param name="soundEffect">流したいSE</param>
        public void PlaySoundEffect(SoundEffect soundEffect)
        {
            var audioClip = _seAudioClipList.FirstOrDefault(clip => clip.name == soundEffect.ToString());

            if (audioClip == null)
            {
                DebugUtility.Log(soundEffect + "は見つかりません");
                return;
            }

            _seSource.PlayOneShot(audioClip);
        }
    }
}