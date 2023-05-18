using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExampleTutorial : MonoBehaviour
{
    // �ŏ��̉摜
    [SerializeField]
    GameObject First;
        
    // �Q�Ԗ�
    [SerializeField]
    GameObject Second;

    // �R�Ԗ�
    [SerializeField]
    GameObject Third;

    // �����`��J�n
    [SerializeField, HideInInspector]
    bool Start_Tutorial = false;

    // �N���b�N��
    [SerializeField, HideInInspector]
    int ClickCount = 0;

    // �X�V����
    private void Update()
    {
        if (!Start_Tutorial)
        {
            First.SetActive(false);
            Second.SetActive(false);
            Third.SetActive(false);
            return;
        }

        // A�{�^����
        if(Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            // ����
            if(ClickCount < 3)
            // �J�E���g�㏸
            ClickCount++;
            
        }

        // �N���b�N���ŕ���
        switch(ClickCount)
        {
            // �O��
            case 0:
                First.SetActive(true);
                Second.SetActive(false);
                Third.SetActive(false);
                break;

            // �P��
            case 1:
                First.SetActive(false);
                Second.SetActive(true);
                Third.SetActive(false);
                break;

            // �Q��
            case 2:
                First.SetActive(false);
                Second.SetActive(false);
                Third.SetActive(true);
                break;

            // ����ȏ�
            default:
                First.SetActive(false);
                Second.SetActive(false);
                Third.SetActive(false);
                break;
        }
    }

    // �`���[�g���A���X�v���C�g�`��J�n
    public void StartTutorialSprite()
    {
        Start_Tutorial = true;
    }
}
