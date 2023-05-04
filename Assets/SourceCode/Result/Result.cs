using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Result : MonoBehaviour
{
    // スコア
    [SerializeField]
    GameObject _Score;

    // レベル
    [SerializeField]
    GameObject _Level;

    // コンボ
    [SerializeField]
    GameObject _Combo;

    // 時間
    [SerializeField]
    GameObject _Time;

    // トランジション
    [SerializeField]
    Result_Transition _transition;

    // タイマー
    private float _Timer = 0f;

    // 初期化
    private void Start()
    {
        // 遷移アニメーション開始
        _transition.Start_INanimation();
    }

    // 更新処理
    public void Update()
    {
        // 遷移アニメーションが終わっていて　全てのカウントが終わっていなかったら
        if (_transition.GetEndOUTTransition() && 
            !_Score.transform.GetChild(1).GetComponent<ScoreNumber>().GetEndCount() &&
            !_Level.transform.GetChild(1).GetComponent<LevelNumber>().GetEndCount() &&
            !_Combo.transform.GetChild(1).GetComponent<ComboNumber>().GetEndCount() &&
            !_Time.transform.GetChild(1).GetComponent<TimeNumber>().GetEndCount()
            )

        {
            _Score.GetComponent<Animator>().SetBool("StartAnimation", true);

            if (_Timer > 1f)
                _Level.GetComponent<Animator>().SetBool("StartAnimation", true);

            if (_Timer > 2f)
                _Combo.GetComponent<Animator>().SetBool("StartAnimation", true);

            if (_Timer > 3f)
                _Time.GetComponent<Animator>().SetBool("StartAnimation", true);

            // タイマー処理
            _Timer += Time.deltaTime;
        }

        // 全てのカウントが終わっていたら
        if (_Score.transform.GetChild(1).GetComponent<ScoreNumber>().GetEndCount() &&
            _Level.transform.GetChild(1).GetComponent<LevelNumber>().GetEndCount() &&
            _Combo.transform.GetChild(1).GetComponent<ComboNumber>().GetEndCount() &&
            _Time.transform.GetChild(1).GetComponent<TimeNumber>().GetEndCount()
            )
        {
            // 遷移アニメーション再生
            _transition.Start_OUTanimation();
        }

        // 遷移アニメーションが終わったら
        if (_transition.GetEndINTransition())
        {
            // シーン遷移
            //SceneManager.LoadScene("ResultScene");
        }
    }
}
