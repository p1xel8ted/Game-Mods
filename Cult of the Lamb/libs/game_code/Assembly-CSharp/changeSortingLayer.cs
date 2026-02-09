// Decompiled with JetBrains decompiler
// Type: changeSortingLayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class changeSortingLayer : BaseMonoBehaviour
{
  public SpriteRenderer sprite;
  public string sortingLayer;

  public void Start() => this.sprite.sortingLayerName = this.sortingLayer;
}
