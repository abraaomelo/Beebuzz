using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FlowerPlant : MonoBehaviour
{
    [Header("Nectar Settings")]
    [Range(0f, 1f)] public float nectarAmount = 1f;
    public float regenerationTime = 10f;
    public float collectSpeed = 0.2f;

    [Header("States")]
    public bool readyToCollect = true;
    public bool producingNectar = false;

    [Header("Particles")]
    [SerializeField] private ParticleSystem pollenParticles;
    [SerializeField] private PollenAttractor pollenAttractor;
    private ParticleSystem.EmissionModule emissionModule;

    [Header("UI")]
    [SerializeField] private Canvas flowerCanvas;
    [SerializeField] private FlowerBar flowerBar;

    private bool playerInRange = false;
    private Coroutine regenCoroutine;

    void Start()
    {
        if (pollenParticles == null)
            pollenParticles = GetComponentInChildren<ParticleSystem>();

        if (pollenParticles != null)
            emissionModule = pollenParticles.emission;

        UpdateParticleEmission();

        BarInitialState();
    }

    void Update()
    {
        if (playerInRange && readyToCollect)
        {
            CollectOverTime();
        }
    }

    void BarInitialState()
    {
        if (flowerCanvas != null)
            flowerCanvas.enabled = false;
        if (flowerBar != null)
            flowerBar.SetImmediate(1f);
    }

    void CollectOverTime()
    {
        nectarAmount -= collectSpeed * Time.deltaTime;
        nectarAmount = Mathf.Clamp01(nectarAmount);
        UpdateParticleEmission();

        if (nectarAmount <= 0f)
        {
            readyToCollect = false;
            producingNectar = true;

            if (regenCoroutine != null)
                StopCoroutine(regenCoroutine);

            regenCoroutine = StartCoroutine(RegenerateNectar());
        }
    }

    IEnumerator RegenerateNectar()
    {
        if (pollenParticles != null)
            emissionModule.rateOverTime = 0f;

        if (flowerCanvas != null)
            flowerCanvas.enabled = true;

        float elapsed = 0f;
        while (elapsed < regenerationTime)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / regenerationTime); // 0..1

            if (flowerBar != null)
                flowerBar.SetImmediate(progress);

            yield return null;
        }

        nectarAmount = 1f;
        producingNectar = false;
        readyToCollect = true;
        UpdateParticleEmission();

        if (flowerBar != null)
            flowerBar.SetImmediate(1f);

        if (flowerCanvas != null)
            flowerCanvas.enabled = false;

        regenCoroutine = null;
    }

    void UpdateParticleEmission()
    {
        if (pollenParticles == null) return;
        float maxRate = 50f;
        if (readyToCollect)
            emissionModule.rateOverTime = nectarAmount * maxRate;
        else
            emissionModule.rateOverTime = 0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            if (pollenAttractor != null) pollenAttractor.attractionActive = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            if (pollenAttractor != null) pollenAttractor.attractionActive = false;
        }
    }

    public void ReduceNectar(float amount)
    {
        if (!readyToCollect) return;

        nectarAmount -= amount;
        nectarAmount = Mathf.Clamp01(nectarAmount);
        UpdateParticleEmission();

        if (nectarAmount <= 0f)
        {
            readyToCollect = false;
            producingNectar = true;
            if (regenCoroutine != null) StopCoroutine(regenCoroutine);
            regenCoroutine = StartCoroutine(RegenerateNectar());
        }
    }
}
