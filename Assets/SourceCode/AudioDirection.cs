using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���̉��o�Ɋւ���֐��T�v�X�N���v�g

public class AudioDirection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // ���ɗ����t���[���� true �ɂȂ�
        if (Music.IsJustChangedBeat())
        {
        }

        // ���߂ɗ����t���[���� true �ɂȂ�
        if (Music.IsJustChangedBar())
        {
        }

        // �w�肵������,��,���j�b�g�ɗ����t���[���� true �ɂȂ�
        if (Music.IsJustChangedAt(1, 2, 3))
        {
            
        }

        // ���g�p����ۂɂ�MusicUnity�X�N���v�g���ꏏ��add component
        // �g�p����T�E���h��ݒ肷��
    }
}
