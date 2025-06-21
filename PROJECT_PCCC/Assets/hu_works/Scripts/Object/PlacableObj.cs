using UnityEngine;

public class PlacableObj : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public enum CarriableType { None, Victim, Object };
    public CarriableType carriableType = CarriableType.None;
}
