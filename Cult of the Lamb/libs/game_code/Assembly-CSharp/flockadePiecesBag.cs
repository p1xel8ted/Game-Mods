// Decompiled with JetBrains decompiler
// Type: flockadePiecesBag
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class flockadePiecesBag : MonoBehaviour
{
  [SerializeField]
  public SpriteRenderer _sprite;

  public void Configure(Sprite s = null)
  {
    if ((Object) s == (Object) null)
      return;
    this._sprite.sprite = s;
    this.transform.localScale *= 0.5f;
    this._sprite.transform.localPosition += new Vector3(0.0f, 0.5f);
  }
}
