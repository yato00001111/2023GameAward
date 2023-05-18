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
    GameObject _message = null;

    // チュートリアル表示オブジェクトのスクリプト(仮)
    [SerializeField]
    private ExampleTutorial exampleTutorial;

    // トランジション
    [SerializeField]
    private Animator transition;

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

    }

    private IEnumerator TutorialFlow()
    {
        //_fieldController.SetTutorial();
        //_playDirector.EnableSpawn(true);
        //_playerController.SetPlayerPause(true);

        //while (!Input.GetKeyDown(KeyCode.RightArrow) && !Input.GetKeyDown(KeyCode.LeftArrow))// 何か押すのを待つ
        //{
        //    yield return null;
        //}
        //_playDirector.EnableSpawn(false);
        //_playerController.SetPlayerPause(false);


        //CreateMessage("止まっているブロックは回転させられます");
        //yield return new WaitForSeconds(0.4f);
        //while (!Input.anyKey)// 何か押すのを待つ
        //{
        //    yield return null;
        //}
        //Destroy(_message); _message = null;

        //yield return new WaitForSeconds(0.3f);

        //CreateMessage("RBで回転させましょう");
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


        //CreateMessage("上手く連鎖できるように頑張って下さい");
        //yield return new WaitForSeconds(0.5f);
        //while (!Input.anyKey)// 何か押すのを待つ
        //{
        //    yield return null;
        //}

        //yield return new WaitForSeconds(3.5f);

        // チュートリアルが終わったら
        transition.SetTrigger("OUT_Animation");

        yield return new WaitForSeconds(3.5f);

        SceneManager.LoadScene("GameScene");
    }

    private void Update()
    {
        // チュートリアルが終わったら
        if (exampleTutorial.GetEndTutorial())
        {
            StartCoroutine("TutorialFlow");
        }
    }
}
