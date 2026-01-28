// Decompiled with JetBrains decompiler
// Type: TextureOFFSET
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class TextureOFFSET : BaseMonoBehaviour
{
  public float scrollSpeed = 0.5f;
  public Renderer rend;

  public void Start() => this.rend = this.GetComponent<Renderer>();

  public void Update()
  {
    this.rend.material.mainTextureOffset = new Vector2(Time.time * this.scrollSpeed, 0.0f);
  }
}
