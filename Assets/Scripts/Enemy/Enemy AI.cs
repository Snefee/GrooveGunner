using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EnemyAI : MonoBehaviour
{
    [Header("AI Configuration")]
    public Transform playerTransform;
    public float moveSpeed = 3.0f;
    public float stoppingDistance = 2.5f;

    [Header("Distance Color")]
    [Tooltip("The base color of the enemy when it is far away.")]
    public Color farColor = Color.red;
    [Tooltip("The base color of the enemy when it is close.")]
    public Color nearColor = Color.green;
    
    [Space]
    [Tooltip("The emission color of the enemy when it is far away.")]
    public Color farEmissionColor = new Color(1f, 0, 0); // Red emission
    [Tooltip("The emission intensity when the enemy is far away.")]
    public float farEmissionIntensity = 1.0f;
    
    [Space]
    [Tooltip("The emission color of the enemy when it is close.")]
    public Color nearEmissionColor = new Color(0, 1f, 0); // Green emission
    [Tooltip("The emission intensity when the enemy is close.")]
    public float nearEmissionIntensity = 2.5f;

    [Space]
    public float maxColorDistance = 20f;
    public float minColorDistance = 10f;

    [Header("Shader Property Names")]
    [Tooltip("The name of the base color property in the enemy's shader. For URP/Lit, this is '_BaseColor'.")]
    public string colorPropertyName = "_BaseColor";
    [Tooltip("The name of the emission color property in the enemy's shader. For URP/Lit, this is '_EmissionColor'.")]
    public string emissionColorPropertyName = "_EmissionColor";

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
        else
        {
            enemyRenderer.material.EnableKeyword("_EMISSION");
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

        Color currentBaseColor = Color.Lerp(farColor, nearColor, t);
        Color currentEmissionColor = Color.Lerp(farEmissionColor, nearEmissionColor, t);
        float currentEmissionIntensity = Mathf.Lerp(farEmissionIntensity, nearEmissionIntensity, t);

        Color finalHDRColor = currentEmissionColor * currentEmissionIntensity;

        enemyRenderer.GetPropertyBlock(propBlock);

        propBlock.SetColor(colorPropertyName, currentBaseColor);
        propBlock.SetColor(emissionColorPropertyName, finalHDRColor);

        enemyRenderer.SetPropertyBlock(propBlock);
    }
}