using Photon.Pun;
using Photon.Realtime;
using System;
using TMPro;
using UnityEngine;

namespace EyalPhoton.Login
{
    public class MainMenu : MonoBehaviourPunCallbacks
    {
        [SerializeField] private GameObject findGamePanel = null;
        [SerializeField] private GameObject waitingStatusPanel = null;
        [SerializeField] private TextMeshProUGUI waitingStatusText = null;
        [SerializeField] private CharacterSelector characterSelector = null;

        private bool isConnecting = false;
        private PlayerCustomPropsManager propsManager = new PlayerCustomPropsManager();

        private const string GAME_VER = "0.1";
        private const int MAX_PLAYERS = 20;

        private void Awake() => PhotonNetwork.AutomaticallySyncScene = false;

        public void FindGame ()
        {
            isConnecting = true;

            findGamePanel.SetActive(false);
            waitingStatusPanel.SetActive(true);

            waitingStatusText.text = "Connecting...";

            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            } else
            {
                PhotonNetwork.GameVersion = GAME_VER;
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            Debug.Log("Connected to master");

            if (isConnecting)
            {
                PhotonNetwork.JoinRandomRoom();
            }

            isConnecting = false;
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);
            waitingStatusPanel.SetActive(false);
            findGamePanel.SetActive(false);

            Debug.Log($"Disconnected due to: {cause}");
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();

            waitingStatusText.text = "Game found, Loading Scene";
            Debug.Log("Client successfuly joined the room");

            // Setting player character according to selected
            propsManager.SetCharId(characterSelector.chrIndex);

            PhotonNetwork.LoadLevel("Scene_Main");
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            base.OnJoinRandomFailed(returnCode, message);
            Debug.Log("No clients are waiting for game, creating a new room");

            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = MAX_PLAYERS });
        }
    }
}