using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerControllerGizmo : MonoBehaviour
    {
        private CharacterController characterController;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
        }

        private void OnDrawGizmosSelected()
        {
            if (characterController == null)
                return;

            Gizmos.color = Color.yellow;

            // Dessiner la capsule autour du Character Controller
            Vector3 top = transform.position + Vector3.up * (characterController.height / 2f);
            Vector3 bottom = transform.position - Vector3.up * (characterController.height / 2f);
            float radius = characterController.radius;

            Gizmos.DrawWireSphere(top, radius);
            Gizmos.DrawWireSphere(bottom, radius);
            Gizmos.DrawLine(top + transform.right * radius, bottom + transform.right * radius);
            Gizmos.DrawLine(top - transform.right * radius, bottom - transform.right * radius);
            Gizmos.DrawLine(top + transform.forward * radius, bottom + transform.forward * radius);
            Gizmos.DrawLine(top - transform.forward * radius, bottom - transform.forward * radius);
        }
    }
}