// Decompiled with JetBrains decompiler
// Type: Lamb.UI.DoctrineSubmenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  public SermonCategory _sermonCategory;
  [SerializeField]
  public TextMeshProUGUI _sermonCategoryTitle;
  [SerializeField]
  public TextMeshProUGUI _sermonCategoryLore;
  [Header("Pages")]
  [SerializeField]
  public DoctrineDetailsPage _detailsPage;
  [SerializeField]
  public DoctrineDetailsPage _detailsPage2;
  [SerializeField]
  public DoctrineForbiddenPage _forbiddenPage;
  [SerializeField]
  public DoctrineLockedPage _lockedPage;
  [Header("Choices")]
  [SerializeField]
  public GridLayoutGroup _gridLayoutgroup;
  [SerializeField]
  public MMUILineRenderer _lineRenderer;
  [SerializeField]
  public MMUILineRenderer _crystalLineRenderer;
  [SerializeField]
  public DoctrineSubmenu.DoctrineChoicePair[] _doctrineChoicePairs;
  public UIMenuBase _currentPage;

  public override void OnShowStarted()
  {
    if (this._lineRenderer.Points.Count != 0 && this._crystalLineRenderer.Points.Count != 0)
      return;
    LayoutRebuilder.ForceRebuildLayoutImmediate(this._gridLayoutgroup.transform as RectTransform);
    List<Vector2> vector2List = new List<Vector2>();
    foreach (DoctrineSubmenu.DoctrineChoicePair doctrineChoicePair in this._doctrineChoicePairs)
    {
      if ((UnityEngine.Object) doctrineChoicePair.ActiveChoice != (UnityEngine.Object) null)
        vector2List.Add((Vector2) doctrineChoicePair.ActiveChoice.transform.localPosition);
    }
    if (vector2List.Count > 1)
    {
      List<MMUILineRenderer.BranchPoint> branchPointList = new List<MMUILineRenderer.BranchPoint>();
      foreach (Vector2 point in vector2List)
        branchPointList.Add(new MMUILineRenderer.BranchPoint(point));
      this._lineRenderer.Points = branchPointList;
      this._lineRenderer.Color = StaticColors.RedColor;
    }
    if (!DataManager.Instance.OnboardedCrystalDoctrine)
      return;
    MMUILineRenderer.Branch branch = this._crystalLineRenderer.Root;
    for (int index = 0; index < this._doctrineChoicePairs.Length; ++index)
    {
      DoctrineSubmenu.DoctrineChoicePair doctrineChoicePair = this._doctrineChoicePairs[index];
      if ((UnityEngine.Object) doctrineChoicePair.ActiveChoice != (UnityEngine.Object) null)
      {
        if (this._crystalLineRenderer.Points.Count == 0)
        {
          branch.Points.Add(new MMUILineRenderer.BranchPoint((Vector2) doctrineChoicePair.OtherChoice(doctrineChoicePair.ActiveChoice).transform.localPosition));
        }
        else
        {
          branch = branch.Points[0].AddNewBranch();
          branch.Color = StaticColors.BlueColor;
          branch.Points.Add(new MMUILineRenderer.BranchPoint((Vector2) doctrineChoicePair.OtherChoice(doctrineChoicePair.ActiveChoice).transform.localPosition));
          if (index > 0 && ((UnityEngine.Object) this._doctrineChoicePairs[index - 1].CrystalChoice == (UnityEngine.Object) null || (UnityEngine.Object) doctrineChoicePair.CrystalChoice == (UnityEngine.Object) null))
            branch.Fill = 0.0f;
        }
      }
    }
    this._crystalLineRenderer.Color = StaticColors.BlueColor;
    this._crystalLineRenderer.UpdateValues();
  }

  public void OnEnable()
  {
    this._sermonCategoryTitle.text = $"{DoctrineUpgradeSystem.GetSermonCategoryLocalizedName(this._sermonCategory)} - {DoctrineUpgradeSystem.GetLevelBySermon(this._sermonCategory).ToNumeral()}";
    this._sermonCategoryLore.text = DoctrineUpgradeSystem.GetSermonCategoryLocalizedDescription(this._sermonCategory);
    List<DoctrineUpgradeSystem.DoctrineType> doctrinesForCategory = DoctrineUpgradeSystem.GetUnlockedDoctrinesForCategory(this._sermonCategory);
    for (int index = 0; index < this._doctrineChoicePairs.Length; ++index)
      this._doctrineChoicePairs[index].Configure(doctrinesForCategory, this._sermonCategory, index + 1);
    if ((UnityEngine.Object) this._doctrineChoicePairs[0].ActiveChoice != (UnityEngine.Object) null)
      this.OverrideDefaultOnce((Selectable) this._doctrineChoicePairs[0].ActiveChoice.GetComponent<MMButton>());
    MonoSingleton<UINavigatorNew>.Instance.OnSelectionChanged += new Action<Selectable, Selectable>(this.OnSelectionChanged);
    MonoSingleton<UINavigatorNew>.Instance.OnDefaultSetComplete += new Action<Selectable>(this.OnSelection);
  }

  public new void OnDisable()
  {
    if (!((UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance != (UnityEngine.Object) null))
      return;
    MonoSingleton<UINavigatorNew>.Instance.OnSelectionChanged -= new Action<Selectable, Selectable>(this.OnSelectionChanged);
    MonoSingleton<UINavigatorNew>.Instance.OnDefaultSetComplete -= new Action<Selectable>(this.OnSelection);
  }

  public void OnSelectionChanged(Selectable current, Selectable previous)
  {
    this.OnSelection(current);
  }

  public void OnSelection(Selectable current)
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

  public void TransitionTo(UIMenuBase newPage)
  {
    if (!((UnityEngine.Object) this._currentPage != (UnityEngine.Object) newPage))
      return;
    this.PerformTransitionTo(this._currentPage, newPage);
    this._currentPage = newPage;
  }

  public virtual void PerformTransitionTo(UIMenuBase from, UIMenuBase to)
  {
    if ((UnityEngine.Object) from != (UnityEngine.Object) null)
      from.Hide();
    to.Show();
  }

  [Serializable]
  public class DoctrineChoicePair
  {
    [SerializeField]
    public DoctrineChoice _choiceA;
    [SerializeField]
    public DoctrineChoice _choiceB;

    public DoctrineChoice ActiveChoice
    {
      get
      {
        if (!this._choiceA.UnlockedWithCrystal && this._choiceA.Type != DoctrineUpgradeSystem.DoctrineType.None)
          return this._choiceA;
        return !this._choiceB.UnlockedWithCrystal && this._choiceB.Type != DoctrineUpgradeSystem.DoctrineType.None ? this._choiceB : (DoctrineChoice) null;
      }
    }

    public DoctrineChoice CrystalChoice
    {
      get
      {
        if (this._choiceA.UnlockedWithCrystal && this._choiceA.Type != DoctrineUpgradeSystem.DoctrineType.None)
          return this._choiceA;
        return this._choiceB.UnlockedWithCrystal && this._choiceB.Type != DoctrineUpgradeSystem.DoctrineType.None ? this._choiceB : (DoctrineChoice) null;
      }
    }

    public DoctrineChoice OtherChoice(DoctrineChoice target)
    {
      return (UnityEngine.Object) target == (UnityEngine.Object) this._choiceA ? this._choiceB : this._choiceA;
    }

    public void Configure(
      List<DoctrineUpgradeSystem.DoctrineType> unlockedDoctrines,
      SermonCategory sermonCategory,
      int level)
    {
      DoctrineUpgradeSystem.DoctrineType sermonReward1 = DoctrineUpgradeSystem.GetSermonReward(sermonCategory, level, true);
      DoctrineUpgradeSystem.DoctrineType sermonReward2 = DoctrineUpgradeSystem.GetSermonReward(sermonCategory, level, false);
      int num1 = unlockedDoctrines.IndexOf(sermonReward1);
      int num2 = unlockedDoctrines.IndexOf(sermonReward2);
      this._choiceA.Configure(unlockedDoctrines.Contains(sermonReward1) ? sermonReward1 : DoctrineUpgradeSystem.DoctrineType.None, num1 >= 4);
      this._choiceB.Configure(unlockedDoctrines.Contains(sermonReward2) ? sermonReward2 : DoctrineUpgradeSystem.DoctrineType.None, num2 >= 4);
    }
  }
}
