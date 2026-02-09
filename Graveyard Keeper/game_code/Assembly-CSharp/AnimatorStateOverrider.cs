// Decompiled with JetBrains decompiler
// Type: AnimatorStateOverrider
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using LinqTools;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class AnimatorStateOverrider : MonoBehaviour
{
  [SerializeField]
  public List<AnimatorStateOverriderAtom> parameters_list = new List<AnimatorStateOverriderAtom>();
  public Animator _source_animator;
  public List<Animator> _destination_animators = new List<Animator>();

  public void Awake()
  {
    this._source_animator = this.GetComponent<Animator>();
    foreach (Animator animator in ((IEnumerable<Animator>) this.GetComponentsInChildren<Animator>(true)).ToList<Animator>())
    {
      if ((Object) animator != (Object) this._source_animator)
        this._destination_animators.Add(animator);
    }
  }

  public void Update()
  {
    if ((Object) this._source_animator != (Object) null && this._destination_animators.Count != 0)
    {
      foreach (AnimatorStateOverriderAtom parameters in this.parameters_list)
      {
        string sourceStateName = parameters.source_state_name;
        string destinationStateName = parameters.destination_state_name;
        switch (parameters.animator_source_state_type)
        {
          case AnimatorStateOverriderAtom.AnimatorStates.FLOAT:
            float num = this._source_animator.GetFloat(sourceStateName);
            using (List<Animator>.Enumerator enumerator = this._destination_animators.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                Animator current = enumerator.Current;
                if ((Object) current != (Object) null)
                  current.SetFloat(destinationStateName, num);
              }
              continue;
            }
          case AnimatorStateOverriderAtom.AnimatorStates.INT:
            int integer = this._source_animator.GetInteger(sourceStateName);
            using (List<Animator>.Enumerator enumerator = this._destination_animators.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                Animator current = enumerator.Current;
                if ((Object) current != (Object) null)
                  current.SetInteger(destinationStateName, integer);
              }
              continue;
            }
          case AnimatorStateOverriderAtom.AnimatorStates.BOOL:
            bool flag = this._source_animator.GetBool(sourceStateName);
            using (List<Animator>.Enumerator enumerator = this._destination_animators.GetEnumerator())
            {
              while (enumerator.MoveNext())
              {
                Animator current = enumerator.Current;
                if ((Object) current != (Object) null)
                  current.SetBool(destinationStateName, flag);
              }
              continue;
            }
          default:
            continue;
        }
      }
    }
    else
      Debug.LogError((object) ("AnimatorStateOverrider:Update, some problems with source or(and) destination animators: source is null(" + this._source_animator?.ToString() == $"), destinationanimtaors count({this._destination_animators.Count.ToString()})"));
  }
}
