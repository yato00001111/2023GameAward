using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class New_Transition : MonoBehaviour
{
    // �A�j���[�V����
    private Animator anime;

    // �C���A�j���[�V�����I��
    private bool _End_INTransition = false;

    // �A�E�g�A�j���[�V�����I��
    private bool _End_OUTTransition = false;

    // ������
    void Start()
    {
        // �A�j���[�V�����擾
        anime = gameObject.GetComponent<Animator>();
    }

    // �t�F�[�h�C���A�j���[�V�����Đ�
    public void Start_INanimation()
    {
        anime.SetTrigger("IN_Animation");
    }

    // �t�F�[�h�A�E�g�A�j���[�V�����Đ�
    public void Start_OUTanimation()
    {
        anime.SetTrigger("OUT_Animation");
    }

    // �t�F�[�h�C���A�j���[�V�����I��(�A�j���[�^�[�p)
    public void EndINTransition()
    {
        _End_INTransition = true;
    }

    // �t�F�[�h�A�E�g�A�j���[�V�����I��(�A�j���[�^�[�p)
    public void EndOUTTransition()
    {
        _End_OUTTransition = true;
    }

    // �t�F�[�h�C���A�j���[�V�����I���擾
    public bool GetEndINTransition()
    {
        return _End_INTransition;
    }

    // �t�F�[�h�A�E�g�A�j���[�V�����I���擾
    public bool GetEndOUTTransition()
    {
        return _End_OUTTransition;
    }
}
