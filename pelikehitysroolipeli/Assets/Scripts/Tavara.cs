using UnityEngine;

public class Tavara : MonoBehaviour
{
    [SerializeField] private string itemName;
    [SerializeField] private float weight;
    [SerializeField] private float volume;

    public string Name => itemName;
    public float Weight => weight;
    public float Volume => volume;

    public Tavara MakeCopy()
    {
        return (Tavara)this.MemberwiseClone();
    }
}