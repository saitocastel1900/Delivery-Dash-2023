using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Manager.Stage;
using UniRx;
using UnityEngine;
using Zenject;

namespace Manager.Command
{
    public class InGameDemoCommandManager : MonoBehaviour
    {
        /// <summary>
        /// StageAnswerData
        /// </summary>
        [SerializeField] private StageAnswerData _stageAnswerData;

        /// <summary>
        /// InGameCommandManager
        /// </summary>
        [SerializeField] private InGameCommandManager _commandManager;

        /// <summary>
        /// IInputEventProvider
        /// </summary>
        [Inject] private IInputEventProvider _input;

        /// <summary>
        /// 進行方向のマップ
        /// </summary>
        private static readonly Dictionary<DirectionType, Vector3> directionMap =
            new Dictionary<DirectionType, Vector3>()
            {
                {DirectionType.AheadMove, Vector3.forward},
                {DirectionType.LeftMove, Vector3.left},
                {DirectionType.RightMove, Vector3.right},
                {DirectionType.BackMove, Vector3.back}
            };

        private void Start()
        {
            _input.IsSkip.SkipLatestValueOnSubscribe().First().Subscribe(_ => ExecuteCommands().Forget());

#if UNITY_EDITOR
            _input.MoveDirection.SkipLatestValueOnSubscribe()
                .Where(_ => _stageAnswerData.IsRecoding == true)
                .Select(value => directionMap.FirstOrDefault(element => element.Value == value))
                .Subscribe(value => _stageAnswerData.CommandAnswerList.Add(value.Key));
#endif
        }

        /// <summary>
        /// コマンドを実行
        /// </summary>
        private async UniTaskVoid ExecuteCommands()
        {
            foreach (var command in _stageAnswerData.CommandAnswerList)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(0.5f));
                _commandManager.OnMove(directionMap[command]);
            }
        }
    }
}