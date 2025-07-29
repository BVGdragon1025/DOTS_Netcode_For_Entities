using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;
using Unity.Transforms;

[UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
partial struct ShootSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        SystemAPI.TryGetSingleton(out NetworkTime networkTime);
        SystemAPI.TryGetSingleton(out EntitiesReferences entitiesReferences);

        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
        foreach((
            RefRO<NetcodePlayerInput> netcodePlayerInput,
            RefRO<LocalTransform> localTransform,
            RefRO<GhostOwner> ghostOwner)
            in SystemAPI.Query<
                RefRO<NetcodePlayerInput>,
                RefRO<LocalTransform>,
                RefRO<GhostOwner>>().WithAll<Simulate>())
        {
            if (networkTime.IsFirstTimeFullyPredictingTick)
            {
                if (netcodePlayerInput.ValueRO.isShooting.IsSet)
                {
                    Entity bulletEntity = entityCommandBuffer.Instantiate(entitiesReferences.bulletEntity);
                    entityCommandBuffer.SetComponent(bulletEntity, LocalTransform.FromPosition(localTransform.ValueRO.Position));
                    entityCommandBuffer.SetComponent(bulletEntity, new GhostOwner
                    {
                        NetworkId = ghostOwner.ValueRO.NetworkId,
                    });
                }
            }

        }
        entityCommandBuffer.Playback(state.EntityManager);

    }

}
