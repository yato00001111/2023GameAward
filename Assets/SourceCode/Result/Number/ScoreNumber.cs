using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreNumber : MonoBehaviour
{
    // �摜�o�͗p
    [SerializeField]
    ImageRenderer _ImageRenderer;

    // �X�R�A
    [SerializeField]
    int _Score = 0;

    // �㏸����X�R�A
    [SerializeField]
    int _NowScore = 0;

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

        // �J�E���g�J�n�t���O���������Ă�����
        if (_StartCount)
        {
            // �X�R�A�ɒB����܂ŏ㏸��������
            if (_NowScore < _Score)
            {
                _NowScore++;
            }
            else
            {
                // �J�E���g�I��
                _EndCount = true;
            }
        }

        // �摜�ŏo��
        _ImageRenderer._Update(_NowScore);
    }

    // �X�R�A�ݒ�
    public void SetScore(int Score)
    {
        _Score = Score;
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
