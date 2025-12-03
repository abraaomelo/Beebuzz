using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BeeInventoryUI : MonoBehaviour
{
    public GameObject InventoryCanvas;
    public GameObject IdentityCard;
    [SerializeField] public TextMeshProUGUI nectarInventory;
    [SerializeField] public TextMeshProUGUI pollenInventory;
    private float totalPollen;

    [Header("Pollen UI")]
    public TextMeshProUGUI YellowPollen;
    public TextMeshProUGUI RedPollen;
    public TextMeshProUGUI BluePollen;
    public TextMeshProUGUI OrangePollen;
    public TextMeshProUGUI PurplePollen;
    public TextMeshProUGUI GreenPollen;

    private Dictionary<PollenType, TextMeshProUGUI> pollenTextRefs;

    private void Awake()
    {
        // liga cada pollen a um Text
        pollenTextRefs = new Dictionary<PollenType, TextMeshProUGUI>()
        {
            { PollenType.Yellow, YellowPollen },
            { PollenType.Red, RedPollen },
            { PollenType.Blue, BluePollen },
            { PollenType.Orange, OrangePollen },
            { PollenType.Purple, PurplePollen },
            { PollenType.Green, GreenPollen }
        };
    }

    public void ClickButton()
    {
        IdentityCard.SetActive(false);
        DeactivateButton();

        // nectar
        int roundedNectar = Mathf.RoundToInt(BeeInventory.Instance.GetNectar());
        nectarInventory.text = "Collected    Nectar          " + roundedNectar;

        // pollen
        foreach (var entry in pollenTextRefs)
        {
            PollenType type = entry.Key;
            TextMeshProUGUI text = entry.Value;
            if (text == null) continue;

            if (BeeInventory.Instance.pollenResources.TryGetValue(type, out Resource resource))
            {
                int amount = Mathf.RoundToInt(resource.amount);
                totalPollen += amount;
                text.text = $"{type} Pollen: {amount}";
            }
            else
            {
                text.text = $"{type} Pollen: 0";
            }
        }
        int roundedPollen = Mathf.RoundToInt(totalPollen);
        pollenInventory.text = "Collected Pollen: " + roundedPollen;
    }

    void DeactivateButton()
    {
        InventoryCanvas.SetActive(true);
    }

    public void ExitButton()
    {
        InventoryCanvas.SetActive(false);
        IdentityCard.SetActive(true);
    }
}
