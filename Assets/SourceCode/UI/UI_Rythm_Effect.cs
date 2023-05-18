using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Effekseer;

public class UI_Rythm_Effect : MonoBehaviour
{
    [Header("エフェクト")]
    [SerializeField] private EffekseerEffectAsset[] _Effects;
    private EffekseerHandle _EffectHandle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayEffect(bool success)
    {
        int _success = -1;
        if (success) _success = 0;
        else _success = 1;
         _EffectHandle = EffekseerSystem.PlayEffect(_Effects[_success], transform.position);
        _EffectHandle.speed = 2;
    }

}
