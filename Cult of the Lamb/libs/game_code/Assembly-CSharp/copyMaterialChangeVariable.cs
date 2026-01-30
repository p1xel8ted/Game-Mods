// Decompiled with JetBrains decompiler
// Type: copyMaterialChangeVariable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class copyMaterialChangeVariable : BaseMonoBehaviour
{
  public Renderer myRenderer;
  public Material material;
  public string floatToChange;
  public float floatVar;

  public void Start()
  {
    this.myRenderer.material = new Material(this.material);
    this.myRenderer.material.SetFloat(this.floatToChange, this.floatVar);
  }
}
