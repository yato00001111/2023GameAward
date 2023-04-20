using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct FallData
{
    public readonly int X { get; }
    public readonly int Y { get; }
    public readonly int Dest { get; } // 落ちる先
    public FallData(int x, int y, int dest)
    {
        X = x;
        Y = y;
        Dest = dest;
    }
}

public class FieldController : MonoBehaviour
{
    public const int FALL_FRAME_PER_CELL = 5;// 単位セル当たりの落下フレーム数
    public const int BOARD_WIDTH = 16;
    public const int BOARD_HEIGHT = 10;

    // 縦座標
    // 真ん中の円のスケールが1.5の場合
    private float[] BLOCK_SCALE = { 0.5733f, 0.6332f, 0.7003f, 0.7664f, 0.8419f, 0.9251f, 1.0100f, 1.10818f, 1.20f, 1.333f, 1.466f, 1.61f };
    // 真ん中の円のスケールが1.0の場合
    //private float[] BLOCK_SCALE = { 0.3833f, 0.4222f, 0.463f, 0.5055f, 0.5533f, 0.6f, 0.65888f, 0.7222f, 0.80f, 0.8955f };
    // 横座標(360と-22.5fは端に行ったときに周回できるように用意した角度)
    private float[] BLOCK_ROTATE = { 0, 22.5f, 45.0f, 67.5f, 90.0f, 112.5f, 135.0f, 157.5f,
                                     180.0f, 202.5f, 225.0f, 247.5f, 270.0f, 292.5f, 315.0f, 337.5f, 360.0f, -22.5f};

    [SerializeField] GameObject prefabBlock = default!;
    [SerializeField] PlayerController[] _playerController = { default!, default! };

    int[,] _board = new int[BOARD_HEIGHT, BOARD_WIDTH];
    int[,] _boardDst = new int[BOARD_HEIGHT, BOARD_WIDTH];
    GameObject[,] _Blocks = new GameObject[BOARD_HEIGHT, BOARD_WIDTH];
    GameObject[,] _BlocksDst = new GameObject[BOARD_HEIGHT, BOARD_WIDTH];

    // 追加された得点を保持
    uint _additiveScore = 0;

    // 落ちる際の一次的変数
    List<FallData> _falls = new();
    int _fallFrames = 0;

    // 削除する際の一次的変数
    List<Vector2Int> _erases = new();
    int _eraseFrames = 0;

    private void ClearAll()
    {
        for (int y = 0; y < BOARD_HEIGHT; y++)
        {
            for (int x = 0; x < BOARD_WIDTH; x++)
            {
                _board[y, x] = 0;
                _boardDst[y, x] = 0;

                if (_Blocks[y, x] != null) Destroy(_Blocks[y, x]);
                _Blocks[y, x] = null;
                if (_BlocksDst[y, x] != null) Destroy(_BlocksDst[y, x]);
                _BlocksDst[y, x] = null;

            }
        }
    }

    public void Start()
    {
        ClearAll();

        // 全マスに置く
        //for (int y = 0; y < BOARD_HEIGHT - 1; y++)
        //{
        //    for (int x = 0; x < BOARD_WIDTH; x++)
        //    {
        //        Settle(new Vector2Int(x, y), Random.Range(1, 7));
        //    }
        //}
    }

    public void Update()
    {
    }

    public static bool IsValidated(Vector2Int pos)
    {
        // 置こうとしている場所は盤面をはみ出していないか
        return 0 <= pos.x && pos.x < BOARD_WIDTH
            && 0 <= pos.y && pos.y < BOARD_HEIGHT;
    }

    public bool CanSettle(Vector2Int pos)
    {
        // 円なので端で座標ループ
        if (pos.x == -1) pos.x = 15;
        if (pos.x == 16) pos.x = 0;
        if (!IsValidated(pos)) return false;

        // 配列の値が埋まっていないか(0になっていないか) 
        return 0 == _board[pos.y, pos.x];
    }


    // 配列「_board」に値を設定するメソッド「Settle」
    public bool Settle(Vector2Int pos, int val)
    {
        // 値を設定する前に置くことができるのかチェック
        if (!CanSettle(pos)) return false;

        _board[pos.y, pos.x] = val;

        Debug.Assert(_Blocks[pos.y, pos.x] == null);
        _Blocks[pos.y, pos.x] = Instantiate(prefabBlock, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.Euler(0, BLOCK_ROTATE[pos.x], 0));
        _Blocks[pos.y, pos.x].transform.localScale = new Vector3(BLOCK_SCALE[pos.y], BLOCK_SCALE[pos.y], BLOCK_SCALE[pos.y]);
        _Blocks[pos.y, pos.x].transform.GetChild(0).GetComponent<BlockController>().SetBlockType((BlockType)val);

        return true;
    }

    //public bool SettlePos(Vector2Int pos)
    //{

    //}

    // 下が空間となっていて落ちるぷよを検索する
    public bool CheckFall()
    {
        _falls.Clear();
        _fallFrames = 0;

        // 落ちる先の高さの記録用
        int[] dsts = new int[BOARD_WIDTH];
        for (int x = 0; x < BOARD_WIDTH; x++) dsts[x] = 0;

        int max_check_line = BOARD_HEIGHT - 1;// 実はぷよぷよ通では最上段は落ちてこない
        for (int y = 0; y < max_check_line; y++)// 下から上に検索
        {
            for (int x = 0; x < BOARD_WIDTH; x++)
            {
                if (_board[y, x] == 0) continue;

                int dst = dsts[x];
                dsts[x] = y + 1;// 上のぷよが落ちてくるなら自分の上

                if (y == 0) continue;// 一番下なら落ちない

                if (_board[y - 1, x] != 0) continue;// 下があれば対象外

                _falls.Add(new FallData(x, y, dst));

                // データを変更しておく
                _board[dst, x] = _board[y, x];
                _board[y, x] = 0;
                _Blocks[dst, x] = _Blocks[y, x];
                _Blocks[y, x] = null;

                dsts[x] = dst + 1;// 次の物は落ちたさらに上に乗る
            }
        }

        return _falls.Count != 0;
    }

    public bool Fall()
    {
        _fallFrames++;

        float dy = _fallFrames / (float)FALL_FRAME_PER_CELL;
        int di = (int)dy;
        for (int i = _falls.Count - 1; 0 <= i; i--)// ループ中で削除しても安全なように後ろから検索
        {
            FallData f = _falls[i];

            Vector3 pos;
            pos.z = 0;
            pos.y = f.Dest;
            pos.x = f.X;
            //Debug.Log("di" + di);

            //Debug.Log("pos.x" + pos.x);
            //Debug.Log("pos.y" + pos.y);
            //Debug.Log("f.Y" + f.Y);
            //Debug.Log("dy" + dy);

            pos.y = f.Y - dy;

            if (f.Y <= f.Dest + di)
            {
                pos.y = f.Dest;
                _falls.RemoveAt(i);
            }
            //_Blocks[f.Dest, f.X].transform.localRotation = Quaternion.Euler(0, BLOCK_ROTATE[(int)pos.x], 0);// 表示位置の更新
            Vector3 p = new Vector3(0, 0, 0);
            if ((f.Y - pos.y) == 1)
            {
                p = Vector3.Lerp(new Vector3(0, (float)BLOCK_SCALE[f.Y], 0), new Vector3(0, (float)BLOCK_SCALE[(int)pos.y], 0), (dy));
            }
            else
            {
                p = Vector3.Lerp(new Vector3(0, (float)BLOCK_SCALE[f.Y], 0), new Vector3(0, (float)BLOCK_SCALE[(int)pos.y], 0), (dy / 2));
            }

            _Blocks[f.Dest, f.X].transform.localScale = new Vector3(p.y, p.y, p.y);// 表示位置の更新

        }

        return _falls.Count != 0;
    }

    // 消したブロックの個数×(連鎖ボーナス＋連結ボーナス＋色数ボーナス)×10
    // ボーナス計算用のテーブル
    static readonly uint[] chainBonusTbl = new uint[] {
        0, 8, 16, 32, 64,
        96, 128, 160, 192, 224,
        256, 288, 320, 352, 384,
        416, 448, 480, 512 };

    static readonly uint[] connectBonusTbl = new uint[] {
        0, 0, 0, 0, 0, 2, 3, 4, 5, 6, 7,
    };

    static readonly uint[] colorBonusTbl = new uint[] {
        0, 3, 6, 12, 24,
    };

    static readonly Vector2Int[] search_tbl = new Vector2Int[] { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
    // 消えるぷよを検索する（同じ色を上下左右に見つけていく。見つけたらフラグを立てて再計算しない）
    public bool CheckErase(int chainCount)
    {
        _eraseFrames = 0;
        _erases.Clear();

        uint[] isChecked = new uint[BOARD_HEIGHT];// メモリを多く使うのは無駄なのでビット処理

        // 得点計算用
        int puyoCount = 0;
        uint colorBits = 0;
        uint connectBonus = 0;

        // add_list は、繋がっている検索処理 get_connection の直前でClearして,get_connectionの中で繋がっているぷよを追加
        // get_connection は再帰処理になるので、delegate
        // 最初のぷよから上下左右を見て、同じ種類のぷよがあれば、そのぷよの上下左右も見るということを続けて、同じ種類の繋がっているぷよを抜き出していく
        // 上下左右を見るのは、それらのオフセットのテーブルを用意しておいて、foreach文で4方向を取り出して検索
        List<Vector2Int> add_list = new();
        int xChainTemp = -1;
        int yChainTemp = -1;
        int typeTemp = -1;
        for (int y = 0; y < BOARD_HEIGHT; y++)
        {
            for (int x = 0; x < BOARD_WIDTH; x++)
            {
                if ((isChecked[y] & (1u << x)) != 0) continue;// 調査済み

                isChecked[y] |= (1u << x);

                int type = _board[y, x];
                if (type == 0) continue;// 空間だった

                puyoCount++;

                System.Action<Vector2Int> get_connection = null;// 再帰で使う場合に必要
                get_connection = (pos) =>
                {
                    add_list.Add(pos);// 削除対象とする

                    foreach (Vector2Int d in search_tbl)
                    {
                        Vector2Int target = pos + d;
                        // ぷよが移動できる範囲にいるかつ同じ種類かつまだ検索していない
                        if (target.x == -1) target.x = 15;
                        if (target.x == 16) target.x = 0;
                        if (target.x < 0 || BOARD_WIDTH <= target.x ||
                            target.y < 0 || BOARD_HEIGHT <= target.y) continue;// 範囲外
                        if (_board[target.y, target.x] != type) continue;// 色違い
                        if ((isChecked[target.y] & (1u << target.x)) != 0) continue;// 検索済み

                        isChecked[target.y] |= (1u << target.x);
                        get_connection(target);
                    }
                };

                add_list.Clear();
                get_connection(new Vector2Int(x, y));

                int xCount = 0;
                int yCount = 0;
                if (3 <= add_list.Count)
                {
                    foreach (Vector2Int d in add_list)
                    {
                        foreach (Vector2Int d2 in add_list)
                        {
                            if (d.y == xChainTemp) continue;
                            if (d.x == yChainTemp) continue;
                            if (d.x == d2.x)
                            {
                                yCount++;
                            }
                            if (d.y == d2.y)
                            {
                                xCount++;
                            }
                        }
                        if (xCount >= 3)
                        {
                            xChainTemp = d.y;
                            typeTemp = type;
                        }
                        if (yCount >= 3)
                        {
                            yChainTemp = d.x;
                            typeTemp = type;
                        }
                        xCount = 0;
                        yCount = 0;
                    }
                    connectBonus += connectBonusTbl[System.Math.Min(add_list.Count, connectBonusTbl.Length - 1)];
                    colorBits |= (1u << type);
                    _erases.AddRange(add_list);
                }
            }
        }
        int reverseX = yChainTemp;
        reverseX = reverseX < 8 ? reverseX + 8 : reverseX - 8;
        for (int y = 0; y < BOARD_HEIGHT; y++)
        {
            for (int x = 0; x < BOARD_WIDTH; x++)
            {
                if (_board[y, x] != typeTemp) continue;
                //Debug.Log("xChainTemp" + xChainTemp);
                //Debug.Log("yChainTemp" + yChainTemp);
                if (xChainTemp == y)
                {
                    _erases.Add(new Vector2Int(x, y));
                }
                if (yChainTemp == x)
                {
                    _erases.Add(new Vector2Int(x, y));
                }
                if (reverseX == x)
                {
                    _erases.Add(new Vector2Int(x, y));
                }
            }
        }
        typeTemp = -1;
        xChainTemp = -1;
        yChainTemp = -1;

        if (chainCount != -1)// 初期化時は得点計算はしない
        {
            // ボーナス計算
            uint colorNum = 0;
            for (; 0 < colorBits; colorBits >>= 1)// 立っているビットの数を数える
            {
                colorNum += (colorBits & 1u);
            }

            uint colorBonus = colorBonusTbl[System.Math.Min(colorNum, colorBonusTbl.Length - 1)];
            uint chainBonus = chainBonusTbl[System.Math.Min(chainCount, chainBonusTbl.Length - 1)];
            uint bonus = System.Math.Max(1, chainBonus + connectBonus + colorBonus);// 0 の時も1入る
            _additiveScore += 10 * (uint)_erases.Count * bonus;

            if (puyoCount == 0) _additiveScore += 1800;// 全消しボーナス
        }

        return _erases.Count != 0;
    }

    public bool Erase()
    {
        _eraseFrames++;

        // 1から増えてちょっとしたら最大に大きくなったあと小さくなって消える
        float t = _eraseFrames * Time.deltaTime;
        t = 1.0f - 10.0f * ((t - 0.1f) * (t - 0.1f) - 0.1f * 0.1f);

        // 大きさが負ならおしまい
        if (t <= 0.0f)
        {
            // データとゲームオブジェクトをここで消す
            foreach (Vector2Int d in _erases)
            {
                Destroy(_Blocks[d.y, d.x]);
                _Blocks[d.y, d.x] = null;
                _board[d.y, d.x] = 0;
            }

            return false;
        }

        // モデルの大きさを変える
        //foreach (Vector2Int d in _erases)
        //{
        //    _Blocks[d.y, d.x].transform.localScale = Vector3.one * t;
        //}

        return true;
    }

    // 9段目にブロックが存在しているか
    public bool CheckDead()
    {
        for (int y = 8; y < BOARD_HEIGHT; y++)
        {
            for (int x = 0; x < BOARD_WIDTH; x++)
            {
                if (_board[y, x] == 0) continue;
                return true;
            }
        }
        return false;
    }

    //void SetTransition(Vector2Int pos, int time)
    //{
    //    // 補間のために保存しておく
    //    // 円なので端と端でループするように
    //    if (is0_15) // x座標0から15
    //    {
    //        _last_position.x = 16;
    //        _last_position.y = _position.y;
    //        is0_15 = false;
    //    }
    //    else if (is15_0) // x座標15から0
    //    {
    //        _last_position.x = 17;
    //        _last_position.y = _position.y;
    //        is15_0 = false;
    //    }
    //    else _last_position = _position;

    //    // 値の更新
    //    _position = pos;

    //    _animationController.Set(time);
    //}

    private bool Translate(bool is_right)
    {
        // 移動先のX軸の記録用

        var trans = (is_right ? Vector2Int.right : Vector2Int.left);

        for (int y = 0; y < BOARD_HEIGHT; y++)
        {
            for (int x = 0; x < BOARD_WIDTH; x++)
            {
                if (_board[y, x] == 0) continue;

                // データを変更しておく
                int posX = x + trans.x;
                if (posX == -1)
                {
                    posX = 15; //x座標の端
                    //is0_15 = true;
                }
                if (posX == 16)
                {
                    posX = 0; //x座標の端
                    //is15_0 = true;
                }
                _boardDst[y, posX] = _board[y, x];
                _BlocksDst[y, posX] = _Blocks[y, x];
                _BlocksDst[y, posX].transform.localRotation = Quaternion.Euler(0, BLOCK_ROTATE[posX], 0);
                for(int i=0; i < 2; i++)
                {
                    if (_playerController[i].GetPos().x == posX && _playerController[i].GetPos().y <= y)
                    {
                        int player_posX = _playerController[i].GetPos().x + trans.x;
                        if (player_posX == -1)
                        {
                            player_posX = 15; //x座標の端
                        }
                        if (player_posX == 16)
                        {
                            player_posX = 0; //x座標の端
                        }
                        _playerController[i].SetPos(new Vector2Int(player_posX, _playerController[i].GetPos().y));
                    }
                }
            }
        }
        for (int y = 0; y < BOARD_HEIGHT; y++)
        {
            for (int x = 0; x < BOARD_WIDTH; x++)
            {
                _board[y, x] = _boardDst[y, x];
                _boardDst[y, x] = 0;
                _Blocks[y, x] = _BlocksDst[y, x];
                _BlocksDst[y, x] = null;
            }
        }

        return true;
    }

    public void Control(LogicalInput _logicalInput)
    {
        // アニメ中はキー入力を受け付けない
        //if (_animationController.Update()) return;

        // 平行移動のキー入力取得
        if (_logicalInput.IsRepeat(LogicalInput.Key.Right) || _logicalInput.IsRepeat(LogicalInput.Key.D))
        {
            if (Translate(true)) return;
        }
        if (_logicalInput.IsRepeat(LogicalInput.Key.Left) || _logicalInput.IsRepeat(LogicalInput.Key.A))
        {
            if (Translate(false)) return;
        }

        // Debug用
        if (Input.GetKey(KeyCode.Z))
        {
            for (int y = 0; y < BOARD_HEIGHT - 1; y++)
            {
                for (int x = 0; x < BOARD_WIDTH; x++)
                {
                    Settle(new Vector2Int(x, y), Random.Range(1, 7));
                }
            }
        }

    }

    // 得点の受け渡し
    public uint popScore()
    {
        uint score = _additiveScore;
        _additiveScore = 0;

        return score;
    }
}
