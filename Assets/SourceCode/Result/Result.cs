using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class Result : MonoBehaviour
{
    // スキップカウント
    private int _SkipCount = 0;
    
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
    private GameObject[] OBJList = new GameObject[(int)ObjList.MAX];

    // トランジション
    [SerializeField]
    ResultTransition _transition;

    // パーティクル
    [SerializeField]
    GameObject Particle;

    // タイマー
    private float _Timer = 0f;

    // スキップ関数配列
    public delegate void Delegate(GameObject gameObject);
    Delegate[] SkipFunctions = new Delegate[2];

    // 初期化
    private void Start()
    {
        // 遷移アニメーション開始
        _transition.Start_INanimation();

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
            if (_Timer > 1f || _SkipCount > 2)
                // ウェーブアニメーション再生
                _Wave.GetComponent<Animator>().SetBool("StartAnimation", true);

            // タイマーが２秒以上　または　ウェーブのカウントがスキップされていたら
            if (_Timer > 2f || _SkipCount > 4)
                // コンボアニメーション再生
                _Combo.GetComponent<Animator>().SetBool("StartAnimation", true);

            // タイマーが３秒以上　または　コンボのカウントがスキップされていたら
            if (_Timer > 3f || _SkipCount > 6)
                // ジャストタイミングアニメーション再生
                _Just_Timing.GetComponent<Animator>().SetBool("StartAnimation", true);

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
            // 遷移アニメーション再生
            _transition.Start_OUTanimation();

            // パーティクルを隠す
            Particle.SetActive(false);
        }

        // 遷移アニメーションが終わったら
        if (_transition.GetEndOUTTransition())
        {
            // シーン遷移
            //SceneManager.LoadScene("");
        }
    }

    // スキップ処理
    private void Skip()
    {
        // 左クリックしたとき
        if(Input.GetMouseButtonDown(0))
        {
            // 関数の配列の中から _SkipCount の数値に応じて　呼び出し関数、<=関数の引数　を変更して関数を呼ぶ
            SkipFunctions[(_SkipCount % 2)](OBJList[(int)_SkipCount / 2]); 
            // クリックしすぎてエラーを出さないように制御
             if(_SkipCount < 7) _SkipCount++;
        }
    }

    // カウントをスキップする
    public static void SkipCount(GameObject gameObject)
    {
        // 子供のNumberスクリプトを取得
        Number obj = gameObject.transform.GetChild(1).GetComponent<Number>();
        // カウント終了関数を呼ぶ
        obj.Finish();
    }

    // 再生中のアニメーションを終わりまでスキップする
    public static void SkipAnimation(GameObject gameObject)
    {
        // アニメーションコンポーネント取得
        Animator animator = gameObject.GetComponent<Animator>();

        // 再生中のアニメーションを取得
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        //再生時間が0～1の範囲なので強制的に１にする
        animator.Play(stateInfo.fullPathHash, 0, 1);
    }
}
