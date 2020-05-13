using Photon.Pun;
using Photon.Realtime;
using System;

namespace EyalPhoton
{
    public class PlayerCustomPropsManager
    {
        private const string CHARACTER_ID_PROP_KEY = "charId";

        public void SetCharId(int charId)
        {
            Player localP = Array.Find(PhotonNetwork.PlayerList, curr => curr.IsLocal);
            localP.CustomProperties.Add(CHARACTER_ID_PROP_KEY, charId);
        }

        public int GetCharId (PhotonView photonView)
        {
            if (photonView.Controller != null)
            {
                return (int) photonView.Controller.CustomProperties[CHARACTER_ID_PROP_KEY];
            }

            return 0;
        }
    }
}
