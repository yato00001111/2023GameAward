using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeNumber : MonoBehaviour
{
    // 分描画
    [SerializeField]
    ImageRenderer _MuinitRenderer;

    // 秒描画
    [SerializeField]
    ImageRenderer _SecondRenderer;

    // タイム(分)
    [SerializeField]
    int _TimeMuinit = 0;

    // タイム(秒)
    [SerializeField]
    int _TimeSecond = 0;

    // 総合タイム(フレーム)
    private int TotalTime = 0;

    // 上昇する時間
    private int _NowTime = 0;

    // 分
    private int _Muinit = 0;

    // 秒
    private int _Second = 0;

    // カウント開始
    private bool _StartCount = false;

    // カウント終了
    private bool _EndCount = false;

    //　親オブジェクト
    public GameObject Parent;

    // 初期化
    private void Start()
    {
        // フレームで管理するので単位をフレームになおす
        TotalTime += _TimeMuinit * 60 * 60;
        TotalTime += _TimeSecond * 60;
    }

    // 更新処理
    void Update()
    {
        // カウント開始フラグが立ってなかったら
        if (!_StartCount)
        {
            // アニメション開始時と同時にカウント開始
            if (Parent.GetComponent<Animator>().GetBool("StartAnimation"))
                _StartCount = true;
        }

        // カウント開始
        if (_StartCount)
        {
            // タイムに達するまで上昇
            if (_NowTime < TotalTime)
            {
                // カウントを速くするために60
                _NowTime += 60;
            }
            else
            {
                // カウント終了
                _EndCount = true;
            }

            // カウント終了フラグがったっていなかったら
            if (!_EndCount)
            {
                // フレーム単位を、秒、分に変換
                if (_NowTime % 60 == 0)
                {
                    _Second++;
                }

                if (_Second == 60)
                {
                    _Second -= 60;
                    _Muinit++;
                }
            }

        }
            // 分描画
            _MuinitRenderer._Update(_Muinit);
            // 秒描画
            _SecondRenderer._Update(_Second);      
    }

    // アニメーション終了フラグを立てる
    void SetEndAnimation()
    {
        _StartCount = true;
    }

    // カウント終了フラグ取得
    public bool GetEndCount()
    {
        return _EndCount;
    }
}
