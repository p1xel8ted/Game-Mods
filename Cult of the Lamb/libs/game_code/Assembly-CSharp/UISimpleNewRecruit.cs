// Decompiled with JetBrains decompiler
// Type: UISimpleNewRecruit
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#nullable disable
public class UISimpleNewRecruit : BaseMonoBehaviour
{
  [SerializeField]
  public GameObject[] panels;
  [SerializeField]
  public Sprite[] panelIcons;
  [SerializeField]
  public TMP_InputField nameText;
  [SerializeField]
  public Button nameAcceptButton;
  [SerializeField]
  public Selectable nameBox;
  [SerializeField]
  public Selectable editNameButton;
  [SerializeField]
  public Selectable nameNextButton;
  [SerializeField]
  public UINewRecruitSelectable formPrefab;
  [SerializeField]
  public RectTransform formContainer;
  [SerializeField]
  public RectTransform formViewport;
  [SerializeField]
  public Selectable formRandom;
  [SerializeField]
  public GameObject lockIcon;
  [SerializeField]
  public Vector3 lockPositionOffset;
  [SerializeField]
  public Selectable formsNextButton;
  [SerializeField]
  public UINewRecruitSelectable variantPrefab;
  [SerializeField]
  public RectTransform variantContainer;
  [SerializeField]
  public RectTransform variantViewport;
  [SerializeField]
  public Selectable variantRandom;
  [SerializeField]
  public Selectable variantNextButton;
  [SerializeField]
  public UINewRecruitSelectable colorPrefab;
  [SerializeField]
  public RectTransform colorContainer;
  [SerializeField]
  public RectTransform colorViewport;
  [SerializeField]
  public Selectable colorRandom;
  [SerializeField]
  public UITrait traitPrefab;
  [SerializeField]
  public RectTransform followerTraitsContainer;
  [SerializeField]
  public RectTransform cultTraitsContainer;
  [SerializeField]
  public Button traitsAcceptButton;
  [SerializeField]
  public Selectable traitsNextButton;
  [SerializeField]
  public Image[] knobs;
  [SerializeField]
  public Image selectedIcon;
  public Material prevMat;
  public Material followerUIMat;
  public RendererMaterialSwap rendMatSwap;
  public List<UINewRecruitSelectable> forms = new List<UINewRecruitSelectable>();
  public List<UINewRecruitSelectable> variants = new List<UINewRecruitSelectable>();
  public List<UINewRecruitSelectable> colors = new List<UINewRecruitSelectable>();
  public Follower follower;
  public Camera camera;
  public System.Action callack;
  public UI_NavigatorSimple navigator;
  public Sprite baseSprite;
  public string previousName;
  public int selectionPanelIndex;
  public int formIndex;
  public int colorIndex;
  public int variantIndex;
  public bool formRandomSelected = true;
  public bool colorRandomSelected = true;

  public void Play(Follower Follower, System.Action Callback)
  {
    GameManager.InMenu = true;
    AudioManager.Instance.PlayOneShot("event:/followers/appearance_menu_appear", PlayerFarming.Instance.gameObject);
    this.navigator = this.GetComponent<UI_NavigatorSimple>();
    this.navigator.OnChangeSelection += new UI_NavigatorSimple.ChangeSelection(this.ChangeSelection);
    this.navigator.OnSelectDown += new System.Action(this.OptionSelected);
    this.callack = Callback;
    this.nameText.text = Follower.Brain.Info.Name;
    this.baseSprite = this.nameText.GetComponentInParent<Button>().image.sprite;
    for (int index = 0; index < WorshipperData.Instance.Characters.Count; ++index)
    {
      if (WorshipperData.Instance.Characters[index].Skin[0].Skin == Follower.Brain.Info.SkinName)
      {
        this.formIndex = index;
        break;
      }
    }
    this.variantIndex = Follower.Brain.Info.SkinVariation;
    this.colorIndex = Follower.Brain.Info.SkinColour;
    this.follower = Follower;
    this.prevMat = this.follower.Spine.GetComponent<Renderer>().sharedMaterial;
    this.follower.Spine.CustomMaterialOverride.Add(this.prevMat, this.followerUIMat);
    this.followerUIMat.SetColor("_Color", Color.white);
    this.rendMatSwap = BiomeBaseManager.Instance.RecruitSpawnLocation.transform.parent.GetComponent<RendererMaterialSwap>();
    if ((UnityEngine.Object) this.rendMatSwap != (UnityEngine.Object) null)
      this.rendMatSwap.SwapAll();
    this.PopulateTraits();
    this.SetPanel(0);
    this.lockIcon.SetActive(false);
    this.camera = Camera.main;
    double num = (double) Follower.SetBodyAnimation("Indoctrinate/indoctrinate-start", false);
    Follower.AddBodyAnimation("Indoctrinate/indoctrinate-loop", true, 0.0f);
  }

