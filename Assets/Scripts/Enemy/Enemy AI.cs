using UnityEngine;

[RequireComponent(typeof(CharacterController))] // Ensures a CharacterController is on the GameObject
public class EnemyAI : MonoBehaviour
{
    [Header("AI Configuration")]
    public Transform playerTransform;
    public float moveSpeed = 3.0f;
    public float stoppingDistance = 2.5f;

    private Animator animator;
    private CharacterController characterController;

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>(); // Get the CharacterController component
    }

    void Update()
    {
        // --- Rotation ---
        if (playerTransform == null) return;
        Vector3 lookDirection = playerTransform.position - transform.position;
        lookDirection.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f);

        // --- Movement ---
        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if (distance > stoppingDistance)
        {
            // Calculate the direction to move in, ignoring the Y axis.
            Vector3 moveDirection = (playerTransform.position - transform.position).normalized;
            moveDirection.y = 0;

            // Use CharacterController.Move to handle movement and collisions.
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
            
            if (animator != null)
            {
                animator.SetBool("IsWalking", true);
            }
        }
        else
        {
            if (animator != null)
            {
                animator.SetBool("IsWalking", false);
            }
        }
    }
}
