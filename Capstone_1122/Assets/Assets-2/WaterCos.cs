using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCos : MonoBehaviour
{
    // Start is called before the first frame update

    public float speed = 1f;
    public float amplitude = 0.5f;

    Mesh _mesh;
    public MeshCollider _collider;

    private void Start()
    {
        _mesh = GetComponent<MeshFilter>().mesh;
        _collider = GetComponent<MeshCollider>();
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3[] vertices = new Vector3[_mesh.vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertex = _mesh.vertices[i];
            vertex.y = Mathf.Cos(Time.time * speed + _mesh.vertices[i].x + _mesh.vertices[i].y + _mesh.vertices[i].z) * amplitude;
            //vertex.y = Mathf.PerlinNoise(_mesh.vertices[i].x + noiseWalk, _mesh.vertices[i].z + Mathf.Cos(Time.time*0.1f)) * noiseStrength;
            vertices[i] = vertex;
        }
        _mesh.vertices = vertices;
        _mesh.RecalculateNormals();

        _collider.sharedMesh = _mesh;
    }
}
