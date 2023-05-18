using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 移動制御
    const int TRANS_TIME = 5; // 移動速度遷移時間
    const int HALF_TRANS_TIME = 15; // 移動速度遷移時間

    public const int BOARD_WIDTH = 8;
    public const int BOARD_HEIGHT = 20;

    // 落下制御
    const int FALL_COUNT_UNIT = 120; // ひとマス落下するカウント数
    [SerializeField] float FALL_COUNT_SPD = 5; // 落下速度
    const int FALL_COUNT_FAST_SPD = 13; // 高速落下時の速度
    const int GROUND_FRAMES = 15; // 接地移動可能時間

    float _fallCount = 0;
    int _groundFrame = GROUND_FRAMES;// 接地時間

    bool is0_15 = false;
    bool is15_0 = false;

    private bool isPause = false;
    private bool isQuick = false;

    [SerializeField] FieldController fieldController = default!;
    [SerializeField] BlockController[] _blockControllers = new BlockController[2] { default!, default! };
    [SerializeField] PlayDirector playDirector = default!;

    Vector2Int _position;// blockの位置

    AnimationController _animationController = new AnimationController();
    Vector2Int _last_position; // 遷移前の位置
    float anim_rate;

    bool isTransR;
    bool isTransL;
    bool isHalfTrans;
    [SerializeField] PlayerController _OtherPlayer = default!;


    LogicalInput _logicalInput = null;

    // 得点
    uint _additiveScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        is0_15 = false;
        is15_0 = false;
        isPause = false;
        isQuick = false;
        isTransR = false;
        isTransL = false;
        isHalfTrans = false;
        anim_rate = 1;
        gameObject.SetActive(false);// ぷよの種類が設定されるまで眠る
    }

    public void SetLogicalInput(LogicalInput reference)
    {
        _logicalInput = reference;
    }

    // 新しくぷよをだす
    public bool Spawn(BlockType axis, BlockType child, Vector2Int position)
    {
        // 初期位置に出せるか確認
        if (!CanMove(position)) return false;

        // パラメータの初期化
        _position = _last_position = position;
        _animationController.Set(1);
        _fallCount = 0;
        _groundFrame = GROUND_FRAMES;

        _blockControllers[0].SetBlockType(axis);
        _blockControllers[1].SetBlockType(child);

        _blockControllers[0].SetPos(new Vector3Int(_position.x, _position.y, 0));
        Vector2Int posChild = CalcChildPuyoPos(_position);
        _blockControllers[1].SetPos(new Vector3Int(posChild.x, posChild.y - 1, 0));

        gameObject.SetActive(true);

        return true;
    }

    private static Vector2Int CalcChildPuyoPos(Vector2Int pos)
    {
        return pos + new Vector2Int(0, 1);
    }

    private bool CanMove(Vector2Int pos)
    {
        if (!fieldController.CanSettle(pos)) return false;
        //if (!fieldController.CanSettle(pos + Vector2Int.up)) return false;

        return true;
    }

    void SetTransition(Vector2Int pos, int time)
    {
        // 補間のために保存しておく
        // 円なので端と端でループするように
        if (is0_15) // x座標0から15
        {
            _last_position.x = BOARD_WIDTH;
            _last_position.y = _position.y;
            is0_15 = false;
        }
        else if (is15_0) // x座標15から0
        {
            _last_position.x = BOARD_WIDTH + 1;
            _last_position.y = _position.y;
            is15_0 = false;
        }
        else _last_position = _position;

        // 値の更新
        _position = pos;

        _animationController.Set(time);

        isTransR = false;
        isTransL = false;
    }

    private bool Translate(bool is_right)
    {
        // 仮想的に移動できるか検証する
        Vector2Int pos = _position + (is_right ? Vector2Int.right : Vector2Int.left);
        if (!CanMove(pos)) return false;
        // 円なので端と端でループするように
        if (pos.x == -1)
        {
            pos.x = BOARD_WIDTH - 1; //x座標の端
            is0_15 = true;
        }
        if (pos.x == BOARD_WIDTH)
        {
            pos.x = 0; //x座標の端
            is15_0 = true;
        }

        // 横判定
        //CheckSide(pos, is_right);

        // 実際に移動
        //_position = pos;
        if (isHalfTrans) SetTransition(pos, TRANS_TIME - 3);
        else SetTransition(pos, TRANS_TIME);

        //_blockController.SetPos(new Vector3Int(_position.x, _position.y, 0));

        return true;
    }
    private bool HalfTranslate()
    {
        Debug.Log("HalfTranslate");

        // 仮想的に移動できるか検証する
        Vector2Int pos;
        if (_position.x < 8) pos = _position + new Vector2Int(8, 0);
        else pos = _position + new Vector2Int(-8, 0);
        if (!CanMove(pos)) return false;
        // 円なので端と端でループするように
        if (pos.x == -1)
        {
            pos.x = 15; //x座標の端
            is0_15 = true;
        }
        if (pos.x == 16)
        {
            pos.x = 0; //x座標の端
            is15_0 = true;
        }

        // 実際に移動
        SetTransition(pos, HALF_TRANS_TIME);
        return true;
    }

    void Settle()
    {
        // 直接接地
        bool is_set0 = fieldController.Settle(_position, (int)_blockControllers[0].GetBlockType());
        Debug.Assert(is_set0);// 置いたのは空いていた場所のはず
        bool is_set1 = fieldController.Settle(CalcChildPuyoPos(_position), (int)_blockControllers[1].GetBlockType());
        Debug.Assert(is_set1);// 置いたのは空いていた場所のはず

        gameObject.SetActive(false);
    }

    void QuickDrop()
    {
        // 落ちれる一番下まで落ちる
        Vector2Int pos = _position;
        do
        {
            pos += Vector2Int.down;
        } while (CanMove(pos));
        pos -= Vector2Int.down;// 一つ上の場所（最後に置けた場所）に戻す

        _position = pos;

        Settle();
    }


    bool Fall(bool is_fast)
    {
        //_fallCount -= is_fast ? FALL_COUNT_FAST_SPD : FALL_COUNT_SPD;
        _fallCount -= FALL_COUNT_SPD;

        // ブロックを飛び越えたら、行けるのかチェック
        while (_fallCount < 0)// ブロックが飛ぶ可能性がないこともない気がするので複数落下に対応
        {
            if (!CanMove(_position + Vector2Int.down))
            {
                // 落ちれないなら
                _fallCount = 0; // 動きを止める
                if (0 < --_groundFrame) return true;// 時間があるなら、移動・回転可能

                // 時間切れになったら本当に固定
                Settle();
                return false;
            }

            // 落ちれるなら下に進む
            _last_position.y = _position.y;
            _position += Vector2Int.down;
            _fallCount += FALL_COUNT_UNIT;
        }

        if (is_fast) _additiveScore++; // 下に入れて、落ちれるときはボーナス追加

        return true;
    }

    void CheckSide(Vector2Int pos, bool is_right)
    {
        if(_OtherPlayer.GetPos().x == pos.x)
        {
            if(is_right)_OtherPlayer.SetisTransR(true);
            if(!is_right)_OtherPlayer.SetisTransL(true);
        }
    }

    void ChangeColor()
    {
        var temp0 = _blockControllers[0].GetBlockType();
        var temp1 = _blockControllers[1].GetBlockType();
        _blockControllers[0].SetBlockType(temp1);
        _blockControllers[1].SetBlockType(temp0);
    }

    void Control()
    {
        // 落とす
        if (!Fall(_logicalInput.IsRaw(LogicalInput.Key.Down))) return;// 接地したら終了

        // アニメ中はキー入力を受け付けない
        if (_animationController.Update()) return;

        //if(isHalfTrans)
        //{
        //    if (HalfTranslate()) return;
        //}
        if (isTransR)
        {
            // 平行移動のキー入力取得
            //if (_logicalInput.IsRepeat(LogicalInput.Key.Right))
            {
                if (Translate(true)) return;
            }
        }
        if(isTransL)
        {
            //if (_logicalInput.IsRepeat(LogicalInput.Key.Left))
            {
                if (Translate(false)) return;
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.Joystick1Button2))
        {
            if (playDirector.GetPlayFlag())
                ChangeColor();
        }

        if (isQuick) return;
        // クイックドロップのキー入力取得
        if (_logicalInput.IsTrigger(LogicalInput.Key.QuickDrop) || _logicalInput.IsTrigger(LogicalInput.Key.JoyA))
        {
            if (playDirector.GetPlayFlag())
                QuickDrop();
        }
    }

    void FixedUpdate()
    {
        if (isPause) return;

        // 操作を受けて動かす
        Control();

        // 表示
        Vector3 dy = Vector3.up * (float)_fallCount / (float)FALL_COUNT_UNIT;
        anim_rate = _animationController.GetNormalized();
        //Debug.Log("anim_rate" + anim_rate);
        //Debug.Log("_position" + _position);
        //Debug.Log("_last_position" + _last_position);
        _blockControllers[0].SetPosInterpolate(_position, _last_position, anim_rate, dy.y);
        _blockControllers[1].SetPosInterpolate(CalcChildPuyoPos(_position), CalcChildPuyoPos(_last_position), anim_rate, dy.y);
        //_blockController.SetRotInterpolate(_position, _last_position, anim_rate);
    }


    static Vector3 Interpolate(Vector2Int pos, Vector2Int pos_last, float rate)
    {
        // 平行移動
        Vector3 p = Vector3.Lerp(
            new Vector3((float)pos.x, (float)pos.y, 0.0f),
            new Vector3((float)pos_last.x, (float)pos_last.y, 0.0f), rate);


        return p;
    }

    public Vector2Int GetPos()
    {
        return _position;
    }

    public void SetPos(Vector2Int pos)
    {
        _position = pos;
    }
    public void SetLastPos(Vector2Int pos)
    {
        _last_position = pos;
    }
    public void SetAnimLate(float rate)
    {
        anim_rate = rate;
    }
    public void SetisTransR(bool transR)
    {
        isTransR = transR;
    }
    public void SetisTransL(bool transL)
    {
        isTransL = transL;
    }
    public void SetisHalfTrans(bool trans)
    {
        isHalfTrans = trans;
    }

    // 得点の受け渡し
    public uint popScore()
    {
        uint score = _additiveScore;
        _additiveScore = 0;

        return score;
    }

    public void SetPlayerPause(bool pause)
    {
        isPause = pause;
    }

    public void SetPlayerQuick(bool quick)
    {
        isQuick = quick;
    }

}
