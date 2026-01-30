// Decompiled with JetBrains decompiler
// Type: SetTimeScale
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
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
