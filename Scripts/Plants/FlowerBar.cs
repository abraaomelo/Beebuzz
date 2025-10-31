using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FlowerBar : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float animateDuration = 0.25f;
    [SerializeField] private Gradient barColor;

    private Image fillImage;
    private float currentFill = 0f;
    private Coroutine runningCoroutine;

    void Awake()
    {
        fillImage = GetComponent<Image>();
    }

    void Start()
    {
        if (fillImage != null)
        {
            currentFill = fillImage.fillAmount;
            UpdateColor(currentFill);
        }
    }
    public void SetImmediate(float normalized)
    {
        normalized = Mathf.Clamp01(normalized);
        currentFill = normalized;

        if (fillImage != null)
        {
            fillImage.fillAmount = currentFill;
            UpdateColor(currentFill);
        }
    }
    public void AnimateTo(float normalized)
    {
        normalized = Mathf.Clamp01(normalized);

        if (runningCoroutine != null)
        {
            StopCoroutine(runningCoroutine);
            runningCoroutine = null;
        }

        runningCoroutine = StartCoroutine(AnimateFillCoroutine(currentFill, normalized, animateDuration));
    }

    private IEnumerator AnimateFillCoroutine(float from, float to, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float p = Mathf.Clamp01(t / duration);
            float value = Mathf.Lerp(from, to, p);
            currentFill = value;
            if (fillImage != null)
            {
                fillImage.fillAmount = currentFill;
                UpdateColor(currentFill);
            }
            yield return null;
        }
        currentFill = to;
        if (fillImage != null)
        {
            fillImage.fillAmount = currentFill;
            UpdateColor(currentFill);
        }

        runningCoroutine = null;
    }

    private void UpdateColor(float normalized)
    {
        if (barColor != null && fillImage != null)
        {
            fillImage.color = barColor.Evaluate(normalized);
        }
    }
}
