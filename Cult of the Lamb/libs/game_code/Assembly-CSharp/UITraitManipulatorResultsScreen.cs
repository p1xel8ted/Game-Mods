// Decompiled with JetBrains decompiler
// Type: UITraitManipulatorResultsScreen
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using Coffee.UIExtensions;
using DG.Tweening;
using FMOD.Studio;
using I2.Loc;
using Lamb.UI;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class UITraitManipulatorResultsScreen : UIMenuBase
{
  [SerializeField]
  public GameObject controlPrompts;
  [SerializeField]
  public IndoctrinationTraitItem traitItem;
  [SerializeField]
  public TMP_Text followerHeader;
  [SerializeField]
  public SkeletonGraphic followerSpine;
  [SerializeField]
  public List<GameObject> followerTraitSlots;
  [SerializeField]
  public UIParticle traitEffect;
  [SerializeField]
  public TraitInfoCardController infoCardController;
  public List<IndoctrinationTraitItem> traitItems = new List<IndoctrinationTraitItem>();
  public FollowerInfo follower;
  public UITraitManipulatorMenuController.Type type;
  public FMOD.Studio.EventInstance shuffleTraitsLoopInstance;
  public string shuffleTraitLoopSFX = "event:/dlc/building/exorcismaltar/exorcism_trait_shake";

  public void Show(FollowerInfo follower, UITraitManipulatorMenuController.Type type)
  {
    this.Show();
    this.type = type;
    this.follower = follower;
    this.followerSpine.ConfigureFollower(follower);
    this.followerHeader.text = string.Format(LocalizationManager.GetTranslation("UI/ParentsTrait"), (object) follower.Name);
    if (type == UITraitManipulatorMenuController.Type.Remove)
    {
      for (int index = 0; index < follower.Traits.Count; ++index)
      {
        if (follower.Traits[index] == follower.TargetTraits[0])
        {
          FollowerTrait.TraitType trait = follower.Traits[index];
          follower.Traits.Remove(trait);
          follower.Traits.Add(trait);
        }
      }
    }
    int index1 = 0;
    for (int index2 = 0; index2 < follower.Traits.Count && index1 < this.followerTraitSlots.Count; ++index2)
    {
      if (type != UITraitManipulatorMenuController.Type.Add || !follower.HasTraitFromNecklace(follower.Traits[index2]))
      {
        IndoctrinationTraitItem indoctrinationTraitItem = Object.Instantiate<IndoctrinationTraitItem>(this.traitItem, this.followerTraitSlots[index1].transform);
        ((RectTransform) indoctrinationTraitItem.transform).anchoredPosition = new Vector2(40f, 40f);
        indoctrinationTraitItem.Configure(follower.Traits[index2]);
        this.traitItems.Add(indoctrinationTraitItem);
        ++index1;
      }
    }
    if (type != UITraitManipulatorMenuController.Type.Remove || this.traitItems.Count != this.followerTraitSlots.Count)
      return;
    List<IndoctrinationTraitItem> traitItems1 = this.traitItems;
    if (traitItems1[traitItems1.Count - 1].TraitType == follower.TargetTraits[0])
      return;
    List<IndoctrinationTraitItem> traitItems2 = this.traitItems;
    traitItems2[traitItems2.Count - 1].Configure(follower.TargetTraits[0]);
  }

  public override IEnumerator DoShowAnimation()
  {
    UITraitManipulatorResultsScreen manipulatorResultsScreen = this;
    manipulatorResultsScreen.controlPrompts.gameObject.SetActive(false);
    manipulatorResultsScreen.CanvasGroup.interactable = false;
    yield return (object) manipulatorResultsScreen.\u003C\u003En__0();
    manipulatorResultsScreen.SetActiveStateForMenu(false);
    yield return (object) new WaitForSecondsRealtime(0.5f);
    if (manipulatorResultsScreen.type == UITraitManipulatorMenuController.Type.Shuffle)
      yield return (object) manipulatorResultsScreen.StartCoroutine((IEnumerator) manipulatorResultsScreen.ShuffleTraitsIE());
    else if (manipulatorResultsScreen.type == UITraitManipulatorMenuController.Type.Remove)
      yield return (object) manipulatorResultsScreen.StartCoroutine((IEnumerator) manipulatorResultsScreen.RemoveTraitIE());
    else if (manipulatorResultsScreen.type == UITraitManipulatorMenuController.Type.Add)
      yield return (object) manipulatorResultsScreen.StartCoroutine((IEnumerator) manipulatorResultsScreen.AddTraitIE());
    yield return (object) new WaitForSecondsRealtime(1f);
    manipulatorResultsScreen.CanvasGroup.interactable = true;
    manipulatorResultsScreen.controlPrompts.gameObject.SetActive(true);
    manipulatorResultsScreen.OverrideDefaultOnce((Selectable) manipulatorResultsScreen.traitItems[0].Selectable);
    manipulatorResultsScreen.SetActiveStateForMenu(true);
    yield return (object) new WaitForEndOfFrame();
    manipulatorResultsScreen.StartCoroutine((IEnumerator) manipulatorResultsScreen.WaitForContinue());
  }

  public IEnumerator WaitForContinue()
  {
    UITraitManipulatorResultsScreen manipulatorResultsScreen = this;
    while (!InputManager.UI.GetAcceptButtonDown())
      yield return (object) null;
    AudioManager.Instance.PlayOneShot("event:/ui/confirm_selection");
    manipulatorResultsScreen.Hide();
  }

  public IEnumerator ShuffleTraitsIE()
  {
    for (int index = this.traitItems.Count - 1; index >= 0; --index)
      this.traitItems[index].transform.DOShakePosition(5f, 1.5f, 20, fadeOut: false).SetEase<Tweener>(Ease.InSine).SetUpdate<Tweener>(true);
    this.shuffleTraitsLoopInstance = AudioManager.Instance.CreateLoop(this.shuffleTraitLoopSFX, true);
    yield return (object) new WaitForSecondsRealtime(2f);
    int i;
    for (i = this.traitItems.Count - 1; i >= 0; --i)
    {
      this.traitEffect.transform.position = this.traitItems[i].transform.position;
      this.traitEffect.Play();
      Object.Destroy((Object) this.traitItems[i].gameObject);
      AudioManager.Instance.PlayOneShot("event:/dlc/building/exorcismaltar/exorcism_trait_disappear");
      if (i == this.traitItems.Count - 1)
        AudioManager.Instance.StopLoop(this.shuffleTraitsLoopInstance);
      yield return (object) new WaitForSecondsRealtime(0.5f);
    }
    this.traitItems.Clear();
    yield return (object) new WaitForSecondsRealtime(1f);
    for (i = 0; i < this.follower.TargetTraits.Count; ++i)
    {
      IndoctrinationTraitItem indoctrinationTraitItem = Object.Instantiate<IndoctrinationTraitItem>(this.traitItem, this.followerTraitSlots[i].transform);
      ((RectTransform) indoctrinationTraitItem.transform).anchoredPosition = new Vector2(40f, 40f);
      indoctrinationTraitItem.Configure(this.follower.TargetTraits[i]);
      this.traitItems.Add(indoctrinationTraitItem);
      this.traitEffect.transform.position = indoctrinationTraitItem.transform.position;
      this.traitEffect.Play();
      AudioManager.Instance.PlayOneShot("event:/dlc/building/exorcismaltar/exorcism_trait_appear");
      yield return (object) new WaitForSecondsRealtime(0.5f);
    }
  }

  public IEnumerator RemoveTraitIE()
  {
    this.traitItems[this.traitItems.Count - 1].transform.DOShakePosition(5f, 1.5f, 20, fadeOut: false).SetEase<Tweener>(Ease.InSine).SetUpdate<Tweener>(true);
    this.shuffleTraitsLoopInstance = AudioManager.Instance.CreateLoop(this.shuffleTraitLoopSFX, true);
    yield return (object) new WaitForSecondsRealtime(2f);
    this.traitEffect.transform.position = this.traitItems[this.traitItems.Count - 1].transform.position;
    this.traitEffect.Play();
    Object.Destroy((Object) this.traitItems[this.traitItems.Count - 1].gameObject);
    AudioManager.Instance.StopLoop(this.shuffleTraitsLoopInstance, STOP_MODE.IMMEDIATE);
    AudioManager.Instance.PlayOneShot("event:/dlc/building/exorcismaltar/exorcism_trait_disappear");
  }

  public IEnumerator AddTraitIE()
  {
    int index = this.follower.Traits.Count;
    foreach (FollowerTrait.TraitType trait in this.follower.Traits)
    {
      if (this.follower.HasTraitFromNecklace(trait))
        index = Mathf.Clamp(index - 1, 0, this.follower.Traits.Count);
    }
    IndoctrinationTraitItem indoctrinationTraitItem = Object.Instantiate<IndoctrinationTraitItem>(this.traitItem, this.followerTraitSlots[index].transform);
    ((RectTransform) indoctrinationTraitItem.transform).anchoredPosition = new Vector2(40f, 40f);
    indoctrinationTraitItem.Configure(this.follower.TargetTraits[0]);
    this.traitItems.Add(indoctrinationTraitItem);
    this.traitEffect.transform.position = this.traitItems[this.traitItems.Count - 1].transform.position;
    this.traitEffect.Play();
    AudioManager.Instance.PlayOneShot("event:/dlc/building/exorcismaltar/exorcism_trait_appear");
    yield return (object) new WaitForEndOfFrame();
  }

  public override void OnCancelButtonInput()
  {
    base.OnCancelButtonInput();
    if (!this.CanvasGroup.interactable)
      return;
    this.Hide();
  }

  public override void OnHideCompleted()
  {
    base.OnHideCompleted();
    AudioManager.Instance.StopLoop(this.shuffleTraitsLoopInstance, STOP_MODE.IMMEDIATE);
    Object.Destroy((Object) this.gameObject);
  }

  [CompilerGenerated]
  [DebuggerHidden]
  public IEnumerator \u003C\u003En__0() => base.DoShowAnimation();
}
