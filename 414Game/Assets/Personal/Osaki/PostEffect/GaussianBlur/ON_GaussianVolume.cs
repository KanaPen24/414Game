using System;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
[VolumeComponentMenu("Gaussian Blur")]
public class ON_GaussianVolume : VolumeComponent
{
    public bool isActive() => rate != 0.0f;
    public ClampedFloatParameter rate = new ClampedFloatParameter(0.0f, 0.0f, 1.0f);
}
