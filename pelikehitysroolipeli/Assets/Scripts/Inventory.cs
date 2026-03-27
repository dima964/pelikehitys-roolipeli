using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public List<Tavara> Items { get; private set; } = new List<Tavara>();

    private float maxWeight = 10f;
    private float maxVolume = 10f;

    private float currentWeight = 0f;
    private float currentVolume = 0f;

    public bool AddItem(Tavara item)
    {
        if (currentWeight + item.Weight > maxWeight)
        {
            UnityEngine.Debug.Log("Too heavy!");
            return false;
        }

        if (currentVolume + item.Volume > maxVolume)
        {
            UnityEngine.Debug.Log("Not enough space!");
            return false;
        }

        Items.Add(item);
        currentWeight += item.Weight;
        currentVolume += item.Volume;

        UnityEngine.Debug.Log("Added: " + item.Name);

        return true;
    }
}