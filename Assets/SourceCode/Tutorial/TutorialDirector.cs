using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TutorialDirector : MonoBehaviour
{
    [SerializeField] GameObject prefabMessage = default!;
    [SerializeField] GameObject gameObjectCanvas = default!;
    [SerializeField] GameObject arrow = default!;
    [SerializeField] GameObject[] ScoreText = default!;
    [SerializeField] T_PlayDirector TplayDirector = default!;
    [SerializeField] PlayerController _playerController = default!;
    [SerializeField] private Animator animator;
    FieldController _fieldController = default!;
    GameObject _message = null;

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
        _fieldController = TplayDirector.GetComponent<FieldController>();
        arrow.SetActive(false);
        //animator.SetTrigger("IN_Animation");
        StartCoroutine("TutorialFlow");
    }

    private IEnumerator TutorialFlow()
    {
        CreateMessage("�u���b�N�͓����F��3�ȏ㑵���Ə����܂�");
        yield return new WaitForSeconds(0.4f);
        while (!Input.anyKey)// ���������̂�҂�
        {
            yield return null;
        }
        Destroy(_message); _message = null;
        yield return new WaitForSeconds(0.3f);

        CreateMessage("�~�܂��Ă���u���b�N�͉�]�������܂�");
        yield return new WaitForSeconds(0.4f);
        while (!Input.anyKey)// ���������̂�҂�
        {
            yield return null;
        }
        Destroy(_message); _message = null;

        _fieldController.SetTutorial();
        _playerController.SetPlayerPause(true);
        yield return new WaitForSeconds(0.3f);

        CreateMessage("RB�ŉ�]�����܂��傤");
        arrow.SetActive(true);
        _playerController.SetPlayerQuick(true);
        TplayDirector.EnableSpawn(true);// �v���C�J�n
        _message.transform.localPosition = new Vector3(-430, 350, 0);
        while (!Input.GetKeyDown(KeyCode.RightArrow) && !Input.GetKeyDown(KeyCode.Joystick1Button5))
        {
            yield return null;
        }
        _fieldController.SetControl(false); //
        Destroy(_message); _message = null;
        foreach(var g in ScoreText)
        {
            g.SetActive(true);
        }
        arrow.SetActive(false);
        _playerController.SetPlayerPause(false);
        TplayDirector.EnableSpawn(false);
        yield return new WaitForSeconds(0.3f);

        while (!TplayDirector.IsWaiting())// �I���҂�
        {
            yield return null;
        }
        foreach (var g in ScoreText)
        {
            g.SetActive(false);
        }

        CreateMessage("��肭�A���ł���悤�Ɋ撣���ĉ�����");
        yield return new WaitForSeconds(0.5f);
        while (!Input.anyKey)// ���������̂�҂�
        {
            yield return null;
        }

        animator.SetTrigger("OUT_Animation");

        yield return new WaitForSeconds(3.5f);
        SceneManager.LoadScene("GameScene");
    }
}
