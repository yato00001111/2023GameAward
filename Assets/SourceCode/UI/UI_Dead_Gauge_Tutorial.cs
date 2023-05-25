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
    private RectTransform Revers_Image_Rect;              // 色彩反転UI画像のRectTransform
    [SerializeField]
    private float Revers_Image_Scale;                     // 色彩反転UI画像のサイズ

    [SerializeField]
    private RectTransform Revers_Image_Rect2;             // 色彩反転UI画像のRectTransform(2枚目)
    [SerializeField]
    private float Revers_Image_Scale2;                    // 色彩反転UI画像のサイズ(2枚目)


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

    [SerializeField]
    private float ElapsedTime2;                           // 死亡ゲージの縮小経過時間

    [SerializeField]
    private bool Is_Disappear_Phase_Flag;                 // 消えるフェーズフラグ

    [SerializeField]
    private float Dead_Gauge_ScaleX;                      // 死亡ゲージUI画像の横幅変数

    [SerializeField]
    private bool Is_Disappear_Phase_End_Flag;             // 消えるフェーズの終了フラグ

    [SerializeField]
    private bool TutorialEndFlag;                         // "チュートリアル"の終了フラグ

    private const float Stepalpha = 0.2f;

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

        // ステップ画像を初期化する
        Step_Image[0].enabled = Step_Image[1].enabled = Step_Image[2].enabled = Step_Image[3].enabled = false;
        Step_Image[0].color   = Step_Image[1].color   = Step_Image[2].color   = Step_Image[3].color   = new Vector4(1, 1, 1, Stepalpha);

        // 色彩反転画像のスケール初期化する
        Revers_Image_Rect.localScale = new Vector3(0, 0, 0);
        Revers_Image_Rect2.localScale = new Vector3(0, 0, 0);
        // 色彩反転UI画像のサイズ変数を初期化する
        Revers_Image_Scale = 0.0f;
        Revers_Image_Scale2 = 0.0f;
        // 死亡ゲージUI画像の横幅変数を初期化する
        Dead_Gauge_ScaleX = 0.0f;
        // 消えるフェーズの終了フラグを初期化する
        Is_Disappear_Phase_End_Flag = false;
        // "チュートリアル"の終了フラグを初期化する
        TutorialEndFlag = false;
        // 死亡ゲージの縮小経過時間を初期化する
        ElapsedTime2 = 0.0f;
    }


    // Update is called once per frame
    void Update()
    {
        // チュートリアル04が開始されたら各画像の位置を戻す
        if (Tutorial04_Start_Flag)
        {
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
                if ((Input.GetKeyDown(KeyCode.Joystick1Button0) || /*確認用*/ Input.GetKeyDown(KeyCode.Return)) && !Input_Stop_Flag[0] && !Input_Stop_Flag[1])
                {
                    // チュートリアル01画像を非表示にする
                    Tutorial_Content_01_Image.enabled = false;
                    // チュートリアル02画像を描画する
                    Tutorial_Content_02_Image.enabled = true;
                }
                // XBoxコントローラーの"A"ボタンが離された
                else if ((Input.GetKeyUp(KeyCode.Joystick1Button0) || /*確認用*/   Input.GetKeyUp(KeyCode.Return)) && !Input_Stop_Flag[0] && !Input_Stop_Flag[1])
                {
                    // クリック制御
                    Input_Stop_Flag[0] = true;
                }
                // XBoxコントローラーの"A"ボタンが押された
                else if ((Input.GetKeyDown(KeyCode.Joystick1Button0) || /*確認用*/  Input.GetKeyDown(KeyCode.Return)) && Input_Stop_Flag[0] && !Input_Stop_Flag[1])
                {
                    // チュートリアル02画像を非表示にする
                    Tutorial_Content_02_Image.enabled = false;
                    // チュートリアル03画像を描画する
                    Tutorial_Content_03_Image.enabled = true;
                }
                // XBoxコントローラーの"A"ボタンが離された
                else if ((Input.GetKeyUp(KeyCode.Joystick1Button0) || /*確認用*/  Input.GetKeyUp(KeyCode.Return)) && Input_Stop_Flag[0] && !Input_Stop_Flag[1])
                {
                    // クリック制御
                    Input_Stop_Flag[1] = true;
                }
                // XBoxコントローラーの"A"ボタンが押された
                else if ((Input.GetKeyDown(KeyCode.Joystick1Button0) || /*確認用*/ Input.GetKeyDown(KeyCode.Return)) && Input_Stop_Flag[0] && Input_Stop_Flag[1])
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

            // 消えるフェーズ中以外は通常通り動かす
            if (!Is_Disappear_Phase_Flag)
            {
                // タイマー起動
                ElapsedTime += Time.deltaTime;
                float Current_Time = Mathf.Clamp01(ElapsedTime / 4.8f);
                // 演出
                OnScale(Current_Time);
                float Current_Value = Mathf.Lerp(0.0f, 1.0f, Current_Time);
                // 回転関数
                UI_Rotate(Current_Value);
            }
           
            // 消えるフェーズに入ったら消えるフェーズフラグを立てる
            if (Dead_Gauge_ScaleX >= 1.0f)
            {
                // 1枚目の色彩反転画像のサイズを大きくしていく
                if (Revers_Image_Scale < 20.0f) Revers_Image_Scale += 0.25f;
                // 1枚目の色彩反転画像のサイズが最大値までいけば2枚目の色彩反転画像のサイズも大きくしていく
                else if (Revers_Image_Scale >= 20.0f && Revers_Image_Scale2 < 15.0f) Revers_Image_Scale2 += 0.25f;
            }


            // 色彩反転演出が終了次第消えるフェーズに入る フラグを立てる
            if (Revers_Image_Scale2 >= 15.0f) Is_Disappear_Phase_Flag = true;

            // 消えるフェーズ中は死亡ゲージのスケールを減らす
            if (Is_Disappear_Phase_Flag)
            {
                // タイマー起動
                ElapsedTime2 += Time.deltaTime;
                float Current_Time = Mathf.Clamp01(ElapsedTime2 / 4.8f);
                // 演出
                OnScale(Current_Time);
                float Current_Value = Mathf.Lerp(0.0f, 1.0f, Current_Time);
                // 回転関数
                UI_Rotate(Current_Value);
            }

            // 消えるフェーズの終了お知らせを受け取ったら
            if (Is_Disappear_Phase_End_Flag)
            {
                // 2枚目の色彩反転画像のサイズを小さくしていく
                if (Revers_Image_Scale2 > 0.0f) Revers_Image_Scale2 -= 0.25f;
                // 2枚目の色彩反転画像のサイズが最小値までいけば1枚目の色彩反転画像のサイズも小さくしていく
                else if (Revers_Image_Scale2 <= 0.0f && Revers_Image_Scale > 0.0f) Revers_Image_Scale -= 0.25f;

                // 色彩反転演出が終了すれば
                if (Revers_Image_Scale <= 0.0f)
                {
                    // チュートリアル終了フラグを立てる
                    TutorialEndFlag = true;
                }
            }


            // 色彩反転画像のサイズ設定
            Revers_Image_Rect.localScale  = new Vector3(Revers_Image_Scale,  Revers_Image_Scale,  Revers_Image_Scale);
            Revers_Image_Rect2.localScale = new Vector3(Revers_Image_Scale2, Revers_Image_Scale2, Revers_Image_Scale2);
        }
    }

    // 死亡ゲージUI画像のスケールを変更
    private void OnScale(float value)
    {
        // 消えるフェーズ中なら360度回転　それ以外は通常通り45度回転
        var Scale = Is_Disappear_Phase_Flag ? Mathf.Lerp(1.0f, 0.0f, value) : Mathf.Lerp(0.0f, 1.0f, value);
        // スケール反映
        Dead_Gauge_Image_Rect.localScale = new Vector3(Scale, 1, 1);
        // 0.2まで行ったら一旦止める
        if (Scale >= 0.2f && !Explanation_End_Flag) Gauge_Stop_Flag = true;

        // スケール値保存する
        Dead_Gauge_ScaleX = Scale;

        // 消えるフェーズのゲージが0になれば
        if (Is_Disappear_Phase_Flag && Scale <= 0.0f) 
        {
            // 消えるフェーズの終了フラグを立てる
            Is_Disappear_Phase_End_Flag = true;
        }
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
        //*************************************//
        // 引数が"5"ならそもそもの描画をしない //
        //*************************************//
        if (Num == 5) 
        {
            for (int index = 0; index < 4; ++index)
            {
                // 念のため半透明にしておく
                Step_Image[index].color = new Vector4(1, 1, 1, Stepalpha);
                // 非表示にする
                Step_Image[index].enabled = false;
            }
            return;
        }

        // この関数が呼ばれた段階で表示する
        for (int index = 0; index < 4; ++index)
        {
            Step_Image[index].enabled = true;
        }

        // 指定のステップまで描画する
        if      (Num == 0) 
        {
            Step_Image[0].color = new Vector4(1, 1, 1, Stepalpha);
            Step_Image[1].color = new Vector4(1, 1, 1, Stepalpha);
            Step_Image[2].color = new Vector4(1, 1, 1, Stepalpha);
            Step_Image[3].color = new Vector4(1, 1, 1, Stepalpha);
        }
        else if (Num == 1)
        {
            Step_Image[0].color = new Vector4(1, 1, 1, 1.0f);
            Step_Image[1].color = new Vector4(1, 1, 1, Stepalpha);
            Step_Image[2].color = new Vector4(1, 1, 1, Stepalpha);
            Step_Image[3].color = new Vector4(1, 1, 1, Stepalpha);
        }
        else if (Num == 2)
        {
            Step_Image[0].color = new Vector4(1, 1, 1, 1.0f);
            Step_Image[1].color = new Vector4(1, 1, 1, 1.0f);
            Step_Image[2].color = new Vector4(1, 1, 1, Stepalpha);
            Step_Image[3].color = new Vector4(1, 1, 1, Stepalpha);
        }
        else if (Num == 3)
        {
            Step_Image[0].color = new Vector4(1, 1, 1, 1.0f);
            Step_Image[1].color = new Vector4(1, 1, 1, 1.0f);
            Step_Image[2].color = new Vector4(1, 1, 1, 1.0f);
            Step_Image[3].color = new Vector4(1, 1, 1, Stepalpha);
        }
        else if (Num == 4)
        {
            Step_Image[0].color = new Vector4(1, 1, 1, 1.0f);
            Step_Image[1].color = new Vector4(1, 1, 1, 1.0f);
            Step_Image[2].color = new Vector4(1, 1, 1, 1.0f);
            Step_Image[3].color = new Vector4(1, 1, 1, 1.0f);
        }
    }

    // チュートリアル終了フラグ取得関数
    public bool GetTutorialEndFlag() { return TutorialEndFlag; }
}
