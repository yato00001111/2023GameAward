using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CountDown : MonoBehaviour
{

    [SerializeField]
    private Animator CountDown_Animation;          // カウントダウンアニメーション

    [SerializeField]
    private AnimationClip CountDown_AnimationClip; // カウントダウンアニメーションクリップ

    [SerializeField]
    private int Animation_Start_Count;             // カウントダウンアニメーション開始までのカウント
                                                   
    [SerializeField]                               
    private bool Game_Start_Flag;                  // ゲーム開始フラグ
                                                   
    [SerializeField]                               
    public float currentTime;                      // 現在のカウントダウンアニメーションの再生時間

    [SerializeField]
    private bool Tutorial_Start_Flag;              // チュートリアル開始フラグ


    // Start is called before the first frame update
    void Start()
    {
        // カウントダウンアニメーションを初期化する
        CountDown_Animation = GetComponent<Animator>();
        // カウントダウンアニメーション開始までのカウントを初期化する
        Animation_Start_Count = 0;
        // ゲーム開始フラグを初期化する
        Game_Start_Flag = false;

        Tutorial_Start_Flag = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Music.IsJustChangedBeat())
        {
            // カウントが" "の時のみアニメーション開始
            Animation_Start_Count++;
        }

        // ゲーム画面に入って一定時間経過したらカウントダウンアニメーション開始
        if (Animation_Start_Count >= 3)
        {
            CountDown_Animation.SetBool("CountDown_Flag", true);

            // タイマー起動
            currentTime += Time.deltaTime;
        }

        // アニメーション再生時間が1になればゲーム開始フラグを立てる
        if (currentTime >= CountDown_AnimationClip.length) Game_Start_Flag = true;
        if(Tutorial_Start_Flag) Game_Start_Flag = true;
    }

    // ゲーム開始フラグを取得関数
    public bool GetGameStartFlag() { return Game_Start_Flag; }

    public void SetTutorialStartFlag(bool flag) { Tutorial_Start_Flag = flag; }
}
