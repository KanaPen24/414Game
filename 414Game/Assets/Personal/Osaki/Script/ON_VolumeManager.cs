using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ON_VolumeManager : MonoBehaviour
{
    private Volume _volume;
    private ON_TimeEffectVolume timeeffect;
    private ON_BraunTubeVolume braunTube;
    // Start is called before the first frame update
    void Start()
    {
        _volume = GetComponent<Volume>();
    }

    // タイマーエフェクトの切り替え
    public void ChangeTimePostEffect(bool flg)
    {
        if (_volume.profile.TryGet<ON_TimeEffectVolume>(out timeeffect))
        {
            timeeffect.active = flg;
        }
    }

    // ブラウン管の遷移
    public void SetBraunRate(float rate)
    {
        if(_volume.profile.TryGet<ON_BraunTubeVolume>(out braunTube))
        {
            //rate = Mathf.Max(rate, 1);
            //rate = Mathf.Min(0, rate);
            braunTube.rate.value = rate;
        }
    }
}
