using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 移動制御
    const int TRANS_TIME = 3; // 移動速度遷移時間

    // 落下制御
    const int FALL_COUNT_UNIT = 120; // ひとマス落下するカウント数
    [SerializeField] int FALL_COUNT_SPD = 5; // 落下速度
    const int FALL_COUNT_FAST_SPD = 13; // 高速落下時の速度
    const int GROUND_FRAMES = 15; // 接地移動可能時間

    int _fallCount = 0;
    int _groundFrame = GROUND_FRAMES;// 接地時間

    bool is0_15 = false;
    bool is15_0 = false;

    private bool isPause = false;

    [SerializeField] FieldController fieldController = default!;
    [SerializeField] BlockController _blockController = default!;

    Vector2Int _position;// blockの位置

    AnimationController _animationController = new AnimationController();
    Vector2Int _last_position; // 遷移前の位置

    LogicalInput _logicalInput = null;

    // 得点
    uint _additiveScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        is0_15 = false;
        is15_0 = false;
        isPause = false;
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

        // ぷよをだす
        _blockController.SetBlockType(axis);
        //_blockController.SetBlockType(child);

        _blockController.SetPos(new Vector3Int(_position.x, _position.y, 0));

        gameObject.SetActive(true);

        return true;
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
            _last_position.x = 16;
            _last_position.y = _position.y;
            is0_15 = false;
        }
        else if (is15_0) // x座標15から0
        {
            _last_position.x = 17;
            _last_position.y = _position.y;
            is15_0 = false;
        }
        else _last_position = _position;

        // 値の更新
        _position = pos;

        _animationController.Set(time);
    }

    private bool Translate(bool is_right)
    {
        // 仮想的に移動できるか検証する
        Vector2Int pos = _position + (is_right ? Vector2Int.right : Vector2Int.left);
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
        //_position = pos;
        SetTransition(pos, TRANS_TIME);

        //_blockController.SetPos(new Vector3Int(_position.x, _position.y, 0));

        return true;
    }

    void Settle()
    {
        // 直接接地
        bool is_set0 = fieldController.Settle(_position, (int)_blockController.GetBlockType());
        Debug.Assert(is_set0);// 置いたのは空いていた場所のはず

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
        _fallCount -= is_fast ? FALL_COUNT_FAST_SPD : FALL_COUNT_SPD;

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

    void CheckSide()
    {

    }

    void Control()
    {
        // 横判定
        CheckSide();

        // 落とす
        if (!Fall(_logicalInput.IsRaw(LogicalInput.Key.Down))) return;// 接地したら終了

        // アニメ中はキー入力を受け付けない
        if (_animationController.Update()) return;

        // 平行移動のキー入力取得
        //if (_logicalInput.IsRepeat(LogicalInput.Key.Right))
        //{
        //    if (Translate(true)) return;
        //}
        //if (_logicalInput.IsRepeat(LogicalInput.Key.Left))
        //{
        //    if (Translate(false)) return;
        //}

        // クイックドロップのキー入力取得
        if (_logicalInput.IsRelease(LogicalInput.Key.QuickDrop) || _logicalInput.IsRelease(LogicalInput.Key.JoyA))
        {
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
        float anim_rate = _animationController.GetNormalized();
        _blockController.SetPosInterpolate(_position, _last_position, anim_rate, dy.y);
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
}
