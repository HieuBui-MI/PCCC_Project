using UnityEngine;

public class WaterSourceConnector : MonoBehaviour
{
    public bool isConnectedToFireHydrant = false;
    public GameObject objConnectedTo;
    private void Update()
    {
        ConnectedChecker();
    }
    void ConnectedChecker()
    {
        isConnectedToFireHydrant = objConnectedTo != null;
    }
}