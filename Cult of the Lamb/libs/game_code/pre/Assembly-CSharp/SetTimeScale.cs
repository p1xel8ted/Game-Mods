// Decompiled with JetBrains decompiler
// Type: SetTimeScale
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SetTimeScale : BaseMonoBehaviour
{
  public float timescale = 1f;
  public bool UpdateUnscaledTime;

  private void Start() => Time.timeScale = this.timescale;

  private void Update()
  {
    if (!this.UpdateUnscaledTime)
      return;
    Shader.SetGlobalFloat("_GlobalTimeUnscaled", Time.unscaledTime);
    Shader.SetGlobalFloat("_GlobalTimeUnscaled1", Time.unscaledTime);
  }
}
