using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NK_SlimeSpooner : MonoBehaviour
{
    //すらいむが出てくるまでの時間
    [SerializeField] private float m_fSpoonTime;
    //すらいむ
    [SerializeField] private GameObject m_gSlime;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("SlimeSpoon",m_fSpoonTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SlimeSpoon()
    {
        Instantiate(m_gSlime, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
