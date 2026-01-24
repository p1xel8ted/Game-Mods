// Decompiled with JetBrains decompiler
// Type: FlockadeBookShelf
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
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
