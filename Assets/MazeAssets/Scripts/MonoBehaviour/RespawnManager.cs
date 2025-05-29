using Unity.XR.CoreUtils;               
using UnityEngine;                      
using UnityEngine.XR.Interaction.Toolkit; 

/// <summary>
/// Manages player respawn functionality for XR rigs
/// </summary>
public class RespawnManager : MonoBehaviour
{
    [SerializeField] private     XROrigin xrRig;       // The XR Origin component (new XR system)
    [SerializeField] private GameObject xrRig_GO;  // The actual GameObject containing the XR Rig
    [SerializeField] private Transform respawnPoint;                // The designated respawn location

    /// <summary>
    /// Moves the XR rig to a specified position in world space
    /// </summary>
    /// <param name="newPosition">The target world position to move to</param>
    public void MoveXRRig(Vector3 newPosition)
    {
        if (xrRig != null)
        {
           
            // Directly sets the XR Rig's position
            xrRig_GO.transform.position = newPosition;
        }
    }

    /// <summary>
    /// Trigger event handler for respawning the player
    /// </summary>
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
 
            // Move player to the designated respawn point
            MoveXRRig(respawnPoint.transform.position);
        }
    }
}