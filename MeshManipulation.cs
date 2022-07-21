using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

public class MeshManipulation : MonoBehaviour
{
    public MeshFilter mesh;
    public Vector3[] newVerts;
    public int i = 0;
    [Range(0.1f,1f)]
    public float amplitude;

    public void Awake()
    {
        newVerts = mesh.mesh.vertices;
    }
    public void Update()
    {
       
        newVerts[i] = mesh.mesh.vertices[i] = new Vector3(mesh.mesh.vertices[i].x, Mathf.Sin(UnityEngine.Random.Range(0,i))*amplitude, mesh.mesh.vertices[i].z);
        mesh.mesh.vertices = newVerts;
        mesh.mesh.RecalculateNormals();
        i++;
        if (i >= mesh.mesh.vertexCount)
            i = 0;
        
    }
}
