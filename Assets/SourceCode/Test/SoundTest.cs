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

    // Start is called before the first frame update
    void Start()
    {
        addText = 0;
        TestText.text = "0";
        BeatTimer = 0.0f;
        PlayFlag = false;

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    // Update is called once per frame
    void Update()
    {
        TestText.text = addText.ToString();


        // 拍ごとにtrue
        if (Music.IsJustChangedBeat())
        {
            // ゲーム開始フラグ起動
            FirstBeat = true;
            //addText += 1;
        }
        // 拍 32frame 拍

        // ゲームが起動したら拍間の時間を計る
        //if (FirstBeat) BeatTimer++;
        if (FirstBeat) BeatTimer += Time.deltaTime;

        // 拍の前後0.048フレームは操作を受け付ける
        if ((BeatTimer > 0.000f && BeatTimer < 0.112f))// || (BeatTimer > 0.448f && BeatTimer < 0.56f))
        {
            PlayFlag = true;
        }
        else if((BeatTimer > 0.448f && BeatTimer < 0.56f))
        {
            PlayFlag = true;
        }
        else
        {
            PlayFlag = false;
        }

        // 次の拍に行ったらタイマーをリセット
        if (BeatTimer > 0.56f)
        {
            BeatTimer = 0.0f;
        }




        // 仮置き 指定フレームだけ操作可能
        if (PlayFlag && Input.GetMouseButtonDown(0)) addText += 1;
    }
}
