using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_Dead_Gauge : MonoBehaviour
{

    [SerializeField]
    private RectTransform Dead_Gauge_Image_Rect;          // 死亡ゲージUI画像のRectTransform

    [SerializeField]
    private int Clap_Count;                               // 拍のカウント変数

    [SerializeField]
    private int Scale_Change_Count;                       // 死亡ゲージUI画像のスケール変更カウント変数

    [SerializeField]
    private int Start_Count;                              // ゲーム開始までのカウント変数

    [SerializeField]
    private float Dead_Gauge_ScaleX;                      // 死亡ゲージUI画像の横幅変数

    [SerializeField]
    private bool Is_Disappear_Phase_Flag;                 // 消えるフェーズフラグ

    [SerializeField]
    private bool Is_Disappear_Phase_End_Flag;             // 消えるフェーズの終了フラグ

    [SerializeField]
    private int Disappear_Phase_Clap_Count;               // 消えるフェーズ中の拍のカウント変数

    [SerializeField]
    private RectTransform Revers_Image_Rect;              // 色彩反転UI画像のRectTransform
    [SerializeField]
    private float Revers_Image_Scale;                     // 色彩反転UI画像のサイズ

    [SerializeField]
    private RectTransform Revers_Image_Rect2;             // 色彩反転UI画像のRectTransform(2枚目)
    [SerializeField]
    private float Revers_Image_Scale2;                    // 色彩反転UI画像のサイズ(2枚目)

    [SerializeField]
    public UI_CountDown uI_CountDown;                     // ゲーム開始カウントダウンスクリプト

    [SerializeField]
    public UI_Needles uI_Needles;                         // 針のスクリプト

    [SerializeField]
    private float ElapsedTime;                            // 死亡ゲージの縮小経過時間

    [SerializeField]
    private int Quota_Count;                              // ノルマ数

    [SerializeField]
    public UI_Objective_Quota uI_Objective_Quota;         // ノルマ数スクリプト

    [SerializeField]
    private bool Beat_Flag;                               // 拍のタイミングフラグ

    [SerializeField]
    private bool Before_Is_Disappear_Phase_Flag;

    [SerializeField]
    private PlayDirector playDirector;

    [SerializeField]
    private FieldController _fieldController;

    private const int normaSpeed = 8;

    // Start is called before the first frame update
    void Start()
    {
        // 死亡ゲージUI画像の横幅変数を初期化する
        Dead_Gauge_ScaleX = 0.0f;
        // 拍のカウント変数を初期化する
        Clap_Count = 0;
        // ゲーム開始までのカウント変数を初期化する
        Start_Count = 5;
        // 消えるフェーズ中の拍のカウント変数を初期化する
        Disappear_Phase_Clap_Count = 0;
        // スケールを初期化する
        Dead_Gauge_Image_Rect.localScale = new Vector3(0, 1, 1);

        // 色彩反転画像のスケール初期化する
        Revers_Image_Rect. localScale = new Vector3(0, 0, 0);
        Revers_Image_Rect2.localScale = new Vector3(0, 0, 0);
        // 色彩反転UI画像のサイズ変数を初期化する
        Revers_Image_Scale  = 0.0f;
        Revers_Image_Scale2 = 0.0f;
        // 消えるフェーズフラグを初期化する
        Is_Disappear_Phase_Flag = false;
        // 消えるフェーズの終了フラグを初期化する
        Is_Disappear_Phase_End_Flag = false;
        // 死亡ゲージの縮小経過時間を初期化する
        ElapsedTime = 0.0f;
        // ノルマ数を初期化する
        Quota_Count = 1;

        // 拍のタイミングフラグを初期化する
        Beat_Flag = false;

        //
        Before_Is_Disappear_Phase_Flag = false;

        // スタートコルチン
        StartCoroutine("BeatPlay");
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
        // 拍が来る度カウントする
        if (Beat_Flag) Clap_Count++;

        // ノルマアイコン更新
        uI_Objective_Quota.SetObjectiveQuota(Quota_Count);

        // 8回拍が来るたびにスケールを1段階増やす & 消えるフェーズになるまで
        if ((Beat_Flag && (Clap_Count - Start_Count) % normaSpeed == 0) && !Is_Disappear_Phase_Flag && uI_CountDown.GetGameStartFlag() && Dead_Gauge_ScaleX < 1.0f)   
        {
            // 演出
            DOTween
              .To(value => OnScale(value, Scale_Change_Count * 0.125f), 0, 1, 0.25f).SetEase(Ease.InOutQuad);
        }

        // 消えるフェーズに入ったら消えるフェーズフラグを立てる
        if (Scale_Change_Count == 8 || Dead_Gauge_ScaleX >= 1.0f) 
        {
            // 1枚目の色彩反転画像のサイズを大きくしていく
            if      (Revers_Image_Scale <  20.0f) Revers_Image_Scale += 0.25f;
            // 1枚目の色彩反転画像のサイズが最大値までいけば2枚目の色彩反転画像のサイズも大きくしていく
            else if (Revers_Image_Scale >= 20.0f && Revers_Image_Scale2 < 15.0f) Revers_Image_Scale2 += 0.25f;          
        }

        if(Dead_Gauge_ScaleX >= 1.0f && !Before_Is_Disappear_Phase_Flag) Before_Is_Disappear_Phase_Flag = true;

        // 色彩反転演出中に針の場所を移動させる
        if ((Revers_Image_Scale2 > 2.97f && Revers_Image_Scale2 < 10.1f) && !Is_Disappear_Phase_End_Flag) uI_Needles.SetNeedlePos();

        // 色彩反転演出が終了次第消えるフェーズに入る フラグを立てる
        if (Revers_Image_Scale2 >= 15.0f) Is_Disappear_Phase_Flag = true;

        // 消えるフェーズ中の拍の数をカウントする
        if (Is_Disappear_Phase_Flag && Beat_Flag) Disappear_Phase_Clap_Count++;

        // 消えるフェーズ中は死亡ゲージのスケールを減らす
        if (Is_Disappear_Phase_Flag)
        {
            // タイマー起動
            ElapsedTime += Time.deltaTime;
            float Current_Time = Mathf.Clamp01(ElapsedTime / 4.8f);
            // 演出
            OnScale(Current_Time, 0);
        }

        // 消えるフェーズの終了お知らせを受け取ったら
        if (Is_Disappear_Phase_End_Flag)
        {
            // 2枚目の色彩反転画像のサイズを小さくしていく
            if      (Revers_Image_Scale2 > 0.0f) Revers_Image_Scale2 -= 0.25f;
            // 2枚目の色彩反転画像のサイズが最小値までいけば1枚目の色彩反転画像のサイズも小さくしていく
            else if (Revers_Image_Scale2 <= 0.0f && Revers_Image_Scale > 0.0f) Revers_Image_Scale -= 0.25f;

            // 色彩反転演出が終了すれば
            if (Revers_Image_Scale <= 0.0f) 
            {
                // 針のスクリプト側の消えるフェーズ終了を知らせる
                uI_Needles.ResetDisappearPhaseFlag();
                // フラグ関連をリセットする
                Is_Disappear_Phase_Flag     = false;
                Is_Disappear_Phase_End_Flag = false;
                // タイマーリセット
                ElapsedTime = 0.0f;

                _fieldController._normaCount -= (Quota_Count);

                // ノルマを増やす
                Quota_Count++;
            }
            Before_Is_Disappear_Phase_Flag = false;
            playDirector.SetStateErase(false);
        }

        // trueのタイミングでfalseにする
        if (Beat_Flag) Beat_Flag = false;

        // 色彩反転画像のサイズ設定
        Revers_Image_Rect.localScale  = new Vector3(Revers_Image_Scale,  Revers_Image_Scale,  Revers_Image_Scale);
        Revers_Image_Rect2.localScale = new Vector3(Revers_Image_Scale2, Revers_Image_Scale2, Revers_Image_Scale2);
    }

    // リズムUI画像のスケールを変更
    private void OnScale(float value,float scale_X)
    {
        // 消えるフェーズ中なら360度回転　それ以外は通常通り45度回転
        var Scale = Is_Disappear_Phase_Flag ? Mathf.Lerp(1.0f, 0.0f, value) : Mathf.Lerp(scale_X, scale_X + 0.125f, value);
        // スケール反映
        Dead_Gauge_Image_Rect.localScale = new Vector3(Scale, 1, 1);
        // スケール変更カウント
        if (!Is_Disappear_Phase_Flag && Scale == scale_X + 0.125f) 
        {
            // カウント
            Scale_Change_Count++;
            // スケール値保存する
            Dead_Gauge_ScaleX = Scale;
        }
        // 消えるフェーズ中ならリセットする
        if (Is_Disappear_Phase_Flag && Scale == 0.0f) 
        {
            // カウントリセット
            Scale_Change_Count = 0;
            // スケール値保存する
            Dead_Gauge_ScaleX = Scale;
            // 消えるフェーズの終了フラグを立てる
            Is_Disappear_Phase_End_Flag = true;
        }
    }


    // 消えるフェーズ真偽フラグ取得関数
    public bool GetIsDisappearPhaseFlag()    { return Is_Disappear_Phase_Flag; }

    public bool GetBeforeIsDisappearPhaseFlag()    { return Before_Is_Disappear_Phase_Flag; }
    public void SetBeforeIsDisappearPhaseFlag(bool flag)    { Before_Is_Disappear_Phase_Flag = flag; }

}
