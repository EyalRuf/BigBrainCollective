using UnityEngine;
using System.Collections;
using System;
using Photon.Pun;

namespace EyalPhoton.Game
{
    public class PlayerInput : MonoBehaviourPun
    {
        [SerializeField] public bool isTest = false;
        public Vector2 movementInput { get; private set; }
        public bool[] isNumberClicked { get; private set; }

        public bool inputLocked { get; set; }
        public bool isLocal { get; private set; }
        void Start()
        {
            inputLocked = false;
            isLocal = this.photonView.IsMine;
            isNumberClicked = new bool[10];
            for (var i = 0; i < 10; i++)
            {
                isNumberClicked[i] = false;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (inputLocked)
            {
                this.movementInput = Vector2.zero;
                this.isNumberClicked[0] = false;
                this.isNumberClicked[1] = false;
                this.isNumberClicked[2] = false;
                this.isNumberClicked[3] = false;
                this.isNumberClicked[4] = false;
                this.isNumberClicked[5] = false;
                this.isNumberClicked[6] = false;
                this.isNumberClicked[7] = false;
                this.isNumberClicked[8] = false;
                this.isNumberClicked[9] = false;
            } else
            {
                this.movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                this.SetNumbersClicked();
            }
        }

        private void SetNumbersClicked()
        {
            this.isNumberClicked[0] = Input.GetKey(KeyCode.Alpha0);
            this.isNumberClicked[1] = Input.GetKey(KeyCode.Alpha1);
            this.isNumberClicked[2] = Input.GetKey(KeyCode.Alpha2);
            this.isNumberClicked[3] = Input.GetKey(KeyCode.Alpha3);
            this.isNumberClicked[4] = Input.GetKey(KeyCode.Alpha4);
            this.isNumberClicked[5] = Input.GetKey(KeyCode.Alpha5);
            this.isNumberClicked[6] = Input.GetKey(KeyCode.Alpha6);
            this.isNumberClicked[7] = Input.GetKey(KeyCode.Alpha7);
            this.isNumberClicked[8] = Input.GetKey(KeyCode.Alpha8);
            this.isNumberClicked[9] = Input.GetKey(KeyCode.Alpha9);
        }

        public void lockInput (bool loc)
        {
            if (this.isLocal)
            {
                this.inputLocked = loc;
            }
        }
    }
}