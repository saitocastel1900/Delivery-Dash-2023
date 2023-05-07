using System;
using UniRx;
using Zenject;

namespace Widget.InGame.StageNumber
{
    public class StageNumberPresenter : IDisposable, IInitializable
    {
        /// <summary>
        /// Model
        /// </summary>
        private IStageNumberModel _model;

        /// <summary>
        /// View
        /// </summary>
        private StageNumberView _view;

        /// <summary>
        /// Disposable
        /// </summary>
        private CompositeDisposable _compositeDisposable;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public StageNumberPresenter(IStageNumberModel model, StageNumberView view)
        {
            _model = model;
            _view = view;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            _compositeDisposable = new CompositeDisposable();
            Bind();
        }

        /// <summary>
        /// Bind
        /// </summary>
        private void Bind()
        {
            _model.StageNumberProp
                .Subscribe(_view.SetText)
                .AddTo(_compositeDisposable);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            _compositeDisposable.Dispose();
        }
    }
}