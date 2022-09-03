using System;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using TMPro;

namespace Voxhull
{
    [RequireComponent(typeof(NetworkManager))]
    [DisallowMultipleComponent]
    public class NetworkManagerHud : MonoBehaviour
    {
        [SerializeField] private NetworkManager m_NetworkManager;
        [SerializeField] private NetworkAuth networkAuth;
        UnityTransport m_Transport;

        [SerializeField] private GUIStyle m_LabelTextStyle;

        // This is needed to make the port field more convenient. GUILayout.TextField is very limited and we want to be able to clear the field entirely so we can't cache this as ushort.
        [SerializeField] private string m_PortString = "6969";
        [SerializeField] private string m_ConnectAddress = "127.0.0.1";

        [SerializeField] private Vector2 drawOffset = new Vector2(10, 10);

        [SerializeField] private Color labelColor = Color.black;

        void Awake()
        {
            // Only cache networking manager but not transport here because transport could change anytime.
            m_NetworkManager ??= GetComponent<NetworkManager>();
            networkAuth ??= GetComponent<NetworkAuth>();
            m_LabelTextStyle ??= new GUIStyle(GUIStyle.none);
        }

        void OnGUI()
        {
            m_LabelTextStyle.normal.textColor = labelColor;

            m_Transport = (UnityTransport)m_NetworkManager.NetworkConfig.NetworkTransport;

            GUILayout.BeginArea(new Rect(drawOffset, new Vector2(200, 200)));

            if (IsRunning(m_NetworkManager))
            {
                DrawStatusGUI();
            }
            else
            {
                DrawConnectGUI();
            }

            GUILayout.EndArea();
        }

        void DrawConnectGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            GUILayout.Label("Address", m_LabelTextStyle);
            GUILayout.Label("Port", m_LabelTextStyle);

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            m_ConnectAddress = GUILayout.TextField(m_ConnectAddress);
            m_PortString = GUILayout.TextField(m_PortString);
            if (ushort.TryParse(m_PortString, out ushort port))
            {
                m_Transport.SetConnectionData(m_ConnectAddress, port);
            }
            else
            {
                m_Transport.SetConnectionData(m_ConnectAddress, 7777);
            }

            GUILayout.EndHorizontal();

            if (GUILayout.Button("Host (Server + Client)"))
            {
                networkAuth.StartHost();
            }

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Server"))
            {
                networkAuth.StartServer();
            }

            if (GUILayout.Button("Client"))
            {
                networkAuth.StartClient();
            }

            GUILayout.EndHorizontal();
        }

        void DrawStatusGUI()
        {
            if (m_NetworkManager.IsServer)
            {
                var mode = m_NetworkManager.IsHost ? "Host" : "Server";
                GUILayout.Label($"{mode} active on port: {m_Transport.ConnectionData.Port.ToString()}", m_LabelTextStyle);
            }
            else
            {
                if (m_NetworkManager.IsConnectedClient)
                {
                    GUILayout.Label($"Client connected {m_Transport.ConnectionData.Address}:{m_Transport.ConnectionData.Port.ToString()}", m_LabelTextStyle);
                }
            }

            if (GUILayout.Button("Shutdown"))
            {
                networkAuth.Leave();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        bool IsRunning(NetworkManager networkManager) => networkManager.IsServer || networkManager.IsClient;
    }
}