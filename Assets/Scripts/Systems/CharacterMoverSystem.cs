using Arenar.PocketFantasyWar.InputSystem;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Physics;

namespace Arenar.SamuraiWarrior
{
    partial struct CharacterMoverSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            //state.RequireForUpdate<InputSingleton>();
        }


        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            /*CharacterMoverJob characterMoverJob = new CharacterMoverJob()
            {
                deltaTime = SystemAPI.Time.DeltaTime,
            };*/
            
            if (SystemAPI.HasSingleton<InputSingleton>())
            {
                InputSingleton input = SystemAPI.GetSingleton<InputSingleton>();

                foreach ((RefRW<LocalTransform> localTransform, RefRO<CharacterMover> characterMover, RefRW<PhysicsVelocity> physicsVelocity) in
                         SystemAPI.Query<RefRW<LocalTransform>, RefRO<CharacterMover>, RefRW<PhysicsVelocity>>().WithAny<Player>())
                {
                    float3 moveDirection = new float3(input.move.x, 0, input.move.y);
                    float3 rotation = new float3(input.look.x, 0, input.look.y);
                }
            }
        }


        [BurstCompile]
        public void OnDestroy(ref SystemState state) {}
    }



    /*public partial struct CharacterMoverJob : IJobEntity
    {
        public float deltaTime;
        
        
        public void Execute(float3 moveDirection, float3 rotation, ref LocalTransform localTransform, in CharacterMover characterMover, ref PhysicsVelocity physicsVelocity)
        {
            if (math.lengthsq(moveDirection) > 0.0001f)
            {
                UpdatePosition(in characterMover, ref physicsVelocity, moveDirection);
            }
            else
            {
                physicsVelocity.Linear = float3.zero;
            }
            
            if (math.lengthsq(rotation) > 0.0001f)
            {
                UpdateRotation(
                    ref localTransform,
                    in characterMover,
                    rotation);
            }
            else if (math.lengthsq(moveDirection) > 0.0001f)
            {
                rotation = moveDirection;
                UpdateRotation(
                    ref localTransform,
                    in characterMover,
                    rotation);
            }
        }
        
        private void UpdatePosition(in CharacterMover moveSpeedComponent,
                                    ref PhysicsVelocity physicsVelocity,
                                    float3 moveDirection)
        {
            moveDirection = math.normalize(moveDirection);
                
            physicsVelocity.Linear = moveDirection * moveSpeedComponent.moveSpeed;
            physicsVelocity.Angular = float3.zero;
        }
        
        private void UpdateRotation(ref LocalTransform localTransform,
                                    in CharacterMover characterMover,
                                    float3 rotation)
        {
            localTransform.Rotation = math.slerp(localTransform.Rotation,
                quaternion.LookRotation(rotation, math.up()),
                characterMover.rotationSpeed * deltaTime);
        }
    }*/
}