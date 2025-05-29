using System;
using Unity.Entities;
using UnityEngine;


/// <summary>
/// MonoBehaviour for authoring/platform configuration in the Unity Editor.
/// Defines platform movement parameters that will be baked into ECS components.
/// </summary>
public class PlatformsAuthoring : MonoBehaviour
{
    public float openingTime = 1.0f;
    private float finalDisplacementY = 17f;

    /// <summary>
    /// Baker class that converts MonoBehaviour data to ECS components.
    /// Runs during the baking process (conversion from GameObject to Entity).
    /// </summary>
    public class PlatformBaker : Baker<PlatformsAuthoring>
    {
        // Create entity with transform usage flags
        public override void Bake(PlatformsAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            // Add Platform component with configured values
            AddComponent(entity, new Platform
            {
                OpeningTime = authoring.openingTime,
                FinalDisplacementY = authoring.finalDisplacementY,
            });
        }
    }
}

/// <summary>
/// ECS component storing platform movement state and configuration.
/// Used by systems to animate platform movement.
/// </summary>
public struct Platform : IComponentData
{
    // Flag to trigger movement
    public bool Opening;

    // Duration of movement animation
    public float OpeningTime;

    // Tracks progress through movement
    public float CurrentTimer;

    // Target vertical displacement
    public float FinalDisplacementY;
}