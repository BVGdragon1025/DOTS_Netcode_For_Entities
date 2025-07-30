using Unity.Entities;
using UnityEngine;

public class PlayerGameObjectPrefab : IComponentData
{
    public GameObject value;
}

public class PlayerAnimatorReference : ICleanupComponentData
{
    public Animator value;
}

public class PlayerAnimatorAuthoring : MonoBehaviour
{
    public GameObject playerGameObjectPrefab;

    public class Baker : Baker<PlayerAnimatorAuthoring>
    {
        public override void Bake(PlayerAnimatorAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponentObject(entity, new PlayerGameObjectPrefab { value = authoring.playerGameObjectPrefab });
        }
    }


}
