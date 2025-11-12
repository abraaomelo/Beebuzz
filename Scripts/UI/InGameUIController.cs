using UnityEngine;
using UnityEngine.UI;

public class InGameUIController : MonoBehaviour
{
    public static InGameUIController Instance;

    [Header("UI Elements")]
    public Button pollinateButton;

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

        if (pollinateButton != null)
        {
            pollinateButton.onClick.AddListener(() =>
            {
                if (currentFlower != null)
                {
                    currentFlower.Pollinate();
                }
                else
                {
                    Debug.Log("Nenhuma flor próxima para polinizar.");
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
}
