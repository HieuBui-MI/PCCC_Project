using UnityEngine;

public class Tool : MonoBehaviour
{
    public enum ToolType { none, FireAxe, Ladder, Bucket, FireHose, FireExtinguisher, SledgeHammer };
    public ToolType toolType = ToolType.none;
}
