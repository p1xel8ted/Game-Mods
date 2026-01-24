// Decompiled with JetBrains decompiler
// Type: BarkInjector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MMTools;
using Spine.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class BarkInjector : MonoBehaviour
{
  [SerializeField]
  public SimpleBark _bark;
  [SerializeField]
  public SimpleBarkRepeating _barkRepeating;
  [SerializeField]
  public SkeletonAnimation _spine;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "_spine")]
  public string _talkAnimation;
  [SerializeField]
  [SpineAnimation("", "", true, false, dataField = "_spine")]
  public string _idleAnimation;
  [SerializeField]
  public List<BarkInjector.ConditionAndBark> _conditionsAndBarks = new List<BarkInjector.ConditionAndBark>();

  public void OnEnable()
  {
    this.ClearBarks();
    foreach (BarkInjector.ConditionAndBark conditionAndBark in this._conditionsAndBarks)
    {
      bool flag = true;
      foreach (Interaction_SimpleConversation.VariableAndCondition condition in conditionAndBark.Conditions)
      {
        if (DataManager.Instance.GetVariable(condition.Variable) != condition.Condition)
          flag = false;
      }
      if (flag)
        this.AddBark(in conditionAndBark);
    }
  }

  public void AddBark(in BarkInjector.ConditionAndBark conditionAndBark)
  {
    foreach (BarkInjector.BarkTerm barkTerm in conditionAndBark.TermsToBark)
    {
      ConversationEntry conversationEntry = new ConversationEntry(this._spine.gameObject, barkTerm.Term, this._talkAnimation, CharacterName: conditionAndBark.CharacterName);
      conversationEntry.DefaultAnimation = this._idleAnimation;
      this._bark?.Entries.Add(conversationEntry);
      this._barkRepeating?.Entries.Add(conversationEntry);
    }
  }

  public void ClearBarks()
  {
    this._bark?.Entries.Clear();
    this._barkRepeating?.Entries.Clear();
  }

  [Serializable]
  public struct ConditionAndBark
  {
    public List<Interaction_SimpleConversation.VariableAndCondition> Conditions;
    [TermsPopup("")]
    public string CharacterName;
    public List<BarkInjector.BarkTerm> TermsToBark;
  }

  [Serializable]
  public struct BarkTerm
  {
    [TermsPopup("")]
    public string Term;
  }
}
