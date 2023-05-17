using System;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
[VolumeComponentMenu("Time Effect")]
public class ON_TimeEffectVolume : VolumeComponent
{
    public bool IsActive() => rate != 0.0f;

    public ClampedFloatParameter rate = new ClampedFloatParameter(0.0f, 0.0f, 1.0f);
}
