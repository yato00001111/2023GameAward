using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Dead_Gauge : MonoBehaviour
{

    [SerializeField]
    private RectTransform Dead_Gauge_Image_Rect;          // ���S�Q�[�WUI�摜��RectTransform

    [SerializeField]
    private float Dead_Gauge_ScaleX;                      // ���S�Q�[�WUI�摜�̉����ϐ�

    // Start is called before the first frame update
    void Start()
    {
        // ���S�Q�[�WUI�摜�̉����ϐ�������������
        Dead_Gauge_ScaleX = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
