using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Netcode;
using System.Text;

namespace Voxhull
{
    public class NetworkAuth : MonoBehaviour
    {
        [SerializeField] private string passcode = "room password";
        [SerializeField] private TMP_InputField passwordInputField;
        [SerializeField] private GameObject passwordEntryUI;
        [SerializeField] private GameObject leaveButton;

        public TMP_InputField PasswordInputField => passwordInputField;

        public void StartHost()
        {
            NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCheck;
            NetworkManager.Singleton.StartHost();
        }

        public void StartServer()
        {
            NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCheck;
            NetworkManager.Singleton.StartServer();
        }

        public void StartClient()
        {
            NetworkManager.Singleton.NetworkConfig.ConnectionData = System.Text.Encoding.ASCII.GetBytes("room password");
            NetworkManager.Singleton.StartClient();
        }

        public void Leave()
        {
            NetworkManager.Singleton.Shutdown();
        }

        private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
        {
            // The client identifier to be authenticated
            var clientId = request.ClientNetworkId;

            // Additional connection data defined by user code
            var connectionData = request.Payload;

            var password = Encoding.ASCII.GetString(connectionData);

            var approval = password == passcode;
            // Your approval logic determines the following values
            response.Approved = true;
            response.CreatePlayerObject = true;

            // The prefab hash value of the NetworkPrefab, if null the default NetworkManager player prefab is used
            response.PlayerPrefabHash = null;

            // Position to spawn the player object (if null it uses default of Vector3.zero)
            response.Position = Vector3.zero;

            // Rotation to spawn the player object (if null it uses the default of Quaternion.identity)
            response.Rotation = Quaternion.identity;

            // If additional approval steps are needed, set this to true until the additional steps are complete
            // once it transitions from true to false the connection approval response will be processed.
            response.Pending = false;
        }
    }
}