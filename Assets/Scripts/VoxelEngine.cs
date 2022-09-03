using System;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEditor.PackageManager;

namespace Voxhull
{
    /*
     * VoxelEngine
     * 
     * Helps manage our voxels
     * 
    */

    public class VoxelEngine : NetworkBehaviour
    {
        public enum EngineMode
        {
            Passive,
            Command,
            Engineering,
            Logistics
        }

        [SerializeField] private GameObject selectionVoxel;
        [SerializeField] private Mesh[] template = new Mesh[6];
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Material tempMat;
        [SerializeField] private CinemachineVirtualCamera playerCamera;
        [SerializeField] private CinemachineCameraOffset offset;
        [SerializeField] private FileHandler fileHandler;
        //[SerializeField] private PlayerData playerData;
        [SerializeField] private ChunkEditor shipBuilder;
        [SerializeField] private BlockLibrary blockLibrary;
        [SerializeField] private GameObject prefab;
        private EngineMode Mode = EngineMode.Passive;

        private World _world = new();
        private ushort _count;

        private bool _firstUpdate = true;

        public override void OnNetworkSpawn()
        {
            //init();
        }

        private void Start()
        {
            init();
        }

        private void init()
        {
            mainCamera = mainCamera != null ? mainCamera : Camera.main;
            playerCamera = playerCamera != null ? playerCamera : EasyRef.Instance.PlayerCamera;
            offset = offset != null ? offset : EasyRef.Instance.CameraOffset;

            RequestSpawnServerRpc();
        }

        private void SpawnShipChunk()
        {
            var go = Instantiate(prefab).GetComponent<NetworkObject>();
           
            go.Spawn();
            UpdateShipBuilderClientRpc(go);
        }

        [ServerRpc]
        private void RequestSpawnServerRpc()
        {
            SpawnShipChunk();
        }

        [ClientRpc]
        private void UpdateShipBuilderClientRpc(NetworkObjectReference no)
        {
            no.TryGet(out var go);
            
            Debug.Log("UpdateShipBuilder go name is : " + go.name);
            
            var chunk = go.GetComponent<Chunk>();
            shipBuilder.SetChunkToEdit(chunk);
            shipBuilder.SetBoundsCollider(go.GetComponent<BoxCollider>());
            var mb = go.GetComponent<MeshBuilder>();
            var size = chunk.chunkDimensions;
            mb.SetChunkDimensions(size);
            shipBuilder.SetMeshConstructor(mb);
            shipBuilder.SetSelectionVoxel(selectionVoxel);
            shipBuilder.SetBlockLibrary(blockLibrary);
            
            playerCamera.Follow = go.transform;
            offset.m_Offset = new Vector3(size.x / 2, size.y / 2, -math.max(size.x, size.y));
        }


        // Update is called once per frame
        private void Update()
        {
            if (_firstUpdate)
            {
                //FirstUpdate();
            }

            //using new InputSystem
            var mousePos = Mouse.current.position.ReadValue();
            var mouseWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, math.abs(offset.m_Offset.z)));


            switch (Mode)
            {
                case EngineMode.Passive:
                    break;
                /*-------------------------*/
                case EngineMode.Engineering:
                    if (selectionVoxel == null) return;
                    shipBuilder.MoveSelectionVoxel(new Vector3(mouseWorldPos.x, mouseWorldPos.y, shipBuilder.Layer));
                    
                    shipBuilder.SelectionVoxelActive(shipBuilder.isSelectorOnShip());

                    break;
                /*-------------------------*/
                case EngineMode.Logistics:
                    break;
                /*-------------------------*/
                case EngineMode.Command:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
   
        }

        private void FirstUpdate()
        {
            //generate a chunk to store our voxels and register some stuff
            var chkGo = new GameObject($"Chunk {_count}")
            {
                transform =
                    {
                        parent = transform.parent
                    }
            };

            //playerCamera.Follow = chunk.transform;
            //offset.m_Offset = new Vector3(size.x * 0.5f, size.y * 0.5f, -size.x);

            _firstUpdate = false;
        }

        /*-------------INPUT PROCESSING---------------------------*/

        public void CameraLook(InputAction.CallbackContext context)
        {
            //Debug.Log("move camera");
        }

        
        
        public void FireButton(InputAction.CallbackContext context)
        {
            switch(Mode)
            {
                case EngineMode.Passive:
                    break;
                /*-------------------------*/
                case EngineMode.Engineering:
                    if (shipBuilder.selectionVoxelActive && context.performed)
                    {
                        var position = shipBuilder.selectionVoxelPosition;
                        var position2 = math.ceil(shipBuilder.ChunkPosition) ;
                        var x = (int)(position.x - position2.x);
                        var y = (int)(position.y - position2.y);
                        var z = (int)(position.z - position2.z);

                        shipBuilder.MeshConstructor.AddVoxel(x, y, z, shipBuilder.BrushType) ;
                    }
                    break;
                /*-------------------------*/
                case EngineMode.Logistics:
                    break;
                /*-------------------------*/
                case EngineMode.Command:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        

        public void PassiveMode(InputAction.CallbackContext context)
        {
            Debug.Log("Passive Mode entered");
            Mode = EngineMode.Passive;
            shipBuilder.SelectionVoxelActive(false);
        }
        
        public void EngineeringMode(InputAction.CallbackContext context)
        {
            Debug.Log("Engineering Mode entered");
            Mode = EngineMode.Engineering;
        }

        public void CommandMode(InputAction.CallbackContext context)
        {
            Debug.Log("Command Mode entered");
            Mode = EngineMode.Command;
            shipBuilder.SelectionVoxelActive(false);
        }

        public void LogisticsMode(InputAction.CallbackContext context)
        {
            Debug.Log("Logistics Mode entered");
            Mode = EngineMode.Logistics;
            shipBuilder.SelectionVoxelActive(false);
        }

        public void Zoom(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            if (context.ReadValue<float>() > 0)
                shipBuilder.DecreaseLayer();
            else if (context.ReadValue<float>() < 0)
                shipBuilder.IncreaseLayer();
                
        }

        public void BrushTypeSelector(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            if (context.ReadValue<float>() > 0)
                shipBuilder.IncreaseBrushType();
            else if (context.ReadValue<float>() < 0)
                shipBuilder.DecreaseBrushType();
        }

    }
}