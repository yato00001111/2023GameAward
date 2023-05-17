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
    int PenaltyCount;


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

        PenaltyCount = 0;

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        StartCoroutine("BeatPlay");
    }




    private IEnumerator BeatPlay()
    {
        while (true)
        {
            PlayFlag = true;
            if (PenaltyFlag)
            {
                PlayFlag = false;
                PenaltyCount++;
            }
            yield return new WaitForSeconds(0.15f);
            PlayFlag = false;
            yield return new WaitForSeconds(0.3f);
            PlayFlag = true;
            if (PenaltyFlag)
            {
                PlayFlag = false;
                PenaltyCount++;
            }
            yield return new WaitForSeconds(0.15f);
        }

    }




    // Update is called once per frame
    void FixedUpdate()
    {

        if (!PlayFlag)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // ペナルティ処理
                PenaltyFlag = true;
            }
        }
        if (PenaltyCount == 2)
        {
            PenaltyFlag = false;
            PenaltyCount = 0;
        }

        TestText.text = addText.ToString();

        //// 仮置き 指定フレームだけ操作可能
        if (PlayFlag && Input.GetMouseButtonDown(0)) addText += 1;
    }
}
// SceneGameBGM 拍間0.6frame
// TitleBGM     拍間0.56frame