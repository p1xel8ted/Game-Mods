// Decompiled with JetBrains decompiler
// Type: StructureProgressBar
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class StructureProgressBar : BaseMonoBehaviour
{
  public Structure structure;
  public Transform ProgressBar;
  public GameObject CandleOn;
  public GameObject CandleOff;
  public float _Y;

  public float Y
  {
    get => this._Y;
    set
    {
      this._Y = value;
      if ((double) this._Y >= 1.0 && this.CandleOff.activeSelf)
      {
        this._Y = 1f;
        this.CandleOn.SetActive(true);
        this.CandleOff.SetActive(false);
        this.structure.ProgressCompleted();
      }
      if ((double) this._Y > 0.0 || this.CandleOff.activeSelf)
        return;
      this.CandleOn.SetActive(false);
      this.CandleOff.SetActive(true);
    }
  }

  public void Start()
  {
    if ((double) this.structure.Structure_Info.Progress < (double) this.structure.Structure_Info.ProgressTarget)
    {
      this.CandleOn.SetActive(false);
      this.CandleOff.SetActive(true);
    }
    else
    {
      this.CandleOn.SetActive(true);
      this.CandleOff.SetActive(false);
    }
  }

  public void Update()
  {
    if ((double) this.Y < 1.0)
    {
      this.Y = this.structure.Structure_Info.Progress / this.structure.Structure_Info.ProgressTarget;
      this.ProgressBar.localPosition = new Vector3(0.0f, (float) ((double) this.Y * 0.800000011920929 - 0.800000011920929), 0.0f);
    }
    if ((double) this.structure.Structure_Info.Progress > 0.0 || (double) this.Y < 1.0)
      return;
    this.Y = this.structure.Structure_Info.Progress / this.structure.Structure_Info.ProgressTarget;
  }
}
