using UnityEngine;
using Unity.Mathematics;

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
    public class Chunk : MonoBehaviour
    {
        public int width = Global.defaultChunkDimensions.x;
        public int depth = Global.defaultChunkDimensions.y;
        public int height = Global.defaultChunkDimensions.z;

        public int3 chunkDimensions;
        
        private void Awake()
        {
            chunkDimensions = new int3(width, depth, height);

        }
    }
}