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
    bool PositivePauseMenu = false;

    // Start is called before the first frame update
    void Start()
    {
        // �|�[�Y���ł͂Ȃ�
        PauseMenu.SetActive(false);
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
        if (SelectUI.transform.position.y == 740.0f)
        {
            // Enter�L�[�������ꂽ��
            if (Input.GetKeyDown(KeyCode.Return))
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
        if (SelectUI.transform.position.y == 540.0f)
        {
            // Enter�L�[�������ꂽ��
            if (Input.GetKeyDown(KeyCode.Return))
            {
                // ���g���C����
            }
        }

        // �I��UI���^�C�g��UI��ɂ�����
        if (SelectUI.transform.position.y == 340.0f)
        {
            // Enter�L�[�������ꂽ��
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SceneManager.LoadScene("Title");
            }
        }
    }

    // �I��UI�̓���
    void ActionSelectUI()
    {
        // ���{�^���������ꂽ��
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            // �I��UI��1����
            SelectUI.transform.position -= new Vector3(0, 200.0f, 0);
        }
        // ��{�^���������ꂽ��
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // �I��UI��1���
            SelectUI.transform.position += new Vector3(0, 200.0f, 0);
        }

        // �I��UI��1�ԏ�ɂ�������
        if (SelectUI.transform.position.y < 340.0f)
        {
            // ����ȏ��ɂ������Ȃ�
            SelectUI.transform.position = new Vector3(956, 340, 0);
        }
        // �I��UI��1�ԉ��ɂ�������
        if (SelectUI.transform.position.y > 740.0f)
        {
            // ����ȏ㉺�ɂ������Ȃ�
            SelectUI.transform.position = new Vector3(956, 740, 0);
        }
    }

    // �|�[�Y���̏���
    void Pausing()
    {
        // P�{�^���������ꂽ��|�[�Y
        if (Input.GetKeyDown(KeyCode.P))
        {
            // �|�[�Y���j���[�\��
            PauseMenu.SetActive(true);
            // �|�[�Y���ł���
            PositivePauseMenu = true;
            // �S�̂̎��Ԃ��~�߂�
            Time.timeScale = 0;
        }
        // L�{�^���������ꂽ��|�[�Y����
        if (Input.GetKeyDown(KeyCode.L))
        {
            // �|�[�Y���j���[��\��
            PauseMenu.SetActive(false);
            // �|�[�Y���ł͂Ȃ�
            PositivePauseMenu = false;
            // �S�̂̎��Ԃ�ʏ��
            Time.timeScale = 1;
        }
    }

}
