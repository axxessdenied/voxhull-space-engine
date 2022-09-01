using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
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
        public int width = 30;  //x
        public int depth = 30;  //z
        public int height = 5;  //y

        public int3 ChunkDimensions;

        /* 
         * converts 3D call to 1D array
         *  
         */
        
       

        private void Awake()
        {
            ChunkDimensions = new int3(width, depth, height);

        }
    }
}