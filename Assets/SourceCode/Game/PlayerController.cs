using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // �ړ�����
    const int TRANS_TIME = 3; // �ړ����x�J�ڎ���

    // ��������
    const int FALL_COUNT_UNIT = 120; // �Ђƃ}�X��������J�E���g��
    [SerializeField] int FALL_COUNT_SPD = 5; // �������x
    const int FALL_COUNT_FAST_SPD = 13; // �����������̑��x
    const int GROUND_FRAMES = 15; // �ڒn�ړ��\����

    int _fallCount = 0;
    int _groundFrame = GROUND_FRAMES;// �ڒn����

    bool is0_15 = false;
    bool is15_0 = false;

    private bool isPause = false;

    [SerializeField] FieldController fieldController = default!;
    [SerializeField] BlockController _blockController = default!;

    Vector2Int _position;// block�̈ʒu

    AnimationController _animationController = new AnimationController();
    Vector2Int _last_position; // �J�ڑO�̈ʒu

    LogicalInput _logicalInput = null;

    // ���_
    uint _additiveScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        is0_15 = false;
        is15_0 = false;
        isPause = false;
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

        // �Ղ������
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
        // ��Ԃ̂��߂ɕۑ����Ă���
        // �~�Ȃ̂Œ[�ƒ[�Ń��[�v����悤��
        if (is0_15) // x���W0����15
        {
            _last_position.x = 16;
            _last_position.y = _position.y;
            is0_15 = false;
        }
        else if (is15_0) // x���W15����0
        {
            _last_position.x = 17;
            _last_position.y = _position.y;
            is15_0 = false;
        }
        else _last_position = _position;

        // �l�̍X�V
        _position = pos;

        _animationController.Set(time);
    }

    private bool Translate(bool is_right)
    {
        // ���z�I�Ɉړ��ł��邩���؂���
        Vector2Int pos = _position + (is_right ? Vector2Int.right : Vector2Int.left);
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
        //_position = pos;
        SetTransition(pos, TRANS_TIME);

        //_blockController.SetPos(new Vector3Int(_position.x, _position.y, 0));

        return true;
    }

    void Settle()
    {
        // ���ڐڒn
        bool is_set0 = fieldController.Settle(_position, (int)_blockController.GetBlockType());
        Debug.Assert(is_set0);// �u�����̂͋󂢂Ă����ꏊ�̂͂�

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
        _fallCount -= is_fast ? FALL_COUNT_FAST_SPD : FALL_COUNT_SPD;

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

    void CheckSide()
    {

    }

    void Control()
    {
        // ������
        CheckSide();

        // ���Ƃ�
        if (!Fall(_logicalInput.IsRaw(LogicalInput.Key.Down))) return;// �ڒn������I��

        // �A�j�����̓L�[���͂��󂯕t���Ȃ�
        if (_animationController.Update()) return;

        // ���s�ړ��̃L�[���͎擾
        //if (_logicalInput.IsRepeat(LogicalInput.Key.Right))
        //{
        //    if (Translate(true)) return;
        //}
        //if (_logicalInput.IsRepeat(LogicalInput.Key.Left))
        //{
        //    if (Translate(false)) return;
        //}

        // �N�C�b�N�h���b�v�̃L�[���͎擾
        if (_logicalInput.IsRelease(LogicalInput.Key.QuickDrop) || _logicalInput.IsRelease(LogicalInput.Key.JoyA))
        {
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
        float anim_rate = _animationController.GetNormalized();
        _blockController.SetPosInterpolate(_position, _last_position, anim_rate, dy.y);
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
}
