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

    // 画面にでる演出メッセージの表示
    void CreateMessage(string message)
    {
        Debug.Assert(_message == null);
        _message = Instantiate(prefabMessage, Vector3.zero, Quaternion.identity,
            gameObjectCanvas.transform);
        _message.transform.localPosition = new Vector3(-430, 300, 0);// 画面左上に配置

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
        CreateMessage("ブロックは同じ色が3つ以上揃うと消えます");
        yield return new WaitForSeconds(0.4f);
        while (!Input.anyKey)// 何か押すのを待つ
        {
            yield return null;
        }
        Destroy(_message); _message = null;
        yield return new WaitForSeconds(0.3f);

        CreateMessage("止まっているブロックは回転させられます");
        yield return new WaitForSeconds(0.4f);
        while (!Input.anyKey)// 何か押すのを待つ
        {
            yield return null;
        }
        Destroy(_message); _message = null;

        _fieldController.SetTutorial();
        _playerController.SetPlayerPause(true);
        yield return new WaitForSeconds(0.3f);

        CreateMessage("RBで回転させましょう");
        arrow.SetActive(true);
        _playerController.SetPlayerQuick(true);
        TplayDirector.EnableSpawn(true);// プレイ開始
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

        while (!TplayDirector.IsWaiting())// 終了待ち
        {
            yield return null;
        }
        foreach (var g in ScoreText)
        {
            g.SetActive(false);
        }

        CreateMessage("上手く連鎖できるように頑張って下さい");
        yield return new WaitForSeconds(0.5f);
        while (!Input.anyKey)// 何か押すのを待つ
        {
            yield return null;
        }

        animator.SetTrigger("OUT_Animation");

        yield return new WaitForSeconds(3.5f);
        SceneManager.LoadScene("GameScene");
    }
}
