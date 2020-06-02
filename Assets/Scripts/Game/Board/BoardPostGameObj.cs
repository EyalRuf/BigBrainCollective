using Photon.Pun;
using TMPro;
using UnityEngine;

namespace EyalPhoton.Game.Board
{
    public class BoardPostGameObj : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI titleText = null;
        [SerializeField] private TextMeshProUGUI bodyText = null;
        [SerializeField] private GameObject commentsPanel = null;
        [SerializeField] private PostCommentGameObj commentPrefab = null;

        private bool expanded = false;
        private bool isMyPost = false;
        public BoardPost boardPost { get; set; }

        void Start ()
        {
            if (boardPost != null)
            {
                this.titleText.text = this.boardPost.postTitle;
                this.bodyText.text = this.boardPost.postBody;

                BoxCollider2D collider = GetComponent<BoxCollider2D>();
                if (collider != null)
                {
                    RectTransform rt = GetComponent<RectTransform>();
                    collider.size = new Vector2(500, 235);
                }
            }
        }
        private void UpdateMyPostAndSpawnComments()
        {
            if (PhotonNetwork.IsConnected)
            {
                this.isMyPost = this.boardPost.postCreatorActorNum == PhotonNetwork.LocalPlayer.ActorNumber;
            }
            else
            {
                this.isMyPost = false;
            }

            if (this.isMyPost && commentsPanel != null)
            {
                PostCommentGameObj[] comments = this.commentsPanel.GetComponentsInChildren<PostCommentGameObj>();
                for (int i = 0; i < comments.Length; i++)
                {
                    comments[i].Destroy();
                }

                for (int i = 0; i < this.boardPost.comments.Count; i++)
                {
                    PostCommentGameObj commentGameObj = Instantiate(commentPrefab, commentsPanel.transform);
                    commentGameObj.comment = this.boardPost.comments[i];
                }
            }
        }

        public void Expand()
        {
            expanded = true;
            this.titleText.text = this.boardPost.postTitle;
            this.bodyText.text = this.boardPost.postBody;
            this.UpdateMyPostAndSpawnComments();
        }

        public void Unexpand()
        {
            expanded = false;
        }
        
        void OnMouseDown()
        {
            if (boardPost != null && !expanded)
            {
                SendMessageUpwards("ExpandPost", boardPost.postId);
            }
        }

        public void Destroy ()
        {
            Destroy(this.gameObject);
        }
    }
}
