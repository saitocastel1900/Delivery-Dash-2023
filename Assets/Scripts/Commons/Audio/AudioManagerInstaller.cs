using UnityEngine;
using Zenject;

namespace Commons.Audio
{
    public class AudioManagerInstaller : MonoInstaller
    {
        [SerializeField] private AudioManager audioManager;
        
        public override void InstallBindings()
        {
            Container.Bind<AudioManager>().FromComponentInNewPrefab(audioManager).AsSingle();
        }
    }
}