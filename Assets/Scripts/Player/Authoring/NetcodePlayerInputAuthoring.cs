using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using UnityEngine;

public class NetcodePlayerInputAuthoring : MonoBehaviour
{
    public float playerSpeed;
    
    public class Baker : Baker<NetcodePlayerInputAuthoring>
    {
        public override void Bake(NetcodePlayerInputAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new NetcodePlayerInput
            {
                playerSpeed = authoring.playerSpeed,
            });
        }
    }

}

public struct NetcodePlayerInput : IInputComponentData
{
    public float2 inputVector;
    public float playerSpeed;
    public InputEvent isShooting;
    public InputEvent isRunning;
}
