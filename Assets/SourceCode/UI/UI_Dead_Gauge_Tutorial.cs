using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_Dead_Gauge_Tutorial : MonoBehaviour
{

    [SerializeField]
    private RectTransform Dead_Gauge_Image_Rect;          // 死亡ゲージUI画像のRectTransform

    [SerializeField]
    private RectTransform Needle_Gauge_Image_Rect;        // 針ゲージUI画像のRectTransform

    [SerializeField]                                     
    private Image Tutorial_Content_01_Image;              // チュートリアル01画像
    [SerializeField]                                     
    private Image Tutorial_Content_02_Image;              // チュートリアル02画像
    [SerializeField]                                     
    private Image Tutorial_Content_03_Image;              // チュートリアル03画像

    [SerializeField]
    private Image[] Step_Image                            // ステップ画像
        = new Image[4];


    [SerializeField]
    private Image[] Arrow_Image                           // 矢印画像
        = new Image[3];                    

    [SerializeField]
    private Image Back_Black_Image;                       // 背景黒画像

    [SerializeField]
    private Animator Arrow_Animator;                      // 矢印アニメーション

    [SerializeField]
    private Image A_Image;                                // Aボタン画像

    [SerializeField]
    private bool Tutorial04_Start_Flag;                   // No04のチュートリアル開始フラグ

    [SerializeField]
    private bool Gauge_Stop_Flag;                         // ゲージストップフラグ

    [SerializeField]
    private bool[] Input_Stop_Flag
        =new bool[2];                                     // クリック制御フラグ

    [SerializeField]
    private bool Explanation_End_Flag;                    // チュートリアル04内の説明終了フラグ

    [SerializeField]
    private float ElapsedTime;                            // 死亡ゲージの拡大経過時間

    // Start is called before the first frame update
    void Start()
    {
        // チュートリアル画像を初期化する
        Tutorial_Content_01_Image.enabled = false;
        Tutorial_Content_02_Image.enabled = false;
        Tutorial_Content_03_Image.enabled = false;

        // 矢印画像を初期化する
        Arrow_Image[0].enabled = Arrow_Image[1].enabled = Arrow_Image[2].enabled = false;

        // Aボタン画像を初期化する
        A_Image.enabled = false;

        // 死亡ゲージの拡大経過時間を初期化する
        ElapsedTime = 0.0f;

        // スケールを初期化する
        Dead_Gauge_Image_Rect.localScale = new Vector3(0, 1, 1);

        // 針ゲージUI画像のRectTransformの回転値を初期化する
        Vector3 Angle;
        Angle.x = Needle_Gauge_Image_Rect.eulerAngles.x;
        Angle.y = 0.0f;
        Angle.z = -157.5f;
        Needle_Gauge_Image_Rect.eulerAngles = Angle;

        // No04のチュートリアル開始フラグを初期化する
        Tutorial04_Start_Flag = false;

        // ゲージストップフラグを初期化する
        Gauge_Stop_Flag = false;

        // クリック制御フラグを初期化する
        Input_Stop_Flag[0] = Input_Stop_Flag[1] = false;

        // チュートリアル04内の説明終了フラグを初期化する
        Explanation_End_Flag = false;

        // 背景黒画像を初期化する
        Back_Black_Image.enabled = false;
    }


    // Update is called once per frame
    void Update()
    {
        // チュートリアル04が開始されたら各画像の位置を戻す
        if (Tutorial04_Start_Flag)
        {
            // 針画像の登場



            // ゲージが0.2地点までいけば説明フェーズに入る
            if (Gauge_Stop_Flag)
            {
                // 矢印描画
                Arrow_Image[2].enabled = true;
                // "A"ボタン描画
                A_Image.enabled = true;
                // 矢印アニメーション
                Arrow_Animator.SetBool("Up",   false);
                Arrow_Animator.SetBool("Down", false);
                Arrow_Animator.SetBool("Side",  true);
                // チュートリアル01画像を描画する
                Tutorial_Content_01_Image.enabled = true;
                // 背景黒画像を描画する
                Back_Black_Image.enabled = true;

                // XBoxコントローラーの"A"ボタンが押された
                if ((Input.GetKeyDown(KeyCode.Joystick1Button0) || /*確認用*/ Input.GetKeyDown(KeyCode.A)) && !Input_Stop_Flag[0] && !Input_Stop_Flag[1])
                {
                    // チュートリアル01画像を非表示にする
                    Tutorial_Content_01_Image.enabled = false;
                    // チュートリアル02画像を描画する
                    Tutorial_Content_02_Image.enabled = true;
                }
                // XBoxコントローラーの"A"ボタンが離された
                else if ((Input.GetKeyUp(KeyCode.Joystick1Button0) || /*確認用*/   Input.GetKeyUp(KeyCode.A)) && !Input_Stop_Flag[0] && !Input_Stop_Flag[1])
                {
                    // クリック制御
                    Input_Stop_Flag[0] = true;
                }
                // XBoxコントローラーの"A"ボタンが押された
                else if ((Input.GetKeyDown(KeyCode.Joystick1Button0) || /*確認用*/  Input.GetKeyDown(KeyCode.A)) && Input_Stop_Flag[0] && !Input_Stop_Flag[1])
                {
                    // チュートリアル02画像を非表示にする
                    Tutorial_Content_02_Image.enabled = false;
                    // チュートリアル03画像を描画する
                    Tutorial_Content_03_Image.enabled = true;
                }
                // XBoxコントローラーの"A"ボタンが離された
                else if ((Input.GetKeyUp(KeyCode.Joystick1Button0) || /*確認用*/  Input.GetKeyUp(KeyCode.A)) && Input_Stop_Flag[0] && !Input_Stop_Flag[1])
                {
                    // クリック制御
                    Input_Stop_Flag[1] = true;
                }
                // XBoxコントローラーの"A"ボタンが押された
                else if ((Input.GetKeyDown(KeyCode.Joystick1Button0) || /*確認用*/ Input.GetKeyDown(KeyCode.A)) && Input_Stop_Flag[0] && Input_Stop_Flag[1])
                {
                    // 説明を終了する
                    Explanation_End_Flag = true;
                    // ゲージを再開させる
                    Gauge_Stop_Flag = false;
                    // 必要な画像以外全て非表示
                    Arrow_Image[2].enabled = false;
                    A_Image.enabled = false;
                    Tutorial_Content_01_Image.enabled = false;
                    Tutorial_Content_02_Image.enabled = false;
                    Tutorial_Content_03_Image.enabled = false;
                    // 背景黒画像を非表示にする
                    Back_Black_Image.enabled = false;
                }
            }

            // 念のため常にゲージを再開させる
            if (Explanation_End_Flag) Gauge_Stop_Flag = false;

            // ゲージが止まっている間はここでリターン
            if (Gauge_Stop_Flag) return;

            // タイマー起動
            ElapsedTime += Time.deltaTime;
            float Current_Time = Mathf.Clamp01(ElapsedTime / 4.8f);
            // 演出
            OnScale(Current_Time);

            float Current_Value = Mathf.Lerp(0.0f, 1.0f, Current_Time);

            // 回転関数
            UI_Rotate(Current_Value);


           
        }
    }

    // 死亡ゲージUI画像のスケールを変更
    private void OnScale(float value)
    {
        // 消えるフェーズ中なら360度回転　それ以外は通常通り45度回転
        var Scale = Mathf.Lerp(0.0f, 1.0f, value);
        // スケール反映
        Dead_Gauge_Image_Rect.localScale = new Vector3(Scale, 1, 1);
        // 0.2まで行ったら一旦止める
        if (Scale >= 0.2f && !Explanation_End_Flag) Gauge_Stop_Flag = true;
    }

    // 針ゲージUIを回転させる関数
    private void UI_Rotate(float timer)
    {
        var Angle = Mathf.Lerp(-157.5f, -517.5f, timer);
        Needle_Gauge_Image_Rect.eulerAngles = new Vector3(Needle_Gauge_Image_Rect.eulerAngles.x, 0f, Angle);
    }

    // No04のチュートリアル開始フラグの真偽設定関数
    public void SetTutorial04StartFlag() { Tutorial04_Start_Flag = true; }

    // 矢印アニメーション開始関数(UP)
    public void SetArrowUpAnimation() 
    {
        // 矢印描画
        Arrow_Image[0].enabled = true;
        Arrow_Image[1].enabled = false;
        Arrow_Image[2].enabled = false;
        // 矢印アニメーション
        Arrow_Animator.SetBool("Up",    true);
        Arrow_Animator.SetBool("Down", false);
        Arrow_Animator.SetBool("Side", false);
    }

    // No04のチュートリアル開始フラグの真偽設定関数
    public void SetArrowDownAnimation() 
    {
        // 矢印描画
        Arrow_Image[1].enabled = true;
        Arrow_Image[0].enabled = false;
        Arrow_Image[2].enabled = false;
        // 矢印アニメーション
        Arrow_Animator.SetBool("Up",   false);
        Arrow_Animator.SetBool("Down",  true);
        Arrow_Animator.SetBool("Side", false);
    }

    public void SetArrowResetAnimation()
    {
        // 矢印描画
        Arrow_Image[0].enabled = false;
        Arrow_Image[1].enabled = false;
        Arrow_Image[2].enabled = false;
    }

    // 何回成功したかの画像設定関数
    public void SetStepImage(int Num)
    {
        // 指定のステップまで描画する
        for (int index = 0; index < Num; ++index) 
        {
            Step_Image[index + 1].enabled = true;
        }
    }
}
