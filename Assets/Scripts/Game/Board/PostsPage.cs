using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

namespace EyalPhoton.Game.Board
{
    public class PostsPage : MonoBehaviour
    {
        // Both panels
        [SerializeField] private Board board = null;
        [SerializeField] private GameObject allPostsPanel = null;
        [SerializeField] private GameObject expandedPostPanel = null;
        [SerializeField] private bool allPostsMode = true;
        public bool shouldBoardBeRendered = true;

        // Expanded post stuff
        [SerializeField] private BoardPostGameObj expandedPost = null;
        [SerializeField] private TMP_InputField commentTextInput = null;
        [SerializeField] private Button submitCommentBtn = null;
        private const int COMMENT_MIN_LENGTH = 5;

        // All posts stuff
        [SerializeField] private GameObject topPanel = null;
        [SerializeField] private GameObject bottomPanel = null;
        [SerializeField] private BoardPostGameObj postPrefab = null;
        [SerializeField] private BoardPostGameObj emptyPostPrefab = null;
        [SerializeField] private Button btnNextPage = null;
        [SerializeField] private Button btnPrevPage = null;
        [SerializeField] private int page = 0;

        void Update()
        {
            if (this.board != null && this.shouldBoardBeRendered)
            {
                InitPage();
            }
        }

        void InitPage ()
        {
            DestroyPostsAndInitBtns();
            InitPosts();
        }

        void DestroyPostsAndInitBtns ()
        {
            btnNextPage.interactable = this.board.GetAmountOfPages(this.allPostsMode) > 0;
            btnPrevPage.interactable = this.board.GetAmountOfPages(this.allPostsMode) > 0;

            BoardPostGameObj[] topPanelPosts = this.topPanel.GetComponentsInChildren<BoardPostGameObj>();
            BoardPostGameObj[] bottomPanelPosts = this.bottomPanel.GetComponentsInChildren<BoardPostGameObj>();

            for (int i = 0; i < topPanelPosts.Length; i++)
            {
                topPanelPosts[i].Destroy();
            }
            for (int i = 0; i < bottomPanelPosts.Length; i++)
            {
                bottomPanelPosts[i].Destroy();
            }
        }

        void InitPosts()
        {
            List<BoardPost> postsToDisplay = this.board.GetPagePosts(this.allPostsMode, this.page);

            int i = 0;
            for (; i < postsToDisplay.Count; i++)
            {
                Transform panelTransform = i < 3 ? topPanel.transform : bottomPanel.transform;
                BoardPostGameObj postGameObj = Instantiate(postPrefab, panelTransform);
                postGameObj.boardPost = postsToDisplay[i];
            }

            for (; i < this.board.GetPostsPerPage(); i++)
            {
                Transform panelTransform = i < 3 ? topPanel.transform : bottomPanel.transform;
                Instantiate(emptyPostPrefab, panelTransform);
            }

            this.shouldBoardBeRendered = false;
        }

        public void PrevPage ()
        {
            this.page = this.page == 0 ? this.board.GetAmountOfPages(this.allPostsMode) : this.page - 1;
            InitPage();
        }

        public void NextPage ()
        {
            this.page = (this.page + 1) % (this.board.GetAmountOfPages(this.allPostsMode) + 1);
            InitPage();
        }

        public void ExpandPost (string bpId)
        {
            BoardPost bp = this.board.getBoardPostById(bpId);
            allPostsPanel.SetActive(false);
            expandedPostPanel.SetActive(true);
            expandedPost.boardPost = bp;
            expandedPost.Expand();
        }

        public void UnexpandPost ()
        {
            allPostsPanel.SetActive(true);
            expandedPostPanel.SetActive(false);
            if (allPostsMode)
            {
                commentTextInput.text = "";
                SetCommentText("");
            }

            expandedPost.Unexpand();
        }

        public void SetCommentText(string text)
        {
            this.submitCommentBtn.interactable = (text.Length >= COMMENT_MIN_LENGTH);
        }

        public void SubmitComment()
        {
            var commentText = this.commentTextInput.text;
            var commentorActorNum = PhotonNetwork.LocalPlayer.ActorNumber;
            var postId = this.expandedPost.boardPost.postId;

            this.board.SendAddComment(postId, commentorActorNum, commentText);
            this.shouldBoardBeRendered = true;
            this.UnexpandPost();
        }

        public void CommentApproved(int commentorActorNum)
        {
            this.board.SendCommentApproved(commentorActorNum);
        }
    }
}