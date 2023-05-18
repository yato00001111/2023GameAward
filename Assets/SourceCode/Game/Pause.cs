using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{

    // �|�[�Y���j���[
    [SerializeField]
    public GameObject PauseMenu;

    // �I��UI
    [SerializeField]
    private GameObject SelectUI;

    // �|�[�Y�����ǂ�����Ԃ�
    bool PositivePauseMenu;

    public int SelectCount;

    // �I��UI���ǂ��ɂ̂��Ă邩��ʒm
    bool OnReStartFlag;
    bool OnRetryFlag;
    bool OnTitleBackFlag;

    bool PushUpButton;
    bool PushDownButton;

    // �p�b�h�̓��͒l
    private float PadVertical;
    private float StickVertical;

    // Start is called before the first frame update
    void Start()
    {

        SelectCount = 0;

        OnReStartFlag = true;
        OnRetryFlag = false;
        OnTitleBackFlag = false;

        PositivePauseMenu = false;

        // �|�[�Y���ł͂Ȃ�
        PauseMenu.SetActive(false);
        SelectUI.SetActive(false);
        SelectUI.transform.position = new Vector3(954, 740, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // �|�[�Y�֐��Ăяo��
        Pausing();

        // �|�[�Y���Ȃ�I��UI�̓���������
        if (PositivePauseMenu)
        {
            ActionSelectUI();

            FunctionSelectUI();
        }
    }

    // �I��UI�̋@�\
    void FunctionSelectUI()
    {
        // �I��UI���ĊJUI��ɂ�����
        if (OnReStartFlag)
        {
            // Enter�L�[�������ꂽ��
            if (Input.GetKeyDown(KeyCode.Return)|| Input.GetKeyDown(KeyCode.Joystick1Button0))
            {
                // �|�[�Y���j���[��\��
                PauseMenu.SetActive(false);
                // �|�[�Y���ł͂Ȃ�
                PositivePauseMenu = false;
                // �S�̂̎��Ԃ�ʏ��
                Time.timeScale = 1;
            }
        }

        // �I��UI�����g���CUI��ɂ�����
        if (OnRetryFlag)
        {
            // Enter�L�[�������ꂽ��
            if (Input.GetKeyDown(KeyCode.Return)|| Input.GetKeyDown(KeyCode.Joystick1Button0))
            {
                // ���g���C����
            }
        }

        // �I��UI���^�C�g��UI��ɂ�����
        if (OnTitleBackFlag)
        {
            // Enter�L�[�������ꂽ��
            if (Input.GetKeyDown(KeyCode.Return)|| Input.GetKeyDown(KeyCode.Joystick1Button0))
            {
                SceneManager.LoadScene("Title");
            }
        }
    }

    // �I��UI�̓���
    void ActionSelectUI()
    {
        PadVertical   = Input.GetAxis("D_Pad");
        StickVertical = Input.GetAxis("L_Stick");


        // �ĊJ��I�𒆂ɉ��{�^���������ꂽ��
        if ((OnReStartFlag && PadVertical < 0) ||
            (OnReStartFlag && StickVertical < -0.2))
        {
            // ���g���C�{�^����I�𒆂�
            SelectUI.transform.position = new Vector3(956, 540, 0);
            SelectCount = 1;
            PushDownButton = true;
            PushUpButton = false;
        }


        // ���g���C��I�𒆂ɉ��{�^���������ꂽ��
        if ((OnRetryFlag && PadVertical < 0)||
            (OnRetryFlag && StickVertical < -0.2))
        {
            // �^�C�g����I�𒆂�
            SelectUI.transform.position = new Vector3(956, 340, 0);
            SelectCount = 2;
            PushDownButton = true;
            PushUpButton = false;
        }
        // ���g���C�I�𒆂ɏ�{�^���������ꂽ��
        if ((OnRetryFlag && PadVertical > 0) ||
            (OnRetryFlag && StickVertical > 0.2))
        {
            // �ĊJ��I�𒆂�
            SelectUI.transform.position = new Vector3(956, 740, 0);
            SelectCount = 0;
            PushUpButton = true;
            PushDownButton = false;
        }


        // �^�C�g����I�𒆂ɏ�{�^���������ꂽ��
        if ((OnTitleBackFlag && PadVertical > 0)||
            (OnTitleBackFlag && StickVertical > 0.2))
        {
            // ���g���C��I�𒆂�
            SelectUI.transform.position = new Vector3(956, 540, 0);
            SelectCount = 1;
            PushUpButton = true;
            PushDownButton = false;
        }


        if (SelectCount == 0)
        {
            if (PadVertical == 0)// || StickVertical == 0)
            {
                OnReStartFlag = true;
                OnTitleBackFlag = false;
                OnRetryFlag = false;
            }
        }

        if (SelectCount == 1)
        {
            // Pad�����𗣂�����
            if (PadVertical == 0)// || StickVertical == 0)
            {
                OnRetryFlag = true;
                if (PushUpButton)
                {
                    OnTitleBackFlag = false;
                }
                if(PushDownButton)
                {
                    OnReStartFlag = false;
                }
            }
        }

        if (SelectCount == 2)
        {
            // Pad�����𗣂�����
            if (PadVertical == 0)// || StickVertical == 0)
            {
                // ���̏���
                OnTitleBackFlag = true;
                OnRetryFlag = false;
                OnReStartFlag = false;
            }
        }
    }

    // �|�[�Y���̏���
    void Pausing()
    {
        // P�{�^���������ꂽ��|�[�Y
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Joystick1Button7))
        {
            // �|�[�Y���j���[�\��
            PauseMenu.SetActive(true);
            SelectUI.SetActive(true);
            // �|�[�Y���ł���
            PositivePauseMenu = true;
            // �S�̂̎��Ԃ��~�߂�
            Time.timeScale = 0;
        }
        //// L�{�^���������ꂽ��|�[�Y����
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    // �|�[�Y���j���[��\��
        //    PauseMenu.SetActive(false);
        //    // �|�[�Y���ł͂Ȃ�
        //    PositivePauseMenu = false;
        //    // �S�̂̎��Ԃ�ʏ��
        //    Time.timeScale = 1;
        //}
    }
}
