using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxhull
{

    public class VoxelEngine : MonoBehaviour
    {
        public ChunkEditor ShipBuilder;
        public GameObject SelectionVoxel;
        private World _world = new World();
        private System.Random _random = new System.Random();
        private ushort _count = 0;

        // Start is called before the first frame update
        void Start()
        {
            var chkGo = new GameObject($"Chunk {_count}");
            chkGo.transform.parent = transform.parent;
            var chunk = chkGo.AddComponent<Chunk>();

            _world.Chunks.Add(_count++, chunk);
        }

        // Update is called once per frame
        void Update()
        {
            //do some stuff

            var success = _world.Chunks.TryGetValue((ushort)0, out var chunk);

            if (success)
            {

                var size = chunk.ChunkDimensions;

                ShipBuilder.SelectionVoxel = SelectionVoxel;
                ShipBuilder.ChunkToEditGo = chunk.gameObject;
                ShipBuilder.ChunkToEdit = chunk;


                /* generate random voxels for testing
                var x = _random.Next(0, size.x);
                var y = _random.Next(0, size.y);
                var z = _random.Next(0, size.z);
                var type = _random.Next(0, 2);

                chunk[x, y, z] = (ushort)type;
                */
            }
        }
    }
}