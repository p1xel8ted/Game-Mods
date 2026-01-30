// Decompiled with JetBrains decompiler
// Type: Astrologist
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
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
