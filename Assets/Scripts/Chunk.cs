using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Voxhull
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent (typeof(MeshRenderer))]
    /*
     * Chunk Class
     * 
     * This will hold the voxels for various objects in the game world.
     * Most likely Ships and various stellar objects.
     * 
     */
    public class Chunk : MonoBehaviour
    {
        public int width = 30;
        public int length = 30;
        public int height = 5;

        public struct ChunkSize
        {
            public int x;
            public int y;
            public int z;

            public ChunkSize(int x, int y, int z)
            {
                this.x = x; this.y = y; this.z = z;
            }

        }

        public ChunkSize ChunkDimensions;
        public ushort[] Voxels => _voxels;

        private Vector3[] _cubeVertices = new[] 
        {
            new Vector3 (0, 0, 0),
            new Vector3 (1, 0, 0),
            new Vector3 (1, 1, 0),
            new Vector3 (0, 1, 0),
            new Vector3 (0, 1, 1),
            new Vector3 (1, 1, 1),
            new Vector3 (1, 0, 1),
            new Vector3 (0, 0, 1),
        };

        private int[] _cubeTriangles = new[] 
        {
            // Front
            0, 2, 1,
            0, 3, 2,
            // Top
            2, 3, 4,
            2, 4, 5,
            // Right
            1, 2, 5,
            1, 5, 6,
            // Left
            0, 7, 4,
            0, 4, 3,
            // Back
            5, 4, 7,
            5, 7, 6,
            // Bottom
            0, 6, 7,
            0, 1, 6
        };

        private static Vector3[] _cubeNormals = new[]
        {
            Vector3.up, Vector3.up, Vector3.up,
            Vector3.up, Vector3.up, Vector3.up,
            Vector3.up, Vector3.up
        };

        private string[] _faceNames = new[]
        {
            "Front", "Front",
            "Top", "Top",
            "Right", "Right",
            "Left", "Left",
            "Back", "Back",
            "Bottom", "Bottom"
        };

        private UInt16[] _voxels;// = new ushort[16 * 16 * 16];
        private MeshFilter _meshFilter;

        public UInt16 this[int x, int y, int z]
        {
            get { return _voxels[x * length * height + y * width + z]; }
            set { _voxels[x * length * height + y * width + z] = value; }
        }

        private void Start()
        {
            ChunkDimensions = new ChunkSize(length, height, width);
            _voxels = new ushort[ChunkDimensions.x * ChunkDimensions.y * ChunkDimensions.z];
            _meshFilter = GetComponent<MeshFilter>();
        }

        private void Update()
        {
            RenderToMesh();
        }

        public void RenderToMesh()
        {
            var vertices = new List<Vector3>();
            var triangles = new List<int>();
            var normals = new List<Vector3>();

            for (var x = 0; x < ChunkDimensions.x; x++)
                for (var y = 0; y < ChunkDimensions.y; y++)
                    for (var z = 0; z < ChunkDimensions.z; z++)
                    {
                        var voxelType = this[x, y, z];
                        
                        if (voxelType == (ushort)0) continue;

                        var pos = new Vector3(x, y, z);
                        var verticesPos = vertices.Count;
                        foreach (var vertice in _cubeVertices)
                            vertices.Add(pos + vertice);

                        foreach (var tri in _cubeTriangles)
                            triangles.Add(verticesPos + tri);

                        foreach (var normal in _cubeNormals)
                            normals.Add(normal);
                    }

            var mesh = new Mesh();
            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles.ToArray(), 0);
            mesh.SetNormals(normals);
            _meshFilter.mesh = mesh;
        }
    }
}