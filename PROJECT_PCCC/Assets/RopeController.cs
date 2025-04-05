using UnityEngine;
using Obi;
// using System.Numerics; // Đảm bảo đã import thư viện Obi

public class RopeController : MonoBehaviour
{

    public Transform point1;
    public Transform point2;
    public GameObject ropePrefab; // Prefab của dây thừng
    public ObiSolver solver;

    private void Start()
    {
        SpawnRope();
    }

    public void SpawnRope()
    {
        if (ropePrefab == null || point1 == null || point2 == null || solver == null)
        {
            Debug.LogError("Thiếu tham chiếu trong RopeController!");
            return;
        }

        // Tạo instance của rope
        GameObject currentRope = Instantiate(ropePrefab);

        // Lấy ObiRope và gán solver
        ObiRope rope = currentRope.GetComponent<ObiRope>();
        rope.transform.parent = solver.transform; // Đặt rope làm con của solver

        // Gán blueprint cho rope
        rope.ropeBlueprint = rope.ropeBlueprint; // Hoặc gán blueprint cụ thể nếu cần
        // rope.RebuildConstraints(); // Đảm bảo các ràng buộc được khởi tạo

        // Lấy danh sách attachments
        ObiParticleAttachment[] attachments = currentRope.GetComponents<ObiParticleAttachment>();
        if (attachments.Length < 2)
        {
            Debug.LogError("Rope prefab cần có 2 ObiParticleAttachment!");
            return;
        }

        // Gắn đầu dây 1 vào point1
        attachments[0].target = point1;
        attachments[0].attachmentType = ObiParticleAttachment.AttachmentType.Static;

        // Gắn đầu dây 2 vào point2
        attachments[1].target = point2;
        attachments[1].attachmentType = ObiParticleAttachment.AttachmentType.Static;

        // Kéo dây ra giữa 2 điểm (tránh dây bị cụm)
        var solverIndices = rope.solverIndices;

        Debug.Log($"Solver Indices Count: {solverIndices.count}");
        if (solverIndices.count < 2)
        {
            Debug.LogError("Rope blueprint không đủ hạt để thiết lập!");
            return;
        }

        // Đặt vị trí hạt đầu tiên và cuối cùng
        Vector3 point1Position = point1.position;
        Vector3 point2Position = point2.position;

        // Di chuyển hạt đầu tiên (đầu dây) đến vị trí point1
        solver.positions[solverIndices[0]] = point1Position;

        // Di chuyển hạt cuối cùng (cuối dây) đến vị trí point2
        solver.positions[solverIndices[solverIndices.count - 1]] = point2Position;

        // Cập nhật dữ liệu hạt
        rope.UpdateParticleProperties();

        // Đặt lại trạng thái dây để đảm bảo không bị cụm
        rope.ResetParticles();
    }
}
