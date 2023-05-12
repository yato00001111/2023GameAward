using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ActionTitleLogo : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // �����Ƃ�true
        if (Music.IsJustChangedBeat())
        {
            DOTween
              .To(value => OnScale(value), 0, 1, 0.1f)
              .SetEase(Ease.InQuad)
              .SetLoops(2, LoopType.Yoyo)
              ;


        }
    }

    // �I�u�W�F�N�g�̃X�P�[����ύX
    private void OnScale(float value)
    {
        var scale = Mathf.Lerp(1, 1.2f, value);
        transform.localScale = new Vector3(scale, scale, scale);
    }
}