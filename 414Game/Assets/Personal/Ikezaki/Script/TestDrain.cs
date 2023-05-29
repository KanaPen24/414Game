using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDrain : MonoBehaviour
{
    [SerializeField] private Transform[] Objects;

    [SerializeField] private VFX_Drain Test;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Test.SetStartPos(Objects[0].position);
        Test.SetEndPos(Objects[1].position);

    }
}
