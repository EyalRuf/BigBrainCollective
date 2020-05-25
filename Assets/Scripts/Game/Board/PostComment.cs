using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace EyalPhoton.Game.Board
{
    [Serializable]
    public class PostComment
    {
        public int creatorActorNum { get; private set; }
        public string commentText { get; private set; }
        public bool isApproved { get; private set; }

        public PostComment(int creatorActorNum, string commentText)
        {
            this.creatorActorNum = creatorActorNum;
            this.commentText = commentText;
            this.isApproved = false;
        }

        public void approveComment ()
        {
            this.isApproved = true; ;
        }

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
    }
}
