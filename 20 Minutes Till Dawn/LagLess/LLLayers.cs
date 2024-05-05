using flanne;
using UnityEngine;

namespace LagLess;

public static class LLLayers
{
    public const int pickerupLayer = 23;
    public const int pickupLayer = 24;
    public const int bulletLayer = 25;
    public const int bulletExplosionLayer = 26;
    public const int summonCollideOnlyBullet = 27;
    public const int enemyLayer = 28;

    public static void SetAllPickerUppersLayers()
    {
        var array = GameObject.FindGameObjectsWithTag("Pickupper");
        foreach (var gameObject in array)
        {
            gameObject.layer = pickerupLayer;
        }
    }


    private static void ChangeLayersRecursively(Transform transform, int layer)
    {
        transform.gameObject.layer = layer;
        foreach (var obj in transform)
        {
            var transform2 = (Transform) obj;
            var flag = !transform2.gameObject.name.StartsWith("FogReveal");
            if (flag)
            {
                ChangeLayersRecursively(transform2, layer);
            }
        }
    }

    public static void SetPooledObjectLayer(GameObject objectToPool)
    {
        var flag = objectToPool.tag == "Pickup";
        if (flag)
        {
            objectToPool.layer = pickupLayer;
        }
        else
        {
            var flag2 = objectToPool.tag == "Bullet";
            if (flag2)
            {
                objectToPool.layer = bulletLayer;
            }
            else
            {
                var flag3 = objectToPool.tag.StartsWith("Enemy");
                if (flag3)
                {
                    objectToPool.layer = enemyLayer;
                }
                else
                {
                    var componentInChildren = objectToPool.GetComponentInChildren<HarmfulOnContact>();
                    var flag4 = componentInChildren && componentInChildren.tag == "Enemy";
                    if (flag4)
                    {
                        ChangeLayersRecursively(objectToPool.transform, bulletExplosionLayer);
                    }
                }
            }
        }
    }
}