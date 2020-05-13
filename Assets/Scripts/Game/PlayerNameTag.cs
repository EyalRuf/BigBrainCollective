using UnityEngine;
using Photon.Pun;
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
            nameText.text = "Name";

            if (photonView.Owner != null)
            {
                nameText.text = photonView.Owner.NickName;
            }
        }
    }
}