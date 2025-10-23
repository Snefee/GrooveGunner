using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EnemyAI : MonoBehaviour
{
    [Header("AI Configuration")]
    public Transform playerTransform;
    public float moveSpeed = 3.0f;
    public float stoppingDistance = 2.5f;

    [Header("Distance Color")]
    public Color farColor = Color.red;
    public Color nearColor = Color.green;
    public float maxColorDistance = 20f;
    public float minColorDistance = 10f;
    public string colorPropertyName = "_BaseColor";

    private Animator animator;
    private CharacterController characterController;
    private Renderer enemyRenderer;
    private MaterialPropertyBlock propBlock;

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        
        enemyRenderer = GetComponentInChildren<Renderer>(true);
        propBlock = new MaterialPropertyBlock();

        if (enemyRenderer == null)
        {
            Debug.LogError("No Renderer found on enemy prefab or its children. Cannot change color.", this);
        }
    }

    void Update()
    {
        if (playerTransform == null || enemyRenderer == null) return;

        // --- Rotation ---
        Vector3 lookDirection = playerTransform.position - transform.position;
        lookDirection.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f);

        // --- Movement & Color ---
        float distance = Vector3.Distance(transform.position, playerTransform.position);

        UpdateColor(distance);

        if (distance > stoppingDistance)
        {
            Vector3 moveDirection = (playerTransform.position - transform.position).normalized;
            moveDirection.y = 0;
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
            
            if (animator != null) animator.SetBool("IsWalking", true);
        }
        else
        {
            if (animator != null) animator.SetBool("IsWalking", false);
        }
    }

    void UpdateColor(float currentDistance)
    {
        if (enemyRenderer == null) return;

        float t = Mathf.InverseLerp(maxColorDistance, minColorDistance, currentDistance);
        Color currentColor = Color.Lerp(farColor, nearColor, t);

        enemyRenderer.GetPropertyBlock(propBlock);
        propBlock.SetColor(colorPropertyName, currentColor); 
        enemyRenderer.SetPropertyBlock(propBlock);
    }
}
