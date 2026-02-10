// Decompiled with JetBrains decompiler
// Type: destroyMe
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class destroyMe : BaseMonoBehaviour
{
  public float timer;
  public bool timeBased = true;
  public float deathtimer = 10f;
  public bool destroy;
  [Space]
  public bool conditionBased;
  public List<destroyMe.VariableAndCondition> conditionalVariables = new List<destroyMe.VariableAndCondition>();

  public void OnEnable()
  {
    this.timer = 0.0f;
    if (!this.conditionBased)
      return;
    foreach (destroyMe.VariableAndCondition conditionalVariable in this.conditionalVariables)
    {
      if (DataManager.Instance.GetVariable(conditionalVariable.Variable) != conditionalVariable.Condition)
        this.Destroy();
    }
  }

  public void OnDisable() => this.timer = 0.0f;

  public void Update()
  {
    this.timer += Time.deltaTime;
    if ((double) this.timer < (double) this.deathtimer || !this.timeBased)
      return;
    this.Destroy();
  }

  public void Destroy()
  {
    if (!this.destroy)
      this.gameObject.Recycle();
    else
      UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  [Serializable]
  public class VariableAndCondition
  {
    public DataManager.Variables Variable;
    public bool Condition = true;
  }
}
