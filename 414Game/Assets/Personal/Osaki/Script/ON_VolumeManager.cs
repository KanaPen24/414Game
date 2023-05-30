using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ON_VolumeManager : MonoBehaviour
{
    private Volume _volume;
    private ON_TimeEffectVolume timeeffect;
    private ON_BraunTubeVolume braunTube;
    private ON_GaussianVolume gaussian;
    // Start is called before the first frame update
    void Start()
    {
        _volume = GetComponent<Volume>();
    }

    // タイマーエフェクトの遷移
    public void ChangeTimePostEffect(float rate)
    {
        if (_volume.profile.TryGet<ON_TimeEffectVolume>(out timeeffect))
        {
            rate = rate > 1 ? 1 : rate;
            rate = rate < 0 ? 0 : rate;
            timeeffect.rate.value = rate;
        }
    }

    // ブラウン管の遷移
    public void SetBraunRate(float rate)
    {
        if(_volume.profile.TryGet<ON_BraunTubeVolume>(out braunTube))
        {
            rate = rate > 1 ? 1 : rate;
            rate = rate < 0 ? 0 : rate;
            braunTube.rate.value = rate;
        }
    }

    // ガウシアンブラーの遷移
    public void SetGaussianRate(float rate)
    {
        if(_volume.profile.TryGet<ON_GaussianVolume>(out gaussian))
        {
            rate = rate > 1 ? 1 : rate;
            rate = rate < 0 ? 0 : rate;
            gaussian.rate.value = rate;
        }
    }
}
