using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EyalPhoton.Login
{
    public class CharacterSelector : MonoBehaviour
    {
        [SerializeField] Animator animator = null;
        [SerializeField] private RuntimeAnimatorController[] characterList = null;
        private const string PLAYER_PREFS_CHR_ID = "chrId";

        public int chrIndex { get; private set; }

        void Start()
        {
            chrIndex = 0;
            InitChrFromPrefs();
        }

        private void InitChrFromPrefs()
        {
            if (!PlayerPrefs.HasKey(PLAYER_PREFS_CHR_ID)) { return; }

            chrIndex = PlayerPrefs.GetInt(PLAYER_PREFS_CHR_ID);
            setCharacterByIndex();
        }

        public void NextChr ()
        {
            chrIndex = (chrIndex + 1) % this.characterList.Length;
            this.setCharacterByIndex();
        }

        public void PrevChr ()
        {
            chrIndex = chrIndex == 0 ? (this.characterList.Length - 1) : (chrIndex - 1);
            this.setCharacterByIndex();
        }

        private void setCharacterByIndex ()
        {
            animator.runtimeAnimatorController = characterList[chrIndex];
        }

        public void SaveChrToPrefs ()
        {
            PlayerPrefs.SetInt(PLAYER_PREFS_CHR_ID, chrIndex);
        }
    }
}