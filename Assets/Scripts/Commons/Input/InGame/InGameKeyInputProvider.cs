using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace Commons.Input
{
    public class InGameKeyInputProvider : IInGameInputEventProvider, IInitializable , IDisposable
    {
        /// <summary>
        /// 進行方向
        /// </summary>
        public IReadOnlyReactiveProperty<Vector3> MoveDirection => _moveDirection;
        private ReactiveProperty<Vector3> _moveDirection = new ReactiveProperty<Vector3>();

        /// <summary>
        /// Undoが押されたか
        /// </summary>
        public IReadOnlyReactiveProperty<bool> IsUndo => _isUndo;
        private ReactiveProperty<bool> _isUndo = new ReactiveProperty<bool>(false);
        
        /// <summary>
        /// Resetが押されたか
        /// </summary>
        public IReadOnlyReactiveProperty<bool> IsReset => _isReset;
        private ReactiveProperty<bool> _isReset = new ReactiveProperty<bool>(false);
        
        /// <summary>
        /// Quitが押されたか
        /// </summary>
        public IReadOnlyReactiveProperty<bool> IsQuit => _isQuit;
        private ReactiveProperty<bool> _isQuit = new ReactiveProperty<bool>(false);

        /// <summary>
        /// Skipが押されたか
        /// </summary>
        public IReadOnlyReactiveProperty<bool> IsSkip => _isSkip;
        private ReactiveProperty<bool> _isSkip = new ReactiveProperty<bool>(false);

        /// <summary>
        /// 
        /// </summary>
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
        
        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            //キーが押されたら、進行方向を設定する
            Observable.Merge(
                    Observable.EveryUpdate().Where(_ => UnityEngine.Input.GetKeyDown(KeyCode.UpArrow))
                        .Select(_ => Vector3.forward),
                    Observable.EveryUpdate().Where(_ => UnityEngine.Input.GetKeyDown(KeyCode.RightArrow))
                        .Select(_ => Vector3.right),
                    Observable.EveryUpdate().Where(_ => UnityEngine.Input.GetKeyDown(KeyCode.DownArrow))
                        .Select(_ => Vector3.back),
                    Observable.EveryUpdate().Where(_ => UnityEngine.Input.GetKeyDown(KeyCode.LeftArrow))
                        .Select(_ => Vector3.left)
                )
                .Subscribe(value => _moveDirection.SetValueAndForceNotify(value))
                .AddTo(_compositeDisposable);

            //キーが押されたら、フラグを設定する
            Observable.Merge(
                    Observable.EveryUpdate().Where(_ => UnityEngine.Input.GetKeyDown(KeyCode.Z)).Select(_ => true),
                    Observable.EveryUpdate().Where(_ => UnityEngine.Input.GetKeyUp(KeyCode.Z)).Select(_ => false)
                )
                .DistinctUntilChanged()
                .Subscribe(value => _isUndo.Value = value)
                .AddTo(_compositeDisposable);

            //キーが押されたら、フラグを設定する
            Observable.EveryUpdate()
                .Select(_=>UnityEngine.Input.GetKeyDown(KeyCode.R))
                .DistinctUntilChanged()
                .Subscribe(value => _isReset.Value = value)
                .AddTo(_compositeDisposable);
            
            //キーが押されたら、フラグを設定する
            Observable.EveryUpdate()
                .Select(_=>UnityEngine.Input.GetKeyDown(KeyCode.Q))
                .DistinctUntilChanged()
                .Subscribe(value => _isQuit.Value = value)
                .AddTo(_compositeDisposable);
            
            //キーが押されたら、フラグを設定する
            Observable.EveryUpdate()
                .Select(_=>UnityEngine.Input.GetKeyDown(KeyCode.Space))
                .DistinctUntilChanged()
                .Subscribe(value => _isSkip.Value = value)
                .AddTo(_compositeDisposable);
        }

        public void Dispose()
        {
            _compositeDisposable.Dispose();
        }
    }
}