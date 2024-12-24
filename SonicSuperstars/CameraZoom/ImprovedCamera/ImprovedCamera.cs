using System;
using System.Collections.Generic;
using arz.input;
using BepInEx.Core.Logging.Interpolation;
using BepInEx.Logging;
using Cinemachine;
using HarmonyLib;
using Il2CppSystem;
using Moon;
using Moon.Scene;
using OriCam;
using OriEne;
using OriGmk;
using OriPlayer;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ImprovedCamera
{
	// Token: 0x02000006 RID: 6
	public class ImprovedCamera
	{
		// Token: 0x0600000A RID: 10 RVA: 0x000026AC File Offset: 0x000008AC
		public static void UpdateAllCamerasFOV()
		{
			foreach (CinemachineVirtualCamera cinemachineVirtualCamera in ImprovedCamera._sceneCameras)
			{
				bool flag = !Object.ReferenceEquals(cinemachineVirtualCamera, null);
				if (flag)
				{
					ImprovedCamera.UpdateCameraFOV(cinemachineVirtualCamera);
				}
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002714 File Offset: 0x00000914
		public static void UpdateCameraFOV(CinemachineVirtualCamera virtualCamera)
		{
			LensSettings lens = virtualCamera.m_Lens;
			lens.FieldOfView = ImprovedCamera.CurrentFOV;
			virtualCamera.m_Lens = lens;
		}

		// Token: 0x04000002 RID: 2
		public const float DefaultCameraFOV = 25f;

		// Token: 0x04000003 RID: 3
		public static float CurrentFOV = Plugin.ConfigFOV.Value;

		// Token: 0x04000004 RID: 4
		public static float NewFOV = Plugin.ConfigFOV.Value;

		// Token: 0x04000005 RID: 5
		public static string CurrentScene = "";

		// Token: 0x04000006 RID: 6
		public static string CurrentZone = "";

		// Token: 0x04000007 RID: 7
		public static float CurrentXOffset = 0f;

		// Token: 0x04000008 RID: 8
		public static float NewXOffset = 0f;

		// Token: 0x04000009 RID: 9
		public static int PlayerDirection = 1;

		// Token: 0x0400000A RID: 10
		public static float disableExtendedCameraTime = 0f;

		// Token: 0x0400000B RID: 11
		public static bool disableExtendedCamera = false;

		// Token: 0x0400000C RID: 12
		public static bool inBreakout = false;

		// Token: 0x0400000D RID: 13
		private static List<CinemachineVirtualCamera> _sceneCameras = new List<CinemachineVirtualCamera>();

		// Token: 0x02000009 RID: 9
		[HarmonyPatch(typeof(CinemachineVirtualCameraBase), "Start")]
		private class SetCameraFOV
		{
			// Token: 0x06000011 RID: 17 RVA: 0x000028F0 File Offset: 0x00000AF0
			private static void Postfix(CinemachineVirtualCameraBase __instance)
			{
				Plugin.Log.LogInfo(ImprovedCamera.CurrentScene);
				bool flag = ImprovedCamera.CurrentScene.Contains("Mini") || ImprovedCamera.CurrentScene.Contains("Special") || ImprovedCamera.CurrentScene.Contains("Bonus");
				if (!flag)
				{
					ImprovedCamera._sceneCameras.Add(GameObject.Find(__instance.gameObject.name).GetComponent<CinemachineVirtualCamera>());
					Plugin.Log.LogInfo("Added camera: " + __instance.gameObject.name);
					ImprovedCamera.UpdateCameraFOV(__instance.gameObject.GetComponent<CinemachineVirtualCamera>());
					bool flag2 = ImprovedCamera.CurrentZone == "Zone02_Act3";
					if (flag2)
					{
						FOVFixes.FixLamp();
					}
				}
			}
		}

		// Token: 0x0200000A RID: 10
		[HarmonyPatch(typeof(PlayerBase), "Update")]
		private class UpdateCamera
		{
			// Token: 0x06000013 RID: 19 RVA: 0x000029C0 File Offset: 0x00000BC0
			private static void Prefix(PlayerBase __instance)
			{
				bool flag = __instance.crntCamera != null && __instance.inputPlyCtlr != null;
				if (flag)
				{
					bool value = Plugin.ConfigControlFOV.Value;
					if (value)
					{
						try
						{
							PlatformPad.Button button = 256;
							PlatformPad.Button button2 = 512;
							bool flag2 = __instance.inputPlyCtlr.IsInputBtnDirectOn(ref button);
							if (flag2)
							{
								ImprovedCamera.NewFOV -= Plugin.ConfigControlFOVSpeed.Value * Time.deltaTime;
							}
							bool flag3 = __instance.inputPlyCtlr.IsInputBtnDirectOn(ref button2);
							if (flag3)
							{
								ImprovedCamera.NewFOV += Plugin.ConfigControlFOVSpeed.Value * Time.deltaTime;
							}
							bool flag4 = __instance.inputPlyCtlr.IsInputBtnDirectOn(ref button2) && __instance.inputPlyCtlr.IsInputBtnDirectOn(ref button);
							if (flag4)
							{
								Plugin.UpdateFOV(ImprovedCamera.CurrentFOV);
								ManualLogSource log = Plugin.Log;
								bool flag5;
								BepInExInfoLogInterpolatedStringHandler bepInExInfoLogInterpolatedStringHandler = new BepInExInfoLogInterpolatedStringHandler(17, 1, ref flag5);
								if (flag5)
								{
									bepInExInfoLogInterpolatedStringHandler.AppendLiteral("Updated FOV to: ");
									bepInExInfoLogInterpolatedStringHandler.AppendFormatted<float>(ImprovedCamera.CurrentFOV);
									bepInExInfoLogInterpolatedStringHandler.AppendLiteral(".");
								}
								log.LogInfo(bepInExInfoLogInterpolatedStringHandler);
							}
						}
						catch
						{
						}
					}
					bool flag6 = ImprovedCamera.CurrentZone == "Zone03_Act1";
					if (flag6)
					{
						FOVFixes.SetDefaultFOVZ03Breakout(__instance);
					}
					bool flag7 = ImprovedCamera.CurrentFOV != ImprovedCamera.NewFOV;
					if (flag7)
					{
						ImprovedCamera.CurrentFOV = Mathf.MoveTowards(ImprovedCamera.CurrentFOV, ImprovedCamera.NewFOV, Time.deltaTime * 10f);
						ImprovedCamera.UpdateAllCamerasFOV();
					}
					bool value2 = Plugin.ConfigExtendedCamera.Value;
					if (value2)
					{
						bool flag8 = ImprovedCamera.disableExtendedCameraTime == 0f && !ImprovedCamera.disableExtendedCamera;
						if (flag8)
						{
							bool flag9 = __instance.VelocityMove >= 3.5f;
							if (flag9)
							{
								ImprovedCamera.NewXOffset = 1.1f * (float)ImprovedCamera.PlayerDirection;
							}
							else
							{
								ImprovedCamera.NewXOffset = 0f;
							}
						}
						bool flag10 = ImprovedCamera.CurrentXOffset != ImprovedCamera.NewXOffset;
						if (flag10)
						{
							ImprovedCamera.CurrentXOffset = Mathf.MoveTowards(ImprovedCamera.CurrentXOffset, ImprovedCamera.NewXOffset, Time.deltaTime * 0.85f);
						}
						MonoSingleton<CinemachineCamera2DManager, ConstantBool._false, ConstantBool._false>.Instance.SingleVirtualCamera2D.SetCameraOffsetX(ImprovedCamera.CurrentXOffset);
					}
					bool flag11 = ImprovedCamera.disableExtendedCameraTime > 0f;
					if (flag11)
					{
						ImprovedCamera.disableExtendedCameraTime -= 1f * Time.deltaTime;
					}
					else
					{
						bool flag12 = ImprovedCamera.disableExtendedCameraTime < 0f;
						if (flag12)
						{
							ImprovedCamera.disableExtendedCameraTime = 0f;
						}
					}
					ImprovedCamera.PlayerDirection = (__instance.IsDirLeft() ? (-1) : 1);
				}
			}
		}

		// Token: 0x0200000B RID: 11
		[HarmonyPatch(typeof(Scene_Manager), "SceneLoaded")]
		private class SceneSetup
		{
			// Token: 0x06000015 RID: 21 RVA: 0x00002C80 File Offset: 0x00000E80
			private static void Prefix(Scene nextScene, LoadSceneMode mode)
			{
				ImprovedCamera.NewFOV = Plugin.ConfigFOV.Value;
				ImprovedCamera.CurrentScene = nextScene.name;
				bool flag = ImprovedCamera.CurrentScene == "GameMain";
				if (flag)
				{
					Plugin.Log.LogInfo("Cleared camera list");
					ImprovedCamera._sceneCameras.Clear();
				}
				bool flag2 = ImprovedCamera.CurrentScene.Contains("Zone");
				if (flag2)
				{
					ImprovedCamera.CurrentZone = ImprovedCamera.CurrentScene;
					FOVFixes.FixClipping();
					FOVFixes.FixBackground();
				}
				bool flag3 = ImprovedCamera.CurrentZone == "Zone10_Act3";
				if (flag3)
				{
					ImprovedCamera.NewFOV = 25f;
				}
			}
		}

		// Token: 0x0200000C RID: 12
		[HarmonyPatch(typeof(EnemySpecialBase), "PlayBossBGM")]
		private class BossReset
		{
			// Token: 0x06000017 RID: 23 RVA: 0x00002D2C File Offset: 0x00000F2C
			private static void Postfix()
			{
				bool flag = ImprovedCamera.CurrentZone == "Zone02_Act2";
				if (flag)
				{
					ImprovedCamera.disableExtendedCamera = true;
					ImprovedCamera.NewXOffset = 0f;
				}
				else
				{
					bool flag2 = ImprovedCamera.CurrentZone == "Zone01_Act1";
					if (!flag2)
					{
						ImprovedCamera.NewFOV = 25f;
						ImprovedCamera.disableExtendedCamera = true;
						ImprovedCamera.NewXOffset = 0f;
					}
				}
			}
		}

		// Token: 0x0200000D RID: 13
		[HarmonyPatch(typeof(EnemySpecialBase), "StopBossBGM")]
		private class BossDefeatReset
		{
			// Token: 0x06000019 RID: 25 RVA: 0x00002D98 File Offset: 0x00000F98
			private static void Postfix()
			{
				bool flag = ImprovedCamera.CurrentZone == "Zone02_Act2";
				if (!flag)
				{
					ImprovedCamera.NewFOV = 25f;
					ImprovedCamera.disableExtendedCamera = false;
				}
			}
		}

		// Token: 0x0200000E RID: 14
		[HarmonyPatch(typeof(GmkGoalBoard), "SetGoalCollisionPlayerRev_SetActive")]
		private class ResetFOVGoalHit
		{
			// Token: 0x0600001B RID: 27 RVA: 0x00002DD4 File Offset: 0x00000FD4
			private static void Postfix()
			{
				ImprovedCamera.NewFOV = 25f;
			}
		}
	}
}
