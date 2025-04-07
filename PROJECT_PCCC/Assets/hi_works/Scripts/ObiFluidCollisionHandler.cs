using UnityEngine;
using Obi;
using System.Collections.Generic; // Cần thiết cho danh sách (List)


public class ObiFluidCollisionHandler : MonoBehaviour
{
    private ObiSolver solver;
    void Start()
    {
        solver = GetComponent<ObiSolver>();
    }

}
