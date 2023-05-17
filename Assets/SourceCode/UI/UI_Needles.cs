using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_Needles : MonoBehaviour
{
    [SerializeField]
    private float Clap_Time;                       // 拍の間隔の秒数
                                                   
    [SerializeField]                               
    private float Current_Time;                    // 確認用現在のタイム
                                                   
    [SerializeField]                               
    private int Clap_Count;                        // 拍のカウント変数
                                                   
    [SerializeField]                               
    private RectTransform Needle_Gauge_Image_Rect; // 針ゲージUI画像のRectTransform
    [SerializeField]
    private Image Needle_Gauge_Image_Color;        // 針ゲージUI画像のColor
    [SerializeField]
    private float Needle_Gauge_Image_AngleZ;       // 針ゲージUI画像のRectTransformの"Z"回転値
    [SerializeField]
    private float Needle_Gauge_Image_RotateZ;      // 針ゲージUI画像のRectTransformの初回Rotate"Z"値
    [SerializeField]
    private float Save_AngleZ;                     // 前回の"Z"回転値を保存する変数
    [SerializeField]
    private int Needle_Gauge_Number;               // 針ゲージの現在の角度番号
    [SerializeField]
    private float Needle_Gauge_ElapsedTime;        // 針ゲージの経過時間
    [SerializeField]                               
    private bool Needle_Gauge_Is_Changing;         // 針ゲージが変化中真偽フラグ


    [SerializeField]
    private RectTransform Rhythm_Gauge_Image_Rect; // リズムゲージUI画像のRectTransform
    [SerializeField]
    private float Rhythm_Gauge_Image_PosX;         // リズムゲージUI画像のRectTransformの"X"移動値
    [SerializeField]
    private float Rhythm_Gauge_ElapsedTime;        // リズムゲージの経過時間
    [SerializeField]
    private bool  Rhythm_Gauge_Is_Changing;        // リズムゲージが変化中真偽フラグ
    [SerializeField]
    private int   Rhythm_Transition_Count;         // リズムゲージの移動カウント

    [SerializeField]
    private RectTransform Rhythm_Image_Rect;       // リズムUI画像のRectTransform
    [SerializeField]
    private bool Succeed_Flag;                     // リズム操作成功真偽フラグ
    [SerializeField]
    private float Save_Succeed_Time;               // リズム操作成功時の時間


    [SerializeField]
    private RectTransform Phase_Gauge_Image_Rect;  // フェーズゲージUI画像のRectTransform

    [SerializeField]
    private int GameStart_ClapCount;               // ゲーム開始直後の拍のタイミング

    bool beat;

    // Start is called before the first frame update
    void Start()
    {
        // 拍の間隔の秒数を初期化する
        Clap_Time                       = 0.0f;
        // 確認用現在のタイム変数を初期化する
        Current_Time                    = 0.0f;
        // 拍のカウント変数 を初期化する
        Clap_Count                      = 0;

        // 針ゲージUI画像のRectTransformの"Z"回転値を初期化する
        Needle_Gauge_Image_AngleZ       = 45.0f;
        // 針ゲージUI画像のRectTransformのRotate"Z"値
        Needle_Gauge_Image_RotateZ      = -157.5f;
        // 針ゲージUI画像のRectTransformの回転値を初期化する
        Vector3 Angle;
        Angle.x = Needle_Gauge_Image_Rect.eulerAngles.x;
        Angle.y = 0.0f;
        Angle.z = Needle_Gauge_Image_RotateZ;
        Needle_Gauge_Image_Rect.eulerAngles = Angle;
        // Z回転値を保存する
        Save_AngleZ = Needle_Gauge_Image_RotateZ;
        // 針ゲージの経過時間を初期化する
        Needle_Gauge_ElapsedTime = 0.0f;
        // 針ゲージが変化中真偽フラグを初期化する
        Needle_Gauge_Is_Changing = false;
        // 針ゲージUI画像のColorを初期化する
        Needle_Gauge_Image_Color.color = new Color(1, 1, 1, 1);

        // リズムゲージUI画像のRectTransformの"X"移動値を初期化する
        Rhythm_Gauge_Image_PosX = 300.0f;
        // リズムゲージUI画像のRectTransformの回転値を初期化する
        Vector3 Pos;
        Pos.x =    0.0f;
        Pos.y =  432.0f;
        Pos.z = -200.0f;
        Rhythm_Gauge_Image_Rect.anchoredPosition = Pos;
        // リズムゲージの経過時間を初期化する
        Rhythm_Gauge_ElapsedTime = 0.0f;
        // リズムゲージが変化中真偽フラグを初期化する
        Rhythm_Gauge_Is_Changing = false;
        // リズムゲージの移動カウントを初期化する
        Rhythm_Transition_Count = 0;

        // リズム操作成功真偽フラグを初期化する
        Succeed_Flag = false;
        // リズム操作成功時の時間を初期化する
        Save_Succeed_Time = 0.0f;

        // ゲーム開始直後の拍のタイミング
        GameStart_ClapCount = 5;

        beat = false;
        StartCoroutine("Beat");
    }

    private IEnumerator Beat()
    {
        int count = 0;
        beat = true;
        while (true)
        {
            yield return new WaitForFixedUpdate();
            count++;
            if(count == 30)
            {
                beat = true;
                count = 0;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        //********************************************//
        //<<初回のみ拍の間隔を検証して秒数を取得する>>//
        //********************************************//
        {
            // 拍のタイミングがそれ以外かを取得する
            // "true" ...拍のタイミング真
            // "false"...拍のタイミング偽
            if (Music.IsJustChangedBeat())
            {
                // カウントが" "の時のみ検証開始
                Clap_Count++;
            }
            // 検証中のみタイマー起動
            if (Clap_Count == 3) Clap_Time += Time.deltaTime;

            // 以後、Clap_Timeが拍と拍までの秒数になる
        }

        // フラグが立っている間は"Lerp"で動かす
        if (Needle_Gauge_Is_Changing)
        {
            // タイマー起動
            Needle_Gauge_ElapsedTime += Time.deltaTime;
            float Current_Time  = Mathf.Clamp01(Needle_Gauge_ElapsedTime / Clap_Time);
            float Current_Value = Mathf.Lerp(0.0f, 1.0f, Current_Time);

            // 回転関数
            UI_Rotate(Current_Value);

            // 上限値まで行けばフラグ終了
            if (Current_Time >= 1f) Needle_Gauge_Is_Changing = false;
        }

        // 針ゲージUI画像を回転させる & ゲーム開始直後の拍のタイミング
        if (Music.IsJustChangedBeat() && Clap_Count > GameStart_ClapCount)   
        {
            // 変化を開始する
            Needle_Gauge_Is_Changing = true;
            Needle_Gauge_ElapsedTime = 0f;
        }


        // フラグが立っている間は"Lerp"で動かす
        if (Rhythm_Gauge_Is_Changing)
        {
            // タイマー起動
            Rhythm_Gauge_ElapsedTime += Time.deltaTime;
            float Current_Time  = Mathf.Clamp01(Rhythm_Gauge_ElapsedTime / (Clap_Time / 2.0f));
            float Current_Value = Mathf.Lerp(0.0f, 1.0f, Current_Time);

            // 移動関数
            UI_Position(Current_Value, Rhythm_Transition_Count);

            // 上限値まで行けばフラグ終了
            if (Current_Time >= 1f && (Rhythm_Transition_Count == 1 || Rhythm_Transition_Count == 3))
            {
                Rhythm_Gauge_ElapsedTime = 0f;
            }
            else if (Current_Time >= 1f && (Rhythm_Transition_Count == 0 || Rhythm_Transition_Count == 2))
            {
                Rhythm_Gauge_Is_Changing = false;
            }
        }
        // リズムゲージUI画像を移動させる & ゲーム開始直後の拍のタイミング
        if (Music.IsJustChangedBeat() && Clap_Count > GameStart_ClapCount) 
        {
            // 変化を開始する
            Rhythm_Gauge_Is_Changing = true;
            Rhythm_Gauge_ElapsedTime = 0f;
        }

        // リズム画像のリズム演出
        if (beat)
        {
            DOTween
              .To(value => OnScale(value), 0, 1, 0.1f) .SetEase(Ease.InQuad) .SetLoops(2, LoopType.Yoyo);
        }

        //***************************************//
        //***<< 操作成功 or 操作失敗　の演出>>***//
        //***************************************//

        // "成功"
        if      ((Rhythm_Gauge_Image_Rect.anchoredPosition.x >= -50.0f && Rhythm_Gauge_Image_Rect.anchoredPosition.x <= 50.0f) && (Input.GetKeyDown(KeyCode.Joystick1Button0) || /*確認用*/ Input.GetKeyDown(KeyCode.A)))
        {
            // 操作成功にする
            Succeed_Flag = true;
            // 現在の時間を保存する
            Save_Succeed_Time = Current_Time;
            // 演出
            DOTween
              .To(value => OnScale(value), 0, 1, 0.1f).SetEase(Ease.InQuad).SetLoops(2, LoopType.Yoyo);
            Needle_Gauge_Image_Color.color = new Color(0.4f, 1, 1, 1);

        }
        // "失敗"
        else if ((Rhythm_Gauge_Image_Rect.anchoredPosition.x < -50.0f || Rhythm_Gauge_Image_Rect.anchoredPosition.x > 50.0f) && (Input.GetKeyDown(KeyCode.Joystick1Button0) || /*確認用*/ Input.GetKeyDown(KeyCode.A))) 
        {
            Needle_Gauge_Image_Color.color = new Color(1, 0.45f, 0.4f, 1);
        }
        // キー入力が離れたら色を元に戻す
        else if(Input.GetKeyUp(KeyCode.Joystick1Button0) || /*確認用*/ Input.GetKeyUp(KeyCode.A))
        {
            Needle_Gauge_Image_Color.color = new Color(1, 1, 1, 1);
        }

        // 操作成功から一定時間経過したら
        if (Succeed_Flag && Current_Time >= Save_Succeed_Time + (Clap_Time / 2.0f)) 
        {
            Succeed_Flag = false;
        }


        // 現在の時間を確認用タイマー起動
        Current_Time += 1.0f * Time.deltaTime;

        if (beat) beat = false;
    }





    // リズムUI画像のスケールを変更
    private void OnScale(float value)
    {
        var Scale = Mathf.Lerp(1, Succeed_Flag ? 1.7f : 1.2f, value);
        Rhythm_Image_Rect.localScale = new Vector3(Scale, Scale, Scale);
    }

    // 拍の間隔の秒数を取得する関数
    public float GetClapSecond() { return Clap_Time; }

    // 現在の回転番号を取得する関数
    public int GetCurrentNumber() { return Needle_Gauge_Number; }


    // 針ゲージUIを回転させる関数
    private void UI_Rotate(float timer)
    {
        var Angle = Mathf.Lerp(Save_AngleZ, Save_AngleZ - Needle_Gauge_Image_AngleZ, timer);
        Needle_Gauge_Image_Rect.eulerAngles = new Vector3(Needle_Gauge_Image_Rect.eulerAngles.x, 0f, Angle);

        // 角度(Angle)を-180～180度の範囲に正規化する
        var NormalizedAngleZ = Mathf.Repeat(Angle + 180, 360) - 180;

        // 45度回転したら 又は 拍のタイミングが来たら強制終了
        if (Angle == Save_AngleZ - Needle_Gauge_Image_AngleZ || Music.IsJustChangedBeat()) 
        {
            // 再度現在の角度を保存する
            Save_AngleZ = Save_AngleZ - Needle_Gauge_Image_AngleZ;
            // 現在の角度番号を設定する
            if (Needle_Gauge_Number == 7) Needle_Gauge_Number = 0;
            else                          Needle_Gauge_Number++;
        }
    }

    // リズムゲージUIを移動させる関数
    private void UI_Position(float timer, int count)
    {
        //************************//
        //<<カウントで分岐させる>>//
        //************************//
        if      (count == 0) 
        {
            var Position = Mathf.Lerp(0.0f, -300.0f, timer);
            Rhythm_Gauge_Image_Rect.anchoredPosition = new Vector3(Position, 632.0f - 200.0f);
            // 指定位置に到達したらリズムゲージ移動カウント設定
            if (Position == -300.0f) Rhythm_Transition_Count = 1;
        }
        else if (count == 1)
        {
            var Position = Mathf.Lerp(-300.0f, 0.0f, timer);
            Rhythm_Gauge_Image_Rect.anchoredPosition = new Vector3(Position, 632.0f - 200.0f);
            // 指定位置に到達したらリズムゲージ移動カウント設定
            if (Position == 0.0f || Music.IsJustChangedBeat()) Rhythm_Transition_Count = 2;
        }
        else if (count == 2)
        {
            var Position = Mathf.Lerp(0.0f, 300.0f, timer);
            Rhythm_Gauge_Image_Rect.anchoredPosition = new Vector3(Position, 632.0f - 200.0f);
            // 指定位置に到達したらリズムゲージ移動カウント設定
            if (Position == 300.0f) Rhythm_Transition_Count = 3;
        }
        else if (count == 3)
        {
            var Position = Mathf.Lerp(300.0f, 0.0f, timer);
            Rhythm_Gauge_Image_Rect.anchoredPosition = new Vector3(Position, 632.0f - 200.0f);
            // 指定位置に到達したらリズムゲージ移動カウント設定
            if (Position == 0.0f || Music.IsJustChangedBeat()) Rhythm_Transition_Count = 0;
        }
    }
}
