using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;

[UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
partial struct NetcodePlayerMovementSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach((
            RefRO<NetcodePlayerInput> netcodePlayerInput,
            RefRW<LocalTransform> localTransform) 
            in SystemAPI.Query<
                RefRO<NetcodePlayerInput>, 
                RefRW<LocalTransform>>().WithAll<Simulate>())
        {
            float moveSpeed = 10.0f;
            float3 moveVector = new float3(
                netcodePlayerInput.ValueRO.inputVector.x,
                0,
                netcodePlayerInput.ValueRO.inputVector.y);

            localTransform.ValueRW.Position += moveSpeed * SystemAPI.Time.DeltaTime * moveVector;

        }
    }

}
