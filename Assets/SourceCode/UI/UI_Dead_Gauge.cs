using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Dead_Gauge : MonoBehaviour
{

    [SerializeField]
    private RectTransform Dead_Gauge_Image_Rect;          // SQ[WUIζΜRectTransform

    [SerializeField]
    private float Dead_Gauge_ScaleX;                      // SQ[WUIζΜ‘Ο

    // Start is called before the first frame update
    void Start()
    {
        // SQ[WUIζΜ‘Οπϊ»·ι
        Dead_Gauge_ScaleX = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
