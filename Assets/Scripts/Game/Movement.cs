using Photon.Pun;
using UnityEngine;

namespace EyalPhoton.Game
{
    [RequireComponent(typeof(PlayerInput))]
    public class Movement : MonoBehaviourPun, IPunObservable
    {
        [SerializeField] private float moveSpeed = 5f;
        private Vector2 localPlayerMovement = Vector2.zero;
        private Rigidbody2D rb = null;

        private PlayerInput pInput = null;
        private CharacterController cController = null;
        private Vector2 remotePlayerPos = Vector2.zero;
        private Vector2 remotePlayerMovement = Vector2.zero;

        private bool isInteractingWithBoard = false;

        // Start is called before the first frame update
        void Start()
        {
            this.cController = GetComponent<CharacterController>();
            this.pInput = GetComponent<PlayerInput>();
            this.rb = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            if (this.pInput.isTest || photonView.IsMine)
            {
                if (!isInteractingWithBoard)
                {
                    localPlayerMovement = this.pInput.movementInput;
                }
            }
            else
            {
                remotePlayerMovement = Vector2.Lerp(transform.position, remotePlayerPos, 0.1f);
            }
        }

        void FixedUpdate()
        {
            if (this.pInput.isTest || photonView.IsMine)
            {
                rb.MovePosition(rb.position + (localPlayerMovement * this.moveSpeed * Time.deltaTime));
            }
            else
            {
                // Moving the network player % of the way towards his real position every frame
                transform.position = remotePlayerMovement;
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext((Vector2) transform.position);
            }
            else
            {
                this.remotePlayerPos = (Vector2) stream.ReceiveNext();
            }
        }
    }
}