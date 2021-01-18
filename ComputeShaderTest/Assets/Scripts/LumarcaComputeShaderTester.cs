using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LumarcaComputeShaderTester : MonoBehaviour
{
    List<float> result = new List<float>();

    public int[] tris = new int[0];
    public Vector3[] verts = new Vector3[0]; 
    public Vector3[] normals = new Vector3[0];

    public ComputeShader computeShader;
    
    // Start is called before the first frame update
    void Start()
    {
        List<float> r = GetIntersectListWithComputeShader(Vector3.up);

        foreach (var e in r)
        {
            print(e);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Vector3 inter = new Vector3();

    float maxY = 10;

    List<float> GetIntersectListWithComputeShader(Vector3 vec)
    {
        result.Clear();
        
        //clear results
        float[] resultsArray = new float[tris.Length/3]; 

        //Pass info to compute shader

        int vector3Size = sizeof(float) * 3;
        int intSize = sizeof(int);

        ComputeBuffer cbTris = new ComputeBuffer(tris.Length, vector3Size);
        ComputeBuffer cbVerts = new ComputeBuffer(verts.Length, vector3Size);
        ComputeBuffer cbNorms = new ComputeBuffer(normals.Length, vector3Size);
        ComputeBuffer cbResults = new ComputeBuffer(resultsArray.Length, intSize);
        
        cbTris.SetData(tris);
        cbVerts.SetData(verts);
        cbNorms.SetData(normals);
        cbResults.SetData(resultsArray);
        
        computeShader.SetFloat("numTris", resultsArray.Length/3);
        computeShader.SetFloat("maxY", maxY);
        
        computeShader.SetBuffer(0, "tris", cbTris);
        computeShader.SetBuffer(0, "verts", cbVerts);
        computeShader.SetBuffer(0, "norms", cbNorms);
        computeShader.SetBuffer(0, "results", cbResults);


        //Vector3 normal = normals[vert1];

        //         inter = checkIntersectTri(vec1, vec2, vec3, inter, down, temp, normal);

        //Run compute shader
        computeShader.Dispatch(0, resultsArray.Length/32 + 1, 1, 1);
        cbResults.GetData(resultsArray);

        
        //Loop through results, add the intersections back
        for (int i = 0; i < resultsArray.Length; i++)
        {
            if (resultsArray[i] < maxY)
            {
                result.Add(resultsArray[i]);
            }
        }
        
        cbTris.Dispose();
        cbVerts.Dispose();
        cbNorms.Dispose();
        cbResults.Dispose();

        return result;
    }
}
