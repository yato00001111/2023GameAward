using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultTransition : MonoBehaviour
{
    // アニメーション
    [SerializeField]
    private Animator anime;

    // インアニメーション終了
    private bool _End_INTransition = false;

    // アウトアニメーション終了
    private bool _End_OUTTransition = false;

    // フェードインアニメーション再生
    public void Start_INanimation()
    {
        anime.SetTrigger("IN_Animation");
    }

    // フェードアウトアニメーション再生
    public void Start_OUTanimation()
    {
        anime.SetTrigger("OUT_Animation");
    }

    // フェードインアニメーション終了(アニメーター用)
    public void EndINTransition()
    {
        _End_INTransition = true;
    }

    // フェードアウトアニメーション終了(アニメーター用)
    public void EndOUTTransition()
    {
        _End_OUTTransition = true;
    }

    // フェードインアニメーション終了取得
    public bool GetEndINTransition()
    {
        return _End_INTransition;
    }

    // フェードアウトアニメーション終了取得
    public bool GetEndOUTTransition()
    {
        return _End_OUTTransition;
    }
}
