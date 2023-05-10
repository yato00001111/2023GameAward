using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelNumber : MonoBehaviour
{
    // 画像出力用
    [SerializeField]
    ImageRenderer _ImageRenderer;

    // レベル
    [SerializeField]
    int _Level = 0;

    // 上昇するレベル
    private int _NowLevel = 0;

    // カウント開始フラグ
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
            // アニメーション開始と同時にカウント開始
            if (Parent.GetComponent<Animator>().GetBool("StartAnimation"))
                _StartCount = true;
        }

        // カウント開始
        if (_StartCount)
        {
            // レベルに達するまで上昇し続ける
            if (_NowLevel < _Level)
            {
                _NowLevel++;
            }
            else
            {
                // カウント終了
                _EndCount = true;
            }
        }

        // 画像で出力
        _ImageRenderer._Update(_NowLevel);
    }

    // レベル設定
    public void SetLevel(int Level)
    {
        _Level = Level;
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
