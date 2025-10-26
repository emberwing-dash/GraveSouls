using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public List<Transform> spawnPoints;      // Assign spawn points in Inspector
    public GameObject projectilePrefab;      // Assign TargetedProjectile prefab
    public float spawnDelay = 0.3f;          // Delay between spawning each projectile

    [Header("Projectile Settings")]
    public float projectileSpeed = 20f;
    public int projectileDamage = 10;

    /// <summary>
    /// Call this to start spawning projectiles sequentially from spawn points.
    /// </summary>
    /// 

    public void StartSpawning()
    {
        if (spawnPoints.Count > 0 && projectilePrefab != null)
            StartCoroutine(SpawnSequentially());
    }

    private IEnumerator SpawnSequentially()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (projectilePrefab != null)
            {
                GameObject proj = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);

                // Assign speed and damage
                TargetedProjectile tp = proj.GetComponent<TargetedProjectile>();
                if (tp != null)
                {
                    tp.speed = projectileSpeed;
                    tp.damage = projectileDamage;
                }
            }

            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
