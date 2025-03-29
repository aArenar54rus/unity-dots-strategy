using Unity.Entities;
using UnityEngine;


namespace Arenar.PocketFantasyWar
{
    public class SquadStateAuthoring : MonoBehaviour
    {
        public UnitState unitState = UnitState.Squad;
        
        
        public class UnitStateAuthoringBaker : Baker<SquadStateAuthoring>
        {
            public override void Bake(SquadStateAuthoring squadPointAuthoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new SquadStateComponent()
                {
                    unitState = squadPointAuthoring.unitState,
                });
            }
        }
    }
    
    
    public struct SquadStateComponent : IComponentData
    {
        public UnitState unitState;
    }
}