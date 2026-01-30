// Decompiled with JetBrains decompiler
// Type: TempleDecorationsSermonPropSwitcher
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
public class TempleDecorationsSermonPropSwitcher : MonoBehaviour
{
  public GameObject[] ShowDuringRitual;
  public GameObject[] HideDuringRitual;
  public GameObject[] ShowDuringSermon;
  public GameObject[] HideDuringSermon;

  public void OnEnable()
  {
    Ritual.OnBegin += new Action<bool>(this.RitualOnBegin);
    Ritual.OnEnd += new Action<bool>(this.RitualOnEnd);
    SermonController.OnBegin += new Action<bool>(this.SermonOnBegin);
    SermonController.OnEnd += new Action<bool>(this.SermonOnEnd);
  }

  public void OnDisable()
  {
    Ritual.OnBegin -= new Action<bool>(this.RitualOnBegin);
    Ritual.OnEnd -= new Action<bool>(this.RitualOnEnd);
    SermonController.OnBegin -= new Action<bool>(this.SermonOnBegin);
    SermonController.OnEnd -= new Action<bool>(this.SermonOnEnd);
  }

  public void RitualOnBegin(bool cancelled)
  {
    this.HandleProps(this.ShowDuringRitual, true);
    this.HandleProps(this.HideDuringRitual, false);
  }

  public void RitualOnEnd(bool cancelled)
  {
    this.HandleProps(this.ShowDuringRitual, false);
    this.HandleProps(this.HideDuringRitual, true);
  }

  public void SermonOnBegin(bool cancelled)
  {
    this.HandleProps(this.ShowDuringSermon, true);
    this.HandleProps(this.HideDuringSermon, false);
  }

  public void SermonOnEnd(bool cancelled)
  {
    this.HandleProps(this.ShowDuringSermon, false);
    this.HandleProps(this.HideDuringSermon, true);
  }

  public void HandleProps(GameObject[] group, bool active)
  {
    for (uint index = 0; (long) index < (long) group.Length; ++index)
      group[(int) index].SetActive(active);
  }
}
