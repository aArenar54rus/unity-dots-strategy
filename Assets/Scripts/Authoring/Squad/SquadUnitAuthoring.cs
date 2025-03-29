using Unity.Entities;
using UnityEngine;



namespace Arenar.PocketFantasyWar
{
    public class SquadUnitAuthoring : MonoBehaviour
    {
        public int squadIndex;
        public int pointIndex;
    }

    
    public class SquadUnitAuthoringBaker : Baker<SquadUnitAuthoring>
    {
        public override void Bake(SquadUnitAuthoring squadUnitAuthoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new SquadUnitComponent()
            {
                squadIndex = squadUnitAuthoring.squadIndex,
                pointIndex = squadUnitAuthoring.pointIndex,
            });
        }
    }

    
    public struct SquadUnitComponent : IComponentData
    {
        public int squadIndex;
        public int pointIndex;
    }
}