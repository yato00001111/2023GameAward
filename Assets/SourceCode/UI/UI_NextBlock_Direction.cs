using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_NextBlock_Direction : MonoBehaviour
{

    [SerializeField]
    private Animator NextBlock_Animator;                 // �l�N�X�g�u���b�NUI�̃A�j���[�V����

    // Start is called before the first frame update
    void Start()
    {
        // �A�j���[�^�[������������
        NextBlock_Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetNextBlockAnimation()
    {
        // �A�j���[�V�����̍Đ����Ԃ�"0"�ɂ���
        NextBlock_Animator.Play(0, -1, 0f);
    }
}
