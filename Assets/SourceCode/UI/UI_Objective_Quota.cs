using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Objective_Quota : MonoBehaviour
{
    public ImageRenderer imageRenderer;

    [SerializeField]
    private int Num; // ノルマ数

    // Start is called before the first frame update
    void Start()
    {
        // ノルマ数初期化する
        Num = 1;
    }

    // Update is called once per frame
    void Update()
    {
        imageRenderer._Update(Num);
    }

    // ノルマ数を設定する関数
    public void SetObjectiveQuota(int num) { Num = num; }
}
