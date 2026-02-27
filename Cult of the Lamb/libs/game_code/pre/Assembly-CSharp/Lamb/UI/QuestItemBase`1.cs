// Decompiled with JetBrains decompiler
// Type: Lamb.UI.QuestItemBase`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using TMPro;
using UnityEngine;

#nullable disable
namespace Lamb.UI;

public abstract class QuestItemBase<T> : MonoBehaviour
{
  protected const string kTrackingOnAnimation = "on";
  protected const string kTrackignTurnOnAnimation = "turn-on";
  protected const string kTrackingOffAnimation = "off";
  [Header("General")]
  [SerializeField]
  protected MMButton _button;
  [SerializeField]
  protected TextMeshProUGUI _title;
  [SerializeField]
  protected RectTransform _objectivesContainer;
  [SerializeField]
  protected QuestItemObjective _questItemObjectiveTemplate;
  protected string _uniqueGroupID = "";
  protected bool _trackedQuest;
  protected List<T> _datas = new List<T>();

  public MMButton Button => this._button;

  public abstract void AddObjectivesData(T data);

  protected abstract void AddObjective(T data, bool failed = false);

  public virtual void Configure(bool failed = false)
  {
  }
}
