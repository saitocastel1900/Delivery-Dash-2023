using UnityEngine;

namespace Commons.Audio
{
    public class AudioManager : MonoBehaviour
    {
        /// <summary>
        /// AudioManagerComponent
        /// </summary>
        [SerializeField] private AudioManagerComponent _component;
        
        /// <summary>
        /// BGMを流す
        /// </summary>
        public void PlayBGM()
        {
            _component.PlayBGM();
        }

        /// <summary>
        /// SEを流す
        /// </summary>
        /// <param name="soundEffect"></param>
        public void PlaySoundEffect(SoundEffect soundEffect)
        {
            _component.PlaySoundEffect(soundEffect);
        }
    }
}