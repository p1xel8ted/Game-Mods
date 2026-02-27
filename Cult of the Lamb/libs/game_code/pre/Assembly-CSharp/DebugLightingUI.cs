// Decompiled with JetBrains decompiler
// Type: DebugLightingUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class DebugLightingUI : BaseMonoBehaviour
{
  private void Start()
  {
  }

  private void OnGUI()
  {
    LightingManager instance = LightingManager.Instance;
    if ((Object) instance == (Object) null)
      return;
    GUIStyle style = new GUIStyle();
    style.fontSize = 30;
    style.normal.textColor = Color.white;
    GUI.Label(new Rect(10f, 10f, 0.0f, 0.0f), "isGlobalOverride: " + instance.inGlobalOverride.ToString(), style);
    GUI.Label(new Rect(10f, 50f, 0.0f, 0.0f), "inOverride: " + instance.inOverride.ToString(), style);
    GUI.Label(new Rect(10f, 90f, 0.0f, 0.0f), "overrideNaturalLight: " + instance.overrideNaturalLightRot.ToString(), style);
  }
}
