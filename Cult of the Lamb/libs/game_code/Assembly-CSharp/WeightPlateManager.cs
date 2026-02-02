// Decompiled with JetBrains decompiler
// Type: WeightPlateManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
public class WeightPlateManager : BaseMonoBehaviour
{
  public static WeightPlateManager.PlayerActivatingWeightPlate OnPlayerActivatingWeightPlate;
  public List<WeightPlate> WeightPlates = new List<WeightPlate>();
  public float ActivateRange = 4f;
  public float DeactivateRange = 6f;
  public bool PlayerInRange;
  public UnityEvent ActivatedCallback;
  public UnityEvent DeactivatedCallback;
  public int _ActivatedCount;

  public void GetPlates()
  {
    this.WeightPlates = new List<WeightPlate>((IEnumerable<WeightPlate>) this.GetComponentsInChildren<WeightPlate>());
  }

  public void Start()
  {
    foreach (WeightPlate weightPlate in this.WeightPlates)
      weightPlate.WeightPlateManager = this;
  }

  public void Update()
  {
    if ((Object) PlayerFarming.Instance == (Object) null)
      return;
    if (!this.PlayerInRange && (double) Vector3.Distance(this.transform.position, PlayerFarming.Instance.transform.position) < (double) this.ActivateRange)
    {
      Debug.Log((object) "SEND MESSAGE!");
      WeightPlateManager.PlayerActivatingWeightPlate activatingWeightPlate = WeightPlateManager.OnPlayerActivatingWeightPlate;
      if (activatingWeightPlate != null)
        activatingWeightPlate(this.WeightPlates);
      this.PlayerInRange = true;
    }
    if (!this.PlayerInRange || (double) Vector3.Distance(this.transform.position, PlayerFarming.Instance.transform.position) <= (double) this.DeactivateRange)
      return;
    this.PlayerInRange = false;
  }

  public void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position, this.ActivateRange, Color.green);
    Utils.DrawCircleXY(this.transform.position, this.DeactivateRange, Color.red);
  }

  public int ActivatedCount
  {
    get => this._ActivatedCount;
    set
    {
      if (this._ActivatedCount != value)
      {
        if (value == this.WeightPlates.Count)
          this.ActivatedCallback?.Invoke();
        if (this._ActivatedCount == this.WeightPlates.Count && value < this.WeightPlates.Count)
          this.DeactivatedCallback?.Invoke();
      }
      this._ActivatedCount = value;
    }
  }

  public delegate void PlayerActivatingWeightPlate(List<WeightPlate> WeightPlates);
}
