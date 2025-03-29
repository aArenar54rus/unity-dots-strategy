using Unity.Entities;
using UnityEngine;

namespace Arenar.PocketFantasyWar
{
	public class HealthComponentAuthoring : MonoBehaviour
	{
		public float health;
		public float healthMax;
	}
	
	public class Baker : Baker<HealthComponentAuthoring>
	{
		public override void Bake(HealthComponentAuthoring healthComponentAuthoring)
		{
			Entity entity = GetEntity(TransformUsageFlags.Dynamic);
			AddComponent(entity, new HealthComponent()
			{
				health = healthComponentAuthoring.health,
				healthMax = healthComponentAuthoring.healthMax,
			});
		}
	}

	public struct HealthComponent : IComponentData
	{
		public float health;
		public float healthMax;
	}
}