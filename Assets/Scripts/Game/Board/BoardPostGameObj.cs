using TMPro;
using UnityEngine;

namespace EyalPhoton.Game.Board
{
    public class BoardPostGameObj : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI titleText = null;
        [SerializeField] private TextMeshProUGUI bodyText = null;

        private bool expanded = false;
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

        public void Expand()
        {
            expanded = true;
            this.titleText.text = this.boardPost.postTitle;
            this.bodyText.text = this.boardPost.postBody;
        }

        public void Unexpand()
        {
            expanded = false;
        }
        
        void OnMouseDown()
        {
            if (boardPost != null && !expanded)
            {
                SendMessageUpwards("PostClicked", boardPost);
            }
        }

        public void Destroy ()
        {
            Destroy(this.gameObject);
        }
    }
}
