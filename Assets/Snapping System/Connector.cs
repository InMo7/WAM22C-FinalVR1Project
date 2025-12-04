using UnityEngine;

public class PositiveEnd : MonoBehaviour
{
    [Tooltip("Whether to align the rotation of the connecting pieces so the connector forwards oppose each other.")]
    public bool alignRotation = true;

    private bool isConnected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isConnected) return;

        NegativeEnd negativeEnd = other.GetComponent<NegativeEnd>();
        if (negativeEnd != null && !negativeEnd.IsConnected)
        {
            ConnectTo(negativeEnd);
        }
    }

    private void ConnectTo(NegativeEnd negativeEnd)
    {
        // Assume the connectors are children of the main pieces to connect
        Transform myPiece = transform.parent;
        Transform otherPiece = negativeEnd.transform.parent;

        if (myPiece == null || otherPiece == null)
        {
            Debug.LogWarning("Connector must be a child of the piece GameObject.");
            return;
        }

        // Snap position: Move my piece so the positive connector coincides with the negative one
        Vector3 positionDelta = negativeEnd.transform.position - transform.position;
        myPiece.position += positionDelta;

        // Optionally snap rotation: Align so the forward directions oppose each other (like plug and socket)
        if (alignRotation)
        {
            Quaternion rotationDelta = Quaternion.FromToRotation(transform.forward, -negativeEnd.transform.forward);
            myPiece.rotation = rotationDelta * myPiece.rotation;
        }

        // Physically connect using a FixedJoint (assumes both pieces have Rigidbody components)
        Rigidbody myRB = myPiece.GetComponent<Rigidbody>();
        Rigidbody otherRB = otherPiece.GetComponent<Rigidbody>();

        if (myRB == null || otherRB == null)
        {
            Debug.LogWarning("Both connecting pieces need Rigidbody components for physical connection.");
            // Fallback: Parent one to the other without physics
            myPiece.SetParent(otherPiece);
        }
        else
        {
            FixedJoint joint = myPiece.gameObject.AddComponent<FixedJoint>();
            joint.connectedBody = otherRB;
        }

        // Mark as connected to prevent multiple snaps
        isConnected = true;
        negativeEnd.SetConnected(true);

        Debug.Log("Snapped together: " + myPiece.name + " to " + otherPiece.name);
    }
}