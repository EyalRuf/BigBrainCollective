using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace EyalPhoton.Game.Board
{
    [Serializable]
    public class BoardPost
    {
        public int postCreatorActorNum { get; set; }
        public string postId { get; set; }
        public string postTitle { get; set; }
        public string postBody { get; set; }
        public List<PostComment> comments { get; set; }

        public BoardPost ()
        {

        }

        public BoardPost(int postCreatorActorNum, string postId, string postTitle, string postBody)
        {
            this.postCreatorActorNum = postCreatorActorNum;
            this.postId = postId;
            this.postTitle = postTitle;
            this.postBody = postBody;
            this.comments = new List<PostComment>();
        }

        public BoardPost(BoardPost post)
        {
            this.postCreatorActorNum = post.postCreatorActorNum;
            this.postId = post.postId;
            this.postTitle = post.postTitle;
            this.postBody = post.postBody;
            this.comments = post.comments;
        }

        //public void addComment (int commentorActorNum, string commentText)
        //{
        //    this.comments.Add(new PostComment(commentorActorNum, commentText));
        //}

        public static byte[] Serialize(object customobject)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, customobject);
                return ms.ToArray();
            }
        }

        public static object Deserialize(byte[] arrBytes)
        {
            using (var memStream = new MemoryStream())
            {
                var binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                var obj = binForm.Deserialize(memStream);
                return obj;
            }
        }

        public static string generatePostID()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
