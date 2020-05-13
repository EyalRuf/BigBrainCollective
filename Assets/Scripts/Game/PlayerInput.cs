using UnityEngine;
using System.Collections;

namespace EyalPhoton.Game
{
    public class PlayerInput : MonoBehaviour
    {
        public Vector2 movementInput { get; private set; }
        [SerializeField] public bool isTest = false;

        // Update is called once per frame
        void Update()
        {
            this.movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        }
    }
}