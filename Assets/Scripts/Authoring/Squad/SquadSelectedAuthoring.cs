using Unity.Entities;
using UnityEngine;


namespace Arenar.PocketFantasyWar
{
    class SquadSelectedAuthoring : MonoBehaviour {}

    class SquadSelectedAuthoringBaker : Baker<SquadSelectedAuthoring>
    {
        public override void Bake(SquadSelectedAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new SquadSelectedComponent()
            {
                onSelectedEvent = false,
                onDeselectedEvent = false,
            });
            
            // make component disabled
            SetComponentEnabled<SquadSelectedComponent>(entity, false);
        }
    }
    

    public struct SquadSelectedComponent : IComponentData, IEnableableComponent
    {
        public bool onSelectedEvent;
        public bool onDeselectedEvent;
    }
}