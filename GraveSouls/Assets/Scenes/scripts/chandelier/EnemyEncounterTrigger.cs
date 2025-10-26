using System.Collections;
using UnityEngine;

public class EnemyEncounterTrigger : MonoBehaviour
{
    [Header("Camera References")]
    public GameObject playerCamera;
    public GameObject cinematicCamera;

    [Header("Enemy Settings")]
    public GameObject enemyObject;
    public string entryAnimation = "entry";
    public string attackAnimation = "attack";
    public string foolAroundAnimation = "foolaround";

    [Header("Projectile Spawner")]
    public ProjectileSpawner projectileSpawner;

    [Header("Trigger Settings")]
    public string triggerTag = "Player";

    [Header("Spiral Attack Settings")]
    public float spiralAngleIncrement = 30f;

    private Animator enemyAnimator;
    private bool inCombatLoop = false;
    private float currentSpiralAngle = 0f;

    [Header("Result Manager")]
    public ResultManager resultManager;

    [Header("Activate at Entry")]
    public GameObject activateOnEntry;

    [Header("Audio Settings")]
    public AudioSource currentMusicSource;    // background/exploration
    public AudioSource battleMusicSource;     // boss fight
    public AudioSource foolAroundAudioSource; // plays before foolAround animation
    public AudioSource shootAudioSource;      // plays on each projectile spawn

    private void Awake()
    {
        if (cinematicCamera != null)
            cinematicCamera.SetActive(false);

        if (enemyObject != null)
        {
            enemyObject.SetActive(false);
            enemyAnimator = enemyObject.GetComponent<Animator>();
        }

        if (battleMusicSource != null)
            battleMusicSource.Stop();

        if (foolAroundAudioSource != null)
            foolAroundAudioSource.Stop();

        if (shootAudioSource != null)
            shootAudioSource.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(triggerTag))
        {
            StartEncounter();

            if (resultManager != null)
                resultManager.StartTimer();

            HandleMusicTransition();
        }
    }

    private void HandleMusicTransition()
    {
        if (currentMusicSource != null && currentMusicSource.isPlaying)
            currentMusicSource.Stop();

        if (battleMusicSource != null)
            battleMusicSource.Play();
    }

    private void StartEncounter()
    {
        if (playerCamera != null) playerCamera.SetActive(false);
        if (cinematicCamera != null) cinematicCamera.SetActive(true);

        if (activateOnEntry != null)
            activateOnEntry.SetActive(true);

        if (enemyObject != null)
        {
            enemyObject.SetActive(true);
            if (enemyAnimator != null)
            {
                enemyAnimator.Play(entryAnimation, 0, 0f);
                StartCoroutine(WaitForEntryToFinish());
            }
        }
    }

    private IEnumerator WaitForEntryToFinish()
    {
        yield return new WaitUntil(() => enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName(entryAnimation));
        yield return new WaitForSeconds(enemyAnimator.GetCurrentAnimatorStateInfo(0).length);

        Collider col = enemyObject.GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        if (cinematicCamera != null) cinematicCamera.SetActive(false);
        if (playerCamera != null) playerCamera.SetActive(true);

        StartCoroutine(CombatLoop());
    }

    private IEnumerator CombatLoop()
    {
        inCombatLoop = true;

        while (inCombatLoop)
        {
            // Attack animation
            enemyAnimator.Play(attackAnimation, 0, 0f);
            yield return new WaitUntil(() => enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName(attackAnimation));

            // Spawn projectiles
            if (projectileSpawner != null && projectileSpawner.spawnPoints.Count > 0 && projectileSpawner.projectilePrefab != null)
            {
                foreach (var spawnPoint in projectileSpawner.spawnPoints)
                {
                    GameObject proj = Instantiate(projectileSpawner.projectilePrefab, spawnPoint.position, Quaternion.identity);

                    // Spiral rotation
                    proj.transform.Rotate(Vector3.up, currentSpiralAngle);
                    currentSpiralAngle += spiralAngleIncrement;

                    // Target player
                    Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;
                    if (player != null)
                    {
                        Vector3 targetPos = player.position;
                        proj.transform.LookAt(targetPos);
                        var tp = proj.GetComponent<TargetedProjectile>();
                        if (tp != null)
                            tp.targetPosition = targetPos;
                    }

                    // Play shoot audio
                    if (shootAudioSource != null)
                        shootAudioSource.Play();

                    yield return new WaitForSeconds(projectileSpawner.spawnDelay);
                }
            }

            yield return new WaitForSeconds(enemyAnimator.GetCurrentAnimatorStateInfo(0).length);

            // FoolAround
            if (foolAroundAudioSource != null)
                foolAroundAudioSource.Play();

            enemyAnimator.Play(foolAroundAnimation, 0, 0f);
            yield return new WaitUntil(() => enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName(foolAroundAnimation));
            yield return new WaitForSeconds(enemyAnimator.GetCurrentAnimatorStateInfo(0).length);
        }
    }
}
