using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;

namespace EyalPhoton.Game.Board
{
    public class PostCommentGameObj : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI commentText = null;
        [SerializeField] private Button upvoteBtn = null;

        public PostComment comment { get; set; }

        // Use this for initialization
        void Start()
        {
            if (comment != null)
            {
                commentText.text = comment.commentText;
                upvoteBtn.interactable = !comment.isApproved;
            } else
            {
                upvoteBtn.interactable = false;
            }
        }

        public void ApproveComment ()
        {
            comment.approveComment();
            upvoteBtn.interactable = false;

            if (comment != null)
            {
                SendMessageUpwards("CommentApproved", comment.commentorActorNum);
            }
        }
        public void Destroy()
        {
            Destroy(this.gameObject);
        }
    }
}