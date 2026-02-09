// Decompiled with JetBrains decompiler
// Type: UIChangingSprite
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
[RequireComponent(typeof (UI2DSprite))]
public class UIChangingSprite : MonoBehaviour
{
  public List<UnityEngine.Sprite> sprites = new List<UnityEngine.Sprite>();

  public void ChangeSprite(int n) => this.GetComponent<UI2DSprite>().sprite2D = this.sprites[n];
}
