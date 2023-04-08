using UnityEngine;
using System.Collections.Generic;

namespace Block
{
    public class Sokoban : MonoBehaviour
    {
        public TextAsset stageFile; // ステージ構造が記述されたテキストファイル

        private int rows; // 行数
        private int columns; // 列数
        private TileType[,] tileList; // タイル情報を管理する二次元配列

        public float tileSize; // タイルのサイズ

        private bool isClear; // ゲームをクリアした場合 true

        // 方向の種類
        private enum DirectionType
        {
            Ahead, // 上
            RIGHT, // 右
            Back, // 下
            LEFT, // 左
        }

        // ゲーム開始時に呼び出される
        private void Start()
        {
            LoadTileData(); // タイルの情報を読み込む
            CreateStage(); // ステージを作成
        }

        // 毎フレーム呼び出される
        private void Update()
        {
            // ゲームクリアしている場合は操作できないようにする
            if (isClear) return;

            // 上矢印が押された場合
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                // プレイヤーが上に移動できるか検証
                TryMovePlayer(DirectionType.Ahead);
            }
            // 右矢印が押された場合
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                // プレイヤーが右に移動できるか検証
                TryMovePlayer(DirectionType.RIGHT);
            }
            // 下矢印が押された場合
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                // プレイヤーが下に移動できるか検証
                TryMovePlayer(DirectionType.Back);
            }
            // 左矢印が押された場合
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                // プレイヤーが左に移動できるか検証
                TryMovePlayer(DirectionType.LEFT);
            }
        }

        public GameObject groundObject; // 地面のスプライト
        public GameObject targetObject; // 目的地のスプライト
        public GameObject playerObject; // プレイヤーのスプライト
        public GameObject blockObject; // ブロックのスプライト

        private GameObject player; // プレイヤーのゲームオブジェクト
        private Vector3 middleOffset; // 中心位置
        private int blockCount; // ブロックの数

        // 各位置に存在するゲームオブジェクトを管理する連想配列
        private Dictionary<GameObject, Vector3Int> gameObjectPosTable = new Dictionary<GameObject, Vector3Int>();

        private void LoadTileData()
        {
            // タイルの情報を一行ごとに分割
            var lines = stageFile.text.Split
            (
                new[] {'\r', '\n'},
                System.StringSplitOptions.RemoveEmptyEntries
            );

            // タイルの列数を計算
            var nums = lines[0].Split(new[] {','});

            // タイルの列数と行数を保持
            rows = lines.Length; // 行数
            columns = nums.Length; // 列数

            // タイル情報を int 型の２次元配列で保持
            tileList = new TileType[columns, rows];
            for (int z = 0; z < rows; z++)
            {
                // 一文字ずつ取得
                var st = lines[z];
                nums = st.Split(new[] {','});
                for (int x = 0; x < columns; x++)
                {
                    // 読み込んだ文字を数値に変換して保持
                    tileList[x, z] = (TileType) int.Parse(nums[x]);
                }
            }
        }

        private void CreateStage()
        {
            // ステージの中心位置を計算
            middleOffset.x = columns * tileSize * 0.5f - tileSize * 0.5f;
            middleOffset.z = rows * tileSize * 0.5f - tileSize * 0.5f;

            for (int z = 0; z < rows; z++)
            {
                for (int x = 0; x < columns; x++)
                {
                    var val = tileList[x, z];

                    // 何も無い場所は無視
                    if (val == TileType.NONE) continue;

                    // タイルの名前に行番号と列番号を付与
                    var name = "tile" + z + "_" + x;

                    // タイルのゲームオブジェクトを作成
                    var tile = Instantiate(groundObject, GetDisplayPosition(x, -1, z), Quaternion.identity);
                    tile.name = name;

                    // 目的地の場合
                    if (val == TileType.TARGET)
                    {
                        // 目的地のゲームオブジェクトを作成
                        var destination = Instantiate(targetObject, GetDisplayPosition(x, 0, z), Quaternion.identity);
                        destination.name = "destination";
                    }

                    // プレイヤーの場合
                    if (val == TileType.PLAYER)
                    {
                        // プレイヤーのゲームオブジェクトを作成
                        player = Instantiate(playerObject, GetDisplayPosition(x, 0, z), Quaternion.identity);
                        player.name = "player";

                        // プレイヤーを連想配列に追加
                        gameObjectPosTable.Add(player, new Vector3Int(x, z));
                    }
                    // ブロックの場合
                    else if (val == TileType.BLOCK)
                    {
                        // ブロックの数を増やす
                        blockCount++;

                        var block = Instantiate(blockObject, GetDisplayPosition(x, 0, z), Quaternion.identity);
                        block.name = "block" + blockCount;

                        // ブロックを連想配列に追加
                        gameObjectPosTable.Add(block, new Vector3Int(x, 0, z));
                    }
                }
            }
        }

        // 指定された行番号と列番号からスプライトの表示位置を計算して返す
        private Vector3 GetDisplayPosition(int x, int y, int z)
        {
            return new Vector3
            (
                x * tileSize - middleOffset.x,
                y * tileSize + middleOffset.y,
                z * -tileSize + middleOffset.z
            );
        }

        // 指定された位置に存在するゲームオブジェクトを返します
        private GameObject GetGameObjectAtPosition(Vector3Int pos)
        {
            foreach (var pair in gameObjectPosTable)
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

        // 指定された位置がステージ内なら true を返す
        private bool IsValidPosition(Vector3Int pos)
        {
            if (0 <= pos.x && pos.x < columns && 0 <= pos.y && pos.y < rows)
            {
                return tileList[pos.x, pos.y] != TileType.NONE;
            }

            return false;
        }

        // 指定された位置のタイルがブロックなら true を返す
        private bool IsBlock(Vector3Int pos)
        {
            var cell = tileList[pos.x, pos.y];
            return cell == TileType.BLOCK || cell == TileType.BLOCK_ON_TARGET;
        }

        private void TryMovePlayer(DirectionType direction)
        {
            // プレイヤーの現在地を取得
            var currentPlayerPos = gameObjectPosTable[player];

            // プレイヤーの移動先の位置を計算
            var nextPlayerPos = GetNextPositionAlong(currentPlayerPos, direction);

            // プレイヤーの移動先がステージ内ではない場合は無視
            if (!IsValidPosition(nextPlayerPos)) return;

            // プレイヤーの移動先にブロックが存在する場合
            if (IsBlock(nextPlayerPos))
            {
                // ブロックの移動先の位置を計算
                var nextBlockPos = GetNextPositionAlong(nextPlayerPos, direction);

                // ブロックの移動先がステージ内の場合かつ
                // ブロックの移動先にブロックが存在しない場合
                if (IsValidPosition(nextBlockPos) && !IsBlock(nextBlockPos))
                {
                    // 移動するブロックを取得
                    var block = GetGameObjectAtPosition(new Vector3Int(nextPlayerPos.x,0,nextPlayerPos.y));

                    // プレイヤーの移動先のタイルの情報を更新
                    UpdateGameObjectPosition(nextPlayerPos);
                    
                    Debug.Log("NextPos"+nextPlayerPos);

                    // ブロックを移動
                    block.transform.position = GetDisplayPosition(nextBlockPos.x, 0, nextBlockPos.y);

                    // ブロックの位置を更新
                    gameObjectPosTable[block] = new Vector3Int(nextBlockPos.x,0,nextBlockPos.y);

                    // ブロックの移動先の番号を更新
                    if (tileList[nextBlockPos.x, nextBlockPos.y] == TileType.GROUND)
                    {
                        // 移動先が地面ならブロックの番号に更新
                        tileList[nextBlockPos.x, nextBlockPos.y] = TileType.BLOCK;
                    }
                    else if (tileList[nextBlockPos.x, nextBlockPos.y] == TileType.TARGET)
                    {
                        // 移動先が目的地ならブロック（目的地の上）の番号に更新
                        tileList[nextBlockPos.x, nextBlockPos.y] = TileType.BLOCK_ON_TARGET;
                    }

                    // プレイヤーの現在地のタイルの情報を更新
                    UpdateGameObjectPosition(currentPlayerPos);

                    // プレイヤーを移動
                    player.transform.position = GetDisplayPosition(nextPlayerPos.x, 0, nextPlayerPos.y);

                    // プレイヤーの位置を更新
                    gameObjectPosTable[player] = nextPlayerPos;

                    // プレイヤーの移動先の番号を更新
                    if (tileList[nextPlayerPos.x, nextPlayerPos.y] == TileType.GROUND)
                    {
                        // 移動先が地面ならプレイヤーの番号に更新
                        tileList[nextPlayerPos.x, nextPlayerPos.y] = TileType.PLAYER;
                    }
                    else if (tileList[nextPlayerPos.x, nextPlayerPos.y] == TileType.TARGET)
                    {
                        // 移動先が目的地ならプレイヤー（目的地の上）の番号に更新
                        tileList[nextPlayerPos.x, nextPlayerPos.y] = TileType.PLAYER_ON_TARGET;
                    }
                }
            }
            // プレイヤーの移動先にブロックが存在しない場合
            else
            {
                // プレイヤーの現在地のタイルの情報を更新
                UpdateGameObjectPosition(currentPlayerPos);

                // プレイヤーを移動
                player.transform.position = GetDisplayPosition(nextPlayerPos.x, 0, nextPlayerPos.y);

                // プレイヤーの位置を更新
                gameObjectPosTable[player] = nextPlayerPos;

                // プレイヤーの移動先の番号を更新
                if (tileList[nextPlayerPos.x, nextPlayerPos.y] == TileType.GROUND)
                {
                    // 移動先が地面ならプレイヤーの番号に更新
                    tileList[nextPlayerPos.x, nextPlayerPos.y] = TileType.PLAYER;
                }
                else if (tileList[nextPlayerPos.x, nextPlayerPos.y] == TileType.TARGET)
                {
                    // 移動先が目的地ならプレイヤー（目的地の上）の番号に更新
                    tileList[nextPlayerPos.x, nextPlayerPos.y] = TileType.PLAYER_ON_TARGET;
                }
            }

            // ゲームをクリアしたかどうか確認
            CheckCompletion();
        }

        // 指定された方向の位置を返す
        private Vector3Int GetNextPositionAlong(Vector3Int pos, DirectionType direction)
        {
            switch (direction)
            {
                // 上
                case DirectionType.Ahead:
                    pos.y -= 1;
                    break;

                // 右
                case DirectionType.RIGHT:
                    pos.x += 1;
                    break;

                // 下
                case DirectionType.Back:
                    pos.y += 1;
                    break;

                // 左
                case DirectionType.LEFT:
                    pos.x -= 1;
                    break;
            }

            return pos;
        }

        // 指定された位置のタイルを更新
        private void UpdateGameObjectPosition(Vector3Int pos)
        {
            // 指定された位置のタイルの番号を取得
            var cell = tileList[pos.x, pos.y];

            // プレイヤーもしくはブロックの場合
            if (cell == TileType.PLAYER || cell == TileType.BLOCK)
            {
                // 地面に変更
                tileList[pos.x, pos.y] = TileType.GROUND;
            }
            // 目的地に乗っているプレイヤーもしくはブロックの場合
            else if (cell == TileType.PLAYER_ON_TARGET || cell == TileType.BLOCK_ON_TARGET)
            {
                // 目的地に変更
                tileList[pos.x, pos.y] = TileType.TARGET;
            }
        }

        // ゲームをクリアしたかどうか確認
        private void CheckCompletion()
        {
            // 目的地に乗っているブロックの数を計算
            int blockOnTargetCount = 0;

            for (int z = 0; z < rows; z++)
            {
                for (int x = 0; x < columns; x++)
                {
                    if (tileList[x, z] == TileType.BLOCK_ON_TARGET)
                    {
                        blockOnTargetCount++;
                    }
                }
            }

            // すべてのブロックが目的地の上に乗っている場合
            if (blockOnTargetCount == blockCount)
            {
                // ゲームクリア
                isClear = true;
            }
        }
    }
}