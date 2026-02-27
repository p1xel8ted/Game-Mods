// Decompiled with JetBrains decompiler
// Type: Lamb.UI.DoctrineSubmenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using src.UINavigator;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
namespace Lamb.UI;

public class DoctrineSubmenu : UISubmenuBase
{
  [Header("Sermon Info")]
  [SerializeField]
  private SermonCategory _sermonCategory;
  [SerializeField]
  private TextMeshProUGUI _sermonCategoryTitle;
  [SerializeField]
  private TextMeshProUGUI _sermonCategoryLore;
  [Header("Pages")]
  [SerializeField]
  private DoctrineDetailsPage _detailsPage;
  [SerializeField]
  private DoctrineDetailsPage _detailsPage2;
  [SerializeField]
  private DoctrineForbiddenPage _forbiddenPage;
  [SerializeField]
  private DoctrineLockedPage _lockedPage;
  [Header("Progress Bar")]
  [SerializeField]
  private GameObject _progressBar;
  [SerializeField]
  private Image _progressBarFill;
  [Header("Choices")]
  [SerializeField]
  private GridLayoutGroup _gridLayoutgroup;
  [SerializeField]
  private MMUILineRenderer _lineRenderer;
  [SerializeField]
  private DoctrineSubmenu.DoctrineChoicePair[] _doctrineChoicePairs;
  private UIMenuBase _currentPage;

  protected override void OnShowStarted()
  {
    LayoutRebuilder.ForceRebuildLayoutImmediate(this._gridLayoutgroup.transform as RectTransform);
    List<Vector2> vector2List = new List<Vector2>();
    foreach (DoctrineSubmenu.DoctrineChoicePair doctrineChoicePair in this._doctrineChoicePairs)
    {
      if ((UnityEngine.Object) doctrineChoicePair.ActiveChoice != (UnityEngine.Object) null)
        vector2List.Add((Vector2) doctrineChoicePair.ActiveChoice.transform.localPosition);
    }
    if (vector2List.Count <= 1)
      return;
    List<MMUILineRenderer.BranchPoint> branchPointList = new List<MMUILineRenderer.BranchPoint>();
    foreach (Vector2 point in vector2List)
      branchPointList.Add(new MMUILineRenderer.BranchPoint(point));
    this._lineRenderer.Points = branchPointList;
    this._lineRenderer.Color = StaticColors.RedColor;
  }

  private void OnEnable()
  {
    this._sermonCategoryTitle.text = $"{DoctrineUpgradeSystem.GetSermonCategoryLocalizedName(this._sermonCategory)} - {DoctrineUpgradeSystem.GetLevelBySermon(this._sermonCategory).ToNumeral()}";
    this._sermonCategoryLore.text = DoctrineUpgradeSystem.GetSermonCategoryLocalizedDescription(this._sermonCategory);
    this._progressBar.SetActive(false);
    List<DoctrineUpgradeSystem.DoctrineType> doctrinesForCategory = DoctrineUpgradeSystem.GetUnlockedDoctrinesForCategory(this._sermonCategory);
    for (int index = 0; index < this._doctrineChoicePairs.Length; ++index)
      this._doctrineChoicePairs[index].Configure(doctrinesForCategory, this._sermonCategory, index + 1);
    if ((UnityEngine.Object) this._doctrineChoicePairs[0].ActiveChoice != (UnityEngine.Object) null)
      this.OverrideDefaultOnce((Selectable) this._doctrineChoicePairs[0].ActiveChoice.GetComponent<MMButton>());
    MonoSingleton<UINavigatorNew>.Instance.OnSelectionChanged += new Action<Selectable, Selectable>(this.OnSelectionChanged);
    MonoSingleton<UINavigatorNew>.Instance.OnDefaultSetComplete += new Action<Selectable>(this.OnSelection);
  }

  private void OnDisable()
  {
    if (!((UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance != (UnityEngine.Object) null))
      return;
    MonoSingleton<UINavigatorNew>.Instance.OnSelectionChanged -= new Action<Selectable, Selectable>(this.OnSelectionChanged);
    MonoSingleton<UINavigatorNew>.Instance.OnDefaultSetComplete -= new Action<Selectable>(this.OnSelection);
  }

  private void OnSelectionChanged(Selectable current, Selectable previous)
  {
    this.OnSelection(current);
  }

  private void OnSelection(Selectable current)
  {
    DoctrineChoice component;
    if (!current.TryGetComponent<DoctrineChoice>(out component))
      return;
    if (component.CurrentState == DoctrineChoice.State.Locked)
      this.TransitionTo((UIMenuBase) this._lockedPage);
    else if (component.CurrentState == DoctrineChoice.State.Unchosen)
      this.TransitionTo((UIMenuBase) this._forbiddenPage);
    else if ((UnityEngine.Object) this._currentPage == (UnityEngine.Object) this._detailsPage2)
    {
      this.TransitionTo((UIMenuBase) this._detailsPage);
      this._detailsPage.UpdateDetails(component.Type);
    }
    else
    {
      this.TransitionTo((UIMenuBase) this._detailsPage2);
      this._detailsPage2.UpdateDetails(component.Type);
    }
  }

  private void TransitionTo(UIMenuBase newPage)
  {
    if (!((UnityEngine.Object) this._currentPage != (UnityEngine.Object) newPage))
      return;
    this.PerformTransitionTo(this._currentPage, newPage);
    this._currentPage = newPage;
  }

  protected virtual void PerformTransitionTo(UIMenuBase from, UIMenuBase to)
  {
    if ((UnityEngine.Object) from != (UnityEngine.Object) null)
      from.Hide();
    to.Show();
  }

  [Serializable]
  private class DoctrineChoicePair
  {
    [SerializeField]
    private DoctrineChoice _choiceA;
    [SerializeField]
    private DoctrineChoice _choiceB;

    public DoctrineChoice ActiveChoice
    {
      get
      {
        if (this._choiceA.Type != DoctrineUpgradeSystem.DoctrineType.None)
          return this._choiceA;
        return this._choiceB.Type != DoctrineUpgradeSystem.DoctrineType.None ? this._choiceB : (DoctrineChoice) null;
      }
    }

    public void Configure(
      List<DoctrineUpgradeSystem.DoctrineType> unlockedDoctrines,
      SermonCategory sermonCategory,
      int level)
    {
      DoctrineUpgradeSystem.DoctrineType sermonReward1 = DoctrineUpgradeSystem.GetSermonReward(sermonCategory, level, true);
      DoctrineUpgradeSystem.DoctrineType sermonReward2 = DoctrineUpgradeSystem.GetSermonReward(sermonCategory, level, false);
      if (unlockedDoctrines.Contains(sermonReward1))
      {
        this._choiceA.Configure(sermonReward1);
        this._choiceB.Configure(DoctrineUpgradeSystem.DoctrineType.None);
      }
      else
      {
        if (!unlockedDoctrines.Contains(sermonReward2))
          return;
        this._choiceA.Configure(DoctrineUpgradeSystem.DoctrineType.None);
        this._choiceB.Configure(sermonReward2);
      }
    }
  }
}
