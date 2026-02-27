// Decompiled with JetBrains decompiler
// Type: copyMaterialChangeVariable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
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
