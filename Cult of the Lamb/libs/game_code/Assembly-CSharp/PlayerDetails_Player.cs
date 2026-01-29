// Decompiled with JetBrains decompiler
// Type: PlayerDetails_Player
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using MMTools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#nullable disable
public class PlayerDetails_Player : BaseMonoBehaviour
{
  public GameObject TarotCardPrefab;
  public GameObject BlankTarotCardPrefab;
  public UINavigator UINavigator;
  public Transform TarotCardParent;
  public CanvasGroup canvasGroup;
  public List<HUD_TrinketCard> _cards = new List<HUD_TrinketCard>();
  public List<WeaponCurseIcons> WeaponIcons = new List<WeaponCurseIcons>();
  public GameObject NoTarotCards;
  public Image Weapon;
  public TextMeshProUGUI WeaponTitle;
  public TextMeshProUGUI WeaponDescription;
  public TextMeshProUGUI WeaponDamage;
  public GameObject WeaponProgressBar;
  public Image WeaponProgress;
  public Image Curse;
  public TextMeshProUGUI CurseTitle;
  public TextMeshProUGUI CurseDescription;
  public TextMeshProUGUI CurseDamage;
  public GameObject CurseProgressBar;
  public Image CurseProgress;
  public Image Ability;
  public TextMeshProUGUI AbilityTitle;
  public TextMeshProUGUI AbilityDescription;
  public GameObject AbilityProgressBar;
  public Image AbilityProgress;
  public MMControlPrompt controlPrompt;
  public TextMeshProUGUI controlPromptTxt;
  public bool createdTrinket;
  public GameObject BlackTint;
  public GameObject canvas;
  public List<HUD_TrinketCard> Cards = new List<HUD_TrinketCard>();
  public bool TarotCardActive;
  public AnimationCurve animationCurve;

  public static string GetWeaponLevel(int _WeaponLevel)
  {
    switch (_WeaponLevel)
    {
      case 0:
        return "";
      case 1:
        return "I";
      case 2:
        return "II";
      case 3:
        return "III";
      case 4:
        return "IV";
      case 5:
        return "V";
      case 6:
        return "VI";
      case 7:
        return "VII";
      case 8:
        return "VIII";
      case 9:
        return "IX";
      case 10:
        return "X";
      case 11:
        return "XI";
      case 12:
        return "XII";
      case 13:
        return "XIII";
      case 14:
        return "XIV";
      case 15:
        return "XV";
      default:
        return "";
    }
  }

  public static string GetWeaponMod(TarotCards.Card card)
  {
    if (card == TarotCards.Card.Spider)
      return "<color=green>Poison</color> ";
    return card == TarotCards.Card.DiseasedHeart ? "<color=red>Cursed</color> " : "";
  }

  public static string GetWeaponCondition(int _WeaponDurability)
  {
    switch (_WeaponDurability)
    {
      case 0:
        return "";
      case 1:
        return "Rusty ";
      case 2:
        return "Polished ";
      case 3:
        return "Blessed ";
      default:
        return "";
    }
  }

  public void GetWeapon()
  {
  }

  public void GetAbility()
  {
    this.AbilityTitle.text = "Replenesh Health";
    this.AbilityDescription.text = "Enlightenment fills your viens fueled by the death of non-beleivers";
    this.controlPrompt.Action = 9;
    this.controlPromptTxt.text = "Hold When Faith bar full";
  }

  public void ButtonDown()
  {
    Debug.Log((object) "On Select Down");
    HUD_TrinketCard component = this.UINavigator.selectable.gameObject.GetComponent<HUD_TrinketCard>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || this.createdTrinket || component.CardType.CardType == TarotCards.Card.Count)
      return;
    this.BlackTint.SetActive(true);
    this.createdTrinket = true;
    this.canvasGroup.interactable = false;
    this.UINavigator.enabled = false;
    UITrinketCards.PlayFromPause(component.CardType, new System.Action(this.TarotCardPopUpDone), this.canvas.gameObject);
  }

  public void TarotCardPopUpDone()
  {
    this.BlackTint.SetActive(false);
    this.createdTrinket = false;
    this.canvasGroup.interactable = true;
    this.UINavigator.enabled = true;
  }

  public void OnDisable()
  {
    this.UINavigator.OnSelectDown -= new System.Action(this.ButtonDown);
    this.UINavigator.OnChangeSelectionUnity -= new UINavigator.ChangeSelectionUnity(this.OnChangeSelectionUnity);
  }

  public void OnChangeSelectionUnity(Selectable PrevSelectable, Selectable NewSelectable)
  {
    Vector3 punch = new Vector3(1.1f, 1.1f, 1.1f);
    NewSelectable.transform.DOPunchScale(punch, 0.5f);
  }

  public void OnEnable()
  {
    Debug.Log((object) "Player Details Enabled");
    if (SceneManager.GetActiveScene().name == "Base Biome 1")
    {
      for (int index = 0; index < PlayerFarming.playersCount; ++index)
        PlayerFarming.players[index].FoundTrinkets.Clear();
    }
    this.BlackTint.SetActive(false);
    this.createdTrinket = false;
    this.UINavigator.OnChangeSelectionUnity += new UINavigator.ChangeSelectionUnity(this.OnChangeSelectionUnity);
    this.UINavigator.OnSelectDown += new System.Action(this.ButtonDown);
    for (int index = 0; index < PlayerFarming.playersCount; ++index)
    {
      if (this.Cards.Count == 0)
        this.InitTrinketCards(PlayerFarming.players[index]);
      else
        this.SetTrinketsCards(PlayerFarming.players[index]);
    }
    this.GetWeapon();
    this.GetAbility();
  }

  public void InitTrinketCards(PlayerFarming playerFarming)
  {
    int num = -1;
    while (++num < 9)
    {
      GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.TarotCardPrefab, this.TarotCardParent);
      gameObject.SetActive(true);
      RectTransform component = gameObject.GetComponent<RectTransform>();
      component.anchorMin = new Vector2(0.5f, 0.5f);
      component.anchorMax = new Vector2(0.5f, 0.5f);
      component.pivot = new Vector2(0.5f, 0.5f);
      this.Cards.Add(gameObject.GetComponent<HUD_TrinketCard>());
    }
    this.SetTrinketsCards(playerFarming);
    this.UINavigator.startingItem = this.Cards[0].GetComponent<Selectable>();
  }

  public void SetTrinketsCards(PlayerFarming playerFarming)
  {
    this.TarotCardActive = false;
    int index = -1;
    if (playerFarming.RunTrinkets.Count > 0)
    {
      while (++index < playerFarming.RunTrinkets.Count)
      {
        if (index < 9)
        {
          Debug.Log((object) playerFarming.RunTrinkets[index].CardType);
          TarotCards tarotCards = TarotCards.Create(playerFarming.RunTrinkets[index].CardType, true);
          this.Cards[index].SetCard(new TarotCards.TarotCard(tarotCards.Type, 0));
          this.Cards[index].Card.enabled = true;
          this.Cards[index].GetComponent<Button>().enabled = true;
          this.UINavigator.Buttons[index].canUse = true;
          this.TarotCardActive = true;
        }
      }
    }
    if (playerFarming.RunTrinkets.Count < 9)
    {
      while (++index < 9 - playerFarming.RunTrinkets.Count)
      {
        this.Cards[index].Card.enabled = false;
        this.Cards[index].GetComponent<Button>().enabled = false;
        this.UINavigator.Buttons[index].canUse = false;
      }
    }
    if (!this.TarotCardActive)
      this.NoTarotCards.SetActive(true);
    else
      this.NoTarotCards.SetActive(false);
  }

  public IEnumerator CardAnimateIn(HUD_TrinketCard card)
  {
    float Progress = 0.0f;
    float Duration = 0.5f;
    card.transform.localPosition = new Vector3(card.transform.localPosition.x, -200f);
    card.Card.enabled = true;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      card.transform.localPosition = new Vector3(card.transform.localPosition.x, Mathf.LerpUnclamped(-200f, 0.0f, this.animationCurve.Evaluate(Progress / Duration)));
      yield return (object) null;
    }
  }
}
