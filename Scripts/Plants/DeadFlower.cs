using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class DeadFlower : MonoBehaviour
{
    [System.Serializable]
    public struct PollenRequirement
    {
        public PollenType type;
        public float amount;
        public Color displayColor;
    }

    [Header("Required Pollens")]
    public List<PollenRequirement> requiredPollens = new List<PollenRequirement>();

    [Header("Settings")]
    public GameObject revivedPrefab;
    public float transitionDuration = 1.5f;
    public ParticleSystem evolutionParticles;

    [Header("UI")]
    public Canvas requirementsCanvas;
    public TextMeshProUGUI requirementsText;

    private Dictionary<PollenType, float> progress = new Dictionary<PollenType, float>();
    private bool playerInRange = false;
    private bool evolving = false;

    void Awake()
    {
        foreach (var req in requiredPollens)
        {
            if (!progress.ContainsKey(req.type))
                progress.Add(req.type, 0f);
        }

        if (requirementsCanvas != null)
            requirementsCanvas.enabled = false;

        UpdateRequirementsText();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        playerInRange = true;

        if (requirementsCanvas != null)
            requirementsCanvas.enabled = true;

        UpdateRequirementsText();

        if (InGameUIController.Instance != null)
        {
            InGameUIController.Instance.ShowPollinateButton(true);
            InGameUIController.Instance.currentFlower = this;
        }


        // foreach (var req in requiredPollens)
        // {
        //     Debug.Log($"   - {req.type}: precisa {req.amount}, já contribuiu {progress[req.type]}");
        // }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        playerInRange = false;

        if (requirementsCanvas != null)
            requirementsCanvas.enabled = false;

        if (InGameUIController.Instance != null)
        {
            InGameUIController.Instance.ShowPollinateButton(false);
            InGameUIController.Instance.currentFlower = null;
        }
    }

    public void Pollinate()
    {
        // if (evolving)
        // {
        //     Debug.Log("Flower is evolving");
        //     return;
        // }

        // if (!playerInRange)
        // {
        //     Debug.Log("🚫 Jogador não está próximo da flor!");
        //     return;
        // }

        bool anyUsed = false;

        foreach (var req in requiredPollens)
        {
            float needed = req.amount - progress[req.type];
            if (needed <= 0f) continue;
            if (BeeInventory.Instance.pollenResources.TryGetValue(req.type, out Resource resource))
            {
                float available = resource.amount;
                if (available <= 0f) continue;

                float use = Mathf.Min(available, needed);
                resource.Subtract(use);
                progress[req.type] += use;
                anyUsed = true;

                BeeInventory.Instance.RecalculateStoredResource();
            }
        }

        UpdateRequirementsText();

        if (IsComplete())
            StartCoroutine(SimpleEvolveFlower());

        if (!anyUsed)
            Debug.Log("Polen disponível");
    }

    private bool IsComplete()
    {
        foreach (var req in requiredPollens)
            if (progress[req.type] < req.amount)
                return false;
        return true;
    }

    private IEnumerator SimpleEvolveFlower()
    {
        evolving = true;

        if (evolutionParticles != null)
            evolutionParticles.Play();

        yield return new WaitForSeconds(transitionDuration);

        if (revivedPrefab != null)
            Instantiate(revivedPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    private void UpdateRequirementsText()
    {
        if (requirementsText == null) return;

        var sb = new System.Text.StringBuilder();

        foreach (var req in requiredPollens)
        {
            float remaining = Mathf.Max(0f, req.amount - progress[req.type]);
            int display = Mathf.RoundToInt(remaining);

            string hex = ColorUtility.ToHtmlStringRGB(req.displayColor);
            sb.Append($"<color=#{hex}>{req.type}: {display}</color>");
            sb.AppendLine();
        }

        requirementsText.text = sb.ToString().TrimEnd();
    }
}
