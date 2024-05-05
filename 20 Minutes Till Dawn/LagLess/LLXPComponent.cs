using System.Collections;
using flanne;
using flanne.Pickups;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LagLess;

// Token: 0x02000011 RID: 17
public class LLXPComponent : MonoBehaviour
{
	// Token: 0x0600002F RID: 47 RVA: 0x00002DFE File Offset: 0x00000FFE
	private void Awake()
	{
		flanneXP = gameObject.GetComponent<XPPickup>();
		defaultExperienceAmount = flanneXP.amount;
	}

	// Token: 0x06000030 RID: 48 RVA: 0x00002E24 File Offset: 0x00001024
	private void OnEnable()
	{
		extraExperienceCollected = 0;
		flanneXP.amount = defaultExperienceAmount;
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		StartCoroutine(FindAndJoinAnotherXP());
	}

	// Token: 0x06000031 RID: 49 RVA: 0x00002E7C File Offset: 0x0000107C
	private void acceptMoreExperience(float amount, int extraExperienceCollected)
	{
		flanneXP.amount += amount;
		this.extraExperienceCollected += extraExperienceCollected + 1;
		var num = Mathf.Min(1f, flanneXP.amount * 0.05f) + Mathf.Min(1f, flanneXP.amount * 0.02f) + Mathf.Min(1f, flanneXP.amount * 0.005f);
		gameObject.transform.localScale = new Vector3(1f + num, 1f + num, 1f + num);
	}

	// Token: 0x06000032 RID: 50 RVA: 0x00002F2C File Offset: 0x0000112C
	private IEnumerator FindAndJoinAnotherXP()
	{
		yield return new WaitForSeconds(1.5f);
		var flag = flanneXP.pickUpCoroutine != null;
		if (flag)
		{
			yield break;
		}
		var xpToJoin = findXPToJoin();
		bool flag2 = xpToJoin;
		if (flag2)
		{
			flanneXP.pickUpCoroutine = JoinXP(xpToJoin);
			StartCoroutine(flanneXP.pickUpCoroutine);
		}
	}

	// Token: 0x06000033 RID: 51 RVA: 0x00002F3B File Offset: 0x0000113B
	private IEnumerator JoinXP(LLXPComponent xpToJoin)
	{
		xpToJoin.acceptMoreExperience(flanneXP.amount, extraExperienceCollected);
		var tweenID = LeanTween.move(gameObject, xpToJoin.transform.position, 0.3f).setEase(LeanTweenType.easeInBounce).id;
		while (LeanTween.isTweening(tweenID))
		{
			yield return null;
		}
		flanneXP.pickUpCoroutine = null;
		gameObject.transform.SetParent(ObjectPooler.SharedInstance.transform);
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.SetActive(false);
	}

	// Token: 0x06000034 RID: 52 RVA: 0x00002F54 File Offset: 0x00001154
	private LLXPComponent findXPToJoin()
	{
		var position = gameObject.transform.position;
		var num = LLConstants.xpSelfPickupRadius + LLConstants.xpSelfPickupRadius * Random.value;
		var array = Physics2D.OverlapCircleAll(position, num, 1 << LLLayers.pickupLayer);
		LLXPComponent llxpcomponent = null;
		var num2 = float.PositiveInfinity;
		foreach (var collider2D in array)
		{
			var component = collider2D.gameObject.GetComponent<LLXPComponent>();
			var flag = component && component != this && component.flanneXP.pickUpCoroutine == null;
			if (flag)
			{
				var sqrMagnitude = (component.gameObject.transform.position - position).sqrMagnitude;
				var flag2 = sqrMagnitude < num2;
				if (flag2)
				{
					num2 = sqrMagnitude;
					llxpcomponent = component;
				}
			}
		}
		return llxpcomponent;
	}

	// Token: 0x06000035 RID: 53 RVA: 0x00003044 File Offset: 0x00001244
	private void OnDisable()
	{
		StopAllCoroutines();
	}


	private float defaultExperienceAmount;


	public int extraExperienceCollected;

	
	public XPPickup flanneXP;

}