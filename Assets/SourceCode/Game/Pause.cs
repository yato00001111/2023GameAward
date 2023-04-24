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
    bool PositivePauseMenu = false;

    // Start is called before the first frame update
    void Start()
    {
        // ポーズ中ではない
        PauseMenu.SetActive(false);
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
        if (SelectUI.transform.position.y == 740.0f)
        {
            // Enterキーが押されたら
            if (Input.GetKeyDown(KeyCode.Return))
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
        if (SelectUI.transform.position.y == 540.0f)
        {
            // Enterキーが押されたら
            if (Input.GetKeyDown(KeyCode.Return))
            {
                // リトライ処理
            }
        }

        // 選択UIがタイトルUI上にあって
        if (SelectUI.transform.position.y == 340.0f)
        {
            // Enterキーが押されたら
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SceneManager.LoadScene("Title");
            }
        }
    }

    // 選択UIの動き
    void ActionSelectUI()
    {
        // 下ボタンが押されたら
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            // 選択UIを1つ下に
            SelectUI.transform.position -= new Vector3(0, 200.0f, 0);
        }
        // 上ボタンが押されたら
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // 選択UIを1つ上に
            SelectUI.transform.position += new Vector3(0, 200.0f, 0);
        }

        // 選択UIが1番上にあったら
        if (SelectUI.transform.position.y < 340.0f)
        {
            // それ以上上にいかせない
            SelectUI.transform.position = new Vector3(956, 340, 0);
        }
        // 選択UIが1番下にあったら
        if (SelectUI.transform.position.y > 740.0f)
        {
            // それ以上下にいかせない
            SelectUI.transform.position = new Vector3(956, 740, 0);
        }
    }

    // ポーズ中の処理
    void Pausing()
    {
        // Pボタンが押されたらポーズ
        if (Input.GetKeyDown(KeyCode.P))
        {
            // ポーズメニュー表示
            PauseMenu.SetActive(true);
            // ポーズ中である
            PositivePauseMenu = true;
            // 全体の時間を止める
            Time.timeScale = 0;
        }
        // Lボタンが押されたらポーズ解除
        if (Input.GetKeyDown(KeyCode.L))
        {
            // ポーズメニュー非表示
            PauseMenu.SetActive(false);
            // ポーズ中ではない
            PositivePauseMenu = false;
            // 全体の時間を通常に
            Time.timeScale = 1;
        }
    }

}
