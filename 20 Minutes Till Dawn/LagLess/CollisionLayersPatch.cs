using flanne;
using HarmonyLib;
using UnityEngine;

namespace LagLess;

[Harmony]
public static class CollisionLayersPatch
{
	
	[HarmonyPostfix]
	[HarmonyPatch(typeof(PlayerController), nameof(PlayerController.Start))]
	private static void PlayerController_Start(ref PlayerController __instance)
	{
		LLLayers.SetAllPickerUppersLayers();
		
		var main = Camera.main;
		if (main != null) main.cullingMask = main.cullingMask | 1 << LLLayers.enemyLayer | 1 << LLLayers.summonCollideOnlyBullet | 1 << LLLayers.bulletLayer | 1 << LLLayers.bulletExplosionLayer | 1 << LLLayers.pickerupLayer;
		var gameObject = GameObject.FindGameObjectWithTag("Pickupper");
		gameObject.layer = LLLayers.pickerupLayer;
		SetupLayers();
	}


	private static void SetupLayers()
	{
		for (var i = 0; i < 32; i++)
		{
			Physics2D.IgnoreLayerCollision(i, LLLayers.pickupLayer, true);
			Physics2D.IgnoreLayerCollision(i, LLLayers.pickerupLayer, true);
			Physics2D.IgnoreLayerCollision(i, LLLayers.summonCollideOnlyBullet, true);
			Physics2D.IgnoreLayerCollision(i, LLLayers.bulletLayer, true);
			Physics2D.IgnoreLayerCollision(i, LLLayers.bulletExplosionLayer, true);
			Physics2D.IgnoreLayerCollision(i, LLLayers.enemyLayer, true);
		}
		Physics2D.IgnoreLayerCollision(LLLayers.pickupLayer, LLLayers.pickerupLayer, false);
		Physics2D.IgnoreLayerCollision(LLLayers.summonCollideOnlyBullet, LLLayers.bulletLayer, false);
		Physics2D.IgnoreLayerCollision(LLLayers.bulletLayer, LLLayers.enemyLayer, false);
		Physics2D.IgnoreLayerCollision(LLLayers.bulletExplosionLayer, LLLayers.enemyLayer, false);
		Physics2D.IgnoreLayerCollision(LLLayers.bulletExplosionLayer, 0, false);
		Physics2D.IgnoreLayerCollision(LLLayers.enemyLayer, LLLayers.enemyLayer, false);
		Physics2D.IgnoreLayerCollision(0, LLLayers.enemyLayer, false);
	}
}