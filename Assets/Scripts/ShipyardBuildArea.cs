using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Chernobog.Studio.Common;
using Unity.Mathematics;

namespace Voxhull
{
    [RequireComponent(typeof(NetworkObject))]
    public class ShipyardBuildArea : NetworkBehaviour
    {
        [SerializeField] private NetworkVariable<int> occupied = new(); //-1 == empty
        [SerializeField] private NetworkVariable<int3> area = new();
        [SerializeField] private BoxCollider boxCollider;
        
        public int Occupied => occupied.Value;
    
        public float X => area.Value.x;
        public float Y => area.Value.y;
        public float Z => area.Value.z;
        public Vector3 AreaAsVector => new Vector3(X, Y, Z);
        
        public override void OnNetworkSpawn()
        {
            occupied.Value = -1;
            if (boxCollider == null) boxCollider = GetComponent<BoxCollider>();
            UpdateTriggerArea();
            area.OnValueChanged += UpdateTriggerArea;

        }

        public void UpdateTriggerArea()
        {
            var tempV = AreaAsVector;
            boxCollider.size = tempV;
            boxCollider.center = tempV / 2;
        }

        public void UpdateTriggerArea(int3 previous, int3 current)
        {
            UpdateTriggerArea();
        }


        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("TriggerStay");
            if (!IsServer && Occupied > -1) return;

            if (other.gameObject.TryGetComponent<Chunk>(out var otherShipChunk)) occupied.Value = otherShipChunk.id;
       
        }

        private void OnTriggerStay(Collider other)
        {
            Debug.Log("TriggerStay");
            if (!IsServer && Occupied > -1) return;

            if (other.gameObject.TryGetComponent<Chunk>(out var otherShipChunk)) occupied.Value = otherShipChunk.id;

        }

        private void OnTriggerExit(Collider other)
        {
            if (!IsServer) return;

            var otherShipChunk = other.gameObject.GetComponent<Chunk>();
            //only change the value to empty if id matches to the occupied id.
            if (otherShipChunk.id != Occupied) return;
            if (otherShipChunk != null) occupied.Value = -1;
            
        }


    }
}