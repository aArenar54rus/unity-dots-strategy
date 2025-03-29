using Unity.Entities;
using UnityEngine;


namespace Arenar.PocketFantasyWar
{
    public class SquadAuthoring : MonoBehaviour
    {
        public int squadIndex;
    }

    
    public class SquadAuthoringBaker : Baker<SquadAuthoring>
    {
        public override void Bake(SquadAuthoring squadPointAuthoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new SquadComponent()
            {
                squadIndex = squadPointAuthoring.squadIndex,
                lastSquadPosition = Vector3.zero,
            });
        }
    }

    
    public struct SquadComponent : IComponentData
    {
        public int squadIndex;
        public Unity.Mathematics.float3 lastSquadPosition;
    }
}