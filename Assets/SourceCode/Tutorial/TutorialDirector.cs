using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TutorialDirector : MonoBehaviour
{
    [SerializeField] GameObject prefabMessage = default!;
    [SerializeField] GameObject gameObjectCanvas = default!;
    [SerializeField] PlayerController _playerController = default!;
    [SerializeField] FieldController _fieldController = default!;
    [SerializeField] PlayDirector _playDirector = default!;
    [SerializeField] UI_CountDown _uiCountDown = default!;
    [SerializeField] UI_Dead_Gauge_Tutorial _uiDeadGaugeTutorial = default!;
    GameObject _message = null;

    // �`���[�g���A���\���I�u�W�F�N�g�̃X�N���v�g(��)
    [SerializeField]
    private ExampleTutorial exampleTutorial;

    // �g�����W�V����
    [SerializeField]
    private Animator transition;

    // ��ʂɂł鉉�o���b�Z�[�W�̕\��
    void CreateMessage(string message)
    {
        Debug.Assert(_message == null);
        _message = Instantiate(prefabMessage, Vector3.zero, Quaternion.identity,
            gameObjectCanvas.transform);
        _message.transform.localPosition = new Vector3(-430, 300, 0);// ��ʍ���ɔz�u

        _message.GetComponent<TextMeshProUGUI>().text = message;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("TutorialFlow");
    }

    private IEnumerator TutorialFlow()
    {
        //_fieldController.SetTutorial();
        //_playDirector.EnableSpawn(true);
        //_playerController.SetPlayerPause(true);

        //while (!Input.GetKeyDown(KeyCode.RightArrow) && !Input.GetKeyDown(KeyCode.LeftArrow))// ���������̂�҂�
        //{
        //    yield return null;
        //}
        //_playDirector.EnableSpawn(false);
        //_playerController.SetPlayerPause(false);


        //CreateMessage("�~�܂��Ă���u���b�N�͉�]�������܂�");
        //yield return new WaitForSeconds(0.4f);
        //while (!Input.anyKey)// ���������̂�҂�
        //{
        //    yield return null;
        //}
        //Destroy(_message); _message = null;

        //yield return new WaitForSeconds(0.3f);

        //CreateMessage("RB�ŉ�]�����܂��傤");
        //_playerController.SetPlayerQuick(true);
        //_message.transform.localPosition = new Vector3(-430, 350, 0);
        //while (!Input.GetKeyDown(KeyCode.RightArrow) && !Input.GetKeyDown(KeyCode.Joystick1Button5))
        //{
        //    yield return null;
        //}
        //_fieldController.SetControl(false); //
        //Destroy(_message); _message = null;
        //_playerController.SetPlayerPause(false);
        //yield return new WaitForSeconds(0.3f);


        //CreateMessage("��肭�A���ł���悤�Ɋ撣���ĉ�����");
        //yield return new WaitForSeconds(0.5f);
        //while (!Input.anyKey)// ���������̂�҂�
        //{
        //    yield return null;
        //}

        //yield return new WaitForSeconds(3.5f);

        yield return new WaitForSeconds(1.5f);

        _uiCountDown.SetTutorialStartFlag(true);
        _playDirector.SetisTutorial(true);

        _uiDeadGaugeTutorial.SetStepImage(0);


        while (!_uiCountDown.GetGameStartFlag())
        {
            yield return null;
        }

        yield return new WaitForSeconds(1.0f);

        _playDirector.EnableSpawn(true);// �v���C�J�n
        yield return new WaitForSeconds(1.0f);

        _fieldController.SetControl(false); //�u���b�N��]�֎~
        _playerController.SetisFall(false); //�v���C���[������~

        _uiDeadGaugeTutorial.SetArrowDownAnimation(); //�N�C�b�N�h���b�v�̈ʒu�ɖ��

        while (_playDirector.GetTutorialControlACount() != 4) //�u���b�N��4��ςނ܂őҋ@
        {
            _uiDeadGaugeTutorial.SetStepImage(_playDirector.GetTutorialControlACount());

            yield return null;
        }
        _uiDeadGaugeTutorial.SetStepImage(4);
        yield return new WaitForSeconds(0.7f);
        _uiDeadGaugeTutorial.SetStepImage(0); //0.7�b��StepImage4������

        _fieldController.SetControl(true); //�u���b�N��]����

        _uiDeadGaugeTutorial.SetArrowResetAnimation(); //��󃊃Z�b�g
        _uiDeadGaugeTutorial.SetArrowUpAnimation(); //��]�̈ʒu�ɖ��


        while (_fieldController.GetTutorialTransCount() != 4) //�u���b�N��4���]������܂ł܂őҋ@
        {
            _uiDeadGaugeTutorial.SetStepImage(_fieldController.GetTutorialTransCount());

            yield return null;
        }

        _uiDeadGaugeTutorial.SetStepImage(4);

        _playerController.SetPlayerPause(true); //player����֎~

        yield return new WaitForSeconds(0.1f); //����]�֎~�ɂ����4��ڂ���]���Ȃ��Ȃ�̂�
        _fieldController.SetControl(false); //�u���b�N��]�֎~
        _uiDeadGaugeTutorial.SetArrowResetAnimation(); //��󃊃Z�b�g
        _uiDeadGaugeTutorial.SetTutorial04StartFlag(); //DeadGauge�J�n

        yield return new WaitForSeconds(0.7f);
        _uiDeadGaugeTutorial.SetStepImage(5); //0.7�b��StepImage��\��



        //// �`���[�g���A�����I�������
        //transition.SetTrigger("OUT_Animation");

        //yield return new WaitForSeconds(3.5f);

        //SceneManager.LoadScene("GameScene");
    }

    private void Update()
    {
        //// �`���[�g���A�����I�������
        //if (exampleTutorial.GetEndTutorial())
        //{
        //    StartCoroutine("TutorialFlow");
        //}
    }
}
