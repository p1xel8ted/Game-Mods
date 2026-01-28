// Decompiled with JetBrains decompiler
// Type: PauseCultManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class PauseCultManager : BaseMonoBehaviour
{
  public Image CultFaithLevel;
  public Image CultHungerLevel;
  public Image CultIllnessLevel;
  public Image CultTiredLevel;
  public TextMeshProUGUI FollowerAmount;
  public float HappinessTotal;
  public float HungerTotal;
  public float IllnessTotal;
  public float TiredTotal;
  public int TotalBrains;
  public float FaithProgress;
  public List<ThoughtData> BadThoughts = new List<ThoughtData>();
  public List<ThoughtData> GoodThoughts = new List<ThoughtData>();
  public List<FollowerThoughtObject> BadThoughtObjects = new List<FollowerThoughtObject>();
  public List<FollowerThoughtObject> GoodThoughtObjects = new List<FollowerThoughtObject>();

  public void Start()
  {
  }

  public void OnEnable()
  {
    foreach (FollowerBrain allBrain in FollowerBrain.AllBrains)
    {
      this.HappinessTotal += allBrain.Stats.Happiness;
      this.HungerTotal += allBrain.Stats.Satiation + (75f - allBrain.Stats.Starvation);
      this.IllnessTotal += allBrain.Stats.Illness;
      this.TiredTotal += allBrain.Stats.Rest;
      ++this.TotalBrains;
    }
    this.FaithProgress = this.TotalBrains <= 0 ? 0.0f : this.HappinessTotal / (100f * (float) this.TotalBrains);
    this.CultFaithLevel.fillAmount = this.FaithProgress;
    this.CultFaithLevel.color = this.ReturnColorBasedOnValue(this.CultFaithLevel.fillAmount);
    this.CultHungerLevel.fillAmount = this.TotalBrains <= 0 ? 0.0f : this.HungerTotal / (175f * (float) this.TotalBrains);
    this.CultHungerLevel.color = this.ReturnColorBasedOnValueHunger(this.CultHungerLevel.fillAmount);
    this.CultIllnessLevel.fillAmount = (float) ((this.TotalBrains <= 0 ? 0.0 : (double) this.IllnessTotal / (100.0 * (double) this.TotalBrains) - 1.0) * -1.0);
    this.CultIllnessLevel.color = this.ReturnColorBasedOnValue(this.CultIllnessLevel.fillAmount);
    this.CultTiredLevel.fillAmount = this.TotalBrains <= 0 ? 0.0f : this.TiredTotal / (100f * (float) this.TotalBrains);
    this.CultTiredLevel.color = this.ReturnColorBasedOnValue(this.CultTiredLevel.fillAmount);
    this.FollowerAmount.text = DataManager.Instance.Followers.Count.ToString();
    this.GoodThoughts = new List<ThoughtData>();
    this.BadThoughts = new List<ThoughtData>();
    foreach (FollowerInfo follower in DataManager.Instance.Followers)
    {
      Debug.Log((object) ("Follower name: " + follower.Name));
      foreach (ThoughtData thought in follower.Thoughts)
      {
        if ((double) thought.Modifier < 0.0)
        {
          bool flag = false;
          foreach (ThoughtData badThought in this.BadThoughts)
          {
            if (badThought.ThoughtType == thought.ThoughtType)
            {
              flag = true;
              badThought.TotalCountDisplay += thought.Quantity;
            }
          }
          if (!flag)
          {
            thought.TotalCountDisplay = thought.Quantity;
            this.BadThoughts.Add(thought);
          }
        }
        else
        {
          bool flag = false;
          foreach (ThoughtData goodThought in this.GoodThoughts)
          {
            if (goodThought.ThoughtType == thought.ThoughtType)
            {
              flag = true;
              goodThought.TotalCountDisplay += thought.Quantity;
            }
          }
          if (!flag)
          {
            thought.TotalCountDisplay = thought.Quantity;
            this.GoodThoughts.Add(thought);
          }
        }
      }
    }
    this.BadThoughts.Sort(new Comparison<ThoughtData>(PauseCultManager.SortLowToHigh));
    this.GoodThoughts.Sort(new Comparison<ThoughtData>(PauseCultManager.SortHighToLow));
    for (int index = 0; index < this.BadThoughtObjects.Count; ++index)
    {
      if (index < this.BadThoughts.Count)
        this.BadThoughtObjects[index].Init(this.BadThoughts[index]);
      else
        this.BadThoughtObjects[index].Init((ThoughtData) null);
    }
    for (int index = 0; index < this.GoodThoughtObjects.Count; ++index)
    {
      if (index < this.GoodThoughts.Count)
        this.GoodThoughtObjects[index].Init(this.GoodThoughts[index]);
      else
        this.GoodThoughtObjects[index].Init((ThoughtData) null);
    }
  }

  public Color ReturnColorBasedOnValue(float f)
  {
    if ((double) f >= 0.0 && (double) f < 0.25)
      return StaticColors.RedColor;
    return (double) f >= 0.25 && (double) f < 0.5 ? StaticColors.OrangeColor : StaticColors.GreenColor;
  }

  public Color ReturnColorBasedOnValueHunger(float f)
  {
    if ((double) f >= 0.0 && (double) f < 0.5)
      return StaticColors.RedColor;
    return (double) f >= 0.5 && (double) f < 0.75 ? StaticColors.OrangeColor : StaticColors.GreenColor;
  }

  public static int SortLowToHigh(ThoughtData t1, ThoughtData t2)
  {
    return t1.Modifier.CompareTo(t2.Modifier);
  }

  public static int SortHighToLow(ThoughtData t1, ThoughtData t2)
  {
    return t2.Modifier.CompareTo(t1.Modifier);
  }

  public void Update()
  {
  }
}
