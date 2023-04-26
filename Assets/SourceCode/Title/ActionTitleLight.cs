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

        SetOpacity(LightImage_L, 0.0f);
        SetOpacity(LightImage_R, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //LightImage_L.DOFade(0, 2);
        //LightImage_R.DOFade(0, 2);

        // ”‚²‚Æ‚Étrue
        if (Music.IsJustChangedBeat())
        {
            DOTween
              .To(value => SetOpacity(LightImage_L,value), 0, 0.25f, 0.1f)
              .SetEase(Ease.InQuad)
              .SetLoops(2, LoopType.Yoyo)
              ;

            DOTween
              .To(value => SetOpacity(LightImage_R, value), 0, 0.25f, 0.1f)
              .SetEase(Ease.InQuad)
              .SetLoops(2, LoopType.Yoyo)
              ;
        }
    }

    void SetOpacity(Image image, float alpha)
    {
        var c = image.color;
        image.color = new Color(c.r, c.g, c.b, alpha);
    }
}
