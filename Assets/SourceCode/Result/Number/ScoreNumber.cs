using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreNumber : MonoBehaviour
{
    // 画像出力用
    [SerializeField]
    ImageRenderer _ImageRenderer;

    // スコア
    [SerializeField]
    int _Score = 0;

    // 上昇するスコア
    [SerializeField]
    int _NowScore = 0;

    // カウント開始
    private bool _StartCount = false;

    // カウント終了
    private bool _EndCount = false;

    //　親オブジェクト
    public GameObject Parent;

    // 更新処理
    void Update()
    {
        // カウント開始フラグが立っていなかったら
        if (!_StartCount)
        {
            // アニメーション開始時と同時にカウント開始
            if (Parent.GetComponent<Animator>().GetBool("StartAnimation"))
                _StartCount = true;
        }

        // カウント開始フラグがっ立っていたら
        if (_StartCount)
        {
            // スコアに達するまで上昇し続ける
            if (_NowScore < _Score)
            {
                _NowScore++;
            }
            else
            {
                // カウント終了
                _EndCount = true;
            }
        }

        // 画像で出力
        _ImageRenderer._Update(_NowScore);
    }

    // スコア設定
    public void SetScore(int Score)
    {
        _Score = Score;
    }

    // カウント開始フラグ設定
    public void SetStartCount(bool flg)
    {
        _StartCount = flg;
    }

    // カウント終了フラグ取得
    public bool GetEndCount()
    {
        return _EndCount;
    }

    // アニメーション終了フラグを立てる
    void SetEndAnimation()
    {
        _StartCount = true;
    }
}
