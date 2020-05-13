using Photon.Pun;
using System;
using UnityEngine;
using EyalPhoton.Login;

namespace EyalPhoton.Game
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimation : MonoBehaviourPun, IPunObservable
    {
        [SerializeField] private Animator animator = null;
        [SerializeField] private RuntimeAnimatorController[] characterList = null;
        
        private PlayerInput pInput = null;
        private PlayerCustomPropsManager  playerProps = new PlayerCustomPropsManager();
        private int characterId = -1;

        private float lastHorizMovementStartTime = 0;
        private float lastHorizMovementDir = 0;
        private float lastVertMovementStartTime = 0;
        private float lastVertMovementDir = 0;

        // Use this for initialization
        void Start()
        {
            this.pInput = GetComponentInParent<PlayerInput>();

            if (photonView.IsMine)
            {
                this.characterId = playerProps.GetCharId(photonView);
                animator.runtimeAnimatorController = characterList[characterId];
            } // Set network player anim conroller
            else if (characterId != -1)
            {
                animator.runtimeAnimatorController = characterList[characterId];
            }

        }

        // Update is called once per frame
        void Update()
        {
            if (!this.pInput.isTest)
            {
                if (photonView.IsMine)
                {
                    ApplyAnimations();
                }
            } else
            {
                ApplyAnimations();
            }
        }

        private void ApplyAnimations()
        {
            bool right = this.pInput.movementInput.x > 0f, 
                left = this.pInput.movementInput.x < 0f, 
                up = this.pInput.movementInput.y > 0f, 
                down = this.pInput.movementInput.y < 0f,
                idle = this.pInput.movementInput == Vector2.zero;

            // No direction => reset time moving and direction
            if (!right && !left)
            {
                lastHorizMovementStartTime = 0;
                lastHorizMovementDir = 0;
            } // Changed direction => reset time moving direction
            else if (right && lastHorizMovementDir <= 0 || left && lastHorizMovementDir >= 0)
            {
                lastHorizMovementStartTime = Time.time;
                lastHorizMovementDir = right ? 1 : -1;
            }
            
            if (!up && !down)
            {
                lastVertMovementStartTime = 0;
                lastVertMovementDir = 0;
            }
            else if (up && lastVertMovementDir <= 0 || down && lastVertMovementDir >= 0)
            {
                lastVertMovementStartTime = Time.time;
                lastVertMovementDir = up ? 1 : -1;
            }

            int dirCount = 0;

            if (right || left)
                dirCount++;
            if (up || down)
                dirCount++;

            if (dirCount > 1)
            {
                var hPower = Math.Abs(this.pInput.movementInput.x);
                var vPower = Math.Abs(this.pInput.movementInput.y);

                if (hPower > vPower)
                {
                    down = false;
                    up = false;
                }
                else if (vPower > hPower)
                {
                    right = false;
                    left = false;
                }
                else
                {
                    if (lastHorizMovementStartTime > lastVertMovementStartTime)
                    {
                        down = false;
                        up = false;
                    }
                    else
                    {
                        right = false;
                        left = false;
                    }
                }
            }

            animator.SetBool("right", right);
            animator.SetBool("left", left);
            animator.SetBool("up", up);
            animator.SetBool("down", down);
            animator.SetBool("idle", idle);
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(characterId);
            }
            else
            {
                this.characterId = (int) stream.ReceiveNext();
            }
        }
    }
}