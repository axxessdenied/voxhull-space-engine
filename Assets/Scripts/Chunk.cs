using UnityEngine;
using Unity.Mathematics;
using Unity.Netcode;

namespace Voxhull
{
    /*
     * Chunk Class
     * 
     * This will hold the voxels for various objects in the game world.
     * Most likely Ships and various stellar objects.
     * 
     * Unity planes have been redefined in this project
     * Y and Z have been swapped for cinemachine compatibility
     */
    [RequireComponent(typeof(NetworkObject))]
    public class Chunk : NetworkBehaviour
    {
        [SerializeField] private BoxCollider boxCollider;
        public int id;

        public int width = Global.defaultChunkDimensions.x;
        public int depth = Global.defaultChunkDimensions.y;
        public int height = Global.defaultChunkDimensions.z;

        public int3 chunkDimensions;
        
        private void Awake()
        {
            chunkDimensions = new int3(width, depth, height);

        }

        public override void OnNetworkSpawn()
        {
            var size = chunkDimensions;
            boxCollider.isTrigger = true;
            boxCollider.size = new Vector3(size.x - Global.chunkBuffer, size.y - Global.chunkBuffer, size.z - 2);
            boxCollider.center = new Vector3(size.x * 0.5f, size.y * 0.5f, size.z * 0.5f);
        }
    }
}