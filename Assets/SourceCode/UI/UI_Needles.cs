using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_Needles : MonoBehaviour
{
    [SerializeField]
    private float Clap_Time;                           // 拍の間隔の秒数
                                                       
    [SerializeField]                                   
    private float Current_Time;                        // 確認用現在のタイム
                                                       
    [SerializeField]                                   
    private int Clap_Count;                            // 拍のカウント変数

    [SerializeField]
    private RectTransform Needle_Gauge_Image_Rect;     // 針ゲージUI画像のRectTransform
    [SerializeField]                                   
    private float Needle_Gauge_Image_AngleZ;           // 針ゲージUI画像のRectTransformの"Z"回転値
    [SerializeField]                                   
    private float Needle_Gauge_Image_RotateZ;          // 針ゲージUI画像のRectTransformの初回Rotate"Z"値
    [SerializeField]                                   
    private float Save_AngleZ;                         // 前回の"Z"回転値を保存する変数
    [SerializeField]                                   
    private int Needle_Gauge_Number;                   // 針ゲージの現在の角度番号
    [SerializeField]                                   
    private float Needle_Gauge_ElapsedTime;            // 針ゲージの経過時間
    [SerializeField]                                   
    private bool Needle_Gauge_Is_Changing;             // 針ゲージが変化中真偽フラグ


    [SerializeField]
    private RectTransform[] Rhythm_Gauge_Image_Rect    // リズムゲージUI画像のRectTransform
        = new RectTransform[3];                        
    [SerializeField]                                   
    private GameObject[] Rhythm_Gauge_Image_OBJ        // リズムゲージUI画像のRectTransform
        = new GameObject[3];                           
    [SerializeField]                                   
    private float Rhythm_Gauge_Image_PosX;             // リズムゲージUI画像のRectTransformの"X"移動値
    [SerializeField]                                   
    private float Rhythm_Gauge_ElapsedTime;            // リズムゲージの経過時間
    [SerializeField]                                   
    private bool  Rhythm_Gauge_Is_Changing;            // リズムゲージが変化中真偽フラグ
    [SerializeField]                                   
    private int   Rhythm_Transition_Count;             // リズムゲージの移動カウント
                                                       
    [SerializeField]                                   
    private RectTransform Rhythm_Image_Rect;           // リズムUI画像のRectTransform
    [SerializeField]                                   
    private bool Succeed_Flag;                         // リズム操作成功真偽フラグ
    [SerializeField]                                   
    private float Save_Succeed_Time;                   // リズム操作成功時の時間
                                                       
                                                       
    [SerializeField]                                   
    private RectTransform Phase_Gauge_Image_Rect;      // フェーズゲージUI画像のRectTransform
                                                       
    [SerializeField]                                   
    private int GameStart_ClapCount;                   // ゲーム開始直後の拍のタイミング
                                                       
    [SerializeField]                                   
    private AudioSource Game_BGM;                      // ゲームのBGM
                                                       
    [SerializeField]                                   
    private bool Beat_Flag;                            // 拍のタイミングフラグ
                                                       
    [SerializeField]                                   
    public UI_CountDown uI_CountDown;                  // ゲーム開始カウントダウンスクリプト
                                                       
    [SerializeField]                                   
    public UI_Dead_Gauge uI_Dead_Gauge;                // 死亡ゲージスクリプト
                                                       
    [SerializeField]                                   
    private bool _Disappear_Phase_Flag;                // 消えるフェーズフラグ

    [SerializeField]
    private bool Disappear_Roatet_End_Flag;            // 消えるフェーズの360回転の終了フラグ

    [SerializeField]
    private AudioClip SE_Metronome;                    // メトロノーム効果音
    [SerializeField] PlayDirector playDirector = default!;

    [SerializeField]
    private AudioSource audioSource;                   // オーディオソース

    [SerializeField]
    private AudioSource BGM_AudioSource;               // BGMのオーディオソース

    [SerializeField]
    private float BGM_Time;                            // 現在のBGMの時間

    [SerializeField]
    private float BGM_Length;                          // BGMの長さ


    [SerializeField] UI_Rythm_Effect uiRythmEffect = default!;

    [SerializeField]
    private int justTiming;                   // リザルト画面に渡す

    bool beat;
    bool TriggerFlag;                                   // トリガーの長押しを効かなくする用

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

        // リズムゲージUI画像のRectTransformの"X"移動値を初期化する
        Rhythm_Gauge_Image_PosX = 300.0f;
        // リズムゲージUI画像のRectTransformの位置を初期化する
        Vector3 Pos;
        for (int Num = 0; Num < 3; Num++) 
        {
            Pos.x = 0.0f;
            Pos.y = 432.0f;
            Pos.z = -200.0f;
            Rhythm_Gauge_Image_Rect[Num].anchoredPosition = Pos;
        }
        // リズムゲージの存在真偽を初期化する
        Rhythm_Gauge_Image_OBJ[0].SetActive(true);
        Rhythm_Gauge_Image_OBJ[1].SetActive(false);
        Rhythm_Gauge_Image_OBJ[2].SetActive(false);
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

        // 拍のタイミングフラグを初期化する
        Beat_Flag = false;

        // 消えるフェーズの準備フラグを初期化する
        _Disappear_Phase_Flag = false;

        // 消えるフェーズの360回転の終了フラグを初期化する
        Disappear_Roatet_End_Flag = false;

        //Componentを取得
        audioSource = GetComponent<AudioSource>();

        // 現在のBGMの時間を初期化する
        BGM_Time = 0.0f;

        // BGMの長さを初期化する
        BGM_Length = 0.0f;


        justTiming = 0;

        TriggerFlag = false;

        // スタートコルチン
        StartCoroutine("BeatPlay");
    }

    private void Awake()
    {
        // 目標フレームレートを60に設定
        Application.targetFrameRate = 60; 
    }

    private IEnumerator BeatPlay()
    {
        int Count = 0;
        Beat_Flag = true;
        while (true)
        {
            yield return new WaitForFixedUpdate();
            // 0.02秒後
            Count++;
            if (Count == 30)
            {
                Beat_Flag = true;
                Count = 0;
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
            if (Beat_Flag)
            {
                // カウントが" "の時のみ検証開始
                Clap_Count++;
            }
            // 検証中のみタイマー起動
            if (Clap_Count == 2) Clap_Time += Time.deltaTime;

            // 以後、Clap_Timeが拍と拍までの秒数になる
        }

        // 常に0.6秒に調整する
        Clap_Time = 0.6f;

        // 最初に詰まる為、スクリプト側から再生する
        if (Clap_Count == 3) Game_BGM.Play();
        // BGMのタイマー起動
        if (Clap_Count > 3) BGM_Time += Time.deltaTime;

        BGM_Length = Game_BGM.clip.length;

        if (Game_BGM.clip.length <= BGM_Time && Beat_Flag/* && Rhythm_Transition_Count == 0*/)
        {
            // 針の移動番号リセット
            Rhythm_Transition_Count = 0;
            // BGM再度再生
            Game_BGM.Play();
            // タイマーリセット
            BGM_Time = 0.0f;
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
            if (Current_Time >= 1f || _Disappear_Phase_Flag) Needle_Gauge_Is_Changing = false;
        }

        // 針ゲージUI画像を回転させる & ゲーム開始直後の拍のタイミング
        if (Beat_Flag && Clap_Count > GameStart_ClapCount && uI_CountDown.GetGameStartFlag() && !_Disappear_Phase_Flag)      
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
        if (Beat_Flag && Clap_Count > GameStart_ClapCount && uI_CountDown.GetGameStartFlag())  
        {
            // 変化を開始する
            Rhythm_Gauge_Is_Changing = true;
            Rhythm_Gauge_ElapsedTime = 0f;
        }

        // リズム画像のリズム演出
        if (Rhythm_Gauge_Image_Rect[0].anchoredPosition.x == 0.0f && !Succeed_Flag && uI_CountDown.GetGameStartFlag()) 
        {
            DOTween
              .To(value => OnScale(value), 0, 1, 0.1f).SetEase(Ease.InQuad).SetLoops(2, LoopType.Yoyo);
        }

        //***************************************//
        //***<< 操作成功 or 操作失敗　の演出>>***//
        //***************************************//
        float TrigerInput = Input.GetAxisRaw("Trigger");
        if (TriggerFlag)
        {
            if (TrigerInput == 0) TriggerFlag = false;
        }

        // "成功"
        if (!_Disappear_Phase_Flag && uI_CountDown.GetGameStartFlag() && (playDirector.GetPlayFlag())
            && (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Joystick1Button2) || Input.GetKeyDown(KeyCode.Joystick1Button4) || Input.GetKeyDown(KeyCode.Joystick1Button5) || (TrigerInput < 0.0f && !TriggerFlag )|| (TrigerInput > 0.0f && !TriggerFlag)
            || /*確認用*/ Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.DownArrow)
            || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)))
        {
            // 操作成功にする
            Succeed_Flag = true;
            // 現在の時間を保存する
            Save_Succeed_Time = Current_Time;
            // 演出
            DOTween
              .To(value => OnScale(value), 0, 1, 0.1f).SetEase(Ease.InQuad).SetLoops(2, LoopType.Yoyo);
            Rhythm_Gauge_Image_OBJ[0].SetActive(false);
            Rhythm_Gauge_Image_OBJ[1].SetActive(false);
            Rhythm_Gauge_Image_OBJ[2].SetActive(true);

            //
            uiRythmEffect.PlayEffect(true);

            justTiming++;
            TriggerFlag = true;
        }
        // "失敗"
        else if (!_Disappear_Phase_Flag && uI_CountDown.GetGameStartFlag() && (!playDirector.GetPlayFlag())
            && (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.Joystick1Button2) || Input.GetKeyDown(KeyCode.Joystick1Button4) || Input.GetKeyDown(KeyCode.Joystick1Button5) || (TrigerInput < 0.0f && !TriggerFlag) || (TrigerInput > 0.0f && !TriggerFlag) || /*確認用*/ Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.DownArrow)
            || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.UpArrow)))
        {
            Rhythm_Gauge_Image_OBJ[0].SetActive(false);
            Rhythm_Gauge_Image_OBJ[1].SetActive(true);
            Rhythm_Gauge_Image_OBJ[2].SetActive(false);

            uiRythmEffect.PlayEffect(false);
            TriggerFlag = true;
        }
        // キー入力が離れたら色を元に戻す
        else if (!_Disappear_Phase_Flag && uI_CountDown.GetGameStartFlag() && Input.GetKeyUp(KeyCode.Joystick1Button0) || Input.GetKeyUp(KeyCode.Joystick1Button2) || Input.GetKeyUp(KeyCode.Joystick1Button4) || Input.GetKeyUp(KeyCode.Joystick1Button5) || TrigerInput == 0.0f || /*確認用*/ Input.GetKeyUp(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.DownArrow)
            || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.UpArrow)) 
        {
            Rhythm_Gauge_Image_OBJ[0].SetActive(true);
            Rhythm_Gauge_Image_OBJ[1].SetActive(false);
            Rhythm_Gauge_Image_OBJ[2].SetActive(false);
        }

        // 操作成功から一定時間経過したら
        if (Succeed_Flag && Current_Time >= Save_Succeed_Time + (Clap_Time / 2.0f)) 
        {
            Succeed_Flag = false;
        }

        // 消えるフェーズ用の回転処理
        if (uI_Dead_Gauge.GetIsDisappearPhaseFlag() && _Disappear_Phase_Flag && !Disappear_Roatet_End_Flag)  
        {
            // タイマー起動
            Needle_Gauge_ElapsedTime += Time.deltaTime;
            float Current_Time  = Mathf.Clamp01(Needle_Gauge_ElapsedTime / (Clap_Time * 8));
            float Current_Value = Mathf.Lerp(0.0f, 1.0f, Current_Time);

            // 回転関数
            UI_Rotate(Current_Value);
        }

        // trueのタイミングでfalseにする
        if (Beat_Flag) Beat_Flag = false;

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
        var Angle = Mathf.Lerp(Save_AngleZ, Save_AngleZ - ((uI_Dead_Gauge.GetIsDisappearPhaseFlag() && _Disappear_Phase_Flag) ? 360.0f : Needle_Gauge_Image_AngleZ), timer);
        Needle_Gauge_Image_Rect.eulerAngles = new Vector3(Needle_Gauge_Image_Rect.eulerAngles.x, 0f, Angle);
        // 角度(Angle)を-180～180度の範囲に正規化する
        var NormalizedAngleZ = Mathf.Repeat(Angle + 180, 360) - 180;

        // 45度回転したら 又は 拍のタイミングが来たら強制終了
        if (!_Disappear_Phase_Flag && (Angle == Save_AngleZ - Needle_Gauge_Image_AngleZ || Beat_Flag)) 
        {
            // 再度現在の角度を保存する
            Save_AngleZ = Save_AngleZ - Needle_Gauge_Image_AngleZ;
            // 現在の角度番号を設定する
            if (Needle_Gauge_Number == 7) Needle_Gauge_Number = 0;
            else                          Needle_Gauge_Number++;
        }

        // 針の角度番号設定
        if(_Disappear_Phase_Flag)
        {
            if      (Angle <= -157.5f && Angle > Save_AngleZ - 45.0f * 1) Needle_Gauge_Number = 0;
            else if (Angle <= -202.5f && Angle > Save_AngleZ - 45.0f * 2) Needle_Gauge_Number = 1;
            else if (Angle <= -247.5  && Angle > Save_AngleZ - 45.0f * 3) Needle_Gauge_Number = 2;
            else if (Angle <= -292.5f && Angle > Save_AngleZ - 45.0f * 4) Needle_Gauge_Number = 3;
            else if (Angle <= -337.5f && Angle > Save_AngleZ - 45.0f * 5) Needle_Gauge_Number = 4;
            else if (Angle <= -382.5f && Angle > Save_AngleZ - 45.0f * 6) Needle_Gauge_Number = 5;
            else if (Angle <= -427.5f && Angle > Save_AngleZ - 45.0f * 7) Needle_Gauge_Number = 6;
            else if (Angle <= -472.5f && Angle > Save_AngleZ - 45.0f * 8) Needle_Gauge_Number = 7;
        }

        // 360度回転したら強制終了
        if (_Disappear_Phase_Flag && (Angle == Save_AngleZ - 360.0f)) 
        {
            // 再度現在の角度を保存する
            Save_AngleZ = Save_AngleZ - 360.0f;
            // 消えるフェーズの360回転の終了フラグを立てる
            Disappear_Roatet_End_Flag = true;
            
        }
    }

    // リズムゲージUIを移動させる関数
    private void UI_Position(float timer, int count)
    {
        //************************//
        //<<カウントで分岐させる>>//
        //************************//
        if (count == 0)
        {
            var Position = Mathf.Lerp(-300.0f, 0.0f, timer);
            for (int Num = 0; Num < 3; ++Num) Rhythm_Gauge_Image_Rect[Num].anchoredPosition = new Vector3(Position, 632.0f - 200.0f);
            // 指定位置に到達したらリズムゲージ移動カウント設定
            if (Position == 0.0f) Rhythm_Transition_Count = 1;
        }
        else if (count == 1)
        {
            var Position = Mathf.Lerp(0.0f, 300.0f, timer);
            for (int Num = 0; Num < 3; ++Num) Rhythm_Gauge_Image_Rect[Num].anchoredPosition = new Vector3(Position, 632.0f - 200.0f);
            // 指定位置に到達したらリズムゲージ移動カウント設定
            if (Position == 300.0f || Beat_Flag)
            {
                //音(Metronome)を鳴らす
                audioSource.PlayOneShot(SE_Metronome);
                Rhythm_Transition_Count = 2;
            }

        }
        else if (count == 2)
        {
            var Position = Mathf.Lerp(300.0f, 0.0f, timer);
            for (int Num = 0; Num < 3; ++Num) Rhythm_Gauge_Image_Rect[Num].anchoredPosition = new Vector3(Position, 632.0f - 200.0f);
            // 指定位置に到達したらリズムゲージ移動カウント設定
            if (Position == 0.0f) Rhythm_Transition_Count = 3;

        }
        else if (count == 3)
        {
            var Position = Mathf.Lerp(0.0f, -300.0f, timer);
            for (int Num = 0; Num < 3; ++Num) Rhythm_Gauge_Image_Rect[Num].anchoredPosition = new Vector3(Position, 632.0f - 200.0f);
            // 指定位置に到達したらリズムゲージ移動カウント設定
            if (Position == -300.0f || Beat_Flag)
            {
                //音(Metronome)を鳴らす
                audioSource.PlayOneShot(SE_Metronome);
                Rhythm_Transition_Count = 0;
            }
        }

    }

    // 針の位置を"0"番目の位置に設定する関数
    public void SetNeedlePos()
    {
        // 回転値固定
        Vector3 Angle;
        Angle.x = Needle_Gauge_Image_Rect.eulerAngles.x;
        Angle.y = 0.0f;
        Angle.z = Needle_Gauge_Image_RotateZ;
        Needle_Gauge_Image_Rect.eulerAngles = Angle;
        // 角度保存する
        Save_AngleZ = Needle_Gauge_Image_RotateZ;
        // カウントリセット
        Needle_Gauge_Number = 0;
        // 消えるフェーズフラグを立てる
        _Disappear_Phase_Flag = true;
        // タイマーリセット
        Needle_Gauge_ElapsedTime = 0.0f;
        // 消えるフェーズの360回転の終了フラグをリセット
        Disappear_Roatet_End_Flag = false;
    }

    // 消えるフェーズフラグをリセットする
    public void ResetDisappearPhaseFlag() { 
        _Disappear_Phase_Flag = false;
        // 角度番号リセット
        Needle_Gauge_Number = 0;

        playDirector.EnableSpawn(true);

    }

    public int GetJustTiming()
    {
        return justTiming;
    }
}
