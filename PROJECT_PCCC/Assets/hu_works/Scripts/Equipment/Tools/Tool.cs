using UnityEngine;

public class Tool : MonoBehaviour
{
    public enum ToolType { none, FireAxe, Ladder, Bucket, FireHose, FireExtinguisher };
    public ToolType toolType = ToolType.none;
}
