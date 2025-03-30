using UnityEngine;
using Obi;

public class ShootFireHose : MonoBehaviour
{
    [SerializeField] public GameObject PlayerCapsule;
    [SerializeField] public GameObject ObiSolver;
    [SerializeField] public GameObject ObiEmitter;
    private ObiEmitter obiEmitterComponent;
    private float targetSpeed = 0f;
    private float speedIncreaseRate = 10f;
    private float speedDecreaseRate = 20f;

    void Start()
    {
        if (ObiEmitter != null)
        {
            // Kiểm tra và lấy component ObiEmitter
            obiEmitterComponent = ObiEmitter.GetComponent<ObiEmitter>();
            if (obiEmitterComponent != null)
            {
                Debug.Log("ObiEmitter contains the 'ObiEmitter' component.");
                obiEmitterComponent.speed = 20f; // Đặt speed ban đầu là 0
            }
            else
            {
                Debug.LogWarning("ObiEmitter does not contain the 'ObiEmitter' component.");
            }
        }
        else
        {
            Debug.LogError("ObiEmitter GameObject is not assigned.");
        }
    }

    // Update is called once per frame
    // void Update()
    // {
    //     if (ObiSolver != null)
    //     {
    //         ObiSolver.transform.rotation = Quaternion.identity;
    //     }

    //     if (PlayerCapsule != null && ObiEmitter != null)
    //     {
    //         ObiEmitter.transform.rotation = PlayerCapsule.transform.rotation;
    //     }

    //     if (obiEmitterComponent != null)
    //     {
    //         if (Input.GetMouseButton(0)) 
    //         {
    //             obiEmitterComponent.speed = Mathf.MoveTowards(obiEmitterComponent.speed, targetSpeed, speedIncreaseRate * Time.deltaTime);
    //         }
    //         else // Thả chuột trái
    //         {
    //             obiEmitterComponent.speed = Mathf.MoveTowards(obiEmitterComponent.speed, targetSpeed, speedDecreaseRate * Time.deltaTime);
    //         }
    //     }
    // }

    void Update()
    {
        if (ObiSolver != null)
        {
            ObiSolver.transform.rotation = Quaternion.identity;
        }

        if (PlayerCapsule != null && ObiEmitter != null)
        {
            ObiEmitter.transform.rotation = PlayerCapsule.transform.rotation;
        }

        if (obiEmitterComponent != null)
        {
            obiEmitterComponent.speed = 20f; // Luôn đặt tốc độ là 20 để test
        }
    }
}
