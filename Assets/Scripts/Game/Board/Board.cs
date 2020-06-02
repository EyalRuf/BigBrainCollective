using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EyalPhoton.Game.Board
{
    public class Board : MonoBehaviourPun
    {
        [SerializeField] private PostsPage allPostsPage = null;
        [SerializeField] private PostsPage myPostsPage = null;
        [SerializeField] private SaturationController satCtrlr = null;
        [SerializeField] private NotificationManager notifications = null;

        private const int POSTS_PER_PAGE = 5;
        private List<BoardPost> boardPosts = null;
        private int commentsLeftToApprove = 4;

        void Awake()
        {
            if (PhotonNetwork.IsConnected)
            {
                bool registrationWorked = PhotonPeer.RegisterType(typeof(BoardPost), (byte)'B', BoardPost.Serialize, BoardPost.Deserialize);
                if (!registrationWorked)
                    Debug.LogError("Registration of custom type to photon network failed. Please restart game and hope for the best :)");
            }
        }

        void Start()
        {
            this.boardPosts = new List<BoardPost>();
            allPostsPage.shouldBoardBeRendered = true;
            myPostsPage.shouldBoardBeRendered = true;
        }

        private List<BoardPost> GetPostsForMyPostsPage()
        {
            var myActorNum = PhotonNetwork.LocalPlayer.ActorNumber;

            // All posts that are mine
            var posts =
                this.boardPosts.FindAll(currPost =>
                    currPost.postCreatorActorNum == myActorNum);

            return posts;
        }

        private List<BoardPost> GetPostsForAllPostsPage()
        {
            var myActorNum = PhotonNetwork.LocalPlayer.ActorNumber;

            // All posts that are not mine + I didn't comment on them
            var posts =
                this.boardPosts.FindAll(currPost =>
                    currPost.postCreatorActorNum != myActorNum)
                .FindAll(currPost =>
                    !currPost.didActorCommentOnPost(myActorNum));

            return posts;
        }

        public List<BoardPost> GetPagePosts (bool isAllPosts, int page)
        {
            var posts = isAllPosts ? this.GetPostsForAllPostsPage() : this.GetPostsForMyPostsPage();

            if (posts.Count == 0)
                return posts;

            if (posts.Count - (page * POSTS_PER_PAGE) > POSTS_PER_PAGE)
                return posts.GetRange(page * POSTS_PER_PAGE, POSTS_PER_PAGE);

            return posts.GetRange((page * POSTS_PER_PAGE), posts.Count - (page * POSTS_PER_PAGE));
        }

        public int GetPostsPerPage ()
        {
            return POSTS_PER_PAGE;
        }

        public int GetAmountOfPages (bool isAllPosts)
        {
            var posts = isAllPosts ? this.GetPostsForAllPostsPage() : this.GetPostsForMyPostsPage();

            return (posts.Count / POSTS_PER_PAGE);
        }

        public BoardPost getBoardPostById(string id)
        {
            return this.boardPosts.Find(posts => posts.postId == id);
        }

        public void SendAddPost(BoardPost newPost)
        {
            this.boardPosts.Add(newPost); // add locally
            this.myPostsPage.shouldBoardBeRendered = true;
            notifications.NotifyPostSubmitted();

            if (PhotonNetwork.IsConnected)
                this.photonView.RPC("AddPostFromRemote", RpcTarget.Others, newPost);
        }

        // Someone adds new post
        [PunRPC]
        void AddPostFromRemote(BoardPost post)
        {
            this.boardPosts.Add(post);
            this.allPostsPage.shouldBoardBeRendered = true;
            notifications.NotifyPostAdded();
        }

        public void SendAddComment(string postId, int commentorActorNum, string comment)
        {
            // add locally
            var post = this.boardPosts.Find(currPost => currPost.postId == postId);
            if (post != null)
            {
                post.comments.Add(new PostComment(commentorActorNum, comment));
            }
            notifications.NotifyCommentSubmitted();

            if (PhotonNetwork.IsConnected)
                this.photonView.RPC("AddCommentFromRemote", RpcTarget.Others, postId, commentorActorNum, comment);
        }

        // Someone comments on a post
        [PunRPC]
        void AddCommentFromRemote(string postId, int commentorActorNum, string comment)
        {
            var post = this.boardPosts.Find(currPost => currPost.postId == postId);

            if (post != null)
            {
                post.comments.Add(new PostComment(commentorActorNum, comment));
                
                // Update my posts page if this post is mine
                if (PhotonNetwork.LocalPlayer.ActorNumber == post.postCreatorActorNum)
                {
                    notifications.NotifyCommetOnPost(post.postTitle);
                    myPostsPage.shouldBoardBeRendered = true;
                }
            }
        }

        public void SendCommentApproved(int commentorActorNum)
        {
            if (PhotonNetwork.IsConnected)
            {
                this.photonView.RPC("CommentApproved", RpcTarget.Others, commentorActorNum);
            }
        }

        [PunRPC]
        void CommentApproved (int commentorActorNum)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == commentorActorNum)
            {
                commentsLeftToApprove--;
                if (commentsLeftToApprove == 0)
                {
                    notifications.NotifyFinishedApprovals();
                } else
                {
                    notifications.NotifyApprovedComment();
                }

                satCtrlr.AddColor();
            }
        }
    }
}