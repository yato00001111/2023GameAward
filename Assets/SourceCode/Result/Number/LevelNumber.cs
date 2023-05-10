using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelNumber : MonoBehaviour
{
    // �摜�o�͗p
    [SerializeField]
    ImageRenderer _ImageRenderer;

    // ���x��
    [SerializeField]
    int _Level = 0;

    // �㏸���郌�x��
    private int _NowLevel = 0;

    // �J�E���g�J�n�t���O
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
            // �A�j���[�V�����J�n�Ɠ����ɃJ�E���g�J�n
            if (Parent.GetComponent<Animator>().GetBool("StartAnimation"))
                _StartCount = true;
        }

        // �J�E���g�J�n
        if (_StartCount)
        {
            // ���x���ɒB����܂ŏ㏸��������
            if (_NowLevel < _Level)
            {
                _NowLevel++;
            }
            else
            {
                // �J�E���g�I��
                _EndCount = true;
            }
        }

        // �摜�ŏo��
        _ImageRenderer._Update(_NowLevel);
    }

    // ���x���ݒ�
    public void SetLevel(int Level)
    {
        _Level = Level;
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
