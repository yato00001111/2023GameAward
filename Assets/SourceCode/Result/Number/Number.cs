using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Number : MonoBehaviour
{
    // 画像出力用
    [SerializeField]
    ImageRenderer _ImageRenderer;

    // 獲得した値
    [SerializeField]
    int _Num = 0;

    // 表示する値
    private int _NowNum = 0;

    // カウント開始
    private bool _StartCount = false;

    // カウント終了
    private bool _EndCount = false;

    // 親オブジェクト
    public GameObject Parent;

    // 更新処理
    private void Update()
    {
        // カウント開始フラグが立っていなかったら
        if (!_StartCount)
        {
            // アニメーション開始時と同時にカウント開始
            if (Parent.GetComponent<Animator>().GetBool("StartAnimation"))
                StartCoroutine(StartCount());
        }

        // カウント開始フラグがっ立っていたら
        if (_StartCount)
        {
            // スコアに達するまで上昇し続ける
            if (_NowNum < _Num)
            {
                _NowNum++;
            }
            else
            {
                // カウント終了
                _EndCount = true;
            }
        }

        // 画像で出力
        _ImageRenderer._Update(_NowNum);
    }

    // カウント開始関数
    private IEnumerator StartCount()
    {
        // ２秒待つ
        yield return new WaitForSeconds(2);

        // カウント開始
        _StartCount = true;
    }

    // カウントを強制的に終了させる
    public void Finish()
    {
        // 表示している値を獲得した値にして
        _NowNum = _Num;
        //　カウント終了フラグを立てる
        _EndCount = true;
    }

    // カウント終了フラグ取得
    public bool GetEndCount()
    {
        return _EndCount;
    }

    // 表示する値の設定
    public void SetNumber(int value)
    {
        _NowNum = value;
    }
}
