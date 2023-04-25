using UnityEngine;
using System.Collections.Generic;
using Commons.Const;
using Zenject;
using UniRx;
using System;

public class InGameMoveCommandManager : MonoBehaviour
{
    [SerializeField] private StageManager _stageManager;

    [Inject] private IInputEventProvider _input;
    
    private Stack<Vector3> _directionList = new Stack<Vector3>();
    private Stack<bool> _blockIsMovedList = new Stack<bool>();

    /// <summary>
    /// 
    /// </summary>
    private GameObject _player;

    public void Initialize()
    {
        _player = GameObject.FindGameObjectWithTag(InGameConst.PlayerObjectTag);
        _input.MoveDirection.Subscribe(TryMovePlayer);
        _input.UndoButton.ThrottleFirst(TimeSpan.FromSeconds(0.5f)).Subscribe(_ => OnUndo());
    }

    private void TryMovePlayer(Vector3 direction)
    {
        bool blockIsMoved = false;
        _directionList.Push(direction);
        var currentPlayerPos = _stageManager.GetPlayerPos(_player);
        var nextPlayerPos = _stageManager.GetNextPosition(currentPlayerPos, direction);

        if (!_stageManager.IsValidPosition(nextPlayerPos)) return;

        if (_stageManager.IsBlock(nextPlayerPos))
        {
            // ブロックの移動先の位置を計算
            var nextBlockPos = _stageManager.GetNextPosition(nextPlayerPos, direction);

            // ブロックの移動先がステージ内の場合かつブロックの移動先にブロックが存在しない場合
            if (_stageManager.IsValidPosition(nextBlockPos) && !_stageManager.IsBlock(nextBlockPos))
            {
                // 移動するブロックを取得
                var block = _stageManager.GetGameObjectAtPosition(new Vector3Int(nextPlayerPos.x, 0, nextPlayerPos.z));

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
            ICommand playerCommand = new MoveCommand(_player.GetComponent<PlayerMover>(),direction);
            PlayerMoveCommandInvoker.Execute(playerCommand);

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
        _blockIsMovedList.Push(blockIsMoved);
    }
    
    /// <summary>
    /// 
    /// </summary>
    private void OnUndo()
    {
        if (_directionList.Count > 0)
        {
            //
            PlayerMoveCommandInvoker.Undo();

            var direction = -_directionList.Pop();
            //
            var currentPlayerPos = _stageManager.GetPlayerPos(_player);
            var nextPlayerPos = _stageManager.GetNextPosition(currentPlayerPos, direction);
            
            //
            _stageManager.SetPlayerPos(_player, nextPlayerPos);
            _stageManager.UpdateGameObjectPos(currentPlayerPos);
            
            switch (_stageManager.GetTileType(nextPlayerPos.x, nextPlayerPos.z))
            {
                case TileType.GROUND:
                    _stageManager.SetTileType(nextPlayerPos.x, nextPlayerPos.z, TileType.PLAYER);
                    break;

                case TileType.TARGET:
                    _stageManager.SetTileType(nextPlayerPos.x, nextPlayerPos.z, TileType.PLAYER_ON_TARGET);
                    break;
            }

            if (_blockIsMovedList.Count > 0 && _blockIsMovedList.Pop())
            {
                BlockMoveCommandInvoker.Undo();
                var nextBlockPos = _stageManager.GetNextPosition(nextPlayerPos, direction);
                // 移動するブロックを取得
                var block = _stageManager.GetGameObjectAtPosition(new Vector3Int(nextPlayerPos.x, 0, nextPlayerPos.z));
                // ブロックの位置を更新
                _stageManager.SetBlockPos(block, new Vector3Int(nextBlockPos.x, 0, nextBlockPos.z));

                switch (_stageManager.GetTileType(nextBlockPos.x, nextBlockPos.z))
                {
                    case TileType.PLAYER:
                        _stageManager.SetTileType(nextBlockPos.x, nextBlockPos.z, TileType.BLOCK);
                        break;
                }
            }
        }
    }
}