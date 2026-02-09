// Decompiled with JetBrains decompiler
// Type: AuraReceiver
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class AuraReceiver : WorldGameObjectComponentBase
{
  public GameRes _auras_params = new GameRes();
  public GameRes _auras_temp_calc = new GameRes();
  public static List<AuraReceiver> _all = new List<AuraReceiver>();
  [SerializeField]
  public List<AuraReceiveSubcomponent> _aura_receivers = new List<AuraReceiveSubcomponent>();
  [SerializeField]
  public List<string> _aura_receiver_ids = new List<string>();
  [NonSerialized]
  public List<string> auras = new List<string>();

  public void Start() => AuraReceiver._all.Add(this);

  public void OnDestroy() => AuraReceiver._all.Remove(this);

  public void AddAuraReceiverSubcomponent(string aura_id)
  {
    if (this._aura_receiver_ids.Contains(aura_id))
      return;
    this._aura_receiver_ids.Add(aura_id);
    this._aura_receivers.Add(new AuraReceiveSubcomponent(this, aura_id));
  }

  public static void OnAreaCalculationStarted()
  {
    foreach (AuraReceiver auraReceiver in AuraReceiver._all)
      auraReceiver._auras_temp_calc = new GameRes();
  }

  public static void OnAreaCalculationFinished()
  {
    foreach (AuraReceiver auraReceiver in AuraReceiver._all)
    {
      auraReceiver._auras_temp_calc.RemoveZeroValues();
      foreach (GameResAtom atom in auraReceiver._auras_params.ToAtomList())
      {
        if (auraReceiver._auras_temp_calc.Get(atom.type).EqualsTo(0.0f))
          auraReceiver.OnAuraDisappeared(atom.type);
      }
      foreach (GameResAtom atom in auraReceiver._auras_temp_calc.ToAtomList())
      {
        if (auraReceiver._auras_params.Get(atom.type).EqualsTo(0.0f))
          auraReceiver.OnAuraAppeared(atom.type);
      }
      auraReceiver._auras_params = auraReceiver._auras_temp_calc.Clone();
    }
  }

  public void ProcessAura(AuraEmitSubcomponent aura, float distance)
  {
    distance = Mathf.Abs(distance) / 96f;
    float num = Mathf.Min(((double) distance <= (double) aura.aura.radius ? 1f : 0.0f) + this._auras_temp_calc.Get(aura.aura_id), 1f);
    this._auras_temp_calc.Set(aura.aura_id, num);
  }

  public static IEnumerable<int> ProcessAuraEmitCalculation(
    AuraEmitSubcomponent aura,
    Vector2 emitter_pos)
  {
    using (List<AuraReceiver>.Enumerator enumerator = AuraReceiver._all.GetEnumerator())
    {
      while (enumerator.MoveNext())
      {
        AuraReceiver current = enumerator.Current;
        current.ProcessAura(aura, ((Vector2) current.tf.position - emitter_pos).magnitude);
      }
      yield break;
    }
  }

  public void OnAuraAppeared(string aura_id)
  {
    Debug.Log((object) ("AuraReceiver.OnAuraAppeared " + aura_id), (UnityEngine.Object) this.wgo);
    this.auras.Add(aura_id);
    foreach (AuraReceiveSubcomponent auraReceiver in this._aura_receivers)
    {
      if (auraReceiver.aura_id == aura_id)
        auraReceiver.OnAuraAppeared();
    }
  }

  public void OnAuraDisappeared(string aura_id)
  {
    Debug.Log((object) ("AuraReceiver.OnAuraRemoved " + aura_id), (UnityEngine.Object) this.wgo);
    this.auras.Remove(aura_id);
    foreach (AuraReceiveSubcomponent auraReceiver in this._aura_receivers)
    {
      if (auraReceiver.aura_id == aura_id)
        auraReceiver.OnAuraDisappeared();
    }
  }

  public void Clear()
  {
    this._aura_receivers.Clear();
    this._aura_receiver_ids.Clear();
  }

  public void Update()
  {
    foreach (AuraSubcomponentBase auraReceiver in this._aura_receivers)
      auraReceiver.Update();
  }

  public float GetAuraParam(string aura_id) => this._auras_params.Get(aura_id);
}
