using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class Result : MonoBehaviour
{
    // スキップカウント
    [HideInInspector]
    public int _SkipCount = 0;
    
    // オブジェクトリスト
    enum ObjList
    {
        Score,      // スコア
        Wave,       // ウェーブ
        Combo,      // コンボ
        JustTiming, // ジャストタイミング

        MAX         // リスト数
    };

    // スコア
    [SerializeField]
    GameObject _Score;

    // ウェーブ
    [SerializeField]
    GameObject _Wave;

    // コンボ
    [SerializeField]
    GameObject _Combo;

    // ジャストタイミング
    [SerializeField]
    GameObject _Just_Timing;

    // オブジェクトの配列
    [HideInInspector]
    public GameObject[] OBJList = new GameObject[(int)ObjList.MAX];

    // トランジション
    [SerializeField]
    New_Transition _transition;

    // リトライボタン
    [SerializeField]
    GameObject RetryButton;

    // タイトルボタン
    [SerializeField]
    GameObject TitleButton;

    // タイマー
    private float _Timer = 0f;

    // 入力遅延タイマー
    [SerializeField]
    private float _lateTimer = 0f;

    // プレイヤーの入力取得
    [SerializeField, HideInInspector]
    KeyCode PlayerInput = 0;

    // ボタン押下フラグ
    [SerializeField]
    private bool PushFlg = false;

    // スキップ関数配列
    public delegate void Delegate(GameObject gameObject);
    Delegate[] SkipFunctions = new Delegate[2];

    // 初期化
    public void Start()
    {
        // 関数配列の設定
        SkipFunctions[0] = SkipAnimation;
        SkipFunctions[1] = SkipCount;

        // オブジェクト配列
        OBJList[(int)ObjList.Score]      = _Score;
        OBJList[(int)ObjList.Wave]       = _Wave;
        OBJList[(int)ObjList.Combo]      = _Combo;
        OBJList[(int)ObjList.JustTiming] = _Just_Timing;
    }

    // 更新処理
    public void Update()
    {
        // 遷移アニメーションが終わってたら
        if (_transition.GetEndINTransition())
        {
            // スコアのアニメーション再生
            _Score.GetComponent<Animator>().SetBool("StartAnimation", true);

            // タイマーが１秒以上　または　スコアのカウントがスキップされていたら
            if (_Timer > 1f || _SkipCount > 1)
                // ウェーブアニメーション再生
                _Wave.GetComponent<Animator>().SetBool("StartAnimation", true);

            // タイマーが２秒以上　または　ウェーブのカウントがスキップされていたら
            if (_Timer > 2f || _SkipCount > 3)
                // コンボアニメーション再生
                _Combo.GetComponent<Animator>().SetBool("StartAnimation", true);

            // タイマーが３秒以上　または　コンボのカウントがスキップされていたら
            if (_Timer > 3f || _SkipCount > 5)
            {
                // ジャストタイミングアニメーション再生
                _Just_Timing.GetComponent<Animator>().SetBool("StartAnimation", true);
            }

            // スキップ処理
            Skip();

            // タイマー処理
            _Timer += Time.deltaTime;
        }

        // 全てのカウントが終わっていたら
        if (_Score.transform.GetChild(1).GetComponent<Number>().GetEndCount() &&
            _Wave.transform.GetChild(1).GetComponent<Number>().GetEndCount() &&
            _Combo.transform.GetChild(1).GetComponent<Number>().GetEndCount() &&
            _Just_Timing.transform.GetChild(1).GetComponent<Number>().GetEndCount()
            )
        {
            // 遷移ボタン表示
            RetryButton.SetActive(true);
            TitleButton.SetActive(true);

            if (_lateTimer > 3f)
            {
                PushFlg = true;
            }

            // Aボタン(Retry)が押されたら
            if (Input.GetKeyDown(KeyCode.Joystick1Button0) && PushFlg)
            {
                // 一度だけの制御
                PushFlg = false;

                PlayerInput = KeyCode.Joystick1Button0;

                // 遷移アニメーション再生
                _transition.Start_OUTanimation();
            }

            //Bボタン(Title)が押されたら
            if (Input.GetKeyDown(KeyCode.Joystick1Button1) && PushFlg)
            {
                // 一度だけの制御
                PushFlg = false;

                PlayerInput = KeyCode.Joystick1Button1;

                // 遷移アニメーション再生
                _transition.Start_OUTanimation();
            }

            _lateTimer += Time.deltaTime;
        }

        // 遷移アニメーションが終わったら
        if (_transition.GetEndOUTTransition())
        {
            // プレイヤーの入力に応じて遷移変更
            if (PlayerInput == KeyCode.Joystick1Button0)
                SceneManager.LoadScene("GameScene");

            if (PlayerInput == KeyCode.Joystick1Button1)
                SceneManager.LoadScene("Title");
        }
    }

    // スキップ処理
    private void Skip()
    {
        // 左クリックしたとき または　いずれかのボタンを押したとき
        if( 
            Input.GetKeyDown(KeyCode.Joystick1Button0) ||
            Input.GetKeyDown(KeyCode.Joystick1Button1) ||
            Input.GetKeyDown(KeyCode.Joystick1Button2) ||
            Input.GetKeyDown(KeyCode.Joystick1Button3) ||
            Input.GetKeyDown(KeyCode.Joystick1Button4) ||
            Input.GetKeyDown(KeyCode.Joystick1Button5)
            )
        {
            // 関数の配列の中から _SkipCount の数値に応じて　呼び出し関数、<=関数の引数　を変更して関数を呼ぶ
            SkipFunctions[(_SkipCount % 2)](OBJList[(int)_SkipCount / 2]); 
        }
    }

    // カウントをスキップする
    private void SkipCount(GameObject gameObject)
    {
        // 子供のNumberスクリプトを取得
        Number obj = gameObject.transform.GetChild(1).GetComponent<Number>();

        // カウント上昇中だったら
        if (!obj.GetEndCount())
        {
            // カウント終了関数を呼ぶ
            obj.Finish();

            // クリックしすぎてエラーを出さないように制御
            if (_SkipCount < 7) _SkipCount++;
        }
    }

    // 再生中のアニメーションを終わりまでスキップする
    private void SkipAnimation(GameObject gameObject)
    {
        // アニメーションコンポーネント取得
        Animator animator = gameObject.GetComponent<Animator>();

        // 再生中のアニメーションを取得
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // アニメーションが再生されていたら
        if (stateInfo.length > 0)
        {
            //再生時間が0～1の範囲なので強制的に１にする
            animator.Play(stateInfo.fullPathHash, 0, 1);

            // クリックしすぎてエラーを出さないように制御
            if (_SkipCount < 7) _SkipCount++;
        }
    }

    // 設定関数
    public void SetResultNumber(int FinalScore, int Wave, int  Combo, int Just_Timing)
    {
        _Score.transform.GetComponent<Number>().SetNumber(FinalScore);
        _Wave.transform.GetComponent<Number>().SetNumber(Wave);
        _Combo.transform.GetComponent<Number>().SetNumber(Combo);
        _Just_Timing.transform.GetComponent<Number>().SetNumber(Just_Timing);
    }
}
