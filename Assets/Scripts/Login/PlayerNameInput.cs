using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EyalPhoton.Login
{
    public class PlayerNameInput : MonoBehaviour
    {
        [SerializeField] private TMP_InputField nameInput = null;
        [SerializeField] private Button continueBtn = null;

        private const string PLAYER_PREFS_NAME_KEY = "PlayerName";
        private const int NAME_MIN_LENGTH = 3;
        private const int NAME_MAX_LENGTH = 15;

        // Start is called before the first frame update
        void Start() => InitInput();

        private void InitInput()
        {
            if (!PlayerPrefs.HasKey(PLAYER_PREFS_NAME_KEY)) { return; }

            string defName = PlayerPrefs.GetString(PLAYER_PREFS_NAME_KEY);
            nameInput.text = defName;
            SetPlayerName(defName);
        }

        public void SetPlayerName(string name)
        {
            bool nameValid = (name.Length >= NAME_MIN_LENGTH);

            if (name.Length > NAME_MAX_LENGTH)
            {
                nameInput.text = name.Remove(name.Length - 1, 1);
            }

            continueBtn.interactable = nameValid;
        }

        public void SavePlayerName()
        {
            string playerName = nameInput.text;
            PhotonNetwork.NickName = playerName;
            PlayerPrefs.SetString(PLAYER_PREFS_NAME_KEY, playerName);
        }
    }
}
