// Decompiled with JetBrains decompiler
// Type: MoonImage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
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
