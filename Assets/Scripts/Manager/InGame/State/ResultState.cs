using System;
using Commons.Utility;
using UniRx;
using Widget.Result;

namespace Manager.InGame.State
{
    public class ResultState : IState , IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        readonly ResultWidgetController _resultWidget;

        /// <summary>
        /// InGameManager
        /// </summary>
        readonly InGameManager _inGame;
        
        /// <summary>
        /// 
        /// </summary>
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="resultWidget"></param>
        /// <param name="inGame">ステート管理クラス</param>
        public ResultState(ResultWidgetController resultWidget, InGameManager inGame)
        {
            _resultWidget = resultWidget;
            _inGame = inGame;
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Enter()
        {
            DebugUtility.Log("ResultState Enter");
            _resultWidget.SetView(true);
            _resultWidget.Animation();
            _resultWidget.IsAnimationFinished.SkipLatestValueOnSubscribe()
                .Subscribe(_ => _inGame.CurrentState.TransitionTo(_inGame.CurrentState.FinishedState)).AddTo(_compositeDisposable);
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            DebugUtility.Log("ResultState Update");
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Exit()
        {
            DebugUtility.Log("ResultState Exit");
        }

        public void Dispose()
        {
            _compositeDisposable.Dispose();
        }
    }
}