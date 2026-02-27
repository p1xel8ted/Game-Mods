// Decompiled with JetBrains decompiler
// Type: MoonImage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MoonImage : BaseMonoBehaviour
{
  public List<Sprite> Images;

  private void Start()
  {
    this.GetComponent<SpriteRenderer>().sprite = this.Images[DataManager.Instance.CurrentDay.MoonPhase];
  }
}
