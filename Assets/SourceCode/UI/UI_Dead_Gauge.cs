using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Dead_Gauge : MonoBehaviour
{

    [SerializeField]
    private RectTransform Dead_Gauge_Image_Rect;          // €–SƒQ[ƒWUI‰æ‘œ‚ÌRectTransform

    [SerializeField]
    private float Dead_Gauge_ScaleX;                      // €–SƒQ[ƒWUI‰æ‘œ‚Ì‰¡••Ï”

    // Start is called before the first frame update
    void Start()
    {
        // €–SƒQ[ƒWUI‰æ‘œ‚Ì‰¡••Ï”‚ğ‰Šú‰»‚·‚é
        Dead_Gauge_ScaleX = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
