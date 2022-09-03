using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Voxhull
{
    public class Ping : NetworkBehaviour
    {
        [SerializeField] private float roundTripTime;
        public float Value => roundTripTime;

        public void UpdateRTT()
        {
            ToServerRpc(Time.time);
        }

        
        [ServerRpc]
        private void ToServerRpc(float ts)
        {
            ToClientRpc(ts);
        }

        [ClientRpc]
        private void ToClientRpc(float ts)
        {
            roundTripTime = Time.time - ts;
        }


    }
}