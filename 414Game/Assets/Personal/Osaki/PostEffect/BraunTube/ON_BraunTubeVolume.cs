using System;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
[VolumeComponentMenu("Braun Tube")]
public class ON_BraunTubeVolume : VolumeComponent
{
    public bool isActive() => check != false;

    public BoolParameter check = new BoolParameter(false);
}
