using System;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;



/// <summary>
/// MonoBehaviour that creates and maintains a corresponding ECS entity
/// </summary>
public class KeyReference : MonoBehaviour
{
    /// <summary>
    /// Current world position of the GameObject
    /// </summary>
    public Vector3 position => transform.position;

    void Start()
    {
        // Get ECS entity manager
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        // Create new entity
        Entity diamondkey = entityManager.CreateEntity();

        // Add components:
        // 1. Reference to this MonoBehaviour
        // 2. Transform with current position
        // 3. DiamondKey tag
        entityManager.AddComponentData(diamondkey, new ComponentReference { gameObjectRef = this });
        entityManager.AddComponentData(diamondkey, new LocalTransform { Position = position });
        entityManager.AddComponentData(diamondkey, new DiamondKey());
    }
}

/// <summary>
/// Component that stores a reference to a Unity GameObject
/// </summary>
public struct ComponentReference : IComponentData
{
    /// <summary>
    /// Wrapped reference to the MonoBehaviour GameObject
    /// </summary>
    public UnityObjectRef<KeyReference> gameObjectRef;

}

/// <summary>
/// Tag component to identify diamond key entities
/// </summary>
public struct DiamondKey : IComponentData { }

/// <summary>
/// System that synchronizes ECS entity positions with their referenced GameObjects
/// </summary>
public partial struct UpdateEntityRefSystem : ISystem
{
    /// <summary>
    /// Updates entity positions to match their linked GameObjects each frame
    /// </summary>
    public void OnUpdate(ref SystemState state)
    {
        // Query all entities with both ComponentReference and LocalTransform
        foreach (var (componentReference, localTransform) in SystemAPI.Query<RefRO<ComponentReference>, RefRW<LocalTransform>>())
        {
            // Sync ECS position with GameObject position
            localTransform.ValueRW.Position = componentReference.ValueRO.gameObjectRef.Value.position;
        }
    }
}
