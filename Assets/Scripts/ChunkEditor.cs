using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Voxhull
{

    public class ChunkEditor : MonoBehaviour
    {
        public GameObject SelectionVoxel;
        public GameObject ChunkToEditGo;
        public Chunk ChunkToEdit;
        public ushort Layer = 0;

        private void Update()
        {
           if (ChunkToEdit != null)
            {
                var size = ChunkToEdit.ChunkDimensions;
                var halfSize = new Chunk.ChunkSize(size.x / 2, size.y, size.z / 2);
                SelectionVoxel.transform.localScale = new Vector3(size.x, 1, size.z);
                SelectionVoxel.transform.position = new Vector3(ChunkToEditGo.transform.position.x + halfSize.x, ChunkToEditGo.transform.position.y + Layer, ChunkToEditGo.transform.position.z + halfSize.z);
                if (!SelectionVoxel.activeInHierarchy) SelectionVoxel.SetActive(true);
            }
        }
    }

}