using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

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
    public const int BOARD_WIDTH = 8;
    public const int BOARD_HEIGHT = 20;

    // 縦座標
    // 真ん中の円のスケールが1.5の場合
    private float[] BLOCK_SCALE = { 0.5733f, 0.6332f, 0.700f, 0.7664f, 0.8419f, 0.9251f, 1.0150f, 1.11f, 1.22f, 1.338f, 1.466f, 1.605f, 1.76f, 1.93f, 2.11f, 2.31f, 2.53f, 2.77f, 3.03f, 3.32f };
    // 真ん中の円のスケールが1.0の場合
    //private float[] BLOCK_SCALE = { 0.3833f, 0.4222f, 0.463f, 0.5055f, 0.5533f, 0.6f, 0.65888f, 0.7222f, 0.80f, 0.8955f };
    // 横座標(360と-22.5fは端に行ったときに周回できるように用意した角度)
    //private float[] BLOCK_ROTATE = { 0, 22.5f, 45.0f, 67.5f, 90.0f, 112.5f, 135.0f, 157.5f,
    //                                 180.0f, 202.5f, 225.0f, 247.5f, 270.0f, 292.5f, 315.0f, 337.5f, 360.0f, -22.5f};
    private float[] BLOCK_ROTATE = { -22.5f, 22.5f,  67.5f,  112.5f, 157.5f,
                                     202.5f, 247.5f, 292.5f, 337.5f, -67.5f};

    [SerializeField] GameObject prefabBlock = default!;
    [SerializeField] PlayerController[] _playerController = { default!, default! };
    [SerializeField] PlayDirector playDirector = default!;
    [SerializeField] UI_Needles needles = default!;

    int[,] _board = new int[BOARD_HEIGHT, BOARD_WIDTH];
    int[,] _boardDst = new int[BOARD_HEIGHT, BOARD_WIDTH];
    GameObject[,] _Blocks = new GameObject[BOARD_HEIGHT, BOARD_WIDTH];
    GameObject[,] _BlocksDst = new GameObject[BOARD_HEIGHT, BOARD_WIDTH];

    AnimationController _animationController = new AnimationController();
    const int TRANS_TIME = 5; // 移動速度遷移時間
    const int HALF_TRANS_TIME = 15; // 移動速度遷移時間

    private bool isControl;

    // 追加された得点を保持
    uint _additiveScore = 0;

    // 落ちる際の一次的変数
    List<FallData> _falls = new();
    int _fallFrames = 0;

    // 削除する際の一次的変数
    List<Vector2Int> _erases = new();
    int _eraseFrames = 0;
    bool isEffect = true;
    [SerializeField] private int _eraseTime = 10;
    [SerializeField] public int _eraseCount;
    [SerializeField] private int _needleEraseCount;

    //
    List<FallData> _rots = new();
    int TransCount = 0;
    bool isHalfTransR;
    bool isHalfTransL;

    //　ノルマ
    [SerializeField] public int _normaCount;

    //SE
    AudioSource audioSource;
    public AudioClip se_block_landing;
    public AudioClip se_erase;
    public AudioClip[] se_erase_block;
    public AudioClip se_kaiten;
    private bool isKaiten = false;

    private bool TriggerFlag;

    int needleX;
    int tempType;
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

        audioSource = GetComponent<AudioSource>();

        isControl = true;
        _animationController.Set(1);
        isKaiten = false;
        isHalfTransR = false;
        isHalfTransL = false;
        _eraseCount = 0;
        _needleEraseCount = 0;
        _normaCount = 0;
        needleX = -1;
        tempType = 0;
        TriggerFlag = false;

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
        if (pos.x == -1) pos.x = BOARD_WIDTH - 1;
        if (pos.x == BOARD_WIDTH) pos.x = 0;
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

        audioSource.PlayOneShot(se_block_landing);

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
    // 消えるブロックを検索する（同じ色を上下左右に見つけていく。見つけたらフラグを立てて再計算しない）
    public bool CheckErase(int chainCount)
    {
        _eraseFrames = 0;
        isEffect = true;
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
        //int xChainTemp = -1;
        //int yChainTemp = -1;
        //int typeTemp = -1;
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
                        if (target.x == -1) target.x = BOARD_WIDTH - 1;
                        if (target.x == BOARD_WIDTH) target.x = 0;
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

                //int xCount = 0;
                //int yCount = 0;
                if (3 <= add_list.Count)
                {
                    //foreach (Vector2Int d in add_list)
                    //{
                    //    foreach (Vector2Int d2 in add_list)
                    //    {
                    //        if (d.y == xChainTemp) continue;
                    //        if (d.x == yChainTemp) continue;
                    //        if (d.x == d2.x)
                    //        {
                    //            yCount++;
                    //        }
                    //        if (d.y == d2.y)
                    //        {
                    //            xCount++;
                    //        }
                    //    }
                    //    if (xCount >= 3)
                    //    {
                    //        xChainTemp = d.y;
                    //        typeTemp = type;
                    //    }
                    //    if (yCount >= 3)
                    //    {
                    //        yChainTemp = d.x;
                    //        typeTemp = type;
                    //    }
                    //    xCount = 0;
                    //    yCount = 0;
                    //}
                    connectBonus += connectBonusTbl[System.Math.Min(add_list.Count, connectBonusTbl.Length - 1)];
                    colorBits |= (1u << type);
                    _erases.AddRange(add_list);
                }
            }
        }
        //int reverseX = yChainTemp;
        //reverseX = reverseX < 8 ? reverseX + 8 : reverseX - 8;
        //for (int y = 0; y < BOARD_HEIGHT; y++)
        //{
        //    for (int x = 0; x < BOARD_WIDTH; x++)
        //    {
        //        if (_board[y, x] != typeTemp) continue;
        //        //Debug.Log("xChainTemp" + xChainTemp);
        //        //Debug.Log("yChainTemp" + yChainTemp);
        //        if (xChainTemp == y)
        //        {
        //            _erases.Add(new Vector2Int(x, y));
        //        }
        //        if (yChainTemp == x)
        //        {
        //            _erases.Add(new Vector2Int(x, y));
        //        }
        //        if (reverseX == x)
        //        {
        //            _erases.Add(new Vector2Int(x, y));
        //        }
        //    }
        //}
        //typeTemp = -1;
        //xChainTemp = -1;
        //yChainTemp = -1;

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
        _eraseCount = _erases.Count;
        if(_eraseCount != 0) _normaCount = _eraseCount / 3;
        return _erases.Count != 0;
    }

    public bool Erase(bool isDisappearPhase)
    {
        _eraseFrames++;
        //// 1から増えてちょっとしたら最大に大きくなったあと小さくなって消える
        //float t = _eraseFrames * Time.deltaTime;
        //t = 1.0f - 10.0f * ((t - 0.1f) * (t - 0.1f) - 0.1f * 0.1f);

        // 大きさが負ならおしまい
        //if (_eraseFrames >= 30.0f)
        //{
        //    // データとゲームオブジェクトをここで消す
        //    foreach (Vector2Int d in _erases)
        //    {
        //        Destroy(_Blocks[d.y, d.x]);
        //        _Blocks[d.y, d.x] = null;
        //        _board[d.y, d.x] = 0;
        //    }

        //    return false;
        //}

        //if(isEffect)
        //{
        //    foreach (Vector2Int d in _erases)
        //    {
        //        int type = _board[d.y, d.x];
        //        _Blocks[d.y, d.x].transform.GetChild(1).GetComponent<EffectController>().PlayEffect(type);
        //    }
        //    audioSource.PlayOneShot(se_erase);
        //    isEffect = false;
        //}
        //if (_eraseFrames > _eraseTime)
        //{
        //    foreach (Vector2Int d in _erases)
        //    {
        //        Destroy(_Blocks[d.y, d.x]);
        //        _Blocks[d.y, d.x] = null;
        //        _board[d.y, d.x] = 0;

        //    }
        //    return false;
        //}

        //return true;

        if (isDisappearPhase)
        {
            foreach (Vector2Int d in _erases)
            {
                if (needles.GetCurrentNumber() == d.x)
                {
                    if (_Blocks[d.y, d.x] == null) continue;
                    //DelayDestroy(d.x, d.y);
                    if (isEffect)
                    {
                        int type = _board[d.y, d.x];
                        _Blocks[d.y, d.x].transform.Find("effect").GetComponent<EffectController>().PlayEffect(type);
                        audioSource.PlayOneShot(se_erase_block[type]);

                    }
                    //if (_eraseFrames > _eraseTime)
                    {
                        Destroy(_Blocks[d.y, d.x]);
                        _Blocks[d.y, d.x] = null;
                        _board[d.y, d.x] = 0;
                        _needleEraseCount++;
                    }
                }
            }
        }
        if (_needleEraseCount == _eraseCount)
        {
            _needleEraseCount = 0;
            return false;
        }
        return true;
    }

    private async void DelayDestroy(int x, int y)
    {
        _Blocks[y, x].transform.Find("effect").GetComponent<EffectController>().PlayEffect(_board[y, x]);
        await Task.Delay(500);
        Destroy(_Blocks[y, x]);
        _Blocks[y, x] = null;
        _board[y, x] = 0;
    }

    // 9段目にブロックが存在しているか
    public void CheckDead()
    {
        for (int y = 8; y < BOARD_HEIGHT; y++)
        {
            for (int x = 0; x < BOARD_WIDTH; x++)
            {
                if (_board[y, x] == 0) continue;
                DelayDestroy(x, y);

            }
        }
    }

    public void EraseBlockSound()
    {     
        foreach (Vector2Int d in _erases)
        {
            if (needles.GetCurrentNumber() == d.x)
            {
                if (needleX == needles.GetCurrentNumber()) continue;
                needleX = needles.GetCurrentNumber();
                //if (tempType == _board[d.y, d.x]) continue;
                tempType = _board[d.y, d.x];
                //Debug.Log("type" + tempType);
                audioSource.PlayOneShot(se_erase_block[tempType]);
            }
        }

    }

    private bool Translate(bool is_right)
    {
        _animationController.Set(TRANS_TIME);
        _rots.Clear();

        // 移動先のX軸の記録用
        var trans = (is_right ? Vector2Int.right : Vector2Int.left);

        for (int y = 0; y < BOARD_HEIGHT; y++)
        {
            for (int x = 0; x < BOARD_WIDTH; x++)
            {
                if (_board[y, x] == 0) continue;

                // データを変更しておく
                int posX = x + trans.x;
                _rots.Add(new FallData(x, y, posX));

                if (posX == -1)
                {
                    posX = BOARD_WIDTH - 1; //x座標の端
                    //is0_15 = true;
                }
                if (posX == BOARD_WIDTH)
                {
                    posX = 0; //x座標の端
                    //is15_0 = true;
                }

                _boardDst[y, posX] = _board[y, x];

                _BlocksDst[y, posX] = _Blocks[y, x];

                for (int i=0; i < 2; i++)
                {
                    if (_playerController[i].GetPos().x == posX && _playerController[i].GetPos().y <= y)
                    {
                        if (is_right)
                        {
                            _playerController[i].SetisTransR(true);
                        }
                        if (!is_right)
                        {
                            _playerController[i].SetisTransL(true);
                        }
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

        if (!isKaiten)
        {
            //audioSource.PlayOneShot(se_kaiten);
            isKaiten = true;
        }
        return true;
    }

    int delay = 0;
    private bool HalfTranslate(bool is_right)
    {
        if (_animationController.Update()) return true;
        if (TransCount < 4)
        {
            delay++;
            //if(delay > 3)
            {
                Translate(is_right);
                TransCount++;
                delay = 0;
            }
        }
        
        return true;
    }

    public void Control(LogicalInput _logicalInput)
    {
        if (isHalfTransR) HalfTranslate(true);
        if (isHalfTransL) HalfTranslate(false);
        if (TransCount == 4) 
        {
            TransCount = 0;
            isHalfTransR = false;
            isHalfTransL = false;
            for (int i = 0; i < 2; i++)
            {
                _playerController[i].SetisHalfTrans(false);
            }
        }
        // アニメ中はキー入力を受け付けない
        if (_animationController.Update()) return;


        float TrigerInput = Input.GetAxisRaw("Trigger");
        if(TriggerFlag)
        {
            if(TrigerInput == 0) TriggerFlag = false;
        }

        if (!isHalfTransR && !isHalfTransL)
        {
            if (Input.GetKeyDown(KeyCode.D) || (TrigerInput > 0.0f && !TriggerFlag))
            {
                if (playDirector.GetPlayFlag())
                {
                    for (int i = 0; i < 2; i++)
                    {
                        _playerController[i].SetisHalfTrans(true);
                    }
                    isHalfTransR = true;
                    TriggerFlag = true;
                    return;
                }
            }
            if (Input.GetKeyDown(KeyCode.A) || (TrigerInput < 0.0f && !TriggerFlag))
            {
                if (playDirector.GetPlayFlag())
                {
                    for (int i = 0; i < 2; i++)
                    {
                        _playerController[i].SetisHalfTrans(true);
                    }
                    isHalfTransL = true;
                    TriggerFlag = true;
                    return;
                }
            }
            // 平行移動のキー入力取得
            if (_logicalInput.IsTrigger(LogicalInput.Key.Right) ||
                _logicalInput.IsTrigger(LogicalInput.Key.RB))
            {
                if(playDirector.GetPlayFlag())
                    if (Translate(true)) return;
            }
            if (_logicalInput.IsTrigger(LogicalInput.Key.Left) ||
                _logicalInput.IsTrigger(LogicalInput.Key.LB))
            {
                if (playDirector.GetPlayFlag())
                    if (Translate(false)) return;
            }
            if (_logicalInput.IsRelease(LogicalInput.Key.Right) ||
                _logicalInput.IsRelease(LogicalInput.Key.RB) || TrigerInput == 0)
            {
                if (playDirector.GetPlayFlag())
                    isKaiten = false;
            }
            if (_logicalInput.IsRelease(LogicalInput.Key.Left) ||
                _logicalInput.IsRelease(LogicalInput.Key.LB))
            {
                if (playDirector.GetPlayFlag())
                    isKaiten = false;
            }
        }

        // Debug用
        //if (Input.GetKey(KeyCode.Z))
        //{
        //    for (int y = 0; y < BOARD_HEIGHT - 1; y++)
        //    {
        //        for (int x = 0; x < BOARD_WIDTH; x++)
        //        {
        //            Settle(new Vector2Int(x, y), Random.Range(1, 6));
        //        }
        //    }
        //}
    }

    public void AnimeTrans()
    {
        float anim_rate = _animationController.GetNormalized();
        //for (int i = 0; i < 2; i++)
        //{
        //    _playerController[i].SetAnimLate(anim_rate);
        //}

        float[,] LerpRot = new float[BOARD_HEIGHT, BOARD_WIDTH];

        float rot;
        for (int i = _rots.Count - 1; 0 <= i; i--)// ループ中で削除しても安全なように後ろから検索
        {
            FallData r = _rots[i];
            //Vector3 pos;
            //pos.z = 0;
            //pos.y = r.Dest;
            //pos.x = r.X;

            if (anim_rate <= 0)
            {
                _rots.RemoveAt(i);
            }

            if (r.Dest == -1) rot = Mathf.Lerp(BLOCK_ROTATE[BOARD_WIDTH + 1], BLOCK_ROTATE[r.X], anim_rate);
            else if (r.Dest == BOARD_WIDTH) rot = Mathf.Lerp(BLOCK_ROTATE[BOARD_WIDTH], BLOCK_ROTATE[r.X], anim_rate);
            else rot = Mathf.Lerp(BLOCK_ROTATE[r.Dest], BLOCK_ROTATE[r.X], anim_rate);
            //Debug.Log("r.Y" + r.Y);
            //Debug.Log("r.Dest" + r.Dest);
            //Debug.Log("anim_rate" + anim_rate);

            int destX;
            if (r.Dest == -1)
            {
                destX = BOARD_WIDTH - 1; //x座標の端
                            //is0_15 = true;
            }
            else if (r.Dest == BOARD_WIDTH)
            {
                destX = 0; //x座標の端
                            //is15_0 = true;
            }
            else destX = r.Dest;
            _Blocks[r.Y, destX].transform.localRotation = Quaternion.Euler(0, rot, 0);// 表示位置の更新

        }
    }

    public void TransUpdate(LogicalInput _logicalInput)
    {
        if (isControl)
        {
            Control(_logicalInput);
            AnimeTrans();
        }
    }

    // 得点の受け渡し
    public uint popScore()
    {
        uint score = _additiveScore;
        _additiveScore = 0;

        return score;
    }

    public void SetControl(bool control)
    {
        isControl = control;
    }
    public bool GetControl()
    {
        return isControl;
    }

    public void SetTutorial()
    {
        // Tutorial(仕様変更後)

        //Settle(new Vector2Int(6, 0), (int)BlockType.Blue);
        //Settle(new Vector2Int(6, 1), (int)BlockType.Blue);

        //Settle(new Vector2Int(2, 0), (int)BlockType.Green);
        Settle(new Vector2Int(2, 1), (int)BlockType.Green);

        // Tutorial(仕様変更前)
        //Settle(new Vector2Int(0, 0), (int)BlockType.Purple);

        //Settle(new Vector2Int(1, 0), (int)BlockType.Red);
        //Settle(new Vector2Int(1, 1), (int)BlockType.Purple);
        //Settle(new Vector2Int(1, 2), (int)BlockType.Red);
        //Settle(new Vector2Int(1, 3), (int)BlockType.Green);

        //Settle(new Vector2Int(2, 0), (int)BlockType.Yellow);
        //Settle(new Vector2Int(2, 1), (int)BlockType.Red);
        //Settle(new Vector2Int(2, 2), (int)BlockType.Yellow);
        //Settle(new Vector2Int(2, 3), (int)BlockType.Blue);

        //Settle(new Vector2Int(3, 0), (int)BlockType.Green);
        //Settle(new Vector2Int(3, 1), (int)BlockType.Yellow);
        //Settle(new Vector2Int(3, 2), (int)BlockType.Green);
        //Settle(new Vector2Int(3, 3), (int)BlockType.Purple);

        //Settle(new Vector2Int(4, 0), (int)BlockType.Blue);
        //Settle(new Vector2Int(4, 1), (int)BlockType.Green);
        //Settle(new Vector2Int(4, 2), (int)BlockType.Blue);
        //Settle(new Vector2Int(4, 3), (int)BlockType.Red);

        //Settle(new Vector2Int(5, 0), (int)BlockType.Purple);
        //Settle(new Vector2Int(5, 1), (int)BlockType.Blue);
        //Settle(new Vector2Int(5, 2), (int)BlockType.Purple);
        //Settle(new Vector2Int(5, 3), (int)BlockType.Yellow);

        //Settle(new Vector2Int(6, 0), (int)BlockType.Red);
        //Settle(new Vector2Int(6, 1), (int)BlockType.Purple);
        //Settle(new Vector2Int(6, 2), (int)BlockType.Red);
        //Settle(new Vector2Int(6, 3), (int)BlockType.Purple);

        //Settle(new Vector2Int(7, 0), (int)BlockType.Yellow);
        //Settle(new Vector2Int(7, 1), (int)BlockType.Red);
        //Settle(new Vector2Int(7, 2), (int)BlockType.Yellow);
        //Settle(new Vector2Int(7, 3), (int)BlockType.Blue);
        //Settle(new Vector2Int(7, 4), (int)BlockType.Yellow);
        //Settle(new Vector2Int(7, 5), (int)BlockType.Blue);
        //Settle(new Vector2Int(7, 6), (int)BlockType.Blue);

        //Settle(new Vector2Int(8, 0), (int)BlockType.Green);
        //Settle(new Vector2Int(8, 1), (int)BlockType.Yellow);
        //Settle(new Vector2Int(8, 2), (int)BlockType.Green);
        //Settle(new Vector2Int(8, 3), (int)BlockType.Purple);

        //Settle(new Vector2Int(9, 0), (int)BlockType.Blue);
        //Settle(new Vector2Int(9, 1), (int)BlockType.Green);
        //Settle(new Vector2Int(9, 2), (int)BlockType.Blue);
        //Settle(new Vector2Int(9, 3), (int)BlockType.Red);

        //Settle(new Vector2Int(10, 0), (int)BlockType.Purple);
        //Settle(new Vector2Int(10, 1), (int)BlockType.Blue);
        //Settle(new Vector2Int(10, 2), (int)BlockType.Purple);
        //Settle(new Vector2Int(10, 3), (int)BlockType.Yellow);

        //Settle(new Vector2Int(11, 0), (int)BlockType.Red);
        //Settle(new Vector2Int(11, 1), (int)BlockType.Purple);
        //Settle(new Vector2Int(11, 2), (int)BlockType.Red);
        //Settle(new Vector2Int(11, 3), (int)BlockType.Green);

        //Settle(new Vector2Int(12, 0), (int)BlockType.Yellow);
        //Settle(new Vector2Int(12, 1), (int)BlockType.Red);
        //Settle(new Vector2Int(12, 2), (int)BlockType.Yellow);
        //Settle(new Vector2Int(12, 3), (int)BlockType.Blue);

        //Settle(new Vector2Int(13, 0), (int)BlockType.Green);
        //Settle(new Vector2Int(13, 1), (int)BlockType.Yellow);
        //Settle(new Vector2Int(13, 2), (int)BlockType.Green);
        //Settle(new Vector2Int(13, 3), (int)BlockType.Purple);

        //Settle(new Vector2Int(14, 0), (int)BlockType.Blue);
        //Settle(new Vector2Int(14, 1), (int)BlockType.Green);
        //Settle(new Vector2Int(14, 2), (int)BlockType.Blue);
        //Settle(new Vector2Int(14, 3), (int)BlockType.Red);

        //Settle(new Vector2Int(15, 0), (int)BlockType.Yellow);
        //Settle(new Vector2Int(15, 1), (int)BlockType.Blue);
        //Settle(new Vector2Int(15, 2), (int)BlockType.Yellow);
        //Settle(new Vector2Int(15, 3), (int)BlockType.Yellow);
    }
}
