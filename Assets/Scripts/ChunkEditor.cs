using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.Serialization;

namespace Voxhull
{

    public class ChunkEditor : MonoBehaviour
    {
        [SerializeField] private GameObject selectionVoxel;
        [SerializeField] private BoxCollider selectionVoxelCollider;
        [SerializeField] private GameObject chunkToEditGo;
        [SerializeField] private Chunk chunkToEdit;
        [SerializeField] private MeshBuilder meshConstructor;
        [SerializeField] private BoxCollider chunkToEditCollider;
        [SerializeField] private int layer = 0;
        [SerializeField] private float gridsize = 1f;

        public MeshBuilder MeshConstructor
        {
            get => meshConstructor;
            private set => meshConstructor = value;
        }

        public bool selectionVoxelActive => selectionVoxel.activeInHierarchy;
        public Vector3 selectionVoxelPosition => selectionVoxel.transform.position;
        public int Layer => layer;
        private bool _firstUpdate = true;
        
        private void Update()
        {
            if (!_firstUpdate || chunkToEdit == null) return;
            layer = chunkToEdit.ChunkDimensions.z - 1;
            _firstUpdate = false;
        }

        public void MoveSelectionVoxel(Vector3 position)
        {
            selectionVoxel.transform.localScale = new Vector3(1, 1, 1) * gridsize;
            var halfGridSize = gridsize * 0.5f;
            position.x = math.ceil(position.x) + halfGridSize;
            position.y = math.ceil(position.y) + halfGridSize;
            position.z += halfGridSize;
            selectionVoxel.transform.position = position;
        }

        public bool isSelectorOnShip()
        {
            return selectionVoxelCollider.bounds.Intersects(chunkToEditCollider.bounds);
        }

        public void IncreaseLayer()
        {
            if (layer < chunkToEdit.ChunkDimensions.z - 1) layer++;
        }

        public void DecreaseLayer()
        {
            if (layer > 0) layer--;
        }

        public void SetSelectionVoxel(GameObject go)
        {
            selectionVoxel = go;
            selectionVoxelCollider = go.GetComponent<BoxCollider>();
        }

        public void SelectionVoxelActive(bool toggle)
        {
            selectionVoxel.SetActive(toggle);
        }

        public void SetMeshConstructor(MeshBuilder meshBuilder)
        {
            meshConstructor = meshBuilder;
        }

        public void SetChunkToEdit(Chunk chunk)
        {
            chunkToEdit = chunk;
        }

        public void SetChunkToEditGo(GameObject go)
        {
            chunkToEditGo = go;
        }

        public void SetBoundsCollider(BoxCollider collider)
        {
            chunkToEditCollider = collider;
        }
    }

}