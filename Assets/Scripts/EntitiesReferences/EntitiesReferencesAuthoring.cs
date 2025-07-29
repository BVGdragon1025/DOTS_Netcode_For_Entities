using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

public class EntitiesReferencesAuthoring : MonoBehaviour
{
    public GameObject playerOnePrefab;
    public GameObject playerTwoPrefab;
    public GameObject bulletPrefab;

    public class Baker : Baker<EntitiesReferencesAuthoring>
    {
        public override void Bake(EntitiesReferencesAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new EntitiesReferences
            {
                playerOneEntity = GetEntity(authoring.playerOnePrefab, TransformUsageFlags.Dynamic),
                playerTwoEntity = GetEntity(authoring.playerTwoPrefab, TransformUsageFlags.Dynamic),
                bulletEntity = GetEntity(authoring.bulletPrefab, TransformUsageFlags.Dynamic),
            });
        }
    }

}

public struct EntitiesReferences : IComponentData
{
    public Entity playerOneEntity;
    public Entity playerTwoEntity;
    public Entity bulletEntity;
}

