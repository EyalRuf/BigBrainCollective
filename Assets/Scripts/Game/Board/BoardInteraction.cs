using System;
using UnityEngine;

namespace EyalPhoton.Game.Board
{
    public class BoardInteraction : MonoBehaviour
    {
        public void PlayerInteractingWithBoard (bool isInteracting)
        {
            PlayerInput[] players = FindObjectsOfType<PlayerInput>();
            Array.ForEach(players, p => p.lockInput(isInteracting));
        }
    }
}