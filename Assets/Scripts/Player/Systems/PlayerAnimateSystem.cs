using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using Unity.Transforms;
using Unity.Mathematics;

[UpdateInGroup(typeof(PresentationSystemGroup), OrderFirst = true)]
partial struct PlayerAnimateSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.Temp);

        foreach(var (playerGameObjectPrefab, entity) 
            in SystemAPI.Query<PlayerGameObjectPrefab>().WithNone<PlayerAnimatorReference>().WithEntityAccess())
        {
            var newCompanionGameObject = Object.Instantiate(playerGameObjectPrefab.value);
            var newAnimatorReference = new PlayerAnimatorReference
            {
                value = newCompanionGameObject.GetComponent<Animator>()
            };
            entityCommandBuffer.AddComponent(entity, newAnimatorReference);
        }

        foreach(var(transform, animatorReference, moveInput ) in 
            SystemAPI.Query<LocalTransform, PlayerAnimatorReference, NetcodePlayerInput>())
        {
            animatorReference.value.SetBool("isRunning", moveInput.isRunning.IsSet);
            animatorReference.value.transform.position = transform.Position;
        }

        foreach(var (animatorReference, entity) in 
            SystemAPI.Query<PlayerAnimatorReference>().WithNone<PlayerGameObjectPrefab, LocalTransform>().WithEntityAccess())
        {
            Object.Destroy(animatorReference.value.gameObject);
            entityCommandBuffer.RemoveComponent<PlayerAnimatorReference>(entity);
        }

        entityCommandBuffer.Playback(state.EntityManager);

    }

}
