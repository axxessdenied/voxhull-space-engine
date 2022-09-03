using UnityEngine;
using Unity.Mathematics;
using UnityEngine.Serialization;

namespace Voxhull
{

    public class ChunkEditor : MonoBehaviour
    {
        [SerializeField] private GameObject selectionVoxel;
        [SerializeField] private BoxCollider selectionVoxelCollider;
        [SerializeField] private Chunk chunkToEdit;
        [SerializeField] private Transform chunkToEditTransform;
        [SerializeField] private MeshBuilder meshConstructor;
        [SerializeField] private BoxCollider chunkToEditCollider;
        [SerializeField] private BlockLibrary blockLibrary;
        
        [SerializeField] private int layer = 1;
        [SerializeField] private float gridSize = 1f;
        [SerializeField] private byte brushType;

        public MeshBuilder MeshConstructor => meshConstructor;
        public Vector3 ChunkPosition => chunkToEditTransform.position;

        public bool selectionVoxelActive => selectionVoxel.activeInHierarchy;
        public Vector3 selectionVoxelPosition => selectionVoxel.transform.position;
        public int Layer => layer;
        private bool _firstUpdate = true;
        
        private void Update()
        {
            if (!_firstUpdate || chunkToEdit == null) return;
            layer = chunkToEdit.chunkDimensions.z - 2;
            _firstUpdate = false;
        }

        public void MoveSelectionVoxel(Vector3 position)
        {
            selectionVoxel.transform.localScale = new Vector3(1, 1, 1) * gridSize;
            var halfGridSize = gridSize * 0.5f;
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
            if (layer < chunkToEdit.chunkDimensions.z - 2) layer++;
        }

        public void DecreaseLayer()
        {
            if (layer > 1) layer--;
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
            chunkToEditTransform = chunk.transform;
        }

        public void SetBoundsCollider(BoxCollider newCollider)
        {
            chunkToEditCollider = newCollider;
        }

        public void SetBlockLibrary(BlockLibrary newBlockLibrary)
        {
            brushType = 0;
            blockLibrary = newBlockLibrary;
        }

        public void SetBrushType(byte newBrush)
        {
            brushType = newBrush;
        }

        public byte BrushType => brushType;

        public void IncreaseBrushType()
        {
            if (blockLibrary.blockList.Count - 1 > brushType) brushType++;
        }

        public void DecreaseBrushType()
        {
            if (brushType > 0) brushType--;
        }
    }

}