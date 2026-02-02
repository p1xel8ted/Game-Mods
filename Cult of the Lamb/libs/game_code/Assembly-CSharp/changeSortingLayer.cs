// Decompiled with JetBrains decompiler
// Type: changeSortingLayer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class changeSortingLayer : BaseMonoBehaviour
{
  public SpriteRenderer sprite;
  public string sortingLayer;

  public void Start() => this.sprite.sortingLayerName = this.sortingLayer;
}
