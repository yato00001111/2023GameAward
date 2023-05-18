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

    // 4�Ԗ�
    [SerializeField]
    GameObject Fource;

    // 5�Ԗ�
    [SerializeField]
    GameObject Fifth;

    // 6�Ԗ�
    [SerializeField]
    GameObject Sixth;

    // 7�Ԗ�
    [SerializeField]
    GameObject Seventh;

    // �����`��J�n
    [SerializeField, HideInInspector]
    bool Start_Tutorial = false;

    // �����`��I��
    [SerializeField, HideInInspector]
    bool End_Tutorial = false;

    // �N���b�N��
    [SerializeField, HideInInspector]
    int ClickCount = 0;

    private void Start()
    {
        StartTutorialSprite();
        First.SetActive(false);
        Second.SetActive(false);
        Third.SetActive(false);
        Fource.SetActive(false);
        Fifth.SetActive(false);
        Sixth.SetActive(false);
        Seventh.SetActive(false);
    }

    // �X�V����
    private void Update()
    {
        if (!Start_Tutorial)
        {
            First.SetActive(false);
            Second.SetActive(false);
            Third.SetActive(false);
            Fource.SetActive(false);
            Fifth.SetActive(false);
            Sixth.SetActive(false);
            Seventh.SetActive(false);
            return;
        }

        // A�{�^����
        if(Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetMouseButtonDown(0))
        {
            // ����
            if(ClickCount < 7)
            // �J�E���g�㏸
            ClickCount++;
            
        }

        // �N���b�N���ŕ���
        switch(ClickCount)
        {
            // �O��
            case 0:
                First.SetActive(true);
                break;

            // �P��
            case 1:
                First.SetActive(false);
                Second.SetActive(true);
                break;

            // �Q��
            case 2:
                Second.SetActive(false);
                Third.SetActive(true);
                break;

            case 3:
                Third.SetActive(false);
                Fource.SetActive(true);
                break;

            case 4:
                Fource.SetActive(false);
                Fifth.SetActive(true);
                break;

            case 5:
                Fifth.SetActive(false);
                Sixth.SetActive(true);
                break;

            case 6:
                Sixth.SetActive(false);
                Seventh.SetActive(true);
                break;

            default:
                Seventh.SetActive(false);

                EndTutorialSprite();
                break;
        }
    }

    // �`���[�g���A���X�v���C�g�`��J�n
    public void StartTutorialSprite()
    {
        Start_Tutorial = true;
    }

    // �`���[�g���A���X�v���C�g�`��I��
    public void EndTutorialSprite()
    {
        End_Tutorial = true;
    }

    public bool GetEndTutorial()
    {
        return End_Tutorial;
    }
}
