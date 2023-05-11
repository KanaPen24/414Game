using System;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
[VolumeComponentMenu("Time Effect")]
public class ON_TimeEffectVolume : VolumeComponent
{
    public bool IsActive() => tintColor != Color.white;

    public ColorParameter tintColor = new ColorParameter(Color.white);
}
