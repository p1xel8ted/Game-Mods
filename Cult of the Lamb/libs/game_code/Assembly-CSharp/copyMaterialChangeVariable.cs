// Decompiled with JetBrains decompiler
// Type: copyMaterialChangeVariable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
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
