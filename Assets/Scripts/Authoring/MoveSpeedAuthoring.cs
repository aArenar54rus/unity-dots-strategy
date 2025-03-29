using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;


namespace Arenar.SamuraiWarrior
{
	public class MoveSpeedAuthoring : MonoBehaviour
	{

		public float moveSpeed = 5;
		public float rotationSpeed = 10;

		
		public class Baker : Baker<MoveSpeedAuthoring>
		{
			public override void Bake(MoveSpeedAuthoring moveSpeedAuthoring)
			{
				Entity entity = GetEntity(TransformUsageFlags.Dynamic);
				AddComponent(entity, new CharacterMover()
				{
					moveSpeed = moveSpeedAuthoring.moveSpeed,
					rotationSpeed = moveSpeedAuthoring.rotationSpeed,
				});
			}
		}
	}
    
	public struct CharacterMover : IComponentData
	{
		public float moveSpeed;
		public float rotationSpeed;
	}
}