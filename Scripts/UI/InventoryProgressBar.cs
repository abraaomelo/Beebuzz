using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryProgressBar : MonoBehaviour
{
    public static InventoryProgressBar Instance;

    public Image fillImage;
    public bool showPercent = true;
    public bool showAbsolute = true;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        UpdateBar();
    }

    public void UpdateBar()
    {
        if (BeeInventory.Instance == null) return;

        float stored = BeeInventory.Instance.storedResource;
        float max = BeeInventory.Instance.maxCapacity;

        if (max <= 0) max = 1f;

        float ratio = Mathf.Clamp01(stored / max);

        if (fillImage != null)
            fillImage.fillAmount = ratio;
    }
}
