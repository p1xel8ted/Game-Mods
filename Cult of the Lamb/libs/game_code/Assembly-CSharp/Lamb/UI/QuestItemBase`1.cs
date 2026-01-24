// Decompiled with JetBrains decompiler
// Type: Lamb.UI.QuestItemBase`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections.Generic;
using TMPro;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public abstract class QuestItemBase<T> : MonoBehaviour
{
  public const string kTrackingOnAnimation = "on";
  public const string kTrackignTurnOnAnimation = "turn-on";
  public const string kTrackingOffAnimation = "off";
  [Header("General")]
  [SerializeField]
  public MMButton _button;
  [SerializeField]
  public TextMeshProUGUI _title;
  [SerializeField]
  public RectTransform _objectivesContainer;
  [SerializeField]
  public QuestItemObjective _questItemObjectiveTemplate;
  public string _uniqueGroupID = "";
  public bool _trackedQuest;
  public List<T> _datas = new List<T>();

  public MMButton Button => this._button;

  public abstract void AddObjectivesData(T data);

  public abstract void AddObjective(T data, bool failed = false);

  public virtual void Configure(bool failed = false)
  {
  }
}
