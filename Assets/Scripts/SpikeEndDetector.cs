using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creative
{
    public class SpikeEndDetector : MonoBehaviour
    {
        private void OnTriggerEnter(Collider collision)
        {
            //Debug.Log("SpikeEndDetector: OnCollisionEnter with " + collision.gameObject.name);
            if (collision.gameObject.CompareTag("Player"))
            {
                // Assuming the player has a method to handle spike end detection
                CharacterController characterController = collision.gameObject.GetComponentInParent<CharacterController>();
                characterController.HighlightCharacter();
            }
        }
    }
}
