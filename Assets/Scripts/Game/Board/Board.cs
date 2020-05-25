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
        private const int POSTS_PER_PAGE = 5;

        private List<BoardPost> boardPosts = null;
        public bool shouldRerenderBoard { get; set; }

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
            this.shouldRerenderBoard = true;
        }

        public void SendAddPost(BoardPost newPost)
        {
            if (PhotonNetwork.IsConnected)
                this.photonView.RPC("AddPostFromRemote", RpcTarget.All, newPost);
            else
            {
                this.AddPostFromRemote(newPost);
            }
        }

        public List<BoardPost> GetPagePosts (int page)
        {
            if (this.boardPosts.Count == 0)
                return new List<BoardPost>();

            if (this.boardPosts.Count - (page * POSTS_PER_PAGE) > POSTS_PER_PAGE)
                return this.boardPosts.GetRange(page * POSTS_PER_PAGE, POSTS_PER_PAGE);

            return this.boardPosts.GetRange((page * POSTS_PER_PAGE), this.boardPosts.Count - (page * POSTS_PER_PAGE));
        }

        public int GetPostsPerPage ()
        {
            return POSTS_PER_PAGE;
        }

        public int GetAmountOfPages ()
        {
            return (this.boardPosts.Count / POSTS_PER_PAGE);
        }

        [PunRPC]
        void AddPostFromRemote(BoardPost post)
        {
            this.boardPosts.Add(post);
            this.shouldRerenderBoard = true;
        }

        public void OpenBoard ()
        {

        }
    }
}