using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Voxhull
{


    public class BlockLibrary : MonoBehaviour
    {
        public struct Block : IEquatable<Block>
        {
            public string name;
            public string description;
            public int blockid;

            public Block(int id, string name, string description)
            {
                this.name = name;
                this.description = description;
                blockid = id;
            }

            public bool Equals(Block other)
            {
                return blockid == other.blockid;
            }

            public override int GetHashCode() => HashCode.Combine(name, description, blockid);
        }

        public List<Block> blockList => blocks;
        private List<Block> blocks = new();
        
        private void Start()
        {
            //read this data from a file eventually.
            blocks.Add(new Block(blocks.Count, "Hull", "Ship Hull"));
            blocks.Add(new Block(blocks.Count, "Floor", "Ship Floor"));
            blocks.Add(new Block(blocks.Count, "Scaffolding", "Ship Scaffolding"));
        }

        private void Update()
        {
            
        }
    }
}