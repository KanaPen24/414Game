using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ON_BreakWeapo : MonoBehaviour
{
    [SerializeField] private GameObject[] objects;
    [Range(0.0f, 1.0f)] public float rate = 0.0f;
    private List<Material> materials;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        materials = new List<Material>();
        for (int i = 0; i < objects.Length; ++i)
        {
            materials.Add(objects[i].GetComponent<Renderer>().material);
        }

        for (int i = 0; i < materials.Count; ++i)
        {
            materials[i].SetFloat("_Rate", rate);
        }
        materials.Clear();
    }
}
