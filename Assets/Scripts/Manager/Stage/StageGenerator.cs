using System.Collections.Generic;
using System.Linq;
using Commons.Const;
using UnityEngine;

namespace Manager.Stage
{
    public class StageGenerator : MonoBehaviour
    {
        /// <summary>
        /// StageData
        /// </summary>
        [SerializeField] private StageData _stageData;

        /// <summary>
        /// Stageオブジェクトの生成場所
        /// </summary>
        [SerializeField] private GameObject _parentObject;

        /// <summary>
        /// 進行方向のマップ
        /// </summary>
        private static readonly Dictionary<Vector3, Vector3Int> DirectionToPositionDelta = new Dictionary<Vector3, Vector3Int>
        {
            { Vector3.forward, new Vector3Int(0, 0, -1) },
            { Vector3.right, new Vector3Int(1, 0, 0) },
            { Vector3.back, new Vector3Int(0, 0, 1) },
            { Vector3.left, new Vector3Int(-1, 0, 0) },
        };
        
        // 各位置に存在するゲームオブジェクトを管理する連想配列
        /// <summary>
        /// 
        /// </summary>
        private readonly Dictionary<GameObject, Vector3Int> _tileObjectData = new Dictionary<GameObject, Vector3Int>();

        /// <summary>
        /// タイル情報を管理
        /// </summary>
        private TileType[,] _tileList;

        /// <summary>
        /// 行数
        /// </summary>
        private int _rowCount;

        /// <summary>
        /// 列数
        /// </summary>
        private int _columnCount;

        /// <summary>
        /// 中心位置
        /// </summary>
        private Vector3 _middleOffset;

        /// <summary>
        /// 設置されているブロックの数
        /// </summary>
        private int _blockCount;

        /// <summary>
        /// ファイルを読み込む
        /// </summary>
        public void LoadTileData()
        {
            //ファイルを区切ってString化する
            var tileRowData = _stageData.StageFile.text.Split
            (
                new[] {'\r', '\n'},
                System.StringSplitOptions.RemoveEmptyEntries
            );
            
            //行数と列数を設定
            var tileColumnData = tileRowData[0].Split(new[] {','});
            _rowCount = tileRowData.Length;
            _columnCount = tileColumnData.Length;
            
            //位置に応じてTypeを設定
            _tileList = new TileType[_columnCount, _rowCount];
            for (int row = 0; row < _rowCount; row++)
            {
                tileColumnData = tileRowData[row].Split(new[] {','});
                for (int column = 0; column < _columnCount; column++)
                {
                    _tileList[column, row] = (TileType) int.Parse(tileColumnData[column]);
                }
            }
        }

        /// <summary>
        /// ステージ生成
        /// </summary>
        public void CreateStage()
        {
            CalcCenterPosition();

            //タイルデータからステージ生成
            for (int z = 0; z < _rowCount; z++)
            {
                for (int x = 0; x < _columnCount; x++)
                {
                    var type = _tileList[x, z];

                    if (type == TileType.NONE) continue;
                    
                    //床オブジェクトは無条件で生成
                    var ground = Instantiate(_stageData.GroundObject,
                        new Vector3(x, -1, z).DisplayPosition(_middleOffset), Quaternion.identity);
                    ground.name = InGameConst.GroundName;
                    ground.transform.parent = _parentObject.transform;

                    //タイプに応じてオブジェクトを生成
                    switch (type)
                    {
                        case TileType.TARGET:
                            var target = Instantiate(_stageData.TargetObject,
                                new Vector3(x, 0, z).DisplayPosition(_middleOffset), Quaternion.identity);
                            target.name = InGameConst.TargetName;
                            target.transform.parent = _parentObject.transform;
                            break;

                        case TileType.PLAYER:
                            var player = Instantiate(_stageData.PlayerObject,
                                new Vector3(x, 0, z).DisplayPosition(_middleOffset), Quaternion.identity);
                            player.name = InGameConst.PlayerName;
                            player.transform.parent = _parentObject.transform;
                            _tileObjectData.Add(player, new Vector3Int(x, 0, z));
                            break;

                        case TileType.BLOCK:
                            var block = Instantiate(_stageData.BlockObject,
                                new Vector3(x, 0, z).DisplayPosition(_middleOffset), Quaternion.identity);
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
        /// オブジェクトに位置座標を設定
        /// </summary>
        public void SetObjectPosition(GameObject obj, Vector3Int pos)
        {
            _tileObjectData[obj] = pos;
        }

        /// <summary>
        /// タイルを設定
        /// </summary>
        public void SetTileType(int x, int z, TileType type)
        {
            _tileList[x, z] = type;
        }

        /// <summary>
        /// タイプに応じてタイルを上書きする
        /// </summary>
        public void UpdateObjectPosition(Vector3Int pos)
        {
            var tile = _tileList[pos.x, pos.z];

            if (tile is TileType.PLAYER or TileType.BLOCK)
            {
                _tileList[pos.x, pos.z] = TileType.GROUND;
            }
            else if (tile is TileType.PLAYER_ON_TARGET or TileType.BLOCK_ON_TARGET)
            {
                _tileList[pos.x, pos.z] = TileType.TARGET;
            }
        }

        /// <summary>
        /// その場所に存在するオブジェクトを取得
        /// </summary>
        public GameObject GetAtPositionObject(Vector3Int pos)
        {
            foreach (var tile in _tileObjectData)
            {
                if (tile.Value == pos)
                {
                    return tile.Key;
                }
            }

            return null;
        }

        /// <summary>
        /// オブジェクトに設定されている位置座標を取得
        /// </summary>
        public Vector3Int GetObjectPosition(GameObject obj)
        {
            return _tileObjectData[obj];
        }

        /// <summary>
        /// 次の進むべき場所を取得
        /// </summary>
        public Vector3Int GetNextPosition(Vector3Int pos, Vector3 direction)
        {
            return pos + DirectionToPositionDelta[direction];
        }

        /// <summary>
        /// タイルを設定
        /// </summary>
        public TileType GetTileType(int x, int z)
        {
            return _tileList[x, z];
        }

        /// <summary>
        /// ブロックが存在するか
        /// </summary>
        public bool IsBlockPosition(Vector3Int pos)
        {
            var tile = _tileList[pos.x, pos.z];
            return tile == TileType.BLOCK || tile == TileType.BLOCK_ON_TARGET;
        }

        /// <summary>
        /// 進行可能か
        /// </summary>
        public bool IsValidPosition(Vector3Int pos)
        {
            if (0 <= pos.x && pos.x < _columnCount && 0 <= pos.z && pos.z < _rowCount)
            {
                return _tileList[pos.x, pos.z] != TileType.NONE;
            }

            return false;
        }

        /// <summary>
        /// 目的地にブロックを配置したか
        /// </summary>
        /// <returns></returns>
        public bool IsDelivered()
        {
            int blockOnTargetCount = _tileList.Cast<TileType>().Count(tile => tile == TileType.BLOCK_ON_TARGET);
            return blockOnTargetCount == _blockCount;
        }

        /// <summary>
        /// 中心位置を計算
        /// </summary>
        private void CalcCenterPosition()
        {
            _middleOffset.x = _columnCount * InGameConst.TileSize * 0.5f - InGameConst.TileSize * 0.5f;
            _middleOffset.z = _rowCount * InGameConst.TileSize * 0.5f - InGameConst.TileSize * 0.5f;
        }
    }
}