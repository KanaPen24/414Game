using System;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
[VolumeComponentMenu("Player Blur")]
public class ON_PlayerBlurVolume : VolumeComponent
{
    public bool isActive() =>  check != false;
    public BoolParameter check = new BoolParameter(false);
    public RenderTexture BlurTexture = null;
}
