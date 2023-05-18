using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{

    // ポーズメニュー
    [SerializeField]
    public GameObject PauseMenu;

    // 選択UI
    [SerializeField]
    private GameObject SelectUI;

    // ポーズ中かどうかを返す
    bool PositivePauseMenu;

    public int SelectCount;

    // 選択中UIがどこにのってるかを通知
    bool OnReStartFlag;
    bool OnRetryFlag;
    bool OnTitleBackFlag;

    bool PushUpButton;
    bool PushDownButton;

    // パッドの入力値
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

        // ポーズ中ではない
        PauseMenu.SetActive(false);
        SelectUI.SetActive(false);
        SelectUI.transform.position = new Vector3(954, 740, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // ポーズ関数呼び出し
        Pausing();

        // ポーズ中なら選択UIの動きを許可
        if (PositivePauseMenu)
        {
            ActionSelectUI();

            FunctionSelectUI();
        }
    }

    // 選択UIの機能
    void FunctionSelectUI()
    {
        // 選択UIが再開UI上にあって
        if (OnReStartFlag)
        {
            // Enterキーが押されたら
            if (Input.GetKeyDown(KeyCode.Return)|| Input.GetKeyDown(KeyCode.Joystick1Button0))
            {
                // ポーズメニュー非表示
                PauseMenu.SetActive(false);
                // ポーズ中ではない
                PositivePauseMenu = false;
                // 全体の時間を通常に
                Time.timeScale = 1;
            }
        }

        // 選択UIがリトライUI上にあって
        if (OnRetryFlag)
        {
            // Enterキーが押されたら
            if (Input.GetKeyDown(KeyCode.Return)|| Input.GetKeyDown(KeyCode.Joystick1Button0))
            {
                // リトライ処理
            }
        }

        // 選択UIがタイトルUI上にあって
        if (OnTitleBackFlag)
        {
            // Enterキーが押されたら
            if (Input.GetKeyDown(KeyCode.Return)|| Input.GetKeyDown(KeyCode.Joystick1Button0))
            {
                SceneManager.LoadScene("Title");
            }
        }
    }

    // 選択UIの動き
    void ActionSelectUI()
    {
        PadVertical   = Input.GetAxis("D_Pad");
        StickVertical = Input.GetAxis("L_Stick");


        // 再開を選択中に下ボタンが押されたら
        if ((OnReStartFlag && PadVertical < 0) ||
            (OnReStartFlag && StickVertical < -0.2))
        {
            // リトライボタンを選択中に
            SelectUI.transform.position = new Vector3(956, 540, 0);
            SelectCount = 1;
            PushDownButton = true;
            PushUpButton = false;
        }


        // リトライを選択中に下ボタンが押されたら
        if ((OnRetryFlag && PadVertical < 0)||
            (OnRetryFlag && StickVertical < -0.2))
        {
            // タイトルを選択中に
            SelectUI.transform.position = new Vector3(956, 340, 0);
            SelectCount = 2;
            PushDownButton = true;
            PushUpButton = false;
        }
        // リトライ選択中に上ボタンが押されたら
        if ((OnRetryFlag && PadVertical > 0) ||
            (OnRetryFlag && StickVertical > 0.2))
        {
            // 再開を選択中に
            SelectUI.transform.position = new Vector3(956, 740, 0);
            SelectCount = 0;
            PushUpButton = true;
            PushDownButton = false;
        }


        // タイトルを選択中に上ボタンが押されたら
        if ((OnTitleBackFlag && PadVertical > 0)||
            (OnTitleBackFlag && StickVertical > 0.2))
        {
            // リトライを選択中に
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
            // Padから手を離したら
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
            // Padから手を離したら
            if (PadVertical == 0)// || StickVertical == 0)
            {
                // 次の処理
                OnTitleBackFlag = true;
                OnRetryFlag = false;
                OnReStartFlag = false;
            }
        }
    }

    // ポーズ中の処理
    void Pausing()
    {
        // Pボタンが押されたらポーズ
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Joystick1Button7))
        {
            // ポーズメニュー表示
            PauseMenu.SetActive(true);
            SelectUI.SetActive(true);
            // ポーズ中である
            PositivePauseMenu = true;
            // 全体の時間を止める
            Time.timeScale = 0;
        }
        //// Lボタンが押されたらポーズ解除
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    // ポーズメニュー非表示
        //    PauseMenu.SetActive(false);
        //    // ポーズ中ではない
        //    PositivePauseMenu = false;
        //    // 全体の時間を通常に
        //    Time.timeScale = 1;
        //}
    }
}
