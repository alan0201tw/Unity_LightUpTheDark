using UnityEngine;
using System;
using System.Linq;

[RequireComponent(typeof(MeshFilter))]
public class Water : MonoBehaviour
{
    [SerializeField]
    private PerlinNoiseLayer[] perlinNoiseLayers;

    [Serializable]
    public struct PerlinNoiseLayer
    {
        public float scale;
        public float speed;
        public float height;
    }

    [SerializeField]
    private bool isEdgeBlend;

    private Mesh m_mesh;
    private Vector3[] m_vertices;

    private void Start()
    {
        m_mesh = GetComponent<MeshFilter>().mesh;
        m_vertices = m_mesh.vertices;
        m_mesh.MarkDynamic();
    }

    private void Update()
    {
        FlattenMesh(m_vertices);
        foreach (PerlinNoiseLayer layer in perlinNoiseLayers)
        {
            AddPerlinNoise(m_vertices, layer, Time.timeSinceLevelLoad);
        }

        m_mesh.SetVertices(m_vertices.ToList());
        m_mesh.RecalculateNormals();
    }

    private static void FlattenMesh(Vector3[] vertices)
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i].y = 0;
        }
    }

    private static void AddPerlinNoise(Vector3[] vertices, PerlinNoiseLayer layer, float time)
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            float x = vertices[i].x * layer.scale + time * layer.speed;
            float z = vertices[i].z * layer.scale + time * layer.speed;

            vertices[i].y = (Mathf.PerlinNoise(x, z) - 0.5f) * layer.height;
        }
    }
}