using TMPro;
using UnityEngine;

public class NectarTankUI : MonoBehaviour
{
    public TextMeshProUGUI nectarText = null;

    void Start()
    {
        float nectarStored = PlayerPrefs.GetFloat("Hive_NectarStored", 0f);
        int rounded = Mathf.RoundToInt(nectarStored);
        nectarText.text = $"Nectar amount:   {rounded}";
    }

    void Update()
    {
        if (BeeHiveTank.Instance != null)
        {
            float amount = BeeHiveTank.Instance.nectarStored;
            int rounded = Mathf.RoundToInt(amount);
            nectarText.text = $"Nectar amount:   {rounded}";
        }
    }
}
