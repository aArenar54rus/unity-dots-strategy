using Unity.Entities;
using UnityEngine;


namespace Arenar.SamuraiWarrior
{
    public class PlayerAuthoring : MonoBehaviour
    {
        public class Baker : Baker<PlayerAuthoring>
        {
            public override void Bake(PlayerAuthoring moveSpeedAuthoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Player());
            }
        }
    }
    
    
    public struct Player : IComponentData {}
}