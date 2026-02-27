// Decompiled with JetBrains decompiler
// Type: UI_DoctrineBookController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using Spine.Unity;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UI_DoctrineBookController : MonoBehaviour
{
  public SkeletonGraphic DoctrineBookSpine;
  public CanvasGroup LeftPageCanvasGroup;
  public CanvasGroup RightPageCanvasGroup;
  public CanvasGroup TabLeft;
  public CanvasGroup TabRight;
  public SermonCategory SelectedCategory;
  public Image SermonCategoryIcon;
  public TextMeshProUGUI SermonCategoryTitle;
  public TextMeshProUGUI SermonCategoryLore;
  public GameObject ProgressBar;
  public Image ProgressBarProgress;
  public List<GameObject> UnlockSelectionsChoices = new List<GameObject>();
  public List<GameObject> UnlockSelections = new List<GameObject>();
  public List<GameObject> CategoryTabsSelected = new List<GameObject>();
  public List<GameObject> CategoryTabsUnselected = new List<GameObject>();
  public Image UnlockIcon;
  public TextMeshProUGUI UnlockTitle;
  public TextMeshProUGUI UnlockType;
  public TextMeshProUGUI UnlockTypeIcon;
  public TextMeshProUGUI UnlockDescription;
  public CanvasGroup CurrentSelection;

  private void LocaliseCategory()
  {
    this.SermonCategoryTitle.text = $"{DoctrineUpgradeSystem.GetSermonCategoryLocalizedName(this.SelectedCategory)} [{(object) (DoctrineUpgradeSystem.GetLevelBySermon(this.SelectedCategory) + 1)}]";
    this.SermonCategoryLore.text = DoctrineUpgradeSystem.GetSermonCategoryLocalizedDescription(this.SelectedCategory);
  }

  private void GetProgress()
  {
    switch (this.SelectedCategory)
    {
      case SermonCategory.Afterlife:
        if (DataManager.Instance.HasBuiltTemple2)
        {
          this.ProgressBar.SetActive(true);
          break;
        }
        this.ProgressBar.SetActive(false);
        break;
      case SermonCategory.LawAndOrder:
      case SermonCategory.Possession:
        if (DataManager.Instance.HasBuiltTemple3)
        {
          this.ProgressBar.SetActive(true);
          break;
        }
        this.ProgressBar.SetActive(false);
        break;
    }
    this.ProgressBarProgress.fillAmount = DoctrineUpgradeSystem.GetXPBySermonNormalised(this.SelectedCategory);
  }

  private void OnChangeSelection(Selectable NewSelectable, Selectable PrevSelectable)
  {
    if ((Object) NewSelectable == (Object) null)
    {
      DOTween.Kill((object) this.CurrentSelection.alpha);
      DOTween.To((DOGetter<float>) (() => this.CurrentSelection.alpha), (DOSetter<float>) (x => this.CurrentSelection.alpha = x), 0.0f, 0.3f);
    }
    else
    {
      UIDoctrineIcon component = NewSelectable.GetComponent<UIDoctrineIcon>();
      if (component.Locked)
      {
        DOTween.Kill((object) this.CurrentSelection.alpha);
        DOTween.To((DOGetter<float>) (() => this.CurrentSelection.alpha), (DOSetter<float>) (x => this.CurrentSelection.alpha = x), 0.0f, 0.3f);
      }
      else
      {
        DOTween.Kill((object) this.CurrentSelection.alpha);
        DOTween.To((DOGetter<float>) (() => this.CurrentSelection.alpha), (DOSetter<float>) (x => this.CurrentSelection.alpha = x), 1f, 0.3f);
        DoctrineUpgradeSystem.DoctrineType type = component.Type;
        this.UnlockTitle.text = DoctrineUpgradeSystem.GetLocalizedName(type);
        this.UnlockDescription.text = DoctrineUpgradeSystem.GetLocalizedDescription(type);
        this.UnlockIcon.sprite = DoctrineUpgradeSystem.GetIcon(type);
        this.UnlockType.text = DoctrineUpgradeSystem.GetDoctrineUnlockString(type);
      }
      AudioManager.Instance.PlayOneShot("event:/upgrade_statue/upgrade_statue_scroll", this.gameObject);
    }
  }
}
