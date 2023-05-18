using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Number : MonoBehaviour
{
    // �摜�o�͗p
    [SerializeField]
    ImageRenderer _ImageRenderer;

    // �l�������l
    [SerializeField]
    int _Num = 0;

    // �\������l
    private int _NowNum = 0;

    // �J�E���g�J�n
    private bool _StartCount = false;

    // �J�E���g�I��
    private bool _EndCount = false;

    // �e�I�u�W�F�N�g
    public GameObject Parent;

    // �X�V����
    private void Update()
    {
        // �J�E���g�J�n�t���O�������Ă��Ȃ�������
        if (!_StartCount)
        {
            // �A�j���[�V�����J�n���Ɠ����ɃJ�E���g�J�n
            if (Parent.GetComponent<Animator>().GetBool("StartAnimation"))
                StartCoroutine(StartCount());
        }

        // �J�E���g�J�n�t���O���������Ă�����
        if (_StartCount)
        {
            // �X�R�A�ɒB����܂ŏ㏸��������
            if (_NowNum < _Num)
            {
                _NowNum++;
            }
            else
            {
                // �J�E���g�I��
                _EndCount = true;
            }
        }

        // �摜�ŏo��
        _ImageRenderer._Update(_NowNum);
    }

    // �J�E���g�J�n�֐�
    private IEnumerator StartCount()
    {
        // �Q�b�҂�
        yield return new WaitForSeconds(2);

        // �J�E���g�J�n
        _StartCount = true;
    }

    // �J�E���g�������I�ɏI��������
    public void Finish()
    {
        // �\�����Ă���l���l�������l�ɂ���
        _NowNum = _Num;
        //�@�J�E���g�I���t���O�𗧂Ă�
        _EndCount = true;
    }

    // �J�E���g�I���t���O�擾
    public bool GetEndCount()
    {
        return _EndCount;
    }

    // �\������l�̐ݒ�
    public void SetNumber(int value)
    {
        _NowNum = value;
    }
}
