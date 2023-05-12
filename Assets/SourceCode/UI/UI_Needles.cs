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
    private float Needle_Gauge_Image_AngleZ;       // 針ゲージUI画像のRectTransformの"Z"回転値
    [SerializeField]
    private float Needle_Gauge_Image_RotateZ;      // 針ゲージUI画像のRectTransformの初回Rotate"Z"値
    [SerializeField]
    private float Save_AngleZ;                     // 前回の"Z"回転値を保存する変数
    [SerializeField]
    private float Current_AngleZ;                  // 現在の"Z"回転値を保存する変数


    [SerializeField]
    private RectTransform Rhythm_Gauge_Image_Rect; // リズムゲージUI画像のRectTransform
    [SerializeField]
    private float Rhythm_Gauge_Image_PosX;         // リズムゲージUI画像のRectTransformの"X"移動値
    [SerializeField]
    private float Rhythm_Gauge_Image_PositionX;    // リズムゲージUI画像のRectTransformの初回Position"X"値
    [SerializeField]                               
    private float Rhythm_Save_PositionX;           // 前回の"X"移動値を保存する変数
    [SerializeField]                               
    private float Rhythm_Current_PositionX;        // 現在の"X"移動値を保存する変数


    [SerializeField]
    private float Inside_Clap_Time;                // 裏拍のタイム
    [SerializeField]
    private bool Inside_Clap_Setting_Flag;         // 裏拍設定フラグ

    [SerializeField]
    private RectTransform Phase_Gauge_Image_Rect;  // フェーズゲージUI画像のRectTransform

    [SerializeField]
    private int Number;
    [SerializeField]
    private int Number_Count;

    [SerializeField]
    private int GameStart_ClapCount;               // ゲーム開始直後の拍のタイミング


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


        // リズムゲージUI画像のRectTransformの"X"移動値を初期化する
        Rhythm_Gauge_Image_PosX      = 300.0f;
        // リズムゲージUI画像のRectTransformのPosition"X"値
        Rhythm_Gauge_Image_PositionX = 000.0f;
        // リズムゲージUI画像のRectTransformの回転値を初期化する
        Vector3 Pos;
        Pos.x = Rhythm_Gauge_Image_PositionX;
        Pos.y =  432.0f;
        Pos.z = -200.0f;
        Rhythm_Gauge_Image_Rect.anchoredPosition = Pos;
        // X移動値を保存する
        Rhythm_Save_PositionX = Rhythm_Gauge_Image_PositionX;


        // ゲーム開始直後の拍のタイミング
        GameStart_ClapCount = 5;
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
        }

        // 針ゲージUI画像を回転させる & ゲーム開始直後の拍のタイミング
        if (Music.IsJustChangedBeat() && Clap_Count > GameStart_ClapCount)   
        {
            DOTween
             .To(angle => UI_Rotate(angle), 0, 1, Clap_Time).SetEase(Ease.InOutQuad);
        }

        // リズムゲージUI画像を移動させる & ゲーム開始直後の拍のタイミング & カウントが偶数の時
        if (Music.IsJustChangedBeat() && Clap_Count > GameStart_ClapCount && Clap_Count % 2 == 0) 
        {
            DOTween
             .To(position => UI_PositionL(position), 0, 1, Clap_Time / 2.0f).SetEase(Ease.InSine).SetLoops(2, LoopType.Yoyo);
        }
        // リズムゲージUI画像を移動させる & ゲーム開始直後の拍のタイミング & カウントが偶数の時
        if (Music.IsJustChangedBeat() && Clap_Count > GameStart_ClapCount && Clap_Count % 2 != 0)
        {
            DOTween
             .To(position => UI_PositionR(position), 0, 1, Clap_Time / 2.0f).SetEase(Ease.InSine).SetLoops(2, LoopType.Yoyo);
        }

        // 確認用タイマー起動
        Current_Time = 1.0f * Time.deltaTime;


        // 裏拍関連処理
        {
            // 裏拍調整
            if (Clap_Count == 4 && !Inside_Clap_Setting_Flag)
            {
                Inside_Clap_Time += (Clap_Time / 2.0f);
            }

            // 裏拍タイマー起動
            Inside_Clap_Time += Time.deltaTime;
        }


        // 現在の角度番号を設定する
        {
            if      (Number_Count == 0) Number = 7;
            else if (Number_Count == 1) Number = 0;
            else if (Number_Count == 2) Number = 1;
            else if (Number_Count == 3) Number = 2;
            else if (Number_Count == 4) Number = 3;
            else if (Number_Count == 5) Number = 4;
            else if (Number_Count == 6) Number = 5;
            else if (Number_Count == 7) Number = 6;
        }

    }

    // 拍の間隔の秒数を取得する関数
    public float GetClapSecond() { return Clap_Time; }

    // 現在の回転番号を取得する関数
    public int GetCurrentNumber() { return Number; }


    // UIを回転させる関数
    private void UI_Rotate(float timer)
    {
        var Angle = Mathf.Lerp(Save_AngleZ, Save_AngleZ - Needle_Gauge_Image_AngleZ, timer);
        Needle_Gauge_Image_Rect.eulerAngles = new Vector3(Needle_Gauge_Image_Rect.eulerAngles.x, 0f, Angle);

        // 角度(Angle)を-180～180度の範囲に正規化する
        var NormalizedAngleZ = Mathf.Repeat(Angle + 180, 360) - 180;
        // 現在の角度を保存する
        Current_AngleZ = NormalizedAngleZ;

        // 45度回転したら
        if (Angle == Save_AngleZ - Needle_Gauge_Image_AngleZ)
        {
            // 再度現在の角度を保存する
            Save_AngleZ = Save_AngleZ - Needle_Gauge_Image_AngleZ;
            if (Number_Count == 7) Number_Count = 0;
            else Number_Count++;
        }
    }

    // UIを移動させる関数
    private void UI_PositionL(float timer)
    {
        var Position = Mathf.Lerp(Rhythm_Save_PositionX, Rhythm_Save_PositionX - Rhythm_Gauge_Image_PosX, timer);
        Rhythm_Gauge_Image_Rect.anchoredPosition = new Vector3(Position, 432.0f - 200.0f);
    }

    private void UI_PositionR(float timer)
    {
        var Position = Mathf.Lerp(Rhythm_Save_PositionX, Rhythm_Save_PositionX + Rhythm_Gauge_Image_PosX, timer);
        Rhythm_Gauge_Image_Rect.anchoredPosition = new Vector3(Position, 432.0f - 200.0f);
    }
}
