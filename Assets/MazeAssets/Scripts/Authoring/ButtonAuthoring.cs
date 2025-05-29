using Unity.Entities;
using UnityEngine;


// MonoBehaviour for configuring button entities in the Unity Editor
public class ButtonAuthoring : MonoBehaviour
{
    // Reference to the platform GameObject this button controls
    public GameObject connectedButton;

    // Handles conversion from GameObject to Entity
    public class ButtonTriggerBaker : Baker<ButtonAuthoring>
    {
        public override void Bake(ButtonAuthoring authoring)
        {
            // Create entity for this button
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            // Get entity reference for the connected platform
            var platformEntity = GetEntity(authoring.connectedButton, TransformUsageFlags.Dynamic);

            // Add trigger component that links to the platform
            AddComponent(entity, new ButtonTrigger
            {
                PlatformEntity = platformEntity
            });
        }
    }
}

// ECS component that stores reference to a connected platform entity
public struct ButtonTrigger : IComponentData
{
    // Entity reference to the platform this button controls
    public Entity PlatformEntity;
}