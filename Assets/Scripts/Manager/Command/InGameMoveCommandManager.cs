using UnityEngine;
using System.Collections.Generic;
using Commons.Const;
using Zenject;
using UniRx;
using System;
using Block;
using Player;

namespace Manager.Command
{
    public class InGameMoveCommandManager : MonoBehaviour
    {
        /// <summary>
        /// 配達したか
        /// </summary>
        public IReactiveProperty<bool> IsDelivered => _isDelivered;
        private BoolReactiveProperty _isDelivered = new BoolReactiveProperty(false);

        /// <summary>
        /// StageManager
        /// </summary>
        [SerializeField] private StageManager _stageManager;

        /// <summary>
        /// 入力
        /// </summary>
        [Inject] private IInputEventProvider _input;

        /// <summary>
        /// 進行方向のリスト
        /// </summary>
        private Stack<Vector3> _directionList = new Stack<Vector3>();

        /// <summary>
        /// ブロックが移動したか
        /// </summary>
        private Stack<bool> _blockIsMovedList = new Stack<bool>();

        /// <summary>
        /// Playerのキャッシュ
        /// </summary>
        private GameObject _player;

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            _player = GameObject.FindGameObjectWithTag(InGameConst.PlayerObjectTag);
            _input.MoveDirection.Subscribe(OnMove);
            _input.UndoButton.Where(value => value == true).ThrottleFirst(TimeSpan.FromSeconds(0.1f))
                .Subscribe(_ => OnUndo());
        }

        /// <summary>
        /// 移動
        /// </summary>
        /// <param name="direction"></param>
        private void OnMove(Vector3 direction)
        {
            bool blockIsMoved = false;

            var currentPlayerPos = _stageManager.GetPlayerPos(_player);
            var nextPlayerPos = _stageManager.GetNextPosition(currentPlayerPos, direction);

            //進行方向がステージ内なら移動する
            if (!_stageManager.IsValidPosition(nextPlayerPos)) return;

            _directionList.Push(direction);

            //進行方向にブロックが存在していたら、、、
            if (_stageManager.IsBlock(nextPlayerPos))
            {
                // ブロックの移動先の位置を計算
                var nextBlockPos = _stageManager.GetNextPosition(nextPlayerPos, direction);

                // ブロックの移動先がステージ内の場合かつブロックの移動先にブロックが存在しない場合
                if (_stageManager.IsValidPosition(nextBlockPos) && !_stageManager.IsBlock(nextBlockPos))
                {
                    // 移動するブロックを取得
                    var block = _stageManager.GetGameObjectAtPosition(new Vector3Int(nextPlayerPos.x, 0,
                        nextPlayerPos.z));

                    // プレイヤーの移動先のタイルの情報を更新
                    _stageManager.UpdateGameObjectPos(nextPlayerPos);

                    // ブロックを移動
                    ICommand blockCommand = new MoveCommand(block.GetComponent<BlockMover>(), direction);
                    BlockMoveCommandInvoker.Execute(blockCommand);

                    // ブロックの位置を更新
                    _stageManager.SetBlockPos(block, new Vector3Int(nextBlockPos.x, 0, nextBlockPos.z));

                    // ブロックの移動先の番号を更新
                    switch (_stageManager.GetTileType(nextBlockPos.x, nextBlockPos.z))
                    {
                        case TileType.GROUND:
                            _stageManager.SetTileType(nextBlockPos.x, nextBlockPos.z, TileType.BLOCK);
                            break;

                        case TileType.TARGET:
                            _stageManager.SetTileType(nextBlockPos.x, nextBlockPos.z, TileType.BLOCK_ON_TARGET);
                            break;
                    }

                    // プレイヤーの現在地のタイルの情報を更新
                    _stageManager.UpdateGameObjectPos(currentPlayerPos);

                    // プレイヤーを移動
                    ICommand playerCommand = new MoveCommand(_player.GetComponent<PlayerMover>(), direction);
                    PlayerMoveCommandInvoker.Execute(playerCommand);

                    // プレイヤーの位置を更新
                    _stageManager.SetPlayerPos(_player, nextPlayerPos);

                    switch (_stageManager.GetTileType(nextPlayerPos.x, nextPlayerPos.z))
                    {
                        case TileType.GROUND:
                            _stageManager.SetTileType(nextPlayerPos.x, nextPlayerPos.z, TileType.PLAYER);
                            break;

                        case TileType.TARGET:
                            _stageManager.SetTileType(nextPlayerPos.x, nextPlayerPos.z, TileType.PLAYER_ON_TARGET);
                            break;
                    }

                    blockIsMoved = true;
                }
            }
            else
            {
                _stageManager.UpdateGameObjectPos(currentPlayerPos);

                ICommand playerCommand = new MoveCommand(_player.GetComponent<PlayerMover>(), direction);
                PlayerMoveCommandInvoker.Execute(playerCommand);

                _stageManager.SetPlayerPos(_player, nextPlayerPos);

                switch (_stageManager.GetTileType(nextPlayerPos.x, nextPlayerPos.z))
                {
                    case TileType.GROUND:
                        _stageManager.SetTileType(nextPlayerPos.x, nextPlayerPos.z, TileType.BLOCK);
                        break;

                    case TileType.TARGET:
                        _stageManager.SetTileType(nextPlayerPos.x, nextPlayerPos.z, TileType.BLOCK_ON_TARGET);
                        break;
                }
            }

            _isDelivered.Value = _stageManager.CheckCompletion();
            _blockIsMovedList.Push(blockIsMoved);
        }

