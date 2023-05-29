using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFX_Drain : MonoBehaviour
{
    private VisualEffect _vfx;
    // Start is called before the first frame update
    void Start()
    {
        _vfx = GetComponent<VisualEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        _vfx.SetVector3("TargetPos", Vector3.zero);
    }
}
