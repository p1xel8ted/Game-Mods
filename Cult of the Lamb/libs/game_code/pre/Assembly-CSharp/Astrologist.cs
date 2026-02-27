// Decompiled with JetBrains decompiler
// Type: Astrologist
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Astrologist : BaseMonoBehaviour
{
  public List<SpriteRenderer> MoonBoards;
  public List<Sprite> MoonBoardsImage;
  public List<GameObject> MoonBoardPositions;
  public List<bool> MoonBoardsUpdated;
  private Sprite BlankImage;
  public List<Sprite> Images;

  private void Start()
  {
    this.SetImages();
    foreach (SpriteRenderer moonBoard in this.MoonBoards)
      this.MoonBoardsUpdated.Add(false);
  }

  private void SetImages()
  {
    int index = -1;
    while (++index < this.MoonBoards.Count)
    {
      this.MoonBoardsImage.Add(this.Images[DataManager.Instance.DayList[index].MoonPhase]);
      this.MoonBoards[index].sprite = this.BlankImage;
    }
  }
}
