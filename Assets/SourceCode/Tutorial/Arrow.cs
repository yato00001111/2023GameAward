using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    // �_�ł�����Ώ�
    [SerializeField] private SpriteRenderer _target;

    // �_�Ŏ���[s]
    [SerializeField] private float _cycle = 1;

    private double _time;

    private void Update()
    {
        // �����������o�߂�����
        _time += Time.deltaTime;

        // ����cycle�ŌJ��Ԃ��g�̃A���t�@�l�v�Z
        var alpha = Mathf.Cos((float)(2 * Mathf.PI * _time / _cycle)) * 0.5f + 0.5f;

        // ��������time�ɂ�����A���t�@�l�𔽉f
        var color = _target.color;
        color.a = alpha;
        _target.color = color;
    }
}
