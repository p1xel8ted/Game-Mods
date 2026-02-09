// Decompiled with JetBrains decompiler
// Type: GJAnimRandomTrigger
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class GJAnimRandomTrigger : MonoBehaviour
{
  public Animator _animator;
  public float time_period = 10f;
  public bool roll_on_awake = true;
  public List<float> weights = new List<float>();
  public List<string> triggers = new List<string>();
  public float _update_time_counter;
  public float _w_sum;
  public bool is_roll_enable = true;

  public void Start()
  {
    this._animator = this.GetComponentInChildren<Animator>();
    if ((Object) this._animator == (Object) null)
    {
      Debug.LogError((object) "GJAnimRandomTrigger: Animator is null", (Object) this);
    }
    else
    {
      this._update_time_counter = 0.0f;
      double sum = (double) this.CalculateSum();
    }
  }

  public float CalculateSum()
  {
    this._w_sum = 0.0f;
    foreach (float weight in this.weights)
      this._w_sum += weight;
    return this._w_sum;
  }

  public void SetRollActive(bool is_active) => this.is_roll_enable = is_active;

  public void Awake()
  {
    if (!this.roll_on_awake)
      return;
    this.DoRoll();
  }

  public void Update()
  {
    this._update_time_counter += Time.deltaTime;
    if ((double) this._update_time_counter < (double) this.time_period || !this.is_roll_enable)
      return;
    this.DoRoll();
  }

  public void DoRoll()
  {
    if ((Object) this._animator == (Object) null)
      this.Start();
    this._update_time_counter = 0.0f;
    float num1 = Random.Range(0.0f, this._w_sum);
    float num2 = 0.0f;
    int index = -1;
    foreach (float weight in this.weights)
    {
      ++index;
      num2 += weight;
      if ((double) num1 <= (double) num2)
      {
        if (!(this.triggers[index] != ""))
          break;
        this._animator.SetTrigger(this.triggers[index]);
        break;
      }
    }
  }
}
