using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;

[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
partial struct GoInGameServerSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EntitiesReferences>();
        state.RequireForUpdate<NetworkId>();
    }

    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
        EntitiesReferences entitiesReferences = SystemAPI.GetSingleton<EntitiesReferences>();

        foreach ((
            RefRO<ReceiveRpcCommandRequest> receiveRpcCommandRequest, 
            Entity entity)
            in SystemAPI.Query<
                RefRO<ReceiveRpcCommandRequest>>()
                .WithAll<GoInGameRequestRpc>()
                .WithEntityAccess())
        {
            Entity sourceConnection = receiveRpcCommandRequest.ValueRO.SourceConnection;

            entityCommandBuffer.AddComponent<NetworkStreamInGame>(sourceConnection);

            Entity playerEntity = entityCommandBuffer.Instantiate(entitiesReferences.playerOneEntity);
            entityCommandBuffer.SetComponent(playerEntity, LocalTransform.FromPosition(new float3(
                UnityEngine.Random.Range(-10, +10), 0, 0)));

            NetworkId networkId = SystemAPI.GetComponent<NetworkId>(sourceConnection);
            
            entityCommandBuffer.AddComponent(playerEntity, new GhostOwner { NetworkId = networkId.Value });
            entityCommandBuffer.AppendToBuffer(sourceConnection, new LinkedEntityGroup{ Value = playerEntity });

            entityCommandBuffer.DestroyEntity(entity);
        }
        entityCommandBuffer.Playback(state.EntityManager);

    }

}
