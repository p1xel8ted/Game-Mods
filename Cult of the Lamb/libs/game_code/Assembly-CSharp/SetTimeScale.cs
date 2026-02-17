// Decompiled with JetBrains decompiler
// Type: SetTimeScale
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SetTimeScale : BaseMonoBehaviour
{
  public float timescale = 1f;
  public bool UpdateUnscaledTime;

  public void Start() => Time.timeScale = this.timescale;

  public void Update()
  {
    if (!this.UpdateUnscaledTime)
      return;
    Shader.SetGlobalFloat("_GlobalTimeUnscaled", Time.unscaledTime);
    Shader.SetGlobalFloat("_GlobalTimeUnscaled1", Time.unscaledTime);
  }
}
