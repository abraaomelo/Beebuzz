using UnityEngine;

public class BeeHiveTank : MonoBehaviour
{
    public static BeeHiveTank Instance;

    [Header("Hive Tank Storage")]
    public float nectarStored;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            LoadTankData();   // <-- CARREGA AO INICIAR
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddNectar(float amount)
    {
        nectarStored += amount;
        Debug.Log($"[HiveTank] Added {amount} nectar. Total: {nectarStored}");

        SaveTankData(); 
    }

    public void SaveTankData()
    {
        PlayerPrefs.SetFloat("Hive_NectarStored", nectarStored);
        PlayerPrefs.Save();
        Debug.Log($"[HiveTank] Saved tank data: {nectarStored}");
    }

    private void LoadTankData()
    {
        nectarStored = PlayerPrefs.GetFloat("Hive_NectarStored", 0f);
    }

    #region UI BUTTON
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (InGameUIController.Instance != null)
        {
            InGameUIController.Instance.ShowStoreNectarButton(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (InGameUIController.Instance != null)
        {
            InGameUIController.Instance.ShowStoreNectarButton(false);
        }
    }
    #endregion
}
