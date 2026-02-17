// Decompiled with JetBrains decompiler
// Type: copyMaterialChangeVariable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
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
