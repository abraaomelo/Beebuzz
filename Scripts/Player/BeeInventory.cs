using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using System.Text;

public class BeeInventory : MonoBehaviour
{
    [Header("Inventory Capacity")]
    public float maxCapacity = 5000f;
    public float storedResource;

    [Header("Stored Resources")]
    public Dictionary<PollenType, Resource> pollenResources = new Dictionary<PollenType, Resource>();
    public Resource nectar = new Resource("Nectar");
    public Resource honey = new Resource("Honey");
    public static BeeInventory Instance;
    private void Awake()
    {
        InstanceInventory();
    }

    private void Start()
    {
        GetResources();
    }

    private void GetResources()
    {
        nectar.amount = PlayerPrefs.GetFloat("nectar", 0);
        //storedResource = nectar.amount;
        honey.amount = PlayerPrefs.GetFloat("honey", 0);

        foreach (PollenType type in Enum.GetValues(typeof(PollenType)))
        {
            if (pollenResources.TryGetValue(type, out Resource resource))
            {
                resource.amount = PlayerPrefs.GetFloat($"pollen_{type}", 0);
                // storedResource += resource.amount;
            }
        }
        RecalculateStoredResource();
        //UpdateUI();
    }

    public void RecalculateStoredResource()
    {
        storedResource = nectar.amount;

        foreach (PollenType type in Enum.GetValues(typeof(PollenType)))
        {
            if (pollenResources.TryGetValue(type, out Resource resource))
                storedResource += resource.amount;
        }
    }

    public bool ReachedMaxLoad()
    {
        if (storedResource >= maxCapacity) return true;
        else return false;
    }


    void InstanceInventory()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            foreach (PollenType type in Enum.GetValues(typeof(PollenType)))
            {
                pollenResources.Add(type, new Resource($"Pollen_{type}"));
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat("nectar", nectar.amount);
        PlayerPrefs.SetFloat("honey", honey.amount);

        foreach (var kvp in pollenResources)
        {
            PlayerPrefs.SetFloat($"pollen_{kvp.Key}", kvp.Value.amount);
        }
        PlayerPrefs.Save();
    }

    public void SaveInventory()
    {
        PlayerPrefs.SetFloat("nectar", nectar.amount);
        PlayerPrefs.SetFloat("honey", honey.amount);

        foreach (var kvp in pollenResources)
            PlayerPrefs.SetFloat($"pollen_{kvp.Key}", kvp.Value.amount);

        PlayerPrefs.Save();
    }


    public void AddPollen(PollenType type, float value)
    {
        if (pollenResources.TryGetValue(type, out Resource resource))
        {
            resource.Add(value);
            // UpdateUI();
        }
        else
        {
            Debug.LogError($"Attempted to add pollen of unknown type: {type}");
        }
        SaveInventory();
    }
    public void AddNectar(float value)
    {
        nectar.Add(value);
        RecalculateStoredResource();
        SaveInventory();
    }
    public float GetNectar()
    {
        return nectar.amount;
    }

    public void AddHoney(float value)
    {
        honey.Add(value);
        SaveInventory();
    }

    public void UseNectar(float value)
    {
        nectar.Subtract(value);
        SaveInventory();
    }

    public void TransferAllNectarToTank()
    {
        if (nectar.amount <= 0)
        {
            Debug.Log("[Inventory] No nectar to transfer.");
            return;
        }

        float amountToTransfer = nectar.amount;

        if (BeeHiveTank.Instance == null)
        {
            Debug.LogError("[Inventory] HiveTank Instance NOT FOUND!");
            return;
        }
        BeeHiveTank.Instance.AddNectar(amountToTransfer);
        nectar.amount = 0;

        PlayerPrefs.SetFloat("nectar", 0);
        PlayerPrefs.Save();

        RecalculateStoredResource();
        SaveInventory();
    }


    #region DEBUG

    [SerializeField] public TextMeshProUGUI nectarInventory;
    [SerializeField] public TextMeshProUGUI pollenInventory;

    public void ShowPollenNectar()
    {
        Debug.Log("TOTAL carregado: " + storedResource);
        Debug.Log("Bolsa cheia? " + ReachedMaxLoad());
    }

    public void DeleteData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        foreach (var resource in pollenResources.Values)
        {
            resource.amount = 0;
        }
        nectar.amount = 0;
        honey.amount = 0;
        RecalculateStoredResource();
        SaveInventory();
    }
    public void UpdateUI()
    {
        int roundedNectar = Mathf.RoundToInt(nectar.amount);
        nectarInventory.text = $"Nectar: {roundedNectar}";
        StringBuilder pollenTextBuilder = new StringBuilder();

        foreach (var kvp in pollenResources)
        {
            float amount = kvp.Value.amount;
            if (amount > 0)
            {
                int roundedPollen = Mathf.RoundToInt(amount);
                pollenTextBuilder.AppendLine($"{kvp.Key} Pollen: {roundedPollen}");
            }
        }
        if (pollenTextBuilder.Length == 0)
        {
            pollenInventory.text = "Pollen: 0";
        }
        else
        {
            pollenInventory.text = pollenTextBuilder.ToString().Trim();
        }
    }

    public void AddAllPollens()
    {
        float value = 1000f;
        nectar.Add(value);
        foreach (var kvp in pollenResources)
        {
            kvp.Value.Add(value);
        }
        RecalculateStoredResource();
        SaveInventory();
    }
    #endregion 

}
