using System.Collections.Generic;
using flanne;
using UnityEngine;

namespace LagLess;

// Token: 0x02000009 RID: 9
public class LLObjectPooler
{
	// Token: 0x06000017 RID: 23 RVA: 0x0000291C File Offset: 0x00000B1C
	public LLObjectPooler(Transform inBaseTransform, List<ObjectPoolItem> itemsToPool)
	{
			baseTransform = inBaseTransform;
			objectPools = new Dictionary<string, LLObjectPool>();
			foreach (var objectPoolItem in itemsToPool)
			{
				addNewPool(objectPoolItem);
			}
		}

	// Token: 0x06000018 RID: 24 RVA: 0x0000298C File Offset: 0x00000B8C
	public void AddObject(string tag, GameObject GO, int amt = 3, bool exp = true)
	{
			var flag = !objectPools.ContainsKey(tag);
			if (flag)
			{
				var objectPoolItem = new ObjectPoolItem(tag, GO, amt, exp);
				addNewPool(objectPoolItem);
			}
		}

	// Token: 0x06000019 RID: 25 RVA: 0x000029C2 File Offset: 0x00000BC2
	public List<GameObject> GetAllPooledObjects(string tag)
	{
			return objectPools[tag].GetAll();
		}

	// Token: 0x0600001A RID: 26 RVA: 0x000029D5 File Offset: 0x00000BD5
	public GameObject GetPooledObject(string tag)
	{
			return objectPools[tag].GetNext();
		}

	// Token: 0x0600001B RID: 27 RVA: 0x000029E8 File Offset: 0x00000BE8
	private void addNewPool(ObjectPoolItem item)
	{
			LLConstants.Logger.LogDebug(string.Format("OPR::addNewPool: {0} | shouldExpand: {1}", item.tag, item.shouldExpand));
			var flag = item.tag == "LargeXP";
			if (flag)
			{
				item.shouldExpand = true;
			}
			var llobjectPool = new LLObjectPool(item, baseTransform);
			objectPools.Add(item.tag, llobjectPool);
		}

	// Token: 0x0400000E RID: 14
	private Dictionary<string, LLObjectPool> objectPools;

	// Token: 0x0400000F RID: 15
	private Transform baseTransform;
}