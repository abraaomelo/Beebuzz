using UnityEngine;
using System.Collections;

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
    [SerializeField] public ParticleSystem pollenParticles;
    private ParticleSystem.EmissionModule emissionModule;

    private bool playerInRange = false;

    void Start()
    {
        if (pollenParticles == null)
        {
            pollenParticles = GetComponentInChildren<ParticleSystem>();
        }
        emissionModule = pollenParticles.emission;
        UpdateParticleEmission();
    }

    void Update()
    {
        if (playerInRange && readyToCollect)
        {
            CollectOverTime();
        }
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
            StartCoroutine(RegenerateNectar());
        }
    }

    IEnumerator RegenerateNectar()
    {
        emissionModule.rateOverTime = 0;
        yield return new WaitForSeconds(regenerationTime);

        nectarAmount = 1f;
        producingNectar = false;
        readyToCollect = true;
        UpdateParticleEmission();
    }

    void UpdateParticleEmission()
    {
        float maxRate = 50f; 
        emissionModule.rateOverTime = nectarAmount * maxRate;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
