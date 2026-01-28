// Decompiled with JetBrains decompiler
// Type: TranslateShaderOffset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
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
