using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboNumber : MonoBehaviour
{
    // �摜�o�͗p
    [SerializeField]
    ImageRenderer _ImageRenderer;

    // �R���{
    [SerializeField]
    int _Combo = 0;

    // �㏸����R���{
    private int _NowCombo = 0;

    // �J�E���g�J�n
    private bool _StartCount = false;

    // �J�E���g�I��
    private bool _EndCount = false;

    //�@�e�I�u�W�F�N�g
    public GameObject Parent;

    // �X�V����
    void Update()
    {
        // �J�E���g�J�n�t���O�������Ă��Ȃ�������
        if (!_StartCount)
        {
            // �A�j���[�V�����J�n���Ɠ����ɃJ�E���g�J�n
            if (Parent.GetComponent<Animator>().GetBool("StartAnimation"))
                _StartCount = true;
        }

        // �J�E���g�J�n
        if (_StartCount)
        {
            // �R���{�ɒB����܂ŏ㏸��������
            if (_NowCombo < _Combo)
            {
                _NowCombo++;
            }
            else
            {
                // �J�E���g�I��
                _EndCount = true;
            }
        }
        // �摜�ŏo��
        _ImageRenderer._Update(_NowCombo);
    }

    // �R���{�ݒ�
    public void SetScore(int Score)
    {
        _Combo = Score;
    }

    // �J�E���g�J�n�t���O�ݒ�
    public void SetStartCount(bool flg)
    {
        _StartCount = flg;
    }

    // �J�E���g�I���t���O�擾
    public bool GetEndCount()
    {
        return _EndCount;
    }

    // �A�j���[�V�����I���t���O�𗧂Ă�
    void SetEndAnimation()
    {
        _StartCount = true;
    }
}
