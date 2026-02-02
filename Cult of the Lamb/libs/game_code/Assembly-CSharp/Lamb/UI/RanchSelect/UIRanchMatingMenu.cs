// Decompiled with JetBrains decompiler
// Type: Lamb.UI.RanchSelect.UIRanchMatingMenu
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using src.UI;
using src.UINavigator;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

#nullable disable
namespace Lamb.UI.RanchSelect;

public class UIRanchMatingMenu : UIRanchSelectBase<RanchMatingMenuItem>
{
  [SerializeField]
  public GameObject overcrowdedAlert;
  [SerializeField]
  public TMP_Text ranchSize;
  [SerializeField]
  public TMP_Text ranchCapacity;
  [SerializeField]
  public UIRanchMatingInfoCard matingInfoCard;
  [SerializeField]
  public MMButton mateButton;
  [SerializeField]
  public ButtonHighlightController highlightController;
  [SerializeField]
  public GameObject mateButtonContainer;
  public RanchSelectEntry animal1;
  public RanchSelectEntry animal2;
  public StructuresData.Ranchable_Animal highlightedAnimal;
  public Interaction_RanchHutch hutch;
  public Action<StructuresData.Ranchable_Animal, StructuresData.Ranchable_Animal> OnAnimalChosen;

  public void Show(
    Interaction_RanchHutch hutch,
    List<RanchSelectEntry> ranchablesSelectEntries,
    int capacity,
    bool instant = false)
  {
    this.Show(ranchablesSelectEntries, capacity, instant);
    this.hutch = hutch;
    this.matingInfoCard.Configure(hutch, (RanchSelectEntry) null, (RanchSelectEntry) null, this);
    this._scrollRect.enabled = true;
    this.mateButtonContainer.SetActive(false);
    this.mateButton.onClick.AddListener(new UnityAction(this.ConfirmMatingButtonPress));
    this.mateButton.OnSelected += new System.Action(this.OnMateButtonSelected);
    this.mateButton.OnDeselected += new System.Action(this.OnMateButtonDeselected);
  }

  public override void OnShowStarted()
  {
    base.OnShowStarted();
    Interaction_Ranch ranch = this.hutch.GetRanch();
    if ((UnityEngine.Object) ranch != (UnityEngine.Object) null && ranch.IsOvercrowded)
      this.overcrowdedAlert.SetActive(true);
    else
      this.overcrowdedAlert.SetActive(false);
    if (!((UnityEngine.Object) ranch != (UnityEngine.Object) null))
      return;
    this.ranchSize.text = ranch.Brain.RanchingTiles.Count.ToString();
    if (ranch.IsOvercrowded)
    {
      TMP_Text ranchCapacity = this.ranchCapacity;
      string[] strArray = new string[5]
      {
        "<color=#FF0000>",
        null,
        null,
        null,
        null
      };
      int num = ranch.Brain.Data.Animals.Count;
      strArray[1] = num.ToString();
      strArray[2] = "/";
      num = ranch.Brain.Capacity;
      strArray[3] = num.ToString();
      strArray[4] = "</color>";
      string str = string.Concat(strArray);
      ranchCapacity.text = str;
    }
    else
      this.ranchCapacity.text = $"{ranch.Brain.Data.Animals.Count.ToString()}/{ranch.Brain.Capacity.ToString()}";
  }

  public override void OnHideStarted()
  {
    base.OnHideStarted();
    foreach (RanchMatingMenuItem followerInfoBox in this._followerInfoBoxes)
      followerInfoBox.OnFollowerHighlighted = followerInfoBox.OnFollowerHighlighted - new Action<RanchSelectEntry>(this.EnableScrollRect);
    this.mateButton.onClick.RemoveListener(new UnityAction(this.ConfirmMatingButtonPress));
    this.mateButton.OnSelected -= new System.Action(this.OnMateButtonSelected);
    this.mateButton.OnDeselected -= new System.Action(this.OnMateButtonDeselected);
  }

  public void EnableScrollRect(RanchSelectEntry entry)
  {
    if ((UnityEngine.Object) this._scrollRect != (UnityEngine.Object) null)
      this._scrollRect.enabled = true;
    for (int index = 0; index < this._followerInfoBoxes.Count; ++index)
      this._followerInfoBoxes[index].Button.Interactable = true;
  }

  public override void AddFollowerEntry(RanchSelectEntry followerSelectEntry)
  {
    RanchMatingMenuItem ranchMatingMenuItem1;
    if (followerSelectEntry.AvailabilityStatus == RanchSelectEntry.Status.Available)
    {
      ranchMatingMenuItem1 = this.PrefabTemplate().Spawn<RanchMatingMenuItem>((Transform) this._availableContentContainer, false);
      if ((UnityEngine.Object) this.overrideSelectable == (UnityEngine.Object) null)
        this.overrideSelectable = ranchMatingMenuItem1.Button;
    }
    else
      ranchMatingMenuItem1 = this.PrefabTemplate().Spawn<RanchMatingMenuItem>((Transform) this._unavailableContentContainer, false);
    ranchMatingMenuItem1.transform.localScale = Vector3.one;
    ranchMatingMenuItem1.Configure(followerSelectEntry, followerSelectEntry.AvailabilityStatus == RanchSelectEntry.Status.Available);
    if (followerSelectEntry.AvailabilityStatus == RanchSelectEntry.Status.Available)
    {
      RanchMatingMenuItem ranchMatingMenuItem2 = ranchMatingMenuItem1;
      ranchMatingMenuItem2.OnFollowerSelected = ranchMatingMenuItem2.OnFollowerSelected + new Action<RanchSelectEntry>(((UIRanchSelectBase<RanchMatingMenuItem>) this).FollowerSelected);
    }
    RanchMatingMenuItem ranchMatingMenuItem3 = ranchMatingMenuItem1;
    ranchMatingMenuItem3.OnFollowerHighlighted = ranchMatingMenuItem3.OnFollowerHighlighted + new Action<RanchSelectEntry>(this.AnimalHighlighted);
    ranchMatingMenuItem1.Button.SetInteractionState(true);
    ranchMatingMenuItem1.Button._vibrateOnConfirm = false;
    this._followerInfoBoxes.Add(ranchMatingMenuItem1);
  }

  public void DisableScrollRect()
  {
    if ((UnityEngine.Object) this._scrollRect != (UnityEngine.Object) null)
      this._scrollRect.enabled = false;
    for (int index = 0; index < this._followerInfoBoxes.Count; ++index)
      this._followerInfoBoxes[index].Button.Interactable = this._followerInfoBoxes[index].RanchSelectEntry == this.animal1 || this._followerInfoBoxes[index].RanchSelectEntry == this.animal2 || this._followerInfoBoxes[index].AnimalInfo == this.highlightedAnimal;
  }

  public override void FollowerSelected(RanchSelectEntry animal)
  {
    if (this.animal1 != null && animal == this.animal1)
    {
      this.matingInfoCard.Configure(this.hutch, animal, this.animal2, this, true);
      this.animal1 = (RanchSelectEntry) null;
      this.EnableScrollRect((RanchSelectEntry) null);
    }
    else if (this.animal2 != null && animal == this.animal2)
    {
      this.matingInfoCard.Configure(this.hutch, this.animal1, animal, this, fadeAnimal2: true);
      this.animal2 = (RanchSelectEntry) null;
      this.EnableScrollRect((RanchSelectEntry) null);
    }
    else if (this.animal1 == null)
    {
      this.animal1 = animal;
      this.matingInfoCard.Configure(this.hutch, this.animal1, this.animal2, this);
    }
    else if (this.animal2 == null)
    {
      this.animal2 = animal;
      this.matingInfoCard.Configure(this.hutch, this.animal1, this.animal2, this);
    }
    else
    {
      this.animal2 = animal;
      this.matingInfoCard.Configure(this.hutch, this.animal1, this.animal2, this);
    }
    if (this.animal1 != null && this.animal2 != null)
    {
      if (!this.mateButtonContainer.activeSelf)
      {
        this.mateButtonContainer.SetActive(true);
        this.mateButtonContainer.transform.localScale = Vector3.one;
        this.mateButtonContainer.transform.DOKill();
        this.mateButtonContainer.transform.DOPunchScale(Vector3.one * 0.2f, 0.2f).SetUpdate<Tweener>(true);
        if (!InputManager.General.MouseInputActive)
        {
          this.DisableScrollRect();
          MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) this.mateButton);
        }
      }
    }
    else
      this.mateButtonContainer.SetActive(false);
    this.UpdateCheckboxes();
  }

  public void AnimalHighlighted(RanchSelectEntry animal)
  {
    if (this.animal1 == null)
      this.matingInfoCard.Configure(this.hutch, animal, this.animal2, this, true);
    else if (this.animal2 == null)
    {
      if (animal == this.animal1 && this.animal1 != null)
        this.matingInfoCard.Configure(this.hutch, this.animal1, (RanchSelectEntry) null, this);
      else
        this.matingInfoCard.Configure(this.hutch, this.animal1, animal, this, fadeAnimal2: true);
    }
    this.highlightedAnimal = animal.AnimalInfo;
    this.UpdateCheckboxes();
    Action<RanchSelectEntry> animalHighlighted = this.OnAnimalHighlighted;
    if (animalHighlighted == null)
      return;
    animalHighlighted(animal);
  }

  public void UpdateCheckboxes()
  {
    foreach (RanchMatingMenuItem followerInfoBox in this.FollowerInfoBoxes)
    {
      if (followerInfoBox.RanchSelectEntry == this.animal1 || followerInfoBox.RanchSelectEntry == this.animal2)
        followerInfoBox.SetChosen();
      else
        followerInfoBox.RemoveChosen(followerInfoBox.RanchSelectEntry.AvailabilityStatus == RanchSelectEntry.Status.Available);
    }
  }

  public override void OnCancelButtonInput()
  {
    if (!this._canvasGroup.interactable)
      return;
    this.EnableScrollRect((RanchSelectEntry) null);
    this.mateButtonContainer.SetActive(false);
    if (this.animal2 != null)
    {
      if (MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable != null && !(bool) (UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable.Selectable.GetComponentInParent<FollowerInformationBox>())
      {
        for (int index = 0; index < this.FollowerInfoBoxes.Count; ++index)
        {
          if (this.FollowerInfoBoxes[index].RanchSelectEntry == this.animal2)
            MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) this.FollowerInfoBoxes[index].Button);
        }
      }
      this.matingInfoCard.Configure(this.hutch, this.animal1, this.animal2, this, fadeAnimal2: true);
      this.animal2 = (RanchSelectEntry) null;
    }
    else if (this.animal1 != null)
    {
      if (MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable != null && !(bool) (UnityEngine.Object) MonoSingleton<UINavigatorNew>.Instance.CurrentSelectable.Selectable.GetComponentInParent<FollowerInformationBox>())
      {
        for (int index = 0; index < this.FollowerInfoBoxes.Count; ++index)
        {
          if (this.FollowerInfoBoxes[index].RanchSelectEntry == this.animal1)
            MonoSingleton<UINavigatorNew>.Instance.NavigateToNew((IMMSelectable) this.FollowerInfoBoxes[index].Button);
        }
      }
      this.matingInfoCard.Configure(this.hutch, this.animal1, (RanchSelectEntry) null, this, true);
      this.animal1 = (RanchSelectEntry) null;
    }
    else if (this.animal1 == null)
    {
      base.OnCancelButtonInput();
      this.Hide();
    }
    this.UpdateCheckboxes();
  }

  public void OnMateButtonSelected()
  {
    this.highlightController.Image.color = new Color(1f, 1f, 1f, 1f);
    this.highlightController.transform.DOKill();
    this.highlightController.transform.DOShakeScale(0.05f, new Vector3(-0.05f, 0.05f, 1f), 3, fadeOut: false).SetUpdate<Tweener>(true);
  }

  public void OnMateButtonDeselected()
  {
    this.highlightController.Image.color = new Color(0.0f, 0.5f, 1f, 1f);
  }

  public void ConfirmMatingButtonPress()
  {
    AudioManager.Instance.PlayOneShot("event:/dlc/animal/shared/mate_confirm");
    Action<StructuresData.Ranchable_Animal, StructuresData.Ranchable_Animal> onAnimalChosen = this.OnAnimalChosen;
    if (onAnimalChosen != null)
      onAnimalChosen(this.animal1.AnimalInfo, this.animal2.AnimalInfo);
    this.Hide();
  }

  public override void OnHideCompleted()
  {
    this.mateButton.onClick.RemoveListener(new UnityAction(this.ConfirmMatingButtonPress));
    this.mateButton.OnSelected -= new System.Action(this.OnMateButtonSelected);
    this.mateButton.OnDeselected -= new System.Action(this.OnMateButtonDeselected);
    base.OnHideCompleted();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public override RanchMatingMenuItem PrefabTemplate()
  {
    return MonoSingleton<UIManager>.Instance.RanchMatingInformationBoxTemplate;
  }
}
