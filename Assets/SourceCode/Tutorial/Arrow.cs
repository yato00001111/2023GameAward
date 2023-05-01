using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    // 点滅させる対象
    [SerializeField] private SpriteRenderer _target;

    // 点滅周期[s]
    [SerializeField] private float _cycle = 1;

    private double _time;

    private void Update()
    {
        // 内部時刻を経過させる
        _time += Time.deltaTime;

        // 周期cycleで繰り返す波のアルファ値計算
        var alpha = Mathf.Cos((float)(2 * Mathf.PI * _time / _cycle)) * 0.5f + 0.5f;

        // 内部時刻timeにおけるアルファ値を反映
        var color = _target.color;
        color.a = alpha;
        _target.color = color;
    }
}
