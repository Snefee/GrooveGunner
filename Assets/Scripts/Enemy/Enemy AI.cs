using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("AI Configuration")]
    public Transform playerTransform;
    public float moveSpeed = 3.0f;
    public float stoppingDistance = 2.5f;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
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
            Vector3 targetPosition = new Vector3(playerTransform.position.x, 
                                                 transform.position.y, 
                                                 playerTransform.position.z);

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            
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
