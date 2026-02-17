// Decompiled with JetBrains decompiler
// Type: FlockadeBookShelf
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Flockade;
using System;
using UnityEngine;

#nullable disable
public class FlockadeBookShelf : MonoBehaviour
{
  [SerializeField]
  public FlockadeBookShelf.FlockadePieceObject[] pieces;

  public void OnEnable()
  {
    for (int index = 0; index < this.pieces.Length; ++index)
      this.pieces[index].Sprite.gameObject.SetActive(DataManager.Instance.PlayerFoundPieces.Contains(this.pieces[index].PieceType));
  }

  [Serializable]
  public struct FlockadePieceObject
  {
    public SpriteRenderer Sprite;
    public FlockadePieceType PieceType;
  }
}
