using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Operate : MonoBehaviour
{

    [SerializeField]
    private Image[] Operate1_Image = new Image[3];      // クイックドロップ操作方法UI画像

    [SerializeField]
    private Image[] Operate2_Image = new Image[3];      // 大回転操作方法UI画像

    [SerializeField]
    private Image[] Operate3_Image = new Image[3];      // 半回転操作方法UI画像

    [SerializeField]
    private Image[] Operate4_Image = new Image[2];      // 入れ替え操作方法UI画像

    [SerializeField]
    private Image Select_Operate_Image;                 // 現在選択中の操作方法UI画像
    [SerializeField]
    private RectTransform Select_Operate_Image_Rect;    // 現在選択中UI画像のRectTransform
    [SerializeField]
    private int[] Select_Operate_Number = new int[4];   // 現在選択中の操作方法UI画像の番号
    [SerializeField]
    private float Select_Operate_Alpha;                 // 現在選択中の操作方法UI画像の透明値
    [SerializeField]
    private bool Select_Operate_Alpha_Flag;             // 現在選択中の操作方法UI画像の透明値フラグ


    [SerializeField]
    private Animator Operate1_Animator;                 // クイックドロップ操作方法UIのアニメーション

    [SerializeField]
    private float Operate2_Image_Angle;                 // 大回転操作方法UI画像の回転値

    [SerializeField]
    private float Operate3_Image_Angle;                 // 半回転操作方法UI画像の回転値

    [SerializeField]
    private RectTransform Operate2_Image_Rect;          // 大回転操作方法UI画像のRectTransform
                                                        
    [SerializeField]                                    
    private RectTransform Operate3_Image_Rect;          // 半回転操作方法UI画像のRectTransform

    [SerializeField]
    private RectTransform Operate4_Image_Rect;          // 入れ替え操作方法UI画像のRectTransform

    [SerializeField]
    private float Operate4_Time;                        // 入れ替え操作方法UI画像のアニメーション時間

    [SerializeField]
    private bool Operate4_Swap_Flag;                    // 入れ替え操作方法UI画像のアニメーションフラグ

    [SerializeField]
    public UI_CountDown uI_CountDown;                   // ゲーム開始カウントダウンスクリプト


    // Start is called before the first frame update
    void Start()
    {
        // 回転値変数を初期化する
        Operate2_Image_Angle = 0.0f;
        Operate3_Image_Angle = 0.0f;
        // 入れ替え操作方法UI画像のアニメーション時間を初期化する
        Operate4_Time = 0.0f;
        // 入れ替え操作方法UI画像のアニメーションフラグを初期化する
        Operate4_Swap_Flag = false;
        // 現在選択中の操作方法UI画像の番号を初期化する
        Select_Operate_Number[0] = Select_Operate_Number[1] =
        Select_Operate_Number[2] = Select_Operate_Number[3] = 0;
        // 現在選択中の操作方法UI画像の透明値を初期化する
        Select_Operate_Alpha = 0.0f;
        // 現在選択中の操作方法UI画像の透明値フラグを初期化する
        Select_Operate_Alpha_Flag = false;
    }

    // Update is called once per frame
    void Update()
    {

        // 選択中の操作方法UI画像の演出設定
        if (Select_Operate_Number[0] != 0 || Select_Operate_Number[1] != 0 ||
            Select_Operate_Number[2] != 0 || Select_Operate_Number[3] != 0 )
        {
            // 透明度演出
            if      (!Select_Operate_Alpha_Flag && Select_Operate_Alpha < 0.3f) Select_Operate_Alpha += 0.025f;
            else if (!Select_Operate_Alpha_Flag && Select_Operate_Alpha >= 0.3f) Select_Operate_Alpha_Flag = true;
            else if (Select_Operate_Alpha_Flag  && Select_Operate_Alpha > 0.0f) Select_Operate_Alpha -= 0.025f;
            else if (Select_Operate_Alpha_Flag  && Select_Operate_Alpha <= 0.0f) Select_Operate_Alpha_Flag = false;

            // 透明値設定
            Select_Operate_Image.color = new Color(1, 1, 1, Select_Operate_Alpha);

            if      (Select_Operate_Number[0] == 1) Select_Operate_Image_Rect.anchoredPosition = new Vector3(-780.0f, 324.0f - 200.0f);
            else if (Select_Operate_Number[1] == 1) Select_Operate_Image_Rect.anchoredPosition = new Vector3(-780.0f,  54.0f - 200.0f);
            else if (Select_Operate_Number[2] == 1) Select_Operate_Image_Rect.anchoredPosition = new Vector3(-585.0f,  54.0f - 200.0f);
            else if (Select_Operate_Number[3] == 1) Select_Operate_Image_Rect.anchoredPosition = new Vector3(-585.0f, 324.0f - 200.0f);

            // スケール変更
            if      (Select_Operate_Number[1] == 1 || Select_Operate_Number[2] == 1) Select_Operate_Image_Rect.localScale = new Vector3(1, 0.9f, 1);
            else if (Select_Operate_Number[0] == 1 || Select_Operate_Number[3] == 1) Select_Operate_Image_Rect.localScale = new Vector3(1, 1, 1);
        }
        // 未選択中はリセットする
        else if (Select_Operate_Number[0] == 0 && Select_Operate_Number[1] == 0 && 
                 Select_Operate_Number[2] == 0 && Select_Operate_Number[3] == 0 ) 
        {
            Select_Operate_Alpha_Flag = false;
            Select_Operate_Alpha = 0.0f;
            Select_Operate_Image.color = new Color(1, 1, 1, Select_Operate_Alpha);
            Select_Operate_Image_Rect.localScale = new Vector3(1, 1, 1);
        }

        float TrigerInput = Input.GetAxisRaw("Trigger");

        // XBoxコントローラーの"A"ボタンが押されている間は"クイックドロップ"UI演出
        if (Input.GetKey(KeyCode.Joystick1Button0) || /*確認用*/ Input.GetKey(KeyCode.DownArrow) && uI_CountDown.GetGameStartFlag())  
        {
            // 操作中画像のみを描画する
            Operate1_Image[1].enabled = true;
            Operate1_Image[2].enabled = true;
            // 通常画像は描画しない
            Operate1_Image[0].enabled = false;
            // 選択中設定
            Select_Operate_Number[0] = 1;
        }
        // XBoxコントローラーの"A"ボタンが押されていない間は通常画像を描画
        else
        {
            // 通常画像のみを描画する
            Operate1_Image[0].enabled = true;
            // 操作中画像は描画しない
            Operate1_Image[1].enabled = false;
            Operate1_Image[2].enabled = false;

            // アニメーションの再生時間を"0"にする
            Operate1_Animator.Play(0, -1, 0f);
            // 選択中設定リセット
            Select_Operate_Number[0] = 0;
        }

        // XBoxコントローラーの"LT RT"ボタンが押されている間は"大回転"UI演出
        if ((TrigerInput < 0.0f || TrigerInput > 0.0f) || /*確認用*/ Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A) && uI_CountDown.GetGameStartFlag()) 
        {
            // 操作中画像のみを描画する
            Operate2_Image[1].enabled = true;
            Operate2_Image[2].enabled = true;
            // 通常画像は描画しない
            Operate2_Image[0].enabled = false;

            // 押されている間、回転値を増やす
            Operate2_Image_Angle = Time.deltaTime * -200.0f;
            // 大回転操作方法UIアイコンを回転させる
            // その際、角度(Operate2_Image_Angle)を0～360度の範囲に正規化する
            var Normalized_Operate2_Angle = Mathf.Repeat(Operate2_Image_Angle, 360);
            Operate2_Image_Rect.Rotate(.0f, .0f, Normalized_Operate2_Angle);
            // 選択中設定
            Select_Operate_Number[1] = 1;
        }
        // XBoxコントローラーの"LT RT"ボタンが押されていない間は通常画像を描画
        else
        {
            // 画像の回転値を初期化する
            Operate2_Image_Angle = 0.0f;
            Vector3 Reset_Angle;
            // 0.0fを格納するとInspector側で-90が入る為、自身のX値を格納する
            Reset_Angle.x = Operate2_Image_Rect.eulerAngles.x;
            Reset_Angle.y = 0.0f;
            Reset_Angle.z = 0.0f;
            // 角度リセット
            Operate2_Image_Rect.eulerAngles = Reset_Angle;

            // 通常画像のみを描画する
            Operate2_Image[0].enabled = true;
            // 操作中画像は描画しない
            Operate2_Image[1].enabled = false;
            Operate2_Image[2].enabled = false;
            // 選択中設定リセット
            Select_Operate_Number[1] = 0;
        }

        // XBoxコントローラーの"LB RB"ボタンが押されている間は"半回転"UI演出
        if ((Input.GetKey(KeyCode.Joystick1Button4) || Input.GetKey(KeyCode.Joystick1Button5)) || /*確認用*/ Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow) && uI_CountDown.GetGameStartFlag()) 
        {
            // 操作中画像のみを描画する
            Operate3_Image[1].enabled = true;
            Operate3_Image[2].enabled = true;
            // 通常画像は描画しない
            Operate3_Image[0].enabled = false;

            // 押されている間、回転値を増やす
            Operate3_Image_Angle = Time.deltaTime * -200.0f;
            // 大回転操作方法UIアイコンを回転させる
            // その際、角度(Operate2_Image_Angle)を0～360度の範囲に正規化する
            var Normalized_Operate3_Angle = Mathf.Repeat(Operate3_Image_Angle, 360);
            Operate3_Image_Rect.Rotate(.0f, .0f, Normalized_Operate3_Angle);
            // 選択中設定
            Select_Operate_Number[2] = 1;
        }
        // XBoxコントローラーの"LB RB"ボタンが押されていない間は通常画像を描画
        else
        {
            // 画像の回転値を初期化する
            Operate3_Image_Angle = 0.0f;
            Vector3 Reset_Angle;
            // 0.0fを格納するとInspector側で-90が入る為、自身のX値を格納する
            Reset_Angle.x = Operate3_Image_Rect.eulerAngles.x;
            Reset_Angle.y = 0.0f;
            Reset_Angle.z = 0.0f;
            // 角度リセット
            Operate3_Image_Rect.eulerAngles = Reset_Angle;

            // 通常画像のみを描画する
            Operate3_Image[0].enabled = true;
            // 操作中画像は描画しない
            Operate3_Image[1].enabled = false;
            Operate3_Image[2].enabled = false;

            // 画像の回転値を初期化する
            Operate3_Image_Angle = 0.0f;
            // 選択中設定リセット
            Select_Operate_Number[2] = 0;
        }

        // XBoxコントローラーの"X"ボタンが押されている間は"入れ替え"UI演出
        if (Input.GetKey(KeyCode.Joystick1Button2) || /*確認用*/ Input.GetKey(KeyCode.UpArrow) && uI_CountDown.GetGameStartFlag())
        {
            // 操作中画像のみを描画する
            Operate4_Image[1].enabled = true;
            // 通常画像は描画しない
            Operate4_Image[0].enabled = false;

            // 押されている間、時間を増やす
            Operate4_Time += Time.deltaTime;
            // 一定時間経過したら
            if (Operate4_Time > 0.25f) 
            {
                // フラグ切り替え
                Operate4_Swap_Flag = !Operate4_Swap_Flag;
                // タイムリセット
                Operate4_Time = 0.0f;
            }

            // 大回転操作方法UIアイコンを回転させる
            if       (Operate4_Swap_Flag && Operate4_Time == .0f) Operate4_Image_Rect.Rotate(.0f, 180.0f, .0f);
            else if (!Operate4_Swap_Flag && Operate4_Time == .0f) Operate4_Image_Rect.Rotate(.0f,    .0f, .0f);
            // 選択中設定
            Select_Operate_Number[3] = 1;

        }
        // XBoxコントローラーの"X"ボタンが押されていない間は通常画像を描画
        else
        {
            // 画像の回転値を初期化する
            Vector3 Reset_Angle;
            // 0.0fを格納するとInspector側で-90が入る為、自身のX値を格納する
            Reset_Angle.x = Operate4_Image_Rect.eulerAngles.x;
            Reset_Angle.y = 0.0f;
            Reset_Angle.z = 0.0f;
            // 角度リセット
            Operate4_Image_Rect.eulerAngles = Reset_Angle;

            // 通常画像のみを描画する
            Operate4_Image[0].enabled = true;
            // 操作中画像は描画しない
            Operate4_Image[1].enabled = false;
            // 時間とフラグをリセット
            Operate4_Swap_Flag = false;
            Operate4_Time = 0.0f;
            // 選択中設定リセット
            Select_Operate_Number[3] = 0;
        }
    }
}
