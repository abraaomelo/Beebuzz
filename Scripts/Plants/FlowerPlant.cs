using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FlowerPlant : MonoBehaviour
{

    [Header("Specifics")]
    public float collectedNectar;
    public float collectedPollen;
    [SerializeField] public PollenType pollenType; 
    [SerializeField] public float nectarCapacity = 5f;
    [SerializeField] public float pollenCapacity = 5f;


    [Header("Flower Settings")]
    [Range(0f, 100f)] public float nectarAmount = 1f;
    [Range(0f, 100f)] public float pollenAmount = 1f;
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
        if (playerInRange && readyToCollect && !BeeInventory.Instance.ReachedMaxLoad())
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
        if (!readyToCollect || producingNectar)
        return;

        float drain = collectSpeed * Time.deltaTime;
        nectarAmount -= drain;
        pollenAmount -= drain;

        nectarAmount = Mathf.Clamp01(nectarAmount);
        pollenAmount = Mathf.Clamp01(pollenAmount);

        float drainedNectarValue = nectarCapacity * drain;
        float drainedPollenValue = pollenCapacity * drain;

        collectedNectar += drainedNectarValue;
        collectedPollen += drainedPollenValue;

        BeeInventory.Instance.AddNectar(drainedNectarValue);
        BeeInventory.Instance.AddPollen(pollenType, drainedPollenValue);

        UpdateParticleEmission();

        if (nectarAmount <= 0f)
        {
            readyToCollect = false;
            producingNectar = true;

            if (regenCoroutine != null)
                StopCoroutine(regenCoroutine);
            regenCoroutine = StartCoroutine(RegenerateFlower());

            collectedNectar = 0f;
            collectedPollen = 0f;
        }
    }

    IEnumerator RegenerateFlower()
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
}
