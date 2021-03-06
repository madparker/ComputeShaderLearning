using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public struct Line
{
    public Vector3 position;
    public Color color;
    public Vector3 top;
    public Vector3 bottom;
}

public class ComputeShaderTest : MonoBehaviour
{
    public ComputeShader computeShader;
    public RenderTexture renderTexure;

    public Line[] data;

    // Start is called before the first frame update
    void Start()
    {
//        data = new Line[32];
//
//        for (int i = 0; i < data.Length; i++) {
//            data[i] = new Line();
//            data[i].position = new Vector3(0, Random.value, 0);
//            data[i].color = Random.ColorHSV();
//        }
//
//
//        int colorSize = sizeof(float) * 4;
//        int vector3Size = sizeof(float) * 3;
//        int totalSize = colorSize + vector3Size * 3;
//
//        ComputeBuffer cb = new ComputeBuffer(data.Length, totalSize);
//
//        print("start!");
//
//        cb.SetData(data);
//
//        computeShader.SetFloat("resolution", data.Length);
//        computeShader.SetBuffer(0, "lines", cb);
//        computeShader.Dispatch(0, data.Length/10, 1, 1);
//
//        cb.GetData(data);
//
//        for (int i = 0; i < data.Length; i++)
//        {
//            print(data[i].position);
//        }
//
//        cb.Dispose();
    }

    ComputeBuffer cb;
    
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        int nums = 10000;
        
        cb = new ComputeBuffer(nums, sizeof(float));

        float[] floats = new float[nums];

        float timeStart = Time.realtimeSinceStartup;
        
        cb.SetData(floats);
        
        renderTexure = new RenderTexture(256, 256, 24);
        renderTexure.enableRandomWrite = true;
        renderTexure.Create();
        
        computeShader.SetBuffer(0, "nums", cb);

        //computeShader.SetTexture(0, "Result", renderTexure);

        computeShader.SetFloat("Resolution", renderTexure.width);
        computeShader.SetFloat("test", 7);

        computeShader.Dispatch(0,
            renderTexure.width / 8,
            renderTexure.height / 8, 1);
        
        cb.GetData(floats);

        print("Time Running Shader: " + (Time.realtimeSinceStartup - timeStart));

        foreach (var f in floats)
        {
            print(f);
        }

        Graphics.Blit(renderTexure, destination);

    }

    // Update is called once per frame
    void Update()
    {

    }
}

