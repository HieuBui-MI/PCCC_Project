using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    public GameObject unbrokenObject;
    public GameObject brokenObject;
    private void Awake() {
        
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Beinteracted(){
        unbrokenObject.SetActive(false);
        brokenObject.SetActive(true);
    }
}
