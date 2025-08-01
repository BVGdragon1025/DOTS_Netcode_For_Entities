using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using UnityEngine;

[UpdateInGroup(typeof(GhostInputSystemGroup))]
partial struct NetcodePlayerInputSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<NetworkStreamInGame>();
        state.RequireForUpdate<NetcodePlayerInput>();
        
    }

    public void OnUpdate(ref SystemState state)
    {
        foreach (
            RefRW<NetcodePlayerInput> netcodePlayerInput in SystemAPI.Query<RefRW<NetcodePlayerInput>>().WithAll<GhostOwnerIsLocal>())
        {
            float2 inputVector = new float2();
            if (Input.GetKey(KeyCode.W))
            {
                inputVector.y = 1.0f;
            }

            if (Input.GetKey(KeyCode.S))
            {
                inputVector.y = -1.0f;
            }

            if (Input.GetKey(KeyCode.A))
            {
                inputVector.x = -1.0f;
            }

            if (Input.GetKey(KeyCode.D))
            {
                inputVector.x = 1.0f;
            }

            netcodePlayerInput.ValueRW.inputVector = inputVector;

            if (math.length(inputVector) > 0)
            {
                netcodePlayerInput.ValueRW.isRunning.Set();
            }
            else
            {
                netcodePlayerInput.ValueRW.isRunning = default;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                netcodePlayerInput.ValueRW.isShooting.Set();
            }
            else
            {
                netcodePlayerInput.ValueRW.isShooting = default;
            }

        }
    }

}
