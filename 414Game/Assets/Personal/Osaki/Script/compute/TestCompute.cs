using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCompute : MonoBehaviour
{
    [SerializeField] private ComputeShader _compute;
    [SerializeField] private Transform cube;
    private ComputeBuffer buffer;

    // Start is called before the first frame update
    void Start()
    {
        buffer = new ComputeBuffer(1, sizeof(float));
        _compute.SetBuffer(0, "Result", buffer);
    }

    // Update is called once per frame
    void Update()
    {
        _compute.SetFloat("positionX", cube.position.x);
        _compute.Dispatch(0, 8, 8, 1);

        var data = new float[1];
        buffer.GetData(data);

        float positionX = data[0];

        var boxPosition = cube.position;
        boxPosition.x = positionX;
        cube.position = boxPosition;
    }

    private void OnDestroy()
    {
        buffer.Release();
    }
}
