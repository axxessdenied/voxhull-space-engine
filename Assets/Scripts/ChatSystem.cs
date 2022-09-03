using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Mathematics;
using System;

namespace Voxhull
{
    public class ChatSystem : NetworkBehaviour
    {
        public struct Message
        {
            public System.DateTime time;
            public string message;
            public ulong sender;

            public Message(System.DateTime dateTime, string message, ulong senderId)
            {
                time = dateTime;
                this.message = message;
                sender = senderId;
            }

        }

        private List<Message> messageLog = new();
        [SerializeField] private string formattedMessageLog;
        [SerializeField] private string messageToSend = "<Enter here>";

        [SerializeField] private GUIStyle labelTextStyle;
        [SerializeField] private Vector2 drawOffset = new(10, 100);
        [SerializeField] private Vector2 chatSize = new(400, 500);

        [SerializeField] private Color labelColor = Color.black;
        [SerializeField] private int chatLines = 5;

        [SerializeField] private bool showChat = true;

        private ulong id;

        private void Awake()
        {
            labelTextStyle ??= new GUIStyle(GUIStyle.none);
        }

        public override void OnNetworkSpawn()
        {
            id = GetComponent<NetworkObject>().NetworkObjectId;
        }

        private void OnGUI()
        {
            var position = new Vector2(Screen.width - chatSize.x, chatSize.y) - drawOffset;
            GUILayout.BeginArea(new Rect(position, chatSize));
            if (IsClient || IsHost || IsServer)
                if (showChat)
                {
                    labelTextStyle.normal.textColor = labelColor;

                    if (IsServer)
                    {
                        DrawServerChat();
                    }
                    else
                    {
                        DrawClientChat();
                    }
                }
                else
                { /* draw button to display chat */ }
            
            GUILayout.EndArea();
        }

        private void DrawServerChat()
        {
            DrawClientChat();
        }

        private void DrawClientChat()
        {
            
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
      
            GUILayout.Label("Chat", labelTextStyle);

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            GUILayout.TextArea(formattedMessageLog);

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            messageToSend = GUILayout.TextField(messageToSend);

            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Send Message"))
            {
                SendMessage();
            }
            GUILayout.EndHorizontal();

        }

        private void SendToClientRpcWithLog(DateTime dateTime, string message, ulong senderId)
        {
                messageLog.Add(new Message(dateTime, message, senderId));
                FormatMessageLog();
                SendToClientRpc(dateTime, message, senderId);
        }

        private void SendMessage()
        {
            var time = DateTime.Now;
            if (NetworkManager.IsServer) SendToClientRpcWithLog(time, messageToSend, id);
            else SendToServerRpc(messageToSend, id);
        }

        [ServerRpc(RequireOwnership = false)]
        private void SendToServerRpc(string message, ulong senderId)
        {
            Debug.Log("SendToServerRpc");
            if (NetworkManager.IsServer) SendToClientRpcWithLog(DateTime.Now, message, id);
            else SendToClientRpc(DateTime.Now, message, senderId);
        }

        [ClientRpc]
        private void SendToClientRpc(DateTime dateTime, string message, ulong senderId)
        {
            Debug.Log("sendToClientRPC");
            messageLog.Add(new Message(dateTime, message, senderId));

            FormatMessageLog();
        }

        private void FormatMessageLog()
        {
            var newMessageLog = "";

            var len = messageLog.Count;
            var start = math.max(len - chatLines, 0);
            for (var i = start; i < len - 1; i++)
                newMessageLog += $"{messageLog[i].time} - < {messageLog[i].sender} > : {messageLog[i].message}\n";

            formattedMessageLog = newMessageLog;
        }
    }
}