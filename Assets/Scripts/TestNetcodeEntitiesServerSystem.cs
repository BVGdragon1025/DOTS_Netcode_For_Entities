using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
partial struct TestNetcodeEntitiesServerSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
        foreach((
            RefRO<SampleRPC> sampleRPC, 
            RefRO<ReceiveRpcCommandRequest> receiveRpcCommandRequest,
            Entity entity) 
            in SystemAPI.Query<
                RefRO<SampleRPC>, 
                RefRO<ReceiveRpcCommandRequest>>().WithEntityAccess())
        {
            
            entityCommandBuffer.DestroyEntity(entity);

        }
        entityCommandBuffer.Playback(state.EntityManager);
    }

}
