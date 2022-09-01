using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using Unity.Mathematics;
using UnityEngine.Serialization;

namespace Voxhull
{
    /*
     * VoxelEngine
     * 
     * Helps manage our voxels
     * 
     * Unity planes have been redefined in this project
     * Y and Z have been swapped for cinemachine compatibility
    */

    public class VoxelEngine : MonoBehaviour
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
        public ChunkEditor shipBuilder;
        public BlockLibrary blockLibraryMain;
        public EngineMode Mode { get; private set; } = EngineMode.Passive;

        private World _world = new();
        private ushort _count;

        private bool _firstUpdate = true;


        // Start is called before the first frame update
        private void Start()
        {
            
        }

        
        
        // Update is called once per frame
        private void Update()
        {
            if (_firstUpdate)
            {
                //generate a chunk to store our voxels and register some stuff
                var chkGo = new GameObject($"Chunk {_count}")
                {
                    transform =
                    {
                        parent = transform.parent
                    }
                };
                var chunk = chkGo.AddComponent<Chunk>();
                var boxCollider = chkGo.AddComponent<BoxCollider>();
                var meshRenderer = chkGo.AddComponent<MeshRenderer>();
                chkGo.AddComponent<MeshFilter>();
                var meshBuilder = chkGo.AddComponent<MeshBuilder>();
                meshBuilder.Construction(chunk.ChunkDimensions, template);
                var size = chunk.ChunkDimensions;
                boxCollider.isTrigger = true;
                boxCollider.size = new Vector3(size.x, size.y, size.z);
                boxCollider.center = new Vector3(size.x * 0.5f, size.y * 0.5f, size.z * 0.5f);

                _world.Chunks.Add(_count++, chunk);
                meshRenderer.material = tempMat;

                shipBuilder.SetChunkToEditGo(chunk.gameObject);
                shipBuilder.SetChunkToEdit(chunk);
                shipBuilder.SetBoundsCollider(boxCollider);
                shipBuilder.SetMeshConstructor(meshBuilder);
                shipBuilder.SetSelectionVoxel(selectionVoxel);
                
                playerCamera.Follow = chunk.transform;
                offset.m_Offset = new Vector3(size.x * 0.5f, size.y * 0.5f, -size.x);
                
                _firstUpdate = false;
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
                        var x = (int)position.x;
                        var y = (int)position.y;
                        var z = (int)position.z;
                        
                        shipBuilder.MeshConstructor.AddVoxel(x, y, z, 1);
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
            Debug.Log($"Zoom. context: {context.ReadValue<float>()}");
            if (!context.started) return;
            if (context.ReadValue<float>() > 0)
                shipBuilder.DecreaseLayer();
            else if (context.ReadValue<float>() < 0)
                shipBuilder.IncreaseLayer();
                
        }

    }
}