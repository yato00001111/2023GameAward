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
        StartCoroutine("TutorialFlow");
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

        yield return new WaitForSeconds(1.5f);

        _uiCountDown.SetTutorialStartFlag(true);
        _playDirector.SetisTutorial(true);

        _uiDeadGaugeTutorial.SetStepImage(0);


        while (!_uiCountDown.GetGameStartFlag())
        {
            yield return null;
        }

        yield return new WaitForSeconds(1.0f);

        _playDirector.EnableSpawn(true);// プレイ開始
        yield return new WaitForSeconds(1.0f);

        _fieldController.SetControl(false); //ブロック回転禁止
        _playerController.SetisFall(false); //プレイヤー落下停止

        _uiDeadGaugeTutorial.SetArrowDownAnimation(); //クイックドロップの位置に矢印

        while (_playDirector.GetTutorialControlACount() != 4) //ブロックを4回積むまで待機
        {
            _uiDeadGaugeTutorial.SetStepImage(_playDirector.GetTutorialControlACount());

            yield return null;
        }
        _uiDeadGaugeTutorial.SetStepImage(4);
        yield return new WaitForSeconds(0.7f);
        _uiDeadGaugeTutorial.SetStepImage(0); //0.7秒後StepImage4つ半透明

        _fieldController.SetControl(true); //ブロック回転許可

        _uiDeadGaugeTutorial.SetArrowResetAnimation(); //矢印リセット
        _uiDeadGaugeTutorial.SetArrowUpAnimation(); //回転の位置に矢印


        while (_fieldController.GetTutorialTransCount() != 4) //ブロックを4回回転させるまでまで待機
        {
            _uiDeadGaugeTutorial.SetStepImage(_fieldController.GetTutorialTransCount());

            yield return null;
        }

        _uiDeadGaugeTutorial.SetStepImage(4);

        _playerController.SetPlayerPause(true); //player操作禁止

        yield return new WaitForSeconds(0.1f); //即回転禁止にすると4回目が回転しなくなるので
        _fieldController.SetControl(false); //ブロック回転禁止
        _uiDeadGaugeTutorial.SetArrowResetAnimation(); //矢印リセット
        _uiDeadGaugeTutorial.SetTutorial04StartFlag(); //DeadGauge開始

        yield return new WaitForSeconds(0.7f);
        _uiDeadGaugeTutorial.SetStepImage(5); //0.7秒後StepImage非表示



        //// チュートリアルが終わったら
        //transition.SetTrigger("OUT_Animation");

        //yield return new WaitForSeconds(3.5f);

        //SceneManager.LoadScene("GameScene");
    }

    private void Update()
    {
        //// チュートリアルが終わったら
        //if (exampleTutorial.GetEndTutorial())
        //{
        //    StartCoroutine("TutorialFlow");
        //}
    }
}
