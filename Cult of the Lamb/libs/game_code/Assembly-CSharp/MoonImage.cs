// Decompiled with JetBrains decompiler
// Type: MoonImage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class MoonImage : BaseMonoBehaviour
{
  public List<Sprite> Images;

  public void Start()
  {
    this.GetComponent<SpriteRenderer>().sprite = this.Images[DataManager.Instance.CurrentDay.MoonPhase];
  }
}
