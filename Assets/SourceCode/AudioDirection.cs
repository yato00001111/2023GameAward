using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 音の演出に関する関数概要スクリプト

public class AudioDirection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 拍に来たフレームで true になる
        if (Music.IsJustChangedBeat())
        {
        }

        // 小節に来たフレームで true になる
        if (Music.IsJustChangedBar())
        {
        }

        // 指定した小節,拍,ユニットに来たフレームで true になる
        if (Music.IsJustChangedAt(1, 2, 3))
        {
            
        }

        // ↑使用する際にはMusicUnityスクリプトを一緒にadd component
        // 使用するサウンドを設定する
    }
}
