using System;
using UniRx;
using Zenject;
using UnityEngine;

namespace Commons.Input
{
    public class SelectKeyInputProvider : ISelectInputEventProvider , IInitializable , IDisposable
    {
        /// <summary>
        /// 右移動が押されたか
        /// </summary>
        public IReadOnlyReactiveProperty<bool> IsRight => _isRight;
        private ReactiveProperty<bool> _isRight = new ReactiveProperty<bool>(false);

        /// <summary>
        /// 左移動が押されたか
        /// </summary>
        public IReadOnlyReactiveProperty<bool> IsLeft => _isLeft;
        private ReactiveProperty<bool> _isLeft = new ReactiveProperty<bool>(false);

        /// <summary>
        /// シーン遷移が押されたか
        /// </summary>
        public IReadOnlyReactiveProperty<bool> IsTransition => _isTransition;
        private ReactiveProperty<bool> _isTransition = new ReactiveProperty<bool>(false);

        /// <summary>
        /// 
        /// </summary>
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();

        public void Initialize()
        {
            //キーが押されたら、フラグを設定する
            Observable.EveryUpdate()
                .Select(_=>UnityEngine.Input.GetKeyDown(KeyCode.RightArrow))
                .DistinctUntilChanged()
                .Subscribe(value => _isRight.Value = value).AddTo(_compositeDisposable);
            
            //キーが押されたら、フラグを設定する
            Observable.EveryUpdate()
                .Select(_=>UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow))
                .DistinctUntilChanged()
                .Subscribe(value => _isLeft.Value = value).AddTo(_compositeDisposable);
            
            //キーが押されたら、フラグを設定する
            Observable.EveryUpdate()
                .Select(_=>UnityEngine.Input.GetKeyDown(KeyCode.Return))
                .DistinctUntilChanged()
                .Subscribe(value => _isTransition.Value = value).AddTo(_compositeDisposable);
        }

        public void Dispose()
        {
            _compositeDisposable.Dispose();
        }
    }
}