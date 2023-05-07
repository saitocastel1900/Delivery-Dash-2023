using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace Commons.Input
{
    public class TitleKeyInputProvider : ITitleInputEventProvider, IInitializable , IDisposable
    {
        /// <summary>
        /// ゲームスタートが押されたか
        /// </summary>
        public IReadOnlyReactiveProperty<bool> IsGameStart => _isGameStart;
        private ReactiveProperty<bool> _isGameStart = new ReactiveProperty<bool>(false);
        
        /// <summary>
        /// 
        /// </summary>
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
        
        public void Initialize()
        {
            //キーが押されたら、フラグを設定する
            Observable.EveryUpdate()
                .Select(_=>UnityEngine.Input.GetKeyDown(KeyCode.Return))
                .DistinctUntilChanged()
                .Subscribe(value => _isGameStart.Value = value)
                .AddTo(_compositeDisposable);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            _compositeDisposable.Dispose();
        }
    }
}