using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ActionTitleLight : MonoBehaviour
{

    GameObject TitleLight_L;
    GameObject TitleLight_R;

    Image LightImage_L;
    Image LightImage_R;

    float TitleTimer;
    float Span = 2;

    // Start is called before the first frame update
    void Start()
    {
        TitleLight_L = GameObject.Find("TitleLight_L");
        TitleLight_R = GameObject.Find("TitleLight_R");

        LightImage_L = TitleLight_L.GetComponent<Image>();
        LightImage_R = TitleLight_R.GetComponent<Image>();

        SetOpacity(LightImage_L, 1.0f);
        SetOpacity(LightImage_R, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        LightImage_L.DOFade(0, 2);
        LightImage_R.DOFade(0, 2);
    }

    void SetOpacity(Image image, float alpha)
    {
        var c = image.color;
        image.color = new Color(c.r, c.g, c.b, alpha);
    }
}
