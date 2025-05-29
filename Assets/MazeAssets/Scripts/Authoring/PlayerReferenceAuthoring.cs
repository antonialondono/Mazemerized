using System;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

/// <summary>
/// This struct is used to reference a GameObject in the ECS world.
/// It implements IComponentData to be used as a component in ECS.
/// </summary>

public struct PlayerReference : IComponentData
{
    // Reference to the GameObject's PlayerReferenceAuthoring component in the ECS world
    public UnityObjectRef<PlayerReferenceAuthoring> playerRef;
}

/// <summary>
/// Tag component used to identify the player entity.
/// Empty struct as it's only used for identification purposes.
/// </summary>
public struct PlayerTag : IComponentData { }

/// <summary>
/// System that updates the position of ECS entities based on their referenced GameObject's position.
/// </summary>

public partial struct PlayerRefSystem : ISystem
{
    /// <summary>
    /// Called every frame to update entity positions.
    /// </summary>
    public void OnUpdate(ref SystemState state)
    {
        // Query all entities with both PlayerReference and LocalTransform components
        foreach (var (playerReference, localTransform) in SystemAPI.Query<RefRO<PlayerReference>, RefRW<LocalTransform>>())
        {
            // Update the ECS entity's position to match the referenced GameObject's position

            localTransform.ValueRW.Position = playerReference.ValueRO.playerRef.Value.position;
        }
    }
}

/// <summary>
/// This class is used to create a GameObject reference in the ECS world.
/// </summary>
public class PlayerReferenceAuthoring : MonoBehaviour
{
    // Static field (appears unused in this snippet)
    internal static readonly object float3;

    /// <summary>
    /// Returns the current position of this GameObject.
    /// </summary>
    public Vector3 position => transform.position;

    /// <summary>
    /// Called when the GameObject is initialized.
    /// Creates a corresponding ECS entity with reference components.
    /// </summary>
    void Start()
    {
        // Get the default ECS world's entity manager
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;


        // Create a new entity
        Entity player = entityManager.CreateEntity();


        // Add components to the entity:
        // 1. PlayerReference to store the GameObject reference
        // 2. LocalTransform to store the position
        // 3. PlayerTag to identify this as the player entity
        entityManager.AddComponentData(player, new PlayerReference { playerRef = this });
        entityManager.AddComponentData(player, new LocalTransform { Position = position });
        entityManager.AddComponentData(player, new PlayerTag());
    }
}
