using UnityEngine;
using System.Collections;
using Photon.Pun;
using System;
using TMPro;

namespace EyalPhoton.Game
{
    public class PlayerNameTag : MonoBehaviourPun
    {
        [SerializeField] private TextMeshProUGUI nameText = null;
        // Use this for initialization
        void Start()
        {
            SetName();
        }

        private void SetName()
        {
            nameText.text = photonView.Owner.NickName;
        }
    }
}