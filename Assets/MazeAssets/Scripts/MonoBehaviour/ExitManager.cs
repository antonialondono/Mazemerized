using UnityEngine;

/// <summary>
/// Handles application exit/quit functionality
/// </summary>
public class ExitManager : MonoBehaviour
{
  

    /// <summary>
    /// Quits the application (works in built executable)
    /// Note: In Unity Editor, this will stop play mode
    /// </summary>
    public void Exit()
    {
        // Calls Unity's application quit function
        Application.Quit();

    }
}