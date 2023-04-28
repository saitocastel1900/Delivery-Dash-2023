using System.Collections.Generic;
using Commons.Const;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] TextAsset _stageFile; // ステージ構造が記述されたテキストファイル
    [SerializeField] float _tileSize; // タイルのサイズ

    private TileType[,] _tileList; // タイル情報を管理する二次元配列
    private int _rowCount; // 行数
    private int _columnCount; // 列数

    [SerializeField] GameObject _groundObject; // 地面のスプライト
    [SerializeField] GameObject _targetObject; // 目的地のスプライト
    [SerializeField] GameObject _playerObject; // プレイヤーのスプライト
    [SerializeField] GameObject _blockObject; // ブロックのスプライト

    [SerializeField] private GameObject _parentObject;
    private Vector3 _middleOffset; // 中心位置
    private int _blockCount;

    // 各位置に存在するゲームオブジェクトを管理する連想配列
    private Dictionary<GameObject, Vector3Int> _tileObjectData = new Dictionary<GameObject, Vector3Int>();
    
    /// <summary>
    /// 
    /// </summary>
    public void LoadTileData()
    {
        // タイルの情報を一行ごとに分割
        var tileRowData = _stageFile.text.Split
        (
            new[] {'\r', '\n'},
            System.StringSplitOptions.RemoveEmptyEntries
        );

        // タイルの列数を計算
        var tileColumnData = tileRowData[0].Split(new[] {','});

        // タイルの列数と行数を保持
        _rowCount = tileRowData.Length; // 行数
        _columnCount = tileColumnData.Length; // 列数

        // タイル情報を int 型の２次元配列で保持
        _tileList = new TileType[_columnCount, _rowCount];
        for (int row = 0; row < _rowCount; row++)
        {
            // 一文字ずつ取得
            tileColumnData = tileRowData[row].Split(new[] {','});
            for (int column = 0; column < _columnCount; column++)
            {
                // 読み込んだ文字を数値に変換して保持
                _tileList[column, row] = (TileType) int.Parse(tileColumnData[column]);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void CreateStage()
    {
        // ステージの中心位置を計算
        _middleOffset.x = _columnCount * _tileSize * 0.5f - _tileSize * 0.5f;
        _middleOffset.z = _rowCount * _tileSize * 0.5f - _tileSize * 0.5f;

        for (int z = 0; z < _rowCount; z++)
        {
            for (int x = 0; x < _columnCount; x++)
            {
                var stageObject = _tileList[x, z];

                // 何も無い場所は無視
                if (stageObject == TileType.NONE) continue;

                // タイルのゲームオブジェクトを作成
                var tile = Instantiate(_groundObject, GetDisplayPos(x, -1, z), Quaternion.identity);
                tile.name = InGameConst.TileName;
                tile.transform.parent = _parentObject.transform;

                switch (stageObject)
                {
                    case TileType.TARGET:
                        var target = Instantiate(_targetObject, GetDisplayPos(x, 0, z), Quaternion.identity);
                        target.name = InGameConst.TargetName;
                        target.transform.parent = _parentObject.transform;
                        break;

                    case TileType.PLAYER:
                        var player = Instantiate(_playerObject, GetDisplayPos(x, 0, z), Quaternion.identity);
                        player.name = InGameConst.PlayerName;
                        player.transform.parent = _parentObject.transform;
                        _tileObjectData.Add(player, new Vector3Int(x, 0, z));
                        break;

                    case TileType.BLOCK:
                        var block = Instantiate(_blockObject, GetDisplayPos(x, 0, z), Quaternion.identity);
                        _blockCount++;
                        block.name = InGameConst.BlockName;
                        block.transform.parent = _parentObject.transform;
                        _tileObjectData.Add(block, new Vector3Int(x, 0, z));
                        break;
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private Vector3 GetDisplayPos(int x, int y, int z)
    {
        return new Vector3
        (
            x * _tileSize - _middleOffset.x,
            y * _tileSize + _middleOffset.y,
            z * -_tileSize + _middleOffset.z
        );
    }

    public bool IsValidPosition(Vector3Int pos)
    {
        if (0 <= pos.x && pos.x < _columnCount && 0 <= pos.z && pos.z < _rowCount)
        {
            return _tileList[pos.x, pos.z] != TileType.NONE;
        }

        return false;
    }

    public GameObject GetGameObjectAtPosition(Vector3Int pos)
    {
        foreach (var pair in _tileObjectData)
        {
            // 指定された位置が見つかった場合
            if (pair.Value == pos)
            {
                // その位置に存在するゲームオブジェクトを返す
                return pair.Key;
            }
        }

        return null;
    }

    public Vector3Int GetPlayerPos(GameObject player)
    {
        return _tileObjectData[player];
    }


    public void SetPlayerPos(GameObject player, Vector3Int pos)
    {
        _tileObjectData[player] = pos;
    }

    public void SetBlockPos(GameObject block, Vector3Int pos)
    {
        _tileObjectData[block] = pos;
    }

    public bool IsBlock(Vector3Int pos)
    {
        var cell = _tileList[pos.x, pos.z];
        return cell == TileType.BLOCK || cell == TileType.BLOCK_ON_TARGET;
    }

    public Vector3Int GetNextPosition(Vector3Int pos, Vector3 direction)
    {
        switch (direction)
        {
            case var value when direction==Vector3.forward:
                pos.z -= 1;
                break;

            case var value when direction==Vector3.right:
                pos.x += 1;
                break;

            case var value when direction==Vector3.back:
                pos.z += 1;
                break;

            case var value when direction==Vector3.left:
                pos.x -= 1;
                break;
        }

        return pos;
    }

    public void UpdateGameObjectPos(Vector3Int pos)
    {
        // 指定された位置のタイルの番号を取得
        var cell = _tileList[pos.x, pos.z];

        // プレイヤーもしくはブロックの場合
        if (cell == TileType.PLAYER || cell == TileType.BLOCK)
        {
            // 地面に変更
            _tileList[pos.x, pos.z] = TileType.GROUND;
        }
        // 目的地に乗っているプレイヤーもしくはブロックの場合
        else if (cell == TileType.PLAYER_ON_TARGET || cell == TileType.BLOCK_ON_TARGET)
        {
            // 目的地に変更
            _tileList[pos.x, pos.z] = TileType.TARGET;
        }
    }

    public void SetTileType(int x, int z, TileType type)
    {
        _tileList[x, z] = type;
    }

    public TileType GetTileType(int x, int z)
    {
        return _tileList[x, z];
    }
    
    // ゲームをクリアしたかどうか確認
    public bool CheckCompletion()
    {
        // 目的地に乗っているブロックの数を計算
        int blockOnTargetCount = 0;

        for (int z = 0; z < _rowCount; z++)
        {
            for (int x = 0; x < _columnCount; x++)
            {
                if (_tileList[x, z] == TileType.BLOCK_ON_TARGET)
                {
                    blockOnTargetCount++;
                }
            }
        }

        // すべてのブロックが目的地の上に乗っている場合
        if (blockOnTargetCount == _blockCount)
        {
            // ゲームクリア
            return true;
        }
        else
        {
            return false;
        }
    }
}