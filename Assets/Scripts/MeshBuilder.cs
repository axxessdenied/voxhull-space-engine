using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Burst;
using UnityEngine.Rendering;

// web* src = https://gist.github.com/andrew-raphael-lukasik/cbf9d0097c3b4da67b5e0ecb3715e219
namespace Voxhull
{


    public class MeshBuilder : MonoBehaviour
    {
        [SerializeField] private int3 chunkDimensions;
        [SerializeField] private Mesh[] template = new Mesh[6];
        public JobHandle Dependency = default(JobHandle);

        private NativeArray<byte> _voxels;
        //stores data for each face of a voxel
        private NativeArray<int> _templateIndices0, _templateIndices1, _templateIndices2, _templateIndices3, _templateIndices4, _templateIndices5;
        private NativeArray<Vector3> _templateVertices0, _templateVertices1, _templateVertices2, _templateVertices3, _templateVertices4, _templateVertices5;
        private NativeArray<Vector3> _templateNormals0, _templateNormals1, _templateNormals2, _templateNormals3, _templateNormals4, _templateNormals5;
        private NativeArray<Vector2> _templateUVs0, _templateUVs1, _templateUVs2, _templateUVs3, _templateUVs4, _templateUVs5;

        private NativeList<int> _indices;
        private NativeList<Vector3> _vertices, _normals;
        private NativeList<Vector2> _uv;
        private NativeList<VoxelsToBitmasksJob.Entry> _relevantVoxelData;

        private Mesh _mesh = null;
        private bool _voxelsChanged, _jobScheduled;


        private void Awake()
        {
            _mesh = new Mesh();
            _mesh.MarkDynamic();
            GetComponent<MeshFilter>().sharedMesh = _mesh;
            
            _indices = new NativeList<int>(Allocator.Persistent);
            _vertices = new NativeList<Vector3>(Allocator.Persistent);
            _normals = new NativeList<Vector3>(Allocator.Persistent);
            _uv = new NativeList<Vector2>(Allocator.Persistent);
            _relevantVoxelData = new NativeList<VoxelsToBitmasksJob.Entry>(Allocator.Persistent);

        }

        private void UpdateVoxelArraySize()
        {
            _voxels = new NativeArray<byte>(chunkDimensions.x * chunkDimensions.y * chunkDimensions.z,
                Allocator.Persistent);
        }

        private void OnDestroy()
        {
            Dependency.Complete();

            if (_voxels.IsCreated) _voxels.Dispose();
            if (_indices.IsCreated) _indices.Dispose();
            if (_vertices.IsCreated) _vertices.Dispose();
            if (_normals.IsCreated) _normals.Dispose();
            if (_uv.IsCreated) _uv.Dispose();
            if (_relevantVoxelData.IsCreated) _relevantVoxelData.Dispose();

            if (_templateVertices0.IsCreated) _templateVertices0.Dispose();
            if (_templateVertices1.IsCreated) _templateVertices1.Dispose();
            if (_templateVertices2.IsCreated) _templateVertices2.Dispose();
            if (_templateVertices3.IsCreated) _templateVertices3.Dispose();
            if (_templateVertices4.IsCreated) _templateVertices4.Dispose();
            if (_templateVertices5.IsCreated) _templateVertices5.Dispose();

            if (_templateNormals0.IsCreated) _templateNormals0.Dispose();
            if (_templateNormals1.IsCreated) _templateNormals1.Dispose();
            if (_templateNormals2.IsCreated) _templateNormals2.Dispose();
            if (_templateNormals3.IsCreated) _templateNormals3.Dispose();
            if (_templateNormals4.IsCreated) _templateNormals4.Dispose();
            if (_templateNormals5.IsCreated) _templateNormals5.Dispose();

            if (_templateUVs0.IsCreated) _templateUVs0.Dispose();
            if (_templateUVs1.IsCreated) _templateUVs1.Dispose();
            if (_templateUVs2.IsCreated) _templateUVs2.Dispose();
            if (_templateUVs3.IsCreated) _templateUVs3.Dispose();
            if (_templateUVs4.IsCreated) _templateUVs4.Dispose();
            if (_templateUVs5.IsCreated) _templateUVs5.Dispose();

            if (_templateIndices0.IsCreated) _templateIndices0.Dispose();
            if (_templateIndices1.IsCreated) _templateIndices1.Dispose();
            if (_templateIndices2.IsCreated) _templateIndices2.Dispose();
            if (_templateIndices3.IsCreated) _templateIndices3.Dispose();
            if (_templateIndices4.IsCreated) _templateIndices4.Dispose();
            if (_templateIndices5.IsCreated) _templateIndices5.Dispose();

            if (_templateUVs0.IsCreated) _templateUVs0.Dispose();
            if (_templateUVs1.IsCreated) _templateUVs1.Dispose();
            if (_templateUVs2.IsCreated) _templateUVs2.Dispose();
            if (_templateUVs3.IsCreated) _templateUVs3.Dispose();
            if (_templateUVs4.IsCreated) _templateUVs4.Dispose();
            if (_templateUVs5.IsCreated) _templateUVs5.Dispose();

            Destroy(_mesh);
        }
        

        private void Update()
        {
            Dependency.Complete();
            if (_jobScheduled)
            {
                Debug.Log($"new mesh data // indices:{_indices.Length}, vertices:{_vertices.Length}, normals:{_normals.Length}, uv:{_uv.Length} ");

                _mesh.Clear();
                _mesh.SetVertices(_vertices.AsArray());
                _mesh.SetNormals(_normals.AsArray());
                _mesh.SetUVs(0, _uv.AsArray());
                _mesh.indexFormat = _indices.Length > ushort.MaxValue ? IndexFormat.UInt32 : IndexFormat.UInt16;
                _mesh.SetIndices(_indices.AsArray(), MeshTopology.Triangles, 0);

                _jobScheduled = false;
            }

            if (!_voxelsChanged) return;
            _vertices.Clear();
            _indices.Clear();
            _normals.Clear();
            _uv.Clear();
            _relevantVoxelData.Clear();
            var maxCellCount = chunkDimensions.x * chunkDimensions.y * chunkDimensions.z;
            
            _relevantVoxelData.Capacity = maxCellCount;

            var voxelsToBitmasksJob = new VoxelsToBitmasksJob
            {
                Dimensions = chunkDimensions,
                Voxels = _voxels,
                Results = _relevantVoxelData.AsParallelWriter()
            };
            
            var vertJob = new VertJob
            {
                Entries = _relevantVoxelData,
                Vertices = _vertices,
                TemplateVertices0 = _templateVertices0,
                TemplateVertices1 = _templateVertices1,
                TemplateVertices2 = _templateVertices2,
                TemplateVertices3 = _templateVertices3,
                TemplateVertices4 = _templateVertices4,
                TemplateVertices5 = _templateVertices5,
            };
            
            var normJob = new NormalsJob
            {
                Entries = _relevantVoxelData,
                Normals = _normals,
                TemplateNormals0 = _templateNormals0,
                TemplateNormals1 = _templateNormals1,
                TemplateNormals2 = _templateNormals2,
                TemplateNormals3 = _templateNormals3,
                TemplateNormals4 = _templateNormals4,
                TemplateNormals5 = _templateNormals5,
            };
            
            var uvJob = new UVJob
            {
                Entries = _relevantVoxelData,
                UV = _uv,
                TemplateUVs0 = _templateUVs0,
                TemplateUVs1 = _templateUVs1,
                TemplateUVs2 = _templateUVs2,
                TemplateUVs3 = _templateUVs3,
                TemplateUVs4 = _templateUVs4,
                TemplateUVs5 = _templateUVs5,
            };
            
            var indicesJob = new IndicesJob
            {
                Entries = _relevantVoxelData,
                Indices = _indices,
                TemplateIndices0 = _templateIndices0,
                TemplateIndices1 = _templateIndices1,
                TemplateIndices2 = _templateIndices2,
                TemplateIndices3 = _templateIndices3,
                TemplateIndices4 = _templateIndices4,
                TemplateIndices5 = _templateIndices5,
                TemplateVertices0Length = _templateVertices0.Length,
                TemplateVertices1Length = _templateVertices1.Length,
                TemplateVertices2Length = _templateVertices2.Length,
                TemplateVertices3Length = _templateVertices3.Length,
                TemplateVertices4Length = _templateVertices4.Length,
                TemplateVertices5Length = _templateVertices5.Length,
            };

            Dependency = voxelsToBitmasksJob.Schedule(_voxels.Length, chunkDimensions.x * chunkDimensions.y, Dependency);
            var parallelJobs = new NativeArray<JobHandle>(4, Allocator.Temp);
            parallelJobs[0] = vertJob.Schedule(Dependency);
            parallelJobs[1] = normJob.Schedule(Dependency);
            parallelJobs[2] = uvJob.Schedule(Dependency);
            parallelJobs[3] = indicesJob.Schedule(Dependency);
            Dependency = JobHandle.CombineDependencies(parallelJobs);

            _voxelsChanged = false;
            _jobScheduled = true;
        }

        private void InitTemplate()
        {
            _templateVertices0 = new NativeArray<Vector3>(template[0].vertices, Allocator.Persistent);// x-
            _templateVertices1 = new NativeArray<Vector3>(template[1].vertices, Allocator.Persistent);// x+
            _templateVertices2 = new NativeArray<Vector3>(template[2].vertices, Allocator.Persistent);// y-
            _templateVertices3 = new NativeArray<Vector3>(template[3].vertices, Allocator.Persistent);// y+
            _templateVertices4 = new NativeArray<Vector3>(template[4].vertices, Allocator.Persistent);// z-
            _templateVertices5 = new NativeArray<Vector3>(template[5].vertices, Allocator.Persistent);// z+

            _templateNormals0 = new NativeArray<Vector3>(template[0].normals, Allocator.Persistent);// x-
            _templateNormals1 = new NativeArray<Vector3>(template[1].normals, Allocator.Persistent);// x+
            _templateNormals2 = new NativeArray<Vector3>(template[2].normals, Allocator.Persistent);// y-
            _templateNormals3 = new NativeArray<Vector3>(template[3].normals, Allocator.Persistent);// y+
            _templateNormals4 = new NativeArray<Vector3>(template[4].normals, Allocator.Persistent);// z-
            _templateNormals5 = new NativeArray<Vector3>(template[5].normals, Allocator.Persistent);// z+

            _templateUVs0 = new NativeArray<Vector2>(template[0].uv, Allocator.Persistent);// x-
            _templateUVs1 = new NativeArray<Vector2>(template[1].uv, Allocator.Persistent);// x+
            _templateUVs2 = new NativeArray<Vector2>(template[2].uv, Allocator.Persistent);// y-
            _templateUVs3 = new NativeArray<Vector2>(template[3].uv, Allocator.Persistent);// y+
            _templateUVs4 = new NativeArray<Vector2>(template[4].uv, Allocator.Persistent);// z-
            _templateUVs5 = new NativeArray<Vector2>(template[5].uv, Allocator.Persistent);// z+

            _templateIndices0 = new NativeArray<int>(template[0].triangles, Allocator.Persistent);// x-
            _templateIndices1 = new NativeArray<int>(template[1].triangles, Allocator.Persistent);// x+
            _templateIndices2 = new NativeArray<int>(template[2].triangles, Allocator.Persistent);// y-
            _templateIndices3 = new NativeArray<int>(template[3].triangles, Allocator.Persistent);// y+
            _templateIndices4 = new NativeArray<int>(template[4].triangles, Allocator.Persistent);// z-
            _templateIndices5 = new NativeArray<int>(template[5].triangles, Allocator.Persistent);// z+
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            float3 cellSize = Vector3.one;
            Gizmos.matrix = transform.localToWorldMatrix;
            if (!Application.isPlaying)
            {
                var len = chunkDimensions.x * chunkDimensions.y * chunkDimensions.z;
                var positionsNative = new NativeList<float3>(len, Allocator.TempJob);
                var job = new OnDrawGizmosJob
                {
                    dimensions = chunkDimensions,
                    voxels = _voxels,
                    Results = positionsNative.AsParallelWriter(),
                };
                job.Schedule(len, chunkDimensions.x * chunkDimensions.y).Complete();
                var positions = positionsNative.ToArray();
                positionsNative.Dispose();

                Gizmos.color = Color.black;
                foreach (var point in positions)
                    Gizmos.DrawCube(point, cellSize);
            }
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube((float3)chunkDimensions * 0.5f, chunkDimensions * cellSize);
        }

        [BurstCompile]
        private struct OnDrawGizmosJob : IJobParallelFor
        {
            public int3 dimensions;
            public NativeArray<byte> voxels;
            [WriteOnly] public NativeList<float3>.ParallelWriter Results;

            void IJobParallelFor.Execute(int i)
            {
                var coords = Utilities.IndexToCoords(i, dimensions);
                
                foreach(var voxel in voxels)
                    if (voxel != 0)
                    {
                        var cellCenter = coords + new float3 { x = 0.5f, y = 0.5f, z = 0.5f };
                        Results.AddNoResize(cellCenter);
                    }
            }
        }
#endif
        /*
         * create a bitmask where air = true
         */
        [BurstCompile]
        public struct VoxelsToBitmasksJob : IJobParallelFor
        {
            [ReadOnly] public int3 Dimensions;
            [ReadOnly] public NativeArray<byte> Voxels;
            [WriteOnly] public NativeList<Entry>.ParallelWriter Results;
            void IJobParallelFor.Execute(int index)
            {
                if (Voxels[index] != 0) return;
                var cellCoords = Utilities.IndexToCoords(index, Dimensions);
                var bitmask = 0;
                for (byte direction = 0; direction < 6; direction++)
                {
                    var neighbourCoords = cellCoords + Utilities.Offset(direction);
                    if (!math.any(neighbourCoords < 0 | neighbourCoords >= Dimensions))// index bounds test
                        if (Voxels[Utilities.CoordsToIndex(neighbourCoords, Dimensions)] != 0)
                            bitmask |= 1 << direction;
                }
                if (bitmask != 0)// ignore cells neighbouring empty space only
                {
                    Results.AddNoResize(new Entry
                    {
                        Bitmask = bitmask,
                        Coords = cellCoords
                    });
                }
            }
            public struct Entry
            {
                public int Bitmask;
                public int3 Coords;
            }
        }

        [BurstCompile]
        public struct IndicesJob : IJob
        {
            [WriteOnly] public NativeList<int> Indices;
            [ReadOnly] public NativeList<VoxelsToBitmasksJob.Entry> Entries;
            [ReadOnly] public NativeArray<int> TemplateIndices0, TemplateIndices1, TemplateIndices2, TemplateIndices3, TemplateIndices4, TemplateIndices5;
            [ReadOnly] public int TemplateVertices0Length, TemplateVertices1Length, TemplateVertices2Length, TemplateVertices3Length, TemplateVertices4Length, TemplateVertices5Length;
            void IJob.Execute()
            {
                var baseIndex = 0;
                foreach (var next in Entries.AsArray())
                {
                    var bitmask = next.Bitmask;
                    for (byte direction = 0; direction < 6; direction++)
                        if ((bitmask & 1 << direction) == 1 << direction)// the same as Voxels[neighbourIndex]!=0, but read from bitmask, so it's faster and you can test many directions at once
                            switch (direction)
                            {
                                case 0: foreach (var index in TemplateIndices0) Indices.Add(baseIndex + index); baseIndex += TemplateVertices0Length; break;// x-
                                case 1: foreach (var index in TemplateIndices1) Indices.Add(baseIndex + index); baseIndex += TemplateVertices1Length; break;// x+
                                case 2: foreach (var index in TemplateIndices2) Indices.Add(baseIndex + index); baseIndex += TemplateVertices2Length; break;// y-
                                case 3: foreach (var index in TemplateIndices3) Indices.Add(baseIndex + index); baseIndex += TemplateVertices3Length; break;// y+
                                case 4: foreach (var index in TemplateIndices4) Indices.Add(baseIndex + index); baseIndex += TemplateVertices4Length; break;// z-
                                case 5: foreach (var index in TemplateIndices5) Indices.Add(baseIndex + index); baseIndex += TemplateVertices5Length; break;// z+
                            }
                }
            }
        }

        [BurstCompile]
        public struct VertJob : IJob
        {
            [WriteOnly] public NativeList<Vector3> Vertices;
            [ReadOnly] public NativeList<VoxelsToBitmasksJob.Entry> Entries;
            [ReadOnly] public NativeArray<Vector3> TemplateVertices0, TemplateVertices1, TemplateVertices2, TemplateVertices3, TemplateVertices4, TemplateVertices5;
            void IJob.Execute()
            {
                foreach (var next in Entries.AsArray())
                {
                    var cellCoords = next.Coords;
                    var bitmask = next.Bitmask;
                    var cellCenter = cellCoords + new float3 { x = 0.5f, y = 0.5f, z = 0.5f };
                    for (byte direction = 0; direction < 6; direction++)
                        if ((bitmask & 1 << direction) == 1 << direction)// the same as Voxels[neighbourIndex]!=0, but read from bitmask, so it's faster and you can test many directions at once
                            switch (direction)
                            {
                                case 0: foreach (var vert in TemplateVertices0) Vertices.Add(cellCenter + (float3)vert); break;// x-
                                case 1: foreach (var vert in TemplateVertices1) Vertices.Add(cellCenter + (float3)vert); break;// x+
                                case 2: foreach (var vert in TemplateVertices2) Vertices.Add(cellCenter + (float3)vert); break;// y-
                                case 3: foreach (var vert in TemplateVertices3) Vertices.Add(cellCenter + (float3)vert); break;// y+
                                case 4: foreach (var vert in TemplateVertices4) Vertices.Add(cellCenter + (float3)vert); break;// z-
                                case 5: foreach (var vert in TemplateVertices5) Vertices.Add(cellCenter + (float3)vert); break;// z+
                            }
                }
            }
        }

        [BurstCompile]
        public struct NormalsJob : IJob
        {
            [WriteOnly] public NativeList<Vector3> Normals;
            [ReadOnly] public NativeList<VoxelsToBitmasksJob.Entry> Entries;
            [ReadOnly] public NativeArray<Vector3> TemplateNormals0, TemplateNormals1, TemplateNormals2, TemplateNormals3, TemplateNormals4, TemplateNormals5;
            void IJob.Execute()
            {
                foreach (var next in Entries.AsArray())
                {
                    var bitmask = next.Bitmask;
                    for (byte direction = 0; direction < 6; direction++)
                        if ((bitmask & 1 << direction) == 1 << direction)// the same as Voxels[neighbourIndex]!=0, but read from bitmask, so it's faster and you can test many directions at once
                            switch (direction)
                            {
                                case 0: Normals.AddRange(TemplateNormals0); break;// x-
                                case 1: Normals.AddRange(TemplateNormals1); break;// x+
                                case 2: Normals.AddRange(TemplateNormals2); break;// y-
                                case 3: Normals.AddRange(TemplateNormals3); break;// y+
                                case 4: Normals.AddRange(TemplateNormals4); break;// z-
                                case 5: Normals.AddRange(TemplateNormals5); break;// z+
                            }
                }
            }
        }

        [BurstCompile]
        public struct UVJob : IJob
        {
            [WriteOnly] public NativeList<Vector2> UV;
            [ReadOnly] public NativeList<VoxelsToBitmasksJob.Entry> Entries;
            [ReadOnly] public NativeArray<Vector2> TemplateUVs0, TemplateUVs1, TemplateUVs2, TemplateUVs3, TemplateUVs4, TemplateUVs5;
            void IJob.Execute()
            {
                foreach (var next in Entries.AsArray())
                {
                    var bitmask = next.Bitmask;
                    for (byte direction = 0; direction < 6; direction++)
                        if ((bitmask & 1 << direction) == 1 << direction)// the same as Voxels[neighbourIndex]!=0, but read from bitmask, so it's faster and you can test many directions at once
                            switch (direction)
                            {
                                case 0: UV.AddRange(TemplateUVs0); break;// x-
                                case 1: UV.AddRange(TemplateUVs1); break;// x+
                                case 2: UV.AddRange(TemplateUVs2); break;// y-
                                case 3: UV.AddRange(TemplateUVs3); break;// y+
                                case 4: UV.AddRange(TemplateUVs4); break;// z-
                                case 5: UV.AddRange(TemplateUVs5); break;// z+
                            }

                }
            }
        }

        public void AddVoxel(int x, int y, int z, byte type)
        {
            Dependency.Complete();
            _voxels[Utilities.CoordsToIndex(x, y, z, chunkDimensions)] = type;
            _voxelsChanged = true;
        }
        
        public void AddVoxel(int3 coords, byte type)
        {
            AddVoxel(coords.x, coords.y, coords.z, type);
        }

        public void AddVoxels(int3[] coords, byte[] type)
        {
            var count = 0;
            foreach(var coord in coords)
                AddVoxel(coord, type[count++]);
        }

        public void SetMeshTemplate(Mesh[] meshTemplates)
        {
            template = meshTemplates;
            InitTemplate();
        }

        public void SetChunkDimensions(int x, int y, int z)
        {
            chunkDimensions.x = x;
            chunkDimensions.y = y;
            chunkDimensions.z = z;
            UpdateVoxelArraySize();
        }

        public void SetChunkDimensions(int3 dimensions)
        {
            chunkDimensions = new int3(dimensions.x, dimensions.y, dimensions.z);
            UpdateVoxelArraySize();
        }

        public void Construction(int3 dimensions, Mesh[] meshTemplates)
        {
            SetMeshTemplate(meshTemplates);
            SetChunkDimensions(dimensions);
        }
        public struct Utilities
        {
            public static int3 Offset(int direction)
            {
                return direction switch
                {
                    0 => new int3 { x = -1 },// x-
                    1 => new int3 { x = +1 },// x+
                    2 => new int3 { y = -1 },// y-
                    3 => new int3 { y = +1 },// y+
                    4 => new int3 { z = -1 },// z-
                    5 => new int3 { z = +1 },// z+
                    _ => throw new System.ArgumentOutOfRangeException(),
                };
            }

            public static int CoordsToIndex(int x, int y, int z, int3 dimensions) => z * dimensions.x * dimensions.y + y * dimensions.x + x;

            public static int CoordsToIndex(int3 coords, int3 dimensions) => CoordsToIndex(coords.x, coords.y, coords.z, dimensions);
            
            public static int3 IndexToCoords(int i, int3 dimensions)
            {
                var numSlices = dimensions.x * dimensions.y;
                var z = i / numSlices;
                var iLayer = i % numSlices;
                var y = iLayer / dimensions.x;
                var x = iLayer % dimensions.x;
                return new int3 { x = x, y = y, z = z };
            }
            public void IndexToCoords(int i, int3 dimensions, out int x, out int y, out int z)
            {
                var coords = IndexToCoords(i, dimensions);
                x = coords.x;
                y = coords.y;
                z = coords.z;
            }
        }
    }
}