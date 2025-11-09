using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [Header("Resources")]
    public Resource pollen = new Resource("Pollen");
    public Resource nectar = new Resource("Nectar");
    public Resource honey = new Resource("Honey");

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat("pollen", pollen.amount);
        PlayerPrefs.SetFloat("nectar", nectar.amount);
        PlayerPrefs.SetFloat("honey", honey.amount);
    }

    private void Start()
    {
        pollen.amount = PlayerPrefs.GetFloat("pollen", 0);
        nectar.amount = PlayerPrefs.GetFloat("nectar", 0);
        honey.amount = PlayerPrefs.GetFloat("honey", 0);
        UpdateUI();
    }

    public void AddPollen(float value)
    {

        pollen.Add(value);
        UpdateUI();
    }

    public void AddNectar(float value)
    {
        nectar.Add(value);
        UpdateUI();
    }
    public float GetNectar()
    {
        return nectar.amount;
    }

    public void AddHoney(float value)
    {
        honey.Add(value);
        UpdateUI();
    }

    public void UseNectar(float value)
    {
        nectar.Subtract(value);
        UpdateUI();
    }

    #region DEBUG

    [SerializeField] public TextMeshProUGUI nectarInventory;
    [SerializeField] public TextMeshProUGUI pollenInventory;
    public void ShowPollenNectar()
    {
        Debug.Log("Pollen: " + pollen.amount);
        Debug.Log("Nectar: " + nectar.amount);
    }

    public void DeleteData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        pollen.amount = 0;
        nectar.amount = 0;
        honey.amount = 0;
        Debug.Log("Data cleamed");

    }
    void UpdateUI()
    {
        // nectarInventory.text = "Nectar: "+nectar.amount.ToString();
        // pollenInventory.text = "Pollen: "+pollen.amount.ToString();
        int roundedNectar = Mathf.RoundToInt(nectar.amount);
        int roundedPollen = Mathf.RoundToInt(pollen.amount);

        nectarInventory.text = $"Nectar: {roundedNectar}";
        pollenInventory.text = $"Pollen: {roundedPollen}";
    }

    #endregion
}
