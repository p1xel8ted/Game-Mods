// Decompiled with JetBrains decompiler
// Type: EnemyDeathCatEyesManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class EnemyDeathCatEyesManager : MonoBehaviour
{
  public static EnemyDeathCatEyesManager Instance;
  [SerializeField]
  public List<EnemyDeathCatEye> eyes;
  [SerializeField]
  public EnemyDeathCatEyesManager.PositionSet[] threeEyeSets;
  [SerializeField]
  public EnemyDeathCatEyesManager.PositionSet[] twoEyeSets;
  [SerializeField]
  public EnemyDeathCatEyesManager.PositionSet[] oneEyeSets;
  [Space]
  [SerializeField]
  public Vector2 timeBetweenAttacks;
  public int previousPositionSetIndex;
  public bool eyesActive;
  [CompilerGenerated]
  public bool \u003CActive\u003Ek__BackingField = true;
  public Coroutine activeRoutine;

  public List<EnemyDeathCatEye> Eyes => this.eyes;

  public bool Active
  {
    get => this.\u003CActive\u003Ek__BackingField;
    set => this.\u003CActive\u003Ek__BackingField = value;
  }

  public void Awake()
  {
    EnemyDeathCatEyesManager.Instance = this;
    foreach (EnemyDeathCatEye eye in this.eyes)
    {
      eye.GetComponent<Health>().enabled = false;
      eye.Spine.gameObject.SetActive(false);
    }
  }

  public void Update()
  {
    this.eyesActive = false;
    foreach (EnemyDeathCatEye eye in this.eyes)
    {
      if (eye.Active)
      {
        this.eyesActive = true;
        break;
      }
    }
    if (this.eyesActive || !this.Active || this.activeRoutine != null)
      return;
    this.SetPositionsAndAttack();
  }

  public void HideAllEyes(float maxDelay)
  {
    foreach (EnemyDeathCatEye eye in this.eyes)
      eye.Hide(UnityEngine.Random.Range(0.0f, maxDelay));
    if (this.activeRoutine == null)
      return;
    this.StopCoroutine(this.activeRoutine);
    this.activeRoutine = (Coroutine) null;
  }

  public void SetPositionsAndAttack()
  {
    this.activeRoutine = this.StartCoroutine((IEnumerator) this.SetPositionsAndAttackIE());
  }

  public IEnumerator SetPositionsAndAttackIE()
  {
    EnemyDeathCatEyesManager deathCatEyesManager = this;
    deathCatEyesManager.HideAllEyes(0.5f);
    yield return (object) new WaitForSeconds(0.5f);
    if (deathCatEyesManager.eyes.Count <= 0)
    {
      deathCatEyesManager.activeRoutine = (Coroutine) null;
    }
    else
    {
      yield return (object) new WaitForSeconds(UnityEngine.Random.Range(0.0f, 0.5f));
      int num1 = deathCatEyesManager.eyes.Count - 1;
      EnemyDeathCatEyesManager.PositionSet[] positionSetArray = deathCatEyesManager.threeEyeSets;
      switch (num1)
      {
        case 0:
          positionSetArray = deathCatEyesManager.oneEyeSets;
          break;
        case 1:
          positionSetArray = deathCatEyesManager.twoEyeSets;
          break;
      }
      int index1;
      do
      {
        index1 = UnityEngine.Random.Range(0, positionSetArray.Length);
      }
      while (index1 == deathCatEyesManager.previousPositionSetIndex);
      EnemyDeathCatEyesManager.PositionSet positionSet = positionSetArray[index1];
      deathCatEyesManager.previousPositionSetIndex = index1;
      for (int index2 = 0; index2 < num1 + 1; ++index2)
      {
        float num2 = Vector3.Distance(deathCatEyesManager.eyes[index2].transform.position, positionSet.Positions[index2]);
        deathCatEyesManager.eyes[index2].Reposition(positionSet.Positions[index2], num2 / 5f);
      }
      while (true)
      {
        bool flag = true;
        foreach (EnemyDeathCatEye eye in deathCatEyesManager.eyes)
        {
          if (!eye.Active)
          {
            flag = false;
            break;
          }
        }
        if (!flag)
          yield return (object) null;
        else
          break;
      }
      deathCatEyesManager.activeRoutine = deathCatEyesManager.StartCoroutine((IEnumerator) deathCatEyesManager.Attack());
    }
  }

  public IEnumerator Attack()
  {
    while (true)
    {
      List<EnemyDeathCatEye> source = new List<EnemyDeathCatEye>();
      foreach (EnemyDeathCatEye eye in this.eyes)
      {
        if (eye.Active)
          source.Add(eye);
      }
      int num1 = 5;
      int index = UnityEngine.Random.Range(0, num1);
      float num2 = UnityEngine.Random.Range(0.0f, 1f);
      float num3 = num2 + UnityEngine.Random.Range(0.0f, 1f);
      int num4 = (double) UnityEngine.Random.value > 0.5 ? 1 : 0;
      if (source.Count >= 3)
      {
        List<EnemyDeathCatEye> list = source.OrderBy<EnemyDeathCatEye, float>((Func<EnemyDeathCatEye, float>) (x => x.transform.position.x)).ToList<EnemyDeathCatEye>();
        if ((double) UnityEngine.Random.value < 0.5)
        {
          list[0].Attack(index, this.eyes.Count, num4 == 0 ? num2 : num3);
          list[2].Attack(index, this.eyes.Count, num4 == 0 ? num2 : num3);
          list[1].Attack((int) Utils.Repeat((double) index + (double) UnityEngine.Random.value > 0.5 ? -1f : 1f, (float) num1), this.eyes.Count, num4 == 0 ? num3 : num2);
        }
        else
        {
          list[0].Attack(index, this.eyes.Count, UnityEngine.Random.Range(0.0f, 1f));
          list[1].Attack(index, this.eyes.Count, UnityEngine.Random.Range(0.0f, 1f));
          list[2].Attack(index, this.eyes.Count, UnityEngine.Random.Range(0.0f, 1f));
        }
      }
      else if (source.Count >= 2)
      {
        source[0].Attack(index, this.eyes.Count, UnityEngine.Random.Range(0.0f, 1f));
        source[1].Attack(index, this.eyes.Count, UnityEngine.Random.Range(0.0f, 1f));
      }
      else if (source.Count >= 1)
        source[0].Attack(index, this.eyes.Count, UnityEngine.Random.Range(0.0f, 1f));
      else
        break;
      while (true)
      {
        bool flag = false;
        foreach (EnemyDeathCatEye eye in this.eyes)
        {
          if (eye.Attacking && eye.Active)
            flag = true;
        }
        if (flag)
          yield return (object) null;
        else
          break;
      }
      yield return (object) new WaitForSeconds(UnityEngine.Random.Range(0.3f, 0.6f));
    }
    this.activeRoutine = (Coroutine) null;
  }

  public void OnDrawGizmosSelected()
  {
    foreach (EnemyDeathCatEyesManager.PositionSet threeEyeSet in this.threeEyeSets)
    {
      Gizmos.color = UnityEngine.Random.ColorHSV();
      foreach (Vector3 position in threeEyeSet.Positions)
        Gizmos.DrawWireSphere(position, 0.3f);
    }
    foreach (EnemyDeathCatEyesManager.PositionSet twoEyeSet in this.twoEyeSets)
    {
      Gizmos.color = UnityEngine.Random.ColorHSV();
      foreach (Vector3 position in twoEyeSet.Positions)
        Gizmos.DrawWireSphere(position, 0.3f);
    }
    foreach (EnemyDeathCatEyesManager.PositionSet oneEyeSet in this.oneEyeSets)
    {
      Gizmos.color = UnityEngine.Random.ColorHSV();
      foreach (Vector3 position in oneEyeSet.Positions)
        Gizmos.DrawWireSphere(position, 0.3f);
    }
  }

  [Serializable]
  public struct PositionSet
  {
    public Vector3[] Positions;
  }
}
