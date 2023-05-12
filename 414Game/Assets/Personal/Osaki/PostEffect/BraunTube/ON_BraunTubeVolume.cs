using System;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
[VolumeComponentMenu("Braun Tube")]
public class ON_BraunTubeVolume : VolumeComponent
{
    public bool isActive() => rate != 0.0f;
    public ClampedFloatParameter rate = new ClampedFloatParameter(0.0f, 0.0f, 1.0f);
}
