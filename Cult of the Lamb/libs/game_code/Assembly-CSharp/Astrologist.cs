// Decompiled with JetBrains decompiler
// Type: Astrologist
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Astrologist : BaseMonoBehaviour
{
  public List<SpriteRenderer> MoonBoards;
  public List<Sprite> MoonBoardsImage;
  public List<GameObject> MoonBoardPositions;
  public List<bool> MoonBoardsUpdated;
  public Sprite BlankImage;
  public List<Sprite> Images;

  public void Start()
  {
    this.SetImages();
    foreach (SpriteRenderer moonBoard in this.MoonBoards)
      this.MoonBoardsUpdated.Add(false);
  }

  public void SetImages()
  {
    int index = -1;
    while (++index < this.MoonBoards.Count)
    {
      this.MoonBoardsImage.Add(this.Images[DataManager.Instance.DayList[index].MoonPhase]);
      this.MoonBoards[index].sprite = this.BlankImage;
    }
  }
}
