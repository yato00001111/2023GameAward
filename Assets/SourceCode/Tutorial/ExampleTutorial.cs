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

    // 4番目
    [SerializeField]
    GameObject Fource;

    // 5番目
    [SerializeField]
    GameObject Fifth;

    // 6番目
    [SerializeField]
    GameObject Sixth;

    // 7番目
    [SerializeField]
    GameObject Seventh;

    // 説明描画開始
    [SerializeField, HideInInspector]
    bool Start_Tutorial = false;

    // 説明描画終了
    [SerializeField, HideInInspector]
    bool End_Tutorial = false;

    // クリック回数
    [SerializeField, HideInInspector]
    int ClickCount = 0;

    private void Start()
    {
        StartTutorialSprite();
        First.SetActive(false);
        Second.SetActive(false);
        Third.SetActive(false);
        Fource.SetActive(false);
        Fifth.SetActive(false);
        Sixth.SetActive(false);
        Seventh.SetActive(false);
    }

    // 更新処理
    private void Update()
    {
        if (!Start_Tutorial)
        {
            First.SetActive(false);
            Second.SetActive(false);
            Third.SetActive(false);
            Fource.SetActive(false);
            Fifth.SetActive(false);
            Sixth.SetActive(false);
            Seventh.SetActive(false);
            return;
        }

        // Aボタンで
        if(Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetMouseButtonDown(0))
        {
            // 制御
            if(ClickCount < 7)
            // カウント上昇
            ClickCount++;
            
        }

        // クリック数で分岐
        switch(ClickCount)
        {
            // ０回
            case 0:
                First.SetActive(true);
                break;

            // １回
            case 1:
                First.SetActive(false);
                Second.SetActive(true);
                break;

            // ２回
            case 2:
                Second.SetActive(false);
                Third.SetActive(true);
                break;

            case 3:
                Third.SetActive(false);
                Fource.SetActive(true);
                break;

            case 4:
                Fource.SetActive(false);
                Fifth.SetActive(true);
                break;

            case 5:
                Fifth.SetActive(false);
                Sixth.SetActive(true);
                break;

            case 6:
                Sixth.SetActive(false);
                Seventh.SetActive(true);
                break;

            default:
                Seventh.SetActive(false);

                EndTutorialSprite();
                break;
        }
    }

    // チュートリアルスプライト描画開始
    public void StartTutorialSprite()
    {
        Start_Tutorial = true;
    }

    // チュートリアルスプライト描画終了
    public void EndTutorialSprite()
    {
        End_Tutorial = true;
    }

    public bool GetEndTutorial()
    {
        return End_Tutorial;
    }
}
