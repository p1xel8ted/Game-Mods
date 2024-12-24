using System;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using OriComp;
using Orion;
using OriPlayer;
using UnityEngine;

namespace ImprovedCamera
{
	// Token: 0x02000004 RID: 4
	public class FOVFixes
	{
		// Token: 0x06000003 RID: 3 RVA: 0x0000206C File Offset: 0x0000026C
		public static void FixLamp()
		{
			try
			{
				bool value = Plugin.ConfigTurnOffLampZ2A2.Value;
				if (value)
				{
					GameObject gameObject = GameObject.Find("Butterfly(Lamp)");
					gameObject.active = false;
				}
				else
				{
					GameObject gameObject2 = GameObject.Find("Butterfly(Lamp)");
					gameObject2.active = true;
					GameObject gameObject3 = GameObject.Find("GuideMaskManager");
					float num = ImprovedCamera.NewFOV / 25f + 0.1f;
					gameObject3.transform.localScale = new Vector3(num, num, gameObject3.transform.localScale.z);
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002110 File Offset: 0x00000310
		public static void FixClipping()
		{
			try
			{
				GameSceneBlockController component = GameObject.Find("GameSceneLevelControllerDevelop").GetComponent<GameSceneBlockController>();
				component.visibleSafeBlock = new Vector2Int(10, 5);
				GameObject gameObject = GameObject.Find(ImprovedCamera.CurrentZone + "_Kazari");
				Il2CppArrayBase<CompAreaCameraActivator> componentsInChildren = gameObject.GetComponentsInChildren<CompAreaCameraActivator>();
				float num = ImprovedCamera.CurrentFOV / 25f + 1.1f;
				foreach (CompAreaCameraActivator compAreaCameraActivator in componentsInChildren)
				{
					compAreaCameraActivator.areaBox = new Rect(new Vector2(compAreaCameraActivator.areaBox.x, compAreaCameraActivator.areaBox.y), new Vector2(compAreaCameraActivator.areaBox.width * num, compAreaCameraActivator.areaBox.height * num));
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x0000221C File Offset: 0x0000041C
		public static void FixBackground()
		{
			bool flag = ImprovedCamera.CurrentZone.Contains("Zone01");
			if (!flag)
			{
				bool flag2 = ImprovedCamera.CurrentZone == "Zone10_Act3";
				if (!flag2)
				{
					GameObject gameObject = GameObject.Find(ImprovedCamera.CurrentZone + "_BackGround");
					bool flag3 = gameObject == null;
					if (!flag3)
					{
						Il2CppArrayBase<Transform> componentsInChildren = gameObject.GetComponentsInChildren<Transform>();
						float num = ImprovedCamera.NewFOV / 25f + 1.1f;
						foreach (Transform transform in componentsInChildren)
						{
							bool flag4 = ImprovedCamera.CurrentZone.Contains("Zone05");
							if (flag4)
							{
								bool flag5 = transform.gameObject.name == "scroll_01";
								if (flag5)
								{
									transform.localScale = new Vector3(transform.localScale.x * num, transform.localScale.y, transform.localScale.z * num);
								}
							}
							else
							{
								string text = "sky";
								bool flag6 = ImprovedCamera.CurrentZone.Contains("Zone02_Act1");
								if (flag6)
								{
									text = "trancepalent";
								}
								bool flag7 = transform.gameObject.name.Contains(text, StringComparison.OrdinalIgnoreCase);
								if (flag7)
								{
									bool flag8 = ImprovedCamera.CurrentZone == "Zone02_Act2";
									if (flag8)
									{
										transform.localScale = new Vector3(transform.localScale.x * num * 4f, transform.localScale.y, transform.localScale.z * num * 4f);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000023F8 File Offset: 0x000005F8
		public static void SetDefaultFOVZ03Breakout(PlayerBase player)
		{
			bool flag = player.crntAction == 26;
			if (flag)
			{
				ImprovedCamera.inBreakout = true;
				ImprovedCamera.NewFOV = 25f;
			}
			bool flag2 = ImprovedCamera.inBreakout && (player.crntAction == 3 || player.crntAction == 6);
			if (flag2)
			{
				ImprovedCamera.inBreakout = false;
				ImprovedCamera.NewFOV = Plugin.ConfigFOV.Value;
			}
		}
	}
}
