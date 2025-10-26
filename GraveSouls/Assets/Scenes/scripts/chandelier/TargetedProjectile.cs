using UnityEngine;

public class TargetedProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float speed = 20f;
    public int damage = 10;
    public float lifetime = 5f;

    public Vector3 targetPosition;

    void Start()
    {
        // Record player position at spawn
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        targetPosition = player != null ? player.transform.position : transform.position + transform.forward * 10f;

        // Rotate projectile to face target
        transform.LookAt(targetPosition);

        // Auto destroy after lifetime
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Move toward recorded target
        Vector3 moveDir = (targetPosition - transform.position).normalized;
        transform.position += moveDir * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Apply damage if player has a health script
            var health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
                FindObjectOfType<GameManager>().TakeDamage();

            }

                Destroy(gameObject);
        }
    }
}
