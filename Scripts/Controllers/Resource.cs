using UnityEngine;

[System.Serializable]
public class Resource
{
    public string resourceName;
    public float amount;

    public Resource(string name, float startAmount = 0)
    {
        resourceName = name;
        amount = startAmount;
    }

    public void Add(float value)
    {
        amount += value;
    }

    public void Subtract(float value)
    {
        amount = Mathf.Max(0, amount - value);
    }
}
