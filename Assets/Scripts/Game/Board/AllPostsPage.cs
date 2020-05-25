using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace EyalPhoton.Game.Board
{
    public class AllPostsPage : MonoBehaviour
    {
        [SerializeField] private GameObject allPostsPanel = null;
        [SerializeField] private GameObject expandedPostPanel = null;
        [SerializeField] private BoardPostGameObj expandedPost = null;
        [SerializeField] private GameObject topPanel = null;
        [SerializeField] private GameObject bottomPanel = null;
        [SerializeField] private BoardPostGameObj postPrefab = null;
        [SerializeField] private BoardPostGameObj emptyPostPrefab = null;
        [SerializeField] private Button btnNextPage = null;
        [SerializeField] private Button btnPrevPage = null;
        [SerializeField] private Board board = null;
        [SerializeField] private int page = 0;

        void Start()
        {
        }

        void Update()
        {
            if (this.board != null && board.shouldRerenderBoard)
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
            btnNextPage.interactable = this.board.GetAmountOfPages() > 0;
            btnPrevPage.interactable = this.board.GetAmountOfPages() > 0;

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
            List<BoardPost> posts = this.board.GetPagePosts(this.page);
            int i = 0;
            for (; i < posts.Count; i++)
            {
                Transform panelTransform = i < 3 ? topPanel.transform : bottomPanel.transform;
                BoardPostGameObj postGameObj = Instantiate(postPrefab, panelTransform);
                postGameObj.boardPost = posts[i];
            }

            for (; i < this.board.GetPostsPerPage(); i++)
            {
                Transform panelTransform = i < 3 ? topPanel.transform : bottomPanel.transform;
                Instantiate(emptyPostPrefab, panelTransform);
            }

            board.shouldRerenderBoard = false;
        }

        public void PrevPage ()
        {
            this.page = this.page == 0 ? this.board.GetAmountOfPages() : this.page - 1;
            InitPage();
        }

        public void NextPage ()
        {
            this.page = (this.page + 1) % (this.board.GetAmountOfPages() + 1);
            InitPage();
        }

        public void PostClicked (BoardPost bp)
        {
            allPostsPanel.SetActive(false);
            expandedPostPanel.SetActive(true);
            expandedPost.boardPost = bp;
            expandedPost.Expand();
        }

        public void UnexpandPost ()
        {
            allPostsPanel.SetActive(true);
            expandedPostPanel.SetActive(false);
            expandedPost.Unexpand();
        }
    }
}