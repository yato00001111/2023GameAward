using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeNumber : MonoBehaviour
{
    // ���`��
    [SerializeField]
    ImageRenderer _MuinitRenderer;

    // �b�`��
    [SerializeField]
    ImageRenderer _SecondRenderer;

    // �^�C��(��)
    [SerializeField]
    int _TimeMuinit = 0;

    // �^�C��(�b)
    [SerializeField]
    int _TimeSecond = 0;

    // �����^�C��(�t���[��)
    private int TotalTime = 0;

    // �㏸���鎞��
    private int _NowTime = 0;

    // ��
    private int _Muinit = 0;

    // �b
    private int _Second = 0;

    // �J�E���g�J�n
    private bool _StartCount = false;

    // �J�E���g�I��
    private bool _EndCount = false;

    //�@�e�I�u�W�F�N�g
    public GameObject Parent;

    // ������
    private void Start()
    {
        // �t���[���ŊǗ�����̂ŒP�ʂ��t���[���ɂȂ���
        TotalTime += _TimeMuinit * 60 * 60;
        TotalTime += _TimeSecond * 60;
    }

    // �X�V����
    void Update()
    {
        // �J�E���g�J�n�t���O�������ĂȂ�������
        if (!_StartCount)
        {
            // �A�j���V�����J�n���Ɠ����ɃJ�E���g�J�n
            if (Parent.GetComponent<Animator>().GetBool("StartAnimation"))
                _StartCount = true;
        }

        // �J�E���g�J�n
        if (_StartCount)
        {
            // �^�C���ɒB����܂ŏ㏸
            if (_NowTime < TotalTime)
            {
                // �J�E���g�𑬂����邽�߂�60
                _NowTime += 60;
            }
            else
            {
                // �J�E���g�I��
                _EndCount = true;
            }

            // �J�E���g�I���t���O���������Ă��Ȃ�������
            if (!_EndCount)
            {
                // �t���[���P�ʂ��A�b�A���ɕϊ�
                if (_NowTime % 60 == 0)
                {
                    _Second++;
                }

                if (_Second == 60)
                {
                    _Second -= 60;
                    _Muinit++;
                }
            }

        }
            // ���`��
            _MuinitRenderer._Update(_Muinit);
            // �b�`��
            _SecondRenderer._Update(_Second);      
    }

    // �A�j���[�V�����I���t���O�𗧂Ă�
    void SetEndAnimation()
    {
        _StartCount = true;
    }

    // �J�E���g�I���t���O�擾
    public bool GetEndCount()
    {
        return _EndCount;
    }
}
