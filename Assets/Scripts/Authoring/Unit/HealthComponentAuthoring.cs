using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;


namespace Arenar.PocketFantasyWar
{
	public class HealthComponentAuthoring : MonoBehaviour
	{
		public float health;
		public float healthMax;
		public UnitDataContainer unitDataContainer;


		public float Health => unitDataContainer != null ? unitDataContainer.UnitData.UnitHealth : health;
		public float HealthMax => unitDataContainer != null ? unitDataContainer.UnitData.UnitHealth : healthMax;

		
		public class Baker : Baker<HealthComponentAuthoring>
		{
			public override void Bake(HealthComponentAuthoring healthComponentAuthoring)
			{
				Entity entity = GetEntity(TransformUsageFlags.Dynamic);
				AddComponent(entity, new HealthComponent()
				{
					health = healthComponentAuthoring.Health,
					healthMax = healthComponentAuthoring.HealthMax,
				});
			}
		}
	}
	
	
	public struct HealthComponent : IComponentData
	{
		public float health;
		public float healthMax;
	}
}