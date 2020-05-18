using System.Collections;
using UnityEngine;
using Photon.Pun;

namespace EyalPhoton.Game
{
    public class EmojiController : MonoBehaviourPun, IPunObservable
    {
        private const string ANIMATOR_BOOLEAN_SHOW_EMOJI = "ShowEmoji";
        private const float EMOJI_SHOW_TIME = 2f;
        private const float EMOJI_CD_TIME = 3.5f;

        [SerializeField] private Animator animator = null;
        [SerializeField] private PlayerInput pInput= null;
        [SerializeField] private SpriteRenderer SR = null;
        [SerializeField] private Sprite[] emojis = null;

        private float currCdTimer = 0f;
        private int localPlayerEmoji = 0;
        private int remotePlayerEmoji = 0;
        private bool localIsEmoting = false;

        void Start()
        {
            if (!photonView.IsMine)
            {
                this.SR.sprite = null;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (photonView.IsMine)
            {
                this.currCdTimer -= Time.deltaTime;
                if (this.currCdTimer > 0f)
                    return;
                else
                    this.currCdTimer = 0f;

                for (var i = 0; i < this.emojis.Length; i++)
                {
                    if (i < 9 && this.pInput.isNumberClicked[i + 1])
                    {
                        this.localPlayerEmoji = i;
                        this.StartCoroutine(playEmoji(this.emojis[i]));
                        break;
                    }
                }
            }
        }

        private IEnumerator playEmoji (Sprite sprite)
        {
            this.SR.sprite = sprite;
            this.currCdTimer = EMOJI_CD_TIME;
            this.localIsEmoting = true;

            this.animator.SetBool(ANIMATOR_BOOLEAN_SHOW_EMOJI, true);

            yield return new WaitForSeconds(EMOJI_SHOW_TIME);

            this.animator.SetBool(ANIMATOR_BOOLEAN_SHOW_EMOJI, false);
            this.localIsEmoting = false;
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(this.localIsEmoting);
                stream.SendNext(this.localPlayerEmoji);
            }
            else
            {
                bool remoteIsEmoting = (bool) stream.ReceiveNext();
                if (remoteIsEmoting)
                {
                    int nextEmoji = (int) stream.ReceiveNext();

                    if (this.remotePlayerEmoji != nextEmoji)
                    {
                        this.remotePlayerEmoji = nextEmoji;
                        this.SR.sprite = this.emojis[nextEmoji];
                    }
                }
            }
        }
    }
}
