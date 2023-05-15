using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_NextBlock_Direction : MonoBehaviour
{

    [SerializeField]
    private Animator NextBlock_Animator;                 // ネクストブロックUIのアニメーション

    // Start is called before the first frame update
    void Start()
    {
        // アニメーターを初期化する
        NextBlock_Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetNextBlockAnimation()
    {
        // アニメーションの再生時間を"0"にする
        NextBlock_Animator.Play(0, -1, 0f);
    }
}
