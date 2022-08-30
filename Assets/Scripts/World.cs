using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Voxhull
{
    /*
     * World class
     * 
     * Will help manage the chunk objects in the scene.
     * 
     */
    public class World
    {
        public Dictionary<UInt16, Chunk> Chunks = new Dictionary<UInt16, Chunk>();
    }
}
