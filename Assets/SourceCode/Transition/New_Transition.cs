using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class New_Transition : MonoBehaviour
{
    // アニメーション
    private Animator anime;

    // インアニメーション終了
    private bool _End_INTransition = false;

    // アウトアニメーション終了
    private bool _End_OUTTransition = false;

    // 初期化
    void Start()
    {
        // アニメーション取得
        anime = gameObject.GetComponent<Animator>();
    }

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
