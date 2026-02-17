// Decompiled with JetBrains decompiler
// Type: CrownAbilitiesManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

#nullable disable
public class CrownAbilitiesManager : BaseMonoBehaviour
{
  public Animator animator;
  public System.Action CancelCallback;
  public UINavigator uiNav;
  public Selectable selectable;
  public Selectable oldSelectable;
  public Animator cardAnimator;
  public Image selectedIconFeatureIcon;
  public TextMeshProUGUI TitleText;
  public TextMeshProUGUI DescriptionText;
  public TextMeshProUGUI CostText;
  public GameObject lockIcon;
  public List<CrownAbilitiesIcon> objectsInScene;
  public StructuresData.ResearchState state;
  public CrownAbilitiesIcon icon;
  public TextMeshProUGUI Souls;
  public bool closing;
  public int currentSouls;

  public void updateListing()
  {
    this.icon = this.uiNav.selectable.gameObject.GetComponent<CrownAbilitiesIcon>();
    if ((UnityEngine.Object) this.cardAnimator != (UnityEngine.Object) null)
      this.cardAnimator.Play("Panel In");
    if ((UnityEngine.Object) this.icon.Icon != (UnityEngine.Object) null)
      this.selectedIconFeatureIcon.sprite = this.icon.Icon.sprite;
    if ((UnityEngine.Object) this.icon.dependancy != (UnityEngine.Object) null && !CrownAbilities.CrownAbilityUnlocked(this.icon.dependancy.type))
      this.lockIcon.SetActive(true);
    this.TitleText.text = CrownAbilities.LocalisedName(this.icon.type);
    this.DescriptionText.text = CrownAbilities.LocalisedDescription(this.icon.type);
    if (!CrownAbilities.CrownAbilityUnlocked(this.icon.type))
    {
      this.CostText.text = "";
      int abilityPoints = DataManager.Instance.AbilityPoints;
      int crownAbilitiesCost = CrownAbilities.GetCrownAbilitiesCost(this.icon.type);
      this.CostText.text += crownAbilitiesCost > abilityPoints ? "<color=#ff0000>" : "<color=#00ff00>";
      this.CostText.text += FontImageNames.GetIconByType(InventoryItem.ITEM_TYPE.SOUL);
      TextMeshProUGUI costText1 = this.CostText;
      costText1.text = $"{costText1.text} x{crownAbilitiesCost.ToString()}";
      TextMeshProUGUI costText2 = this.CostText;
      costText2.text = $"{costText2.text}   ({abilityPoints.ToString()})";
      this.CostText.text += "\n";
      this.CostText.text += "</color>";
      if (!((UnityEngine.Object) this.icon.dependancy != (UnityEngine.Object) null) || CrownAbilities.CrownAbilityUnlocked(this.icon.dependancy.type))
        return;
      CrownAbilitiesIcon component = this.icon.dependancy.GetComponent<CrownAbilitiesIcon>();
      this.CostText.text += "<color=#ff0000>";
      this.CostText.text += "Requires ";
      this.CostText.text += CrownAbilities.LocalisedName(component.type);
      this.CostText.text += "\n";
      this.CostText.text += "</color>";
    }
    else
    {
      this.lockIcon.SetActive(false);
      this.CostText.text = "Already Bought";
    }
  }

  public void OnEnable() => Time.timeScale = 0.0f;

  public void Play(GameObject CameraTarget, System.Action CancelCallback)
  {
    this.CancelCallback = CancelCallback;
    GameManager.GetInstance().RemoveAllFromCamera();
    GameManager.GetInstance().AddToCamera(CameraTarget);
    GameManager.GetInstance().CameraSetOffset(new Vector3(-5f, 0.0f, 0.0f));
    GameManager.GetInstance().CameraSetTargetZoom(15f);
    this.closing = false;
    this.updateListing();
    this.updateSouls();
  }

  public IEnumerator Closing(bool SetActive = false)
  {
    // ISSUE: reference to a compiler-generated field
    int num = this.\u003C\u003E1__state;
    CrownAbilitiesManager abilitiesManager = this;
    if (num != 0)
    {
      if (num != 1)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.\u003C\u003E1__state = -1;
      abilitiesManager.closing = SetActive;
      UnityEngine.Object.Destroy((UnityEngine.Object) abilitiesManager.gameObject);
      return false;
    }
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = -1;
    abilitiesManager.closing = true;
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    GameManager.GetInstance().CameraResetTargetZoom();
    Time.timeScale = 1f;
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E2__current = (object) new WaitForEndOfFrame();
    // ISSUE: reference to a compiler-generated field
    this.\u003C\u003E1__state = 1;
    return true;
  }

  public void updateSouls()
  {
    this.Souls.text = "AbilityTokens " + DataManager.Instance.AbilityPoints.ToString();
  }

  public void Update()
  {
    this.updateSouls();
    this.selectable = this.uiNav.selectable;
    if ((UnityEngine.Object) this.selectable != (UnityEngine.Object) this.oldSelectable)
    {
      this.updateListing();
      this.oldSelectable = this.selectable;
    }
    if (this.closing)
      return;
    if (InputManager.UI.GetCancelButtonDown())
    {
      this.StartCoroutine((IEnumerator) this.Closing());
      GameManager.GetInstance().RemoveAllFromCamera();
      GameManager.GetInstance().AddPlayerToCamera();
      System.Action cancelCallback = this.CancelCallback;
      if (cancelCallback == null)
        return;
      cancelCallback();
    }
    else
    {
      if (!InputManager.UI.GetAcceptButtonDown())
        return;
      bool flag = this.icon.gameObject.GetComponent<CrownAbilitiesIcon>().CanAfford;
      if (CheatConsole.BuildingsFree)
        flag = true;
      if (flag)
      {
        this.StartCoroutine((IEnumerator) this.Closing());
        CrownAbilities.UnlockAbility(this.icon.type);
        DataManager.Instance.AbilityPoints -= CrownAbilities.GetCrownAbilitiesCost(this.icon.type);
        for (int index = 0; index < this.objectsInScene.Count; ++index)
        {
          if ((UnityEngine.Object) this.objectsInScene[index] != (UnityEngine.Object) null)
            this.objectsInScene[index].Init();
        }
        GameManager.GetInstance().RemoveAllFromCamera();
        GameManager.GetInstance().AddPlayerToCamera();
      }
      else
        this.icon.Shake();
    }
  }
}
