using System;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Core.Logging.Interpolation;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;

namespace ImprovedCamera
{
	// Token: 0x02000007 RID: 7
	[BepInPlugin("CameraZoom", "CameraZoom", "2.0.0")]
	public class Plugin : BasePlugin
	{
		// Token: 0x0600000E RID: 14 RVA: 0x000027C4 File Offset: 0x000009C4
		public override void Load()
		{
			Plugin.ConfigFOV = base.Config.Bind<float>("Settings", "Field of View", 30f, "What to change the camera FOV to");
			Plugin.ConfigControlFOV = base.Config.Bind<bool>("Settings", "Control FOV", false, "Change FOV in realtime using the Trigger Buttons (LT Zooms In & RT Zooms Out) and press both together to save the FOV");
			Plugin.ConfigControlFOVSpeed = base.Config.Bind<float>("Settings", "Control FOV Speed", 0.1f, "How fast the camera zooms in/out with the control FOV setting");
			Plugin.ConfigExtendedCamera = base.Config.Bind<bool>("Settings", "Extended Camera", true, "Causes the camera to shift when the player is running");
			Plugin.ConfigTurnOffLampZ2A2 = base.Config.Bind<bool>("Speed Jungle Settings", "Turn off lamp Z2A2", false, "Turns off lamp in dark areas in Speed Jungle Act 2");
			Plugin.Log = base.Log;
			ManualLogSource log = Plugin.Log;
			bool flag;
			BepInExInfoLogInterpolatedStringHandler bepInExInfoLogInterpolatedStringHandler = new BepInExInfoLogInterpolatedStringHandler(18, 1, ref flag);
			if (flag)
			{
				bepInExInfoLogInterpolatedStringHandler.AppendLiteral("Plugin ");
				bepInExInfoLogInterpolatedStringHandler.AppendFormatted<string>("CameraZoom");
				bepInExInfoLogInterpolatedStringHandler.AppendLiteral(" is loaded!");
			}
			log.LogInfo(bepInExInfoLogInterpolatedStringHandler);
			Harmony harmony = new Harmony("ImporvedCamera");
			harmony.PatchAll();
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000028D7 File Offset: 0x00000AD7
		public static void UpdateFOV(float newFov)
		{
			Plugin.ConfigFOV.Value = newFov;
		}

		// Token: 0x0400000E RID: 14
		internal static ManualLogSource Log;

		// Token: 0x0400000F RID: 15
		public static ConfigEntry<float> ConfigFOV;

		// Token: 0x04000010 RID: 16
		public static ConfigEntry<bool> ConfigControlFOV;

		// Token: 0x04000011 RID: 17
		public static ConfigEntry<float> ConfigControlFOVSpeed;

		// Token: 0x04000012 RID: 18
		public static ConfigEntry<bool> ConfigExtendedCamera;

		// Token: 0x04000013 RID: 19
		public static ConfigEntry<bool> ConfigTurnOffLampZ2A2;
	}
}
