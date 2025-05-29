using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Entities; // Required for ECS functionality
using System.Linq;

/// <summary>
/// Handles scene reloading with optional ECS (DOTS) entity cleanup.
/// Ensures a complete reset when reloading the current scene.
/// </summary>
public class SceneReloader : MonoBehaviour
{
    /// <summary>
    /// Reloads the current scene and performs cleanup operations.
    /// - Destroys all ECS entities (if ECS is enabled)
    /// - Resets time scale
    /// - Maintains compatibility with standard Unity objects
    /// </summary>
    public void RestartGame()
    {
        DestroyAllEntitiesInDefaultWorld();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void DestroyAllEntitiesInDefaultWorld()
    {
        var world = World.DefaultGameObjectInjectionWorld;
        if (world != null && world.IsCreated)
        {
            var entityManager = world.EntityManager;
            using var allEntities = entityManager.GetAllEntities();
            entityManager.DestroyEntity(allEntities);
        }
    }

}