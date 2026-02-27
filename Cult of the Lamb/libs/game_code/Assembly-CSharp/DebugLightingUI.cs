// Decompiled with JetBrains decompiler
// Type: DebugLightingUI
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class DebugLightingUI : BaseMonoBehaviour
{
  public void Start()
  {
  }

  public void OnGUI()
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
