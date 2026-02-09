// Decompiled with JetBrains decompiler
// Type: ChancedStringValue
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;
using UnityEngine;

#nullable disable
[Serializable]
public class ChancedStringValue
{
  [SerializeField]
  public string _id = "";
  [SerializeField]
  public string _id2 = "";
  [SerializeField]
  public SmartExpression _expression;
  [SerializeField]
  public bool _chanced;

  public string GetValue(WorldGameObject wgo = null, WorldGameObject character = null)
  {
    return this._expression == null || !this._chanced || (double) this._expression.EvaluateFloat(wgo, character) * 100.0 >= (double) UnityEngine.Random.Range(0, 100) ? this._id : this._id2;
  }
}
