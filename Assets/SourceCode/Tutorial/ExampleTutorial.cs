using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExampleTutorial : MonoBehaviour
{
    // 最初の画像
    [SerializeField]
    GameObject First;
        
    // ２番目
    [SerializeField]
    GameObject Second;

    // ３番目
    [SerializeField]
    GameObject Third;

    // 説明描画開始
    [SerializeField, HideInInspector]
    bool Start_Tutorial = false;

    // クリック回数
    [SerializeField, HideInInspector]
    int ClickCount = 0;

    // 更新処理
    private void Update()
    {
        if (!Start_Tutorial)
        {
            First.SetActive(false);
            Second.SetActive(false);
            Third.SetActive(false);
            return;
        }

        // Aボタンで
        if(Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            // 制御
            if(ClickCount < 3)
            // カウント上昇
            ClickCount++;
            
        }

        // クリック数で分岐
        switch(ClickCount)
        {
            // ０回
            case 0:
                First.SetActive(true);
                Second.SetActive(false);
                Third.SetActive(false);
                break;

            // １回
            case 1:
                First.SetActive(false);
                Second.SetActive(true);
                Third.SetActive(false);
                break;

            // ２回
            case 2:
                First.SetActive(false);
                Second.SetActive(false);
                Third.SetActive(true);
                break;

            // それ以上
            default:
                First.SetActive(false);
                Second.SetActive(false);
                Third.SetActive(false);
                break;
        }
    }

    // チュートリアルスプライト描画開始
    public void StartTutorialSprite()
    {
        Start_Tutorial = true;
    }
}
