using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Level : MonoBehaviour
{

    [SerializeField]
    private float WaveTimer = 0.0f;       // ウェーブタイマー

    [SerializeField]
    private int Now_Level = 0;            // 現在のレベル

    [SerializeField]                      
    private Image WaveFrame_Image;        // ウェーブフレーム画像
                                          
    [SerializeField]                      
    private Image WaveGauge_Image;        // ウェーブゲージ画像

    [SerializeField]
    private RectTransform WaveGauge_Rect; // ウェーブゲージRectTransform

    [SerializeField]
    private Image[] LevelNumber_Images
        = new Image[11];                  // 現在のレベル番号画像

    [SerializeField]
    private Image[] LevelGauge_Images
    = new Image[11];                      // 現在のレベルゲージ画像


    // Start is called before the first frame update
    void Start()
    {
        // 画像初期設定
        //WaveFrame_Image = GetComponent<Image>();
        //WaveGauge_Image = GetComponent<Image>();

        // ウェーブゲージの初期幅を"0"にする
        // 最大値は"266"
        // WaveGauge_Rect  = gameObject.Find("Wave").GetComponent<RectTransform>();
        WaveGauge_Rect.sizeDelta = new Vector2(0.0f, 9.0f);

        // 難易度初期化
        Now_Level = 1;
    }

    // Update is called once per frame
    void Update()
    {
        // ウェーブタイマー処理
        UpdateWaveTimer();

        // ウェーブゲージの幅処理
        UpdateWaveGauge();

        // レベル関連演出処理
        UpdateLevelPerform();
    }

    void UpdateWaveTimer()
    {
        // ウェーブタイム起動
        WaveTimer += Time.deltaTime;

        // 20秒経過後
        if(WaveTimer>20.0f)
        {
            // タイマー起動
            WaveTimer = 0.0f;

            //***<<レベルアップ>>***//
            Now_Level += 1;
        }
    }

    void UpdateWaveGauge()
    {
        // 最大幅まで伸ばす
        float Width = 13.3f * WaveTimer;
        WaveGauge_Rect.sizeDelta = new Vector2(Width, 9.0f);
    }


    void UpdateLevelPerform()
    {
        // 現在のレベル関連の画像描画処理
        for (int Num = 0; Num < 11; ++Num) 
        {
            //***<<レベル番号>>***//
            // 描画する
            if (Num == Now_Level - 1) 
            {
                LevelNumber_Images[Num].enabled = true;
            }
            // 描画しない
            else
            {
                LevelNumber_Images[Num].enabled = false;
            }

            //***<<レベルゲージ>>***//
            // 描画する

            LevelGauge_Images[]

            if (Num == Now_Level - 1)
            {
                LevelNumber_Images[Num].enabled = true;
            }
            // 描画しない
            else
            {
                LevelNumber_Images[Num].enabled = false;
            }
        }
       
    }
}
