// Decompiled with JetBrains decompiler
// Type: TranslateShaderOffset
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
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
