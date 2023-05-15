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

    // Start is called before the first frame update
    void Start()
    {
        // 死亡ゲージUI画像の横幅変数を初期化する
        Dead_Gauge_ScaleX = 0.0f;
        // 拍のカウント変数を初期化する
        Clap_Count = 0;
        // ゲーム開始までのカウント変数を初期化する
        Start_Count = 5;

        // スケールを初期化する
        Dead_Gauge_Image_Rect.localScale = new Vector3(0, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        // 拍が来る度カウントする
        if (Music.IsJustChangedBeat()) Clap_Count++;

        // 8回拍が来るたびにスケールを1段階増やす
        if (Music.IsJustChangedBeat() && (Clap_Count - Start_Count) % 8 == 0) 
        {
            // 演出
            DOTween
              .To(value => OnScale(value, Scale_Change_Count * 0.125f), 0, 1, 1.0f).SetEase(Ease.InOutQuad);
        }

    }

    // リズムUI画像のスケールを変更
    private void OnScale(float value,float scale_X)
    {
        var Scale = Mathf.Lerp(scale_X, scale_X + 0.125f, value);
        Dead_Gauge_Image_Rect.localScale = new Vector3(Scale, 1, 1);
        // スケール変更カウント
        if (Scale == scale_X + 0.125f) Scale_Change_Count++;
    }
}
