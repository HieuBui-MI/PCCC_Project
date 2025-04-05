using UnityEngine;

public class FireHydrant : MonoBehaviour
{
    public bool isConnectedToFireTruck = false;
    public GameObject objConnectedTo;

    private void Update() {
        ConnectedChecker();
    }

    void ConnectedChecker() {
        isConnectedToFireTruck = objConnectedTo != null;
    }
}
