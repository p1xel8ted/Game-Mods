// Decompiled with JetBrains decompiler
// Type: TextureOFFSET
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class TextureOFFSET : BaseMonoBehaviour
{
  public float scrollSpeed = 0.5f;
  private Renderer rend;

  private void Start() => this.rend = this.GetComponent<Renderer>();

  private void Update()
  {
    this.rend.material.mainTextureOffset = new Vector2(Time.time * this.scrollSpeed, 0.0f);
  }
}
