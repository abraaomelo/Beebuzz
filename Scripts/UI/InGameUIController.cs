using UnityEngine;
using UnityEngine.UI;

public class InGameUIController : MonoBehaviour
{
    public static InGameUIController Instance;

    [Header("UI Elements")]
    public Button pollinateButton;
    public Button storeNectarButton;

    [HideInInspector] public DeadFlower currentFlower;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        ShowPollinateButton(false);
        ShowStoreNectarButton(false);

        if (pollinateButton != null)
        {
            pollinateButton.onClick.AddListener(() =>
            {
                if (currentFlower != null)
                {
                    currentFlower.Pollinate();
                }
            });
        }
    }

    public void ShowPollinateButton(bool state)
    {
        if (pollinateButton != null)
        {
            pollinateButton.gameObject.SetActive(state);
        }
    }

    public void ShowStoreNectarButton(bool state)
    {
        if (storeNectarButton != null)
        {
            storeNectarButton.gameObject.SetActive(state);
        }
    }
}
