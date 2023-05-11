using System;
using UnityEngine;
using UnityEngine.Rendering;

[Serializable]
[VolumeComponentMenu("Braun Tube")]
public class ON_BraunTubeVolume : VolumeComponent
{
    public bool isActive() => useEffect != false;
    public BoolParameter useEffect = new BoolParameter(false);
}
