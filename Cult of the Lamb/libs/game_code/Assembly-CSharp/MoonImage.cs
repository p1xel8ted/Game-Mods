// Decompiled with JetBrains decompiler
// Type: MoonImage
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
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
