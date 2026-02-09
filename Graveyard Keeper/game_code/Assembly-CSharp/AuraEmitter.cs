// Decompiled with JetBrains decompiler
// Type: AuraEmitter
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class AuraEmitter : WorldGameObjectComponentBase
{
  [SerializeField]
  public List<AuraEmitSubcomponent> _auras = new List<AuraEmitSubcomponent>();
  [SerializeField]
  public List<string> _aura_ids = new List<string>();
  public static List<AuraEmitter> _all = new List<AuraEmitter>();

  public void Start() => AuraEmitter._all.Add(this);

  public void OnDestroy() => AuraEmitter._all.Remove(this);

  public IEnumerable<int> DoEmitCalculations()
  {
    AuraEmitter auraEmitter = this;
    Vector2 pos = (Vector2) auraEmitter.tf.position;
    foreach (AuraEmitSubcomponent aura in auraEmitter._auras)
    {
      foreach (int num in AuraReceiver.ProcessAuraEmitCalculation(aura, pos))
        ;
      yield return 0;
    }
  }

  public void AddAura(string aura_id)
  {
    if (this._aura_ids.Contains(aura_id))
      return;
    this._aura_ids.Add(aura_id);
    List<AuraEmitSubcomponent> auras = this._auras;
    AuraEmitSubcomponent emitSubcomponent = new AuraEmitSubcomponent();
    emitSubcomponent.aura_id = aura_id;
    auras.Add(emitSubcomponent);
  }

  public static void ProcessAurasCalculation()
  {
    AuraReceiver.OnAreaCalculationStarted();
    foreach (AuraEmitter auraEmitter in AuraEmitter._all)
    {
      foreach (int doEmitCalculation in auraEmitter.DoEmitCalculations())
        ;
    }
    AuraReceiver.OnAreaCalculationFinished();
  }

  public void Clear()
  {
    this._auras.Clear();
    this._aura_ids.Clear();
  }
}
