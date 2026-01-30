// Decompiled with JetBrains decompiler
// Type: SetAmbientLighting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class SetAmbientLighting : BaseMonoBehaviour
{
  public Color color;

  public void Start() => RenderSettings.ambientLight = this.color;

  public void Update()
  {
  }
}
