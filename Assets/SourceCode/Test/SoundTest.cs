using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SoundTest : MonoBehaviour
{
    // デバッグ用テキスト
    public Text TestText;
    public int addText;

    // 1拍目到達通知
    private bool FirstBeat = false;

    // 操作可能フラグ
    public bool PlayFlag;
    // 拍に来た時点で起動するタイマー
    public float BeatTimer;

    public bool PenaltyFlag;

    // 偶数ビート
    public bool EvenBeat;
    // 奇数ビート
    public bool OddBeat;

    public int BeatCount;
    bool Pena1;
    bool Pena2;


    // Start is called before the first frame update
    void Start()
    {
        addText = 0;
        TestText.text = "0";
        BeatTimer = 0.0f;
        PlayFlag = false;

        PenaltyFlag = false;
        BeatCount = 0;
        OddBeat = false;
        EvenBeat = false;

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        TestText.text = addText.ToString();


        // 拍ごとにtrue
        if (Music.IsJustChangedBeat())
        {
            // ゲーム開始フラグ起動
            FirstBeat = true;
            BeatCount += 1;
            //addText += 1;
        }

        // ゲームが起動したら拍間の時間を計る
        if (FirstBeat) BeatTimer += Time.deltaTime;

        // 拍の前後0.15フレームは操作を受け付ける
        if (!PenaltyFlag)
        {
            if (BeatTimer > 0.0f && BeatTimer < 0.15f)
            {
                PlayFlag = true;
            }
            if (BeatTimer > 0.45f && BeatTimer < 0.6f)
            {
                PlayFlag = true;
            }
            else
            {
                PlayFlag = false;
            }
        }

        // 次の拍に行ったらタイマーをリセット
        if (BeatTimer > 0.6f)
        {
            BeatTimer = 0.0f;
        }

        PenaltyMethod();

        // 仮置き 指定フレームだけ操作可能
        if (PlayFlag && Input.GetMouseButtonDown(0)) addText += 1;
    }


    void PenaltyMethod()
    {
        // もし操作可能フレーム以外で操作が行われたら
        if (!PlayFlag && Input.GetMouseButtonDown(0))
        {
            // ペナルティ処理
            PenaltyFlag = true;
        }

        // 現在の拍が奇数が偶数かを判定
        if (BeatCount % 2 == 0)
        {
            EvenBeat = true;
            OddBeat = false;
        }
        else
        {
            OddBeat = true;
            EvenBeat = false;
        }


        // もし拍が偶数の時にペナルティが発生したら
        if (PenaltyFlag && EvenBeat)
        {
            // 偶数用ペナルティフラグを立てる
            Pena2 = true;
        }
        // もし拍が奇数の時にペナルティが発生したら
        if (PenaltyFlag && OddBeat)
        {
            // 奇数用ペナルティフラグを立てる
            Pena1 = true;
        }

        // もしペナルティ(偶数)が発生していたら奇数時に解除
        if (Pena2 && OddBeat)
        {
            PenaltyFlag = false;
            Pena2 = false;
        }
        // その逆
        if (Pena1 && EvenBeat)
        {
            PenaltyFlag = false;
            Pena1 = false;
        }
    }
}
// SceneGameBGM 拍間0.6frame
// TitleBGM     拍間0.56frame