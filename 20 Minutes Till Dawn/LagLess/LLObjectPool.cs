using System.Collections.Generic;
using flanne;
using flanne.Pickups;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LagLess;

// Token: 0x0200000A RID: 10
public class LLObjectPool
{
	// Token: 0x0600001C RID: 28 RVA: 0x00002A59 File Offset: 0x00000C59
	public LLObjectPool(ObjectPoolItem inBaseObject, Transform inBaseTransform)
	{
			baseTransform = inBaseTransform;
			baseObject = inBaseObject;
			items = createPoolItems(baseObject);
		}

	// Token: 0x0600001D RID: 29 RVA: 0x00002A8A File Offset: 0x00000C8A
	public List<GameObject> GetAll()
	{
			return items;
		}

	// Token: 0x0600001E RID: 30 RVA: 0x00002A94 File Offset: 0x00000C94
	public GameObject GetNext()
	{
			for (var i = currentIndex; i < items.Count; i++)
			{
				var flag = !items[i].activeInHierarchy;
				if (flag)
				{
					currentIndex = i + 1;
					return items[i];
				}
			}
			for (var j = 0; j < currentIndex; j++)
			{
				var flag2 = !items[j].activeInHierarchy;
				if (flag2)
				{
					currentIndex = j + 1;
					return items[j];
				}
			}
			var shouldExpand = baseObject.shouldExpand;
			if (shouldExpand)
			{
				var gameObject = cloneBaseObject(baseObject);
				items.Add(gameObject);
				return gameObject;
			}
			return null;
		}

	// Token: 0x0600001F RID: 31 RVA: 0x00002B84 File Offset: 0x00000D84
	private List<GameObject> createPoolItems(ObjectPoolItem baseObject)
	{
			var list = new List<GameObject>();
			for (var i = 0; i < baseObject.amountToPool; i++)
			{
				list.Add(cloneBaseObject(baseObject));
			}
			return list;
		}

	// Token: 0x06000020 RID: 32 RVA: 0x00002BC4 File Offset: 0x00000DC4
	private GameObject cloneBaseObject(ObjectPoolItem objectPoolItem)
	{
			var gameObject = Object.Instantiate(objectPoolItem.objectToPool, baseTransform, true);
			gameObject.SetActive(false);
			var flag = gameObject.tag == "Pickup";
			if (flag)
			{
				var component = gameObject.GetComponent<XPPickup>();
				bool flag2 = component;
				if (flag2)
				{
					gameObject.AddComponent(typeof(LLXPComponent));
				}
			}
			LLLayers.SetPooledObjectLayer(gameObject);
			return gameObject;
		}

	// Token: 0x04000010 RID: 16
	private List<GameObject> items;

	// Token: 0x04000011 RID: 17
	private int currentIndex;

	// Token: 0x04000012 RID: 18
	private ObjectPoolItem baseObject;

	// Token: 0x04000013 RID: 19
	private Transform baseTransform;
}