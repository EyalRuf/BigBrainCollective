using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;

namespace EyalPhoton.Game.Board
{
    public class NewPostPage : MonoBehaviour
    {
        [SerializeField] private GameObject newPostPanel = null;
        [SerializeField] private GameObject submitPanel = null;
        [SerializeField] private GameObject textNoPostsRemaining = null;
        [SerializeField] private TextMeshProUGUI textPostsRemaining = null;
        [SerializeField] private TMP_InputField titleInput = null;
        [SerializeField] private TMP_InputField bodyInput = null;
        [SerializeField] private Button submitBtn = null;
        [SerializeField] private Board board = null;
        private int postsRemaining = 3;

        private const int TITLE_MIN_LENGTH = 3;
        private const int BODY_MIN_LENGTH = 5;

        void Start()
        {
            this.textPostsRemaining.text = "Posts remaining: " + postsRemaining;
        }

        public void ValidatePost()
        {
            bool bodyValid = (this.bodyInput.text.Length >= BODY_MIN_LENGTH);
            bool titleValid = (this.titleInput.text.Length >= TITLE_MIN_LENGTH);

            this.submitBtn.interactable = bodyValid && titleValid;
        }

        public void SubmitPost()
        {
            // Sending post to board
            BoardPost bp = new BoardPost(PhotonNetwork.LocalPlayer.ActorNumber,
                BoardPost.generatePostID(), this.titleInput.text, this.bodyInput.text);
            this.board.SendAddPost(bp);

            // Reducing posts left + chaging display in case 0 remaining
            this.postsRemaining--;
            this.titleInput.text = "";
            this.bodyInput.text = "";
            this.textPostsRemaining.text = "Posts remaining: " + postsRemaining;
            if (this.postsRemaining == 0)
            {
                newPostPanel.SetActive(false);
                submitPanel.SetActive(false);
                textNoPostsRemaining.SetActive(true);
            }
        }
    }
}
