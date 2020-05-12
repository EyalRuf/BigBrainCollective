using Photon.Pun;
using UnityEngine;

namespace EyalPhoton.Game
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerInput))]
    public class Movement : MonoBehaviourPun, IPunObservable
    {
        [SerializeField] private float moveSpeed = 0f;
        [SerializeField] private bool IS_TEST = false;
        private const float ROOM_BOUNDS_X_MIN = -7.96f;
        private const float ROOM_BOUNDS_X_MAX = 7.34f;
        private const float ROOM_BOUNDS_Y_MIN = -3.34f;
        private const float ROOM_BOUNDS_Y_MAX = 2.92f;

        private PlayerInput pInput = null;
        private CharacterController cController = null;
        private Vector2 remotePlayerPos = Vector2.zero;

        // Start is called before the first frame update
        void Start()
        {
            this.cController = GetComponent<CharacterController>();
            this.pInput = GetComponent<PlayerInput>();
        }

        // Update is called once per frame
        void Update()
        {
            if (IS_TEST)
            {
                this.applyLocalMovement();
            } else
            {
                if (photonView.IsMine)
                {
                    this.applyLocalMovement();
                }
                else
                {
                    this.applyNetworkMovement();
                }
            }
        }

        private void applyLocalMovement()
        {
            Vector2 movement = this.pInput.movementInput;
            cController.Move(movement * this.moveSpeed * Time.deltaTime);
            this.ensureBoundaries();
        }

        private void applyNetworkMovement()
        {
            // Moving the network player 0.05% of the way towards his real position every frame
            transform.position = Vector2.Lerp(transform.position, remotePlayerPos, 0.05f);
        }

        private void ensureBoundaries ()
        {
            var pos = this.transform.position;
            if (pos.x < ROOM_BOUNDS_X_MIN)
            {
                pos.x = ROOM_BOUNDS_X_MIN;
            } else if (pos.x > ROOM_BOUNDS_X_MAX)
            {
                pos.x = ROOM_BOUNDS_X_MAX;
            }

            if (pos.y < ROOM_BOUNDS_Y_MIN)
            {
                pos.y = ROOM_BOUNDS_Y_MIN;
            }
            else if (pos.y > ROOM_BOUNDS_Y_MAX)
            {
                pos.y = ROOM_BOUNDS_Y_MAX;
            }

            transform.position = pos;
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