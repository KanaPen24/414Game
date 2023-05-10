using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ON_TimePostEffect : MonoBehaviour
{
    private Volume _volume;
    private ON_TimeEffectVolume timeeffect;
    // Start is called before the first frame update
    void Start()
    {
        _volume = GetComponent<Volume>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            ChangeTimePostEffect(true);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            ChangeTimePostEffect(false);
        }
    }

    public void ChangeTimePostEffect(bool flg)
    {
        if(_volume.profile.TryGet<ON_TimeEffectVolume>(out timeeffect))
        {
            timeeffect.active = flg;
        }
    }
}
