// Decompiled with JetBrains decompiler
// Type: TranslateShaderOffset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class TranslateShaderOffset : MonoBehaviour
{
  public string propertyName = "_MainTex";
  public Vector2 velocity;
  public Vector2 offset;

  public void Start()
  {
    this.offset = this.GetComponent<Renderer>().material.GetTextureOffset(this.propertyName);
  }

  public void Update()
  {
    this.offset += this.velocity * Time.deltaTime;
    this.GetComponent<Renderer>().material.SetTextureOffset(this.propertyName, this.offset);
  }
}
