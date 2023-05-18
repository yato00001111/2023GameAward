using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // �ړ�����
    const int TRANS_TIME = 5; // �ړ����x�J�ڎ���
    const int HALF_TRANS_TIME = 15; // �ړ����x�J�ڎ���

    public const int BOARD_WIDTH = 8;
    public const int BOARD_HEIGHT = 20;

    // ��������
    const int FALL_COUNT_UNIT = 120; // �Ђƃ}�X��������J�E���g��
    [SerializeField] float FALL_COUNT_SPD = 5; // �������x
    const int FALL_COUNT_FAST_SPD = 13; // �����������̑��x
    const int GROUND_FRAMES = 15; // �ڒn�ړ��\����

    float _fallCount = 0;
    int _groundFrame = GROUND_FRAMES;// �ڒn����

    bool is0_15 = false;
    bool is15_0 = false;

    private bool isPause = false;
    private bool isQuick = false;

    [SerializeField] FieldController fieldController = default!;
    [SerializeField] BlockController[] _blockControllers = new BlockController[2] { default!, default! };
    [SerializeField] PlayDirector playDirector = default!;

    Vector2Int _position;// block�̈ʒu

    AnimationController _animationController = new AnimationController();
    Vector2Int _last_position; // �J�ڑO�̈ʒu
    float anim_rate;

    bool isTransR;
    bool isTransL;
    bool isHalfTrans;
    [SerializeField] PlayerController _OtherPlayer = default!;


    LogicalInput _logicalInput = null;

    // ���_
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
        gameObject.SetActive(false);// �Ղ�̎�ނ��ݒ肳���܂Ŗ���
    }

    public void SetLogicalInput(LogicalInput reference)
    {
        _logicalInput = reference;
    }

    // �V�����Ղ������
    public bool Spawn(BlockType axis, BlockType child, Vector2Int position)
    {
        // �����ʒu�ɏo���邩�m�F
        if (!CanMove(position)) return false;

        // �p�����[�^�̏�����
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
        // ��Ԃ̂��߂ɕۑ����Ă���
        // �~�Ȃ̂Œ[�ƒ[�Ń��[�v����悤��
        if (is0_15) // x���W0����15
        {
            _last_position.x = BOARD_WIDTH;
            _last_position.y = _position.y;
            is0_15 = false;
        }
        else if (is15_0) // x���W15����0
        {
            _last_position.x = BOARD_WIDTH + 1;
            _last_position.y = _position.y;
            is15_0 = false;
        }
        else _last_position = _position;

        // �l�̍X�V
        _position = pos;

        _animationController.Set(time);

        isTransR = false;
        isTransL = false;
    }

    private bool Translate(bool is_right)
    {
        // ���z�I�Ɉړ��ł��邩���؂���
        Vector2Int pos = _position + (is_right ? Vector2Int.right : Vector2Int.left);
        if (!CanMove(pos)) return false;
        // �~�Ȃ̂Œ[�ƒ[�Ń��[�v����悤��
        if (pos.x == -1)
        {
            pos.x = BOARD_WIDTH - 1; //x���W�̒[
            is0_15 = true;
        }
        if (pos.x == BOARD_WIDTH)
        {
            pos.x = 0; //x���W�̒[
            is15_0 = true;
        }

        // ������
        //CheckSide(pos, is_right);

        // ���ۂɈړ�
        //_position = pos;
        if (isHalfTrans) SetTransition(pos, TRANS_TIME - 3);
        else SetTransition(pos, TRANS_TIME);

        //_blockController.SetPos(new Vector3Int(_position.x, _position.y, 0));

        return true;
    }
    private bool HalfTranslate()
    {
        Debug.Log("HalfTranslate");

        // ���z�I�Ɉړ��ł��邩���؂���
        Vector2Int pos;
        if (_position.x < 8) pos = _position + new Vector2Int(8, 0);
        else pos = _position + new Vector2Int(-8, 0);
        if (!CanMove(pos)) return false;
        // �~�Ȃ̂Œ[�ƒ[�Ń��[�v����悤��
        if (pos.x == -1)
        {
            pos.x = 15; //x���W�̒[
            is0_15 = true;
        }
        if (pos.x == 16)
        {
            pos.x = 0; //x���W�̒[
            is15_0 = true;
        }

        // ���ۂɈړ�
        SetTransition(pos, HALF_TRANS_TIME);
        return true;
    }

    void Settle()
    {
        // ���ڐڒn
        bool is_set0 = fieldController.Settle(_position, (int)_blockControllers[0].GetBlockType());
        Debug.Assert(is_set0);// �u�����̂͋󂢂Ă����ꏊ�̂͂�
        bool is_set1 = fieldController.Settle(CalcChildPuyoPos(_position), (int)_blockControllers[1].GetBlockType());
        Debug.Assert(is_set1);// �u�����̂͋󂢂Ă����ꏊ�̂͂�

        gameObject.SetActive(false);
    }

    void QuickDrop()
    {
        // ��������ԉ��܂ŗ�����
        Vector2Int pos = _position;
        do
        {
            pos += Vector2Int.down;
        } while (CanMove(pos));
        pos -= Vector2Int.down;// ���̏ꏊ�i�Ō�ɒu�����ꏊ�j�ɖ߂�

        _position = pos;

        Settle();
    }


    bool Fall(bool is_fast)
    {
        //_fallCount -= is_fast ? FALL_COUNT_FAST_SPD : FALL_COUNT_SPD;
        _fallCount -= FALL_COUNT_SPD;

        // �u���b�N���щz������A�s����̂��`�F�b�N
        while (_fallCount < 0)// �u���b�N����ԉ\�����Ȃ����Ƃ��Ȃ��C������̂ŕ��������ɑΉ�
        {
            if (!CanMove(_position + Vector2Int.down))
            {
                // ������Ȃ��Ȃ�
                _fallCount = 0; // �������~�߂�
                if (0 < --_groundFrame) return true;// ���Ԃ�����Ȃ�A�ړ��E��]�\

                // ���Ԑ؂�ɂȂ�����{���ɌŒ�
                Settle();
                return false;
            }

            // �������Ȃ牺�ɐi��
            _last_position.y = _position.y;
            _position += Vector2Int.down;
            _fallCount += FALL_COUNT_UNIT;
        }

        if (is_fast) _additiveScore++; // ���ɓ���āA�������Ƃ��̓{�[�i�X�ǉ�

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
        // ���Ƃ�
        if (!Fall(_logicalInput.IsRaw(LogicalInput.Key.Down))) return;// �ڒn������I��

        // �A�j�����̓L�[���͂��󂯕t���Ȃ�
        if (_animationController.Update()) return;

        //if(isHalfTrans)
        //{
        //    if (HalfTranslate()) return;
        //}
        if (isTransR)
        {
            // ���s�ړ��̃L�[���͎擾
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
        // �N�C�b�N�h���b�v�̃L�[���͎擾
        if (_logicalInput.IsTrigger(LogicalInput.Key.QuickDrop) || _logicalInput.IsTrigger(LogicalInput.Key.JoyA))
        {
            if (playDirector.GetPlayFlag())
                QuickDrop();
        }
    }

    void FixedUpdate()
    {
        if (isPause) return;

        // ������󂯂ē�����
        Control();

        // �\��
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
        // ���s�ړ�
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

    // ���_�̎󂯓n��
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
