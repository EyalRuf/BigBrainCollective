using Cinemachine;
using Photon.Pun;
using UnityEngine;

namespace EyalPhoton.Game
{
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject playerPrefab = null;
        [SerializeField] private CinemachineVirtualCamera playerCam = null;
        void Start()
        {
            if (PhotonNetwork.IsConnected)
            {
                var player = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
                this.playerCam.Follow = player.transform;
                this.playerCam.LookAt = player.transform;
            }
        }
    }
}