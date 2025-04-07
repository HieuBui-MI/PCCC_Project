using UnityEngine;

public class PipeConnector : MonoBehaviour
{
    public bool isConnectedToObj = false;
    public GameObject objConnectedTo;
    public bool isConnectToFireHoseOnly = false;

    private void Update() {
        ConnectedChecker();
    }

    void ConnectedChecker() {
        isConnectedToObj = objConnectedTo != null;
    }
}