        /// <summary>
        /// 移動巻き戻し
        /// </summary>
        private void OnUndo()
        {
            //移動していたら移動を巻き戻す
            if (_directionList.Count > 0)
            {
                var direction = _directionList.Pop();
                
                var currentPlayerPos = _stageManager.GetPlayerPos(_player);
                var nextPlayerPos = _stageManager.GetNextPosition(currentPlayerPos, -direction);

                //ブロックが動かされていたら、ブロックも移動を巻き戻す
                if (_blockIsMovedList.Count > 0 && _blockIsMovedList.Pop())
                {
                    var currentBlockPos = _stageManager.GetNextPosition(currentPlayerPos, direction);

                    _stageManager.UpdateGameObjectPos(currentBlockPos);

                    // 移動するブロックを取得
                    var block = _stageManager.GetGameObjectAtPosition(new Vector3Int(currentBlockPos.x, 0,
                        currentBlockPos.z));

                    BlockMoveCommandInvoker.Undo();

                    // ブロックの位置を更新
                    _stageManager.SetBlockPos(block, new Vector3Int(currentPlayerPos.x, 0, currentPlayerPos.z));

                    switch (_stageManager.GetTileType(currentPlayerPos.x, currentPlayerPos.z))
                    {
                        case TileType.PLAYER:
                            _stageManager.SetTileType(currentPlayerPos.x, currentPlayerPos.z, TileType.BLOCK);
                            break;

                        case TileType.TARGET:
                            _stageManager.SetTileType(currentPlayerPos.x, currentPlayerPos.z, TileType.BLOCK_ON_TARGET);
                            break;
                    }
                    
                    _stageManager.UpdateGameObjectPos(nextPlayerPos);
                    
                    PlayerMoveCommandInvoker.Undo();
                    
                    _stageManager.SetPlayerPos(_player, nextPlayerPos);

                    switch (_stageManager.GetTileType(nextPlayerPos.x, nextPlayerPos.z))
                    {
                        case TileType.GROUND:
                            _stageManager.SetTileType(nextPlayerPos.x, nextPlayerPos.z, TileType.PLAYER);
                            break;

                        case TileType.TARGET:
                            _stageManager.SetTileType(nextPlayerPos.x, nextPlayerPos.z, TileType.PLAYER_ON_TARGET);
                            break;
                    }
                }
                else
                {
                    _stageManager.UpdateGameObjectPos(currentPlayerPos);
                    
                    PlayerMoveCommandInvoker.Undo();
                    
                    _stageManager.SetPlayerPos(_player, nextPlayerPos);

                    switch (_stageManager.GetTileType(nextPlayerPos.x, nextPlayerPos.z))
                    {
                        case TileType.GROUND:
                            _stageManager.SetTileType(nextPlayerPos.x, nextPlayerPos.z, TileType.PLAYER);
                            break;

                        case TileType.TARGET:
                            _stageManager.SetTileType(nextPlayerPos.x, nextPlayerPos.z, TileType.PLAYER_ON_TARGET);
                            break;
                    }
                }
            }
        }
    }
}