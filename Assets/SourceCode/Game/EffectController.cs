using Effekseer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    [Header("エフェクト")]
    [SerializeField] private EffekseerEffectAsset[] _tantaiEffects;
    private EffekseerHandle _tantaiEffectHandle;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PlayEffect(int Color)
    {
        _tantaiEffectHandle = EffekseerSystem.PlayEffect(_tantaiEffects[Color], transform.position);
        _tantaiEffectHandle.SetScale(transform.lossyScale);
        _tantaiEffectHandle.SetRotation(transform.rotation);
    }
}