  public void OnDestroy()
  {
    this.navigator.OnChangeSelection -= new UI_NavigatorSimple.ChangeSelection(this.ChangeSelection);
    this.navigator.OnSelectDown -= new System.Action(this.OptionSelected);
  }

  public void Update()
  {
    if (!this.nameText.isFocused)
    {
      if (InputManager.UI.GetPageNavigateRightDown())
        this.ChangePanel(true);
      else if (InputManager.UI.GetPageNavigateLeftDown())
        this.ChangePanel(false);
    }
    if (!this.lockIcon.activeSelf)
      return;
    this.lockIcon.transform.position = Camera.main.WorldToScreenPoint(this.follower.transform.position) + this.lockPositionOffset;
  }

  public void RandomForm() => this.formIndex = WorshipperData.Instance.GetRandomAvailableSkin();

  public void RandomColor()
  {
    this.colorIndex = UnityEngine.Random.Range(0, WorshipperData.Instance.Characters[this.formIndex].SlotAndColours.Count);
  }

  public void RandomVariant()
  {
    this.variantIndex = UnityEngine.Random.Range(0, WorshipperData.Instance.Characters[this.formIndex].Skin.Count);
  }

  public void ChangePanel(bool right)
  {
    this.selectionPanelIndex = Mathf.Clamp(this.selectionPanelIndex + (right ? 1 : -1), 0, 3);
    this.SetPanel(this.selectionPanelIndex);
    this.UpdateFollow(this.formIndex, this.colorIndex, this.variantIndex);
  }

  public void SetPanel(int index)
  {
    if ((bool) (UnityEngine.Object) this.navigator.selectable && (bool) (UnityEngine.Object) this.navigator.selectable.GetComponent<UINewRecruitSelectable>())
      this.navigator.selectable.GetComponent<UINewRecruitSelectable>().OnDeselect((BaseEventData) null);
    else if ((bool) (UnityEngine.Object) this.navigator.selectable && (bool) (UnityEngine.Object) this.navigator.selectable.GetComponent<UITrait>())
      this.navigator.selectable.GetComponent<UITrait>().OnDeselect((BaseEventData) null);
    this.selectionPanelIndex = index;
    for (int index1 = 0; index1 < this.panels.Length; ++index1)
    {
      if (index1 == this.selectionPanelIndex)
      {
        this.knobs[index1].transform.DOScale(2f, 0.2f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
        DOTweenModuleUI.DOColor(this.knobs[index1], Color.white, 0.2f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
      }
      else
      {
        this.knobs[index1].transform.DOScale(1f, 0.2f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
        DOTweenModuleUI.DOColor(this.knobs[index1], Color.gray, 0.2f).SetUpdate<TweenerCore<Color, Color, ColorOptions>>(true);
      }
      this.panels[index1].SetActive(index1 == this.selectionPanelIndex);
    }
    this.selectedIcon.sprite = this.panelIcons[this.selectionPanelIndex];
    this.selectedIcon.transform.DOMove(new Vector3(this.knobs[this.selectionPanelIndex].transform.position.x, this.selectedIcon.transform.position.y, this.selectedIcon.transform.position.z), 0.2f).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    if (this.selectionPanelIndex == 0)
      this.navigator.startingItem = this.editNameButton;
    else if (this.selectionPanelIndex == 1)
    {
      this.PopulateForms();
      this.navigator.startingItem = this.GetCurrentCharacter();
      this.GetSnapToPositionToBringChildIntoView(this.formViewport, this.formContainer, this.navigator.startingItem.transform as RectTransform, 0.0f);
    }
    else if (this.selectionPanelIndex == 2)
    {
      this.PopulateColors();
      this.PopulateVariants();
      this.UpdateVariants();
      this.navigator.startingItem = this.GetCurrentSkinColor();
    }
    else if (this.selectionPanelIndex == 3)
      this.navigator.startingItem = (Selectable) this.traitsAcceptButton;
    this.navigator.setDefault();
  }

  public Selectable GetCurrentCharacter()
  {
    for (int index = 0; index < this.forms.Count; ++index)
    {
      if (this.forms[index].FormIndex == this.formIndex)
        return (Selectable) this.forms[index].Selectable;
    }
    return (Selectable) null;
  }

  public Selectable GetCurrentSkinColor()
  {
    for (int index = 0; index < this.colors.Count; ++index)
    {
      if (this.colors[index].SkinColour == this.colorIndex)
        return (Selectable) this.colors[index].Selectable;
    }
    return (Selectable) null;
  }

  public void RandomiseName() => this.nameText.text = FollowerInfo.GenerateName();

  public void PopulateForms()
  {
    if (this.forms.Count > 0)
      return;
    for (int index = 0; index < WorshipperData.Instance.Characters.Count; ++index)
    {
      UINewRecruitSelectable recruitSelectable = UnityEngine.Object.Instantiate<UINewRecruitSelectable>(this.formPrefab, (Transform) this.formContainer);
      recruitSelectable.gameObject.SetActive(true);
      recruitSelectable.FormIndex = index;
      this.forms.Add(recruitSelectable);
      SkeletonGraphic componentInChildren = this.forms[index].GetComponentInChildren<SkeletonGraphic>();
      WorshipperData.SkinAndData character = WorshipperData.Instance.Characters[index];
      componentInChildren.Skeleton.SetSkin(character.Skin[Mathf.Min(0, character.Skin.Count - 1)].Skin);
      foreach (WorshipperData.SlotAndColor slotAndColour in character.SlotAndColours[Mathf.Min(0, character.SlotAndColours.Count - 1)].SlotAndColours)
      {
        Spine.Slot slot = componentInChildren.Skeleton.FindSlot(slotAndColour.Slot);
        if (slot != null)
          slot.SetColor(slotAndColour.color);
      }
      if (!DataManager.GetFollowerSkinUnlocked(character.Skin[0].Skin))
      {
        this.forms[index].Lock();
      }
      else
      {
        this.forms[index].Show(character.Skin[Mathf.Min(0, character.Skin.Count - 1)].Skin);
        this.forms[index].transform.SetSiblingIndex(1);
      }
    }
  }

  public void PopulateColors()
  {
    for (int index = this.colors.Count - 1; index >= 0; --index)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.colors[index].gameObject);
    this.colors.Clear();
    WorshipperData.SkinAndData character = WorshipperData.Instance.Characters[this.formIndex];
    for (int index = 0; index < character.SlotAndColours.Count; ++index)
    {
      UINewRecruitSelectable recruitSelectable = UnityEngine.Object.Instantiate<UINewRecruitSelectable>(this.colorPrefab, (Transform) this.colorContainer);
      recruitSelectable.gameObject.SetActive(true);
      recruitSelectable.FormIndex = this.formIndex;
      recruitSelectable.SkinVariationIndex = this.variantIndex;
      recruitSelectable.SkinColour = index;
      this.colors.Add(recruitSelectable);
      SkeletonGraphic componentInChildren = this.colors[index].GetComponentInChildren<SkeletonGraphic>();
      componentInChildren.Skeleton.SetSkin(character.Skin[Mathf.Min(0, character.Skin.Count - 1)].Skin);
      foreach (WorshipperData.SlotAndColor slotAndColour in character.SlotAndColours[Mathf.Min(index, character.SlotAndColours.Count - 1)].SlotAndColours)
      {
        Spine.Slot slot = componentInChildren.Skeleton.FindSlot(slotAndColour.Slot);
        if (slot != null)
          slot.SetColor(slotAndColour.color);
      }
      if (!DataManager.GetFollowerSkinUnlocked(character.Skin[0].Skin))
        this.colors[index].Lock();
      else
        this.colors[index].Show();
    }
  }

  public void UpdateVariants()
  {
    WorshipperData.SkinAndData character = WorshipperData.Instance.Characters[this.formIndex];
    for (int index = 0; index < this.variants.Count; ++index)
    {
      this.variants[index].SkinColour = this.colorIndex;
      SkeletonGraphic componentInChildren = this.variants[index].GetComponentInChildren<SkeletonGraphic>();
      componentInChildren.Skeleton.SetSkin(character.Skin[Mathf.Min(this.variants[index].SkinVariationIndex, character.Skin.Count - 1)].Skin);
      foreach (WorshipperData.SlotAndColor slotAndColour in character.SlotAndColours[Mathf.Min(this.colorIndex, character.SlotAndColours.Count - 1)].SlotAndColours)
      {
        Spine.Slot slot = componentInChildren.Skeleton.FindSlot(slotAndColour.Slot);
        if (slot != null)
          slot.SetColor(slotAndColour.color);
      }
    }
    this.variantRandom.GetComponent<UINewRecruitSelectable>().FormIndex = this.formIndex;
  }

  public void PopulateVariants()
  {
    for (int index = this.variants.Count - 1; index >= 0; --index)
      UnityEngine.Object.Destroy((UnityEngine.Object) this.variants[index].gameObject);
    this.variants.Clear();
    WorshipperData.SkinAndData character = WorshipperData.Instance.Characters[this.formIndex];
    for (int index = 0; index < character.Skin.Count; ++index)
    {
      UINewRecruitSelectable recruitSelectable = UnityEngine.Object.Instantiate<UINewRecruitSelectable>(this.variantPrefab, (Transform) this.variantContainer);
      recruitSelectable.gameObject.SetActive(true);
      recruitSelectable.FormIndex = this.formIndex;
      recruitSelectable.SkinVariationIndex = index;
      recruitSelectable.SkinColour = this.colorIndex;
      this.variants.Add(recruitSelectable);
      SkeletonGraphic componentInChildren = this.variants[index].GetComponentInChildren<SkeletonGraphic>();
      componentInChildren.Skeleton.SetSkin(character.Skin[Mathf.Min(index, character.Skin.Count - 1)].Skin);
      WorshipperData.SlotsAndColours slotAndColour1 = character.SlotAndColours[this.colorIndex];
      foreach (WorshipperData.SlotAndColor slotAndColour2 in character.SlotAndColours[Mathf.Min(this.variants[index].SkinVariationIndex, character.SlotAndColours.Count - 1)].SlotAndColours)
      {
        Spine.Slot slot = componentInChildren.Skeleton.FindSlot(slotAndColour2.Slot);
        if (slot != null)
          slot.SetColor(slotAndColour1.AllColor);
      }
      if (!DataManager.GetFollowerSkinUnlocked(character.Skin[0].Skin))
        this.variants[index].Lock();
      else
        this.variants[index].Show();
    }
  }

  public void OptionSelected()
  {
    if (!(bool) (UnityEngine.Object) this.navigator.selectable || !(bool) (UnityEngine.Object) this.navigator.selectable.GetComponent<UINewRecruitSelectable>())
      return;
    UINewRecruitSelectable component = this.navigator.selectable.GetComponent<UINewRecruitSelectable>();
    if (!component.Locked.activeSelf)
    {
      if (component.FormIndex != this.formIndex)
      {
        this.colorIndex = 0;
        this.variantIndex = 0;
      }
      if (component.Random)
      {
        if (this.selectionPanelIndex == 1)
          this.RandomForm();
        else if (this.selectionPanelIndex == 2)
        {
          if ((UnityEngine.Object) component.transform.parent == (UnityEngine.Object) this.colorContainer)
            this.RandomColor();
          else if ((UnityEngine.Object) component.transform.parent == (UnityEngine.Object) this.variantContainer)
            this.RandomVariant();
        }
      }
      else if ((UnityEngine.Object) component.transform.parent == (UnityEngine.Object) this.formContainer)
      {
        this.formIndex = component.FormIndex;
        this.formRandomSelected = false;
        this.StartCoroutine((IEnumerator) this.SetSelectable(this.formsNextButton));
      }
      else if ((UnityEngine.Object) component.transform.parent == (UnityEngine.Object) this.colorContainer)
      {
        this.colorIndex = component.SkinColour;
        this.colorRandomSelected = false;
      }
      else if ((UnityEngine.Object) component.transform.parent == (UnityEngine.Object) this.variantContainer)
      {
        this.variantIndex = component.SkinVariationIndex;
        this.StartCoroutine((IEnumerator) this.SetSelectable(this.variantNextButton));
      }
    }
    this.UpdateFollow(this.formIndex, this.colorIndex, this.variantIndex);
  }

  public IEnumerator SetSelectable(Selectable selectable)
  {
    yield return (object) new WaitForEndOfFrame();
    this.navigator.startingItem = selectable;
    this.navigator.setDefault();
  }

  public void ChangeSelection(Selectable NewSelectable, Selectable PrevSelectable)
  {
    UINewRecruitSelectable component = NewSelectable.GetComponent<UINewRecruitSelectable>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
    {
      if ((UnityEngine.Object) component.transform.parent == (UnityEngine.Object) this.formContainer)
        this.UpdateFollow(component.FormIndex, 0, 0, component.IsLocked);
      else if ((UnityEngine.Object) component.transform.parent == (UnityEngine.Object) this.colorContainer)
        this.UpdateFollow(this.formIndex, component.SkinColour, this.variantIndex);
      else if ((UnityEngine.Object) component.transform.parent == (UnityEngine.Object) this.variantContainer)
        this.UpdateFollow(this.formIndex, this.colorIndex, component.SkinVariationIndex);
    }
    if (this.selectionPanelIndex == 1 && (UnityEngine.Object) PrevSelectable != (UnityEngine.Object) null && (double) NewSelectable.transform.position.y != (double) PrevSelectable.transform.position.y)
    {
      bool flag = (double) NewSelectable.transform.position.y < (double) PrevSelectable.transform.position.y;
      if (flag && (bool) (UnityEngine.Object) NewSelectable.FindSelectableOnDown())
        this.GetSnapToPositionToBringChildIntoView(this.formViewport, this.formContainer, NewSelectable.FindSelectableOnDown().transform as RectTransform);
      else if (!flag && (bool) (UnityEngine.Object) NewSelectable.FindSelectableOnUp())
        this.GetSnapToPositionToBringChildIntoView(this.formViewport, this.formContainer, NewSelectable.FindSelectableOnUp().transform as RectTransform);
    }
    if (!((UnityEngine.Object) NewSelectable == (UnityEngine.Object) this.nameBox))
      return;
    this.EditingName();
  }

  public void UpdateFollow(int formIndex, int colorIndex, int variantIndex, bool locked = false)
  {
    this.follower.Brain.Info.SkinCharacter = formIndex;
    this.follower.Brain.Info.SkinVariation = variantIndex;
    this.follower.Brain.Info.SkinColour = colorIndex;
    this.follower.Brain.Info.SkinName = WorshipperData.Instance.Characters[formIndex].Skin[variantIndex].Skin;
    this.follower.Brain.Info.Name = this.nameText.text;
    this.follower.SetOutfit(FollowerOutfitType.Rags, false);
    this.follower.Spine.GetComponent<Renderer>().sharedMaterial.SetColor("_Color", locked ? Color.black : Color.white);
    if (!locked && (this.selectionPanelIndex == 2 || this.selectionPanelIndex == 1))
    {
      Animator component = BiomeBaseManager.Instance.RecruitSpawnLocation.transform.parent.GetComponent<Animator>();
      if ((UnityEngine.Object) component != (UnityEngine.Object) null)
        component.SetTrigger("UpdateFollower");
    }
    this.lockIcon.transform.position = Camera.main.WorldToScreenPoint(this.follower.transform.position) + this.lockPositionOffset;
    this.lockIcon.SetActive(locked);
    this.UpdateSelectedItems();
  }

  public void GetSnapToPositionToBringChildIntoView(
    RectTransform viewport,
    RectTransform content,
    RectTransform child,
    float moveSpeed = 0.2f)
  {
    Canvas.ForceUpdateCanvases();
    Vector2 localPosition1 = (Vector2) viewport.localPosition;
    Vector2 localPosition2 = (Vector2) child.localPosition;
    Vector2 endValue = new Vector2((float) (0.0 - ((double) localPosition1.x + (double) localPosition2.x)), (float) (0.0 - ((double) localPosition1.y + (double) localPosition2.y)));
    endValue.x = content.localPosition.x;
    if (RectTransformUtility.RectangleContainsScreenPoint(viewport, (Vector2) child.transform.position))
      return;
    if ((double) moveSpeed == 0.0)
      content.localPosition = (Vector3) endValue;
    else
      content.DOLocalMove((Vector3) endValue, moveSpeed).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
  }

  public void UpdateSelectedItems()
  {
    for (int index = 0; index < this.forms.Count; ++index)
    {
      if (this.forms[index].FormIndex == this.formIndex)
        this.forms[index].SetSelected();
      else
        this.forms[index].SetUnselected();
    }
    for (int index = 0; index < this.colors.Count; ++index)
    {
      if (this.colors[index].SkinColour == this.colorIndex)
        this.colors[index].SetSelected();
      else
        this.colors[index].SetUnselected();
    }
    for (int index = 0; index < this.variants.Count; ++index)
    {
      if (this.variants[index].SkinVariationIndex == this.variantIndex)
        this.variants[index].SetSelected();
      else
        this.variants[index].SetUnselected();
    }
  }

  public void AcceptName() => this.ChangePanel(true);

  public void EditingName()
  {
    this.previousName = this.nameText.text;
    this.nameText.Select();
    Button componentInParent = this.nameText.GetComponentInParent<Button>();
    componentInParent.image.sprite = componentInParent.spriteState.selectedSprite;
    this.nameText.onSubmit.AddListener(new UnityAction<string>(this.OnSubmit));
    this.nameText.onEndEdit.AddListener(new UnityAction<string>(this.OnEndEdit));
  }

  public void OnSubmit(string r)
  {
    this.navigator.startingItem = (Selectable) this.editNameButton.GetComponentInParent<Button>();
    this.navigator.setDefault();
    if (this.previousName != r)
    {
      double num = (double) this.follower.SetBodyAnimation("Indoctrinate/indoctrinate-react", false);
      this.follower.AddBodyAnimation("pray", true, 0.0f);
    }
    this.nameText.onEndEdit.RemoveListener(new UnityAction<string>(this.OnSubmit));
  }

  public void OnEndEdit(string r)
  {
    this.nameText.GetComponentInParent<Button>().image.sprite = this.baseSprite;
    if (this.previousName != r)
    {
      double num = (double) this.follower.SetBodyAnimation("Indoctrinate/indoctrinate-react", false);
      this.follower.AddBodyAnimation("pray", true, 0.0f);
    }
    this.nameText.onEndEdit.RemoveListener(new UnityAction<string>(this.OnEndEdit));
  }

  public void Done()
  {
    GameManager.InMenu = false;
    if ((UnityEngine.Object) this.rendMatSwap != (UnityEngine.Object) null)
      this.rendMatSwap.SwapAll();
    this.follower.Spine.CustomMaterialOverride.Remove(this.prevMat);
    System.Action callack = this.callack;
    if (callack != null)
      callack();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
    AudioManager.Instance.PlayOneShot("event:/followers/appearance_accept", PlayerFarming.Instance.gameObject);
  }

  public void PopulateTraits()
  {
    foreach (FollowerTrait.TraitType trait in this.follower.Brain.Info.Traits)
      this.InstantiateTrait(trait, this.followerTraitsContainer);
    foreach (FollowerTrait.TraitType cultTrait in DataManager.Instance.CultTraits)
      this.InstantiateTrait(cultTrait, this.cultTraitsContainer);
    this.traitPrefab.gameObject.SetActive(false);
  }

  public UITrait InstantiateTrait(FollowerTrait.TraitType trait, RectTransform container)
  {
    UITrait uiTrait = UnityEngine.Object.Instantiate<UITrait>(this.traitPrefab, (Transform) container);
    uiTrait.Play(trait);
    uiTrait.gameObject.SetActive(true);
    return uiTrait;
  }
}
