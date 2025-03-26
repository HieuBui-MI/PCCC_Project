using UnityEngine;

public class CableBuilder : MonoBehaviour
{
    [Header("Settings")]
    public GameObject segmentPrefab;
    public int segmentCount = 10;
    public float segmentLength = 0.5f;
    public float cableWidth = 0.1f;
    public Transform startAnchor;
    public Transform endAnchor;

    void Start()
    {
        GenerateCable();
    }

    void GenerateCable()
    {
        Transform previousSegment = startAnchor;
        
        for (int i = 0; i < segmentCount; i++)
        {
            // Tạo segment mới
            GameObject newSegment = Instantiate(segmentPrefab, transform);
            newSegment.name = "Segment_" + i;
            
            // Đặt vị trí và kích thước
            newSegment.transform.position = startAnchor.position + Vector3.down * i * segmentLength;
            newSegment.transform.localScale = new Vector3(cableWidth, segmentLength, cableWidth);
            
            // Cấu hình Rigidbody
            Rigidbody rb = newSegment.GetComponent<Rigidbody>();
            
            // Thêm Configurable Joint
            ConfigurableJoint joint = newSegment.AddComponent<ConfigurableJoint>();
            joint.connectedBody = previousSegment.GetComponent<Rigidbody>();
            
            // Cấu hình Joint
            ConfigureJoint(joint);
            
            previousSegment = newSegment.transform;
        }

        // Kết nối segment cuối với endAnchor
        ConfigurableJoint endJoint = endAnchor.gameObject.AddComponent<ConfigurableJoint>();
        endJoint.connectedBody = previousSegment.GetComponent<Rigidbody>();
        ConfigureJoint(endJoint);
    }

    void ConfigureJoint(ConfigurableJoint joint)
    {
        // Cấu hình Anchor
        joint.anchor = Vector3.zero;
        joint.connectedAnchor = new Vector3(0, -segmentLength/2, 0);
        
        // Giới hạn chuyển động
        joint.xMotion = ConfigurableJointMotion.Locked;
        joint.yMotion = ConfigurableJointMotion.Locked;
        joint.zMotion = ConfigurableJointMotion.Locked;
        
        // Cho phép xoay
        joint.angularXMotion = ConfigurableJointMotion.Free;
        joint.angularYMotion = ConfigurableJointMotion.Free;
        joint.angularZMotion = ConfigurableJointMotion.Free;
        
        // Thiết lập giới hạn xoay
        SoftJointLimit limit = new SoftJointLimit();
        limit.limit = 30f;
        joint.angularZLimit = limit;
        joint.angularYLimit = limit;
        
        // Thêm độ đàn hồi
        JointDrive drive = new JointDrive();
        drive.positionSpring = 100f;
        drive.positionDamper = 10f;
        drive.maximumForce = Mathf.Infinity;
        
        joint.angularXDrive = drive;
        joint.angularYZDrive = drive;
    }
}