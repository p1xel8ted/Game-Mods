// Decompiled with JetBrains decompiler
// Type: UICardManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System;
using System.Collections;
using TMPro;
using UnityEngine;

#nullable disable
public class UICardManager : BaseMonoBehaviour
{
  public GameObject CardPrefab;
  public GameObject BlankCardPrefab;
  public TextMeshProUGUI SoulsCount;
  public GameObject SoulPrefab;
  public RectTransform SoulSpawnPoint;
  public UIWeaponCardFeature DisplayFeature;
  public System.Action CallBack;
  public UINavigator WeaponsUINavigator;
  public RectTransform WeaponRT;
  public UINavigator CursesUINavigator;
  public RectTransform CursesRT;
  public UINavigator TrinketsUINavigator;
  public RectTransform TrinketRT;

  public void Start()
  {
    this.SoulsCount.text = "<sprite name=\"icon_spirits\"> " + Inventory.Souls.ToString();
    this.StartCoroutine((IEnumerator) this.Begin());
    this.SpawnTrinketsCards();
    this.DisplayFeature.gameObject.SetActive(false);
    GameManager.GetInstance().CameraSetOffset(new Vector3(-3f, 0.0f, 0.0f));
  }

  public IEnumerator Begin()
  {
    this.WeaponsUINavigator.ControlsEnabled = false;
    this.CursesUINavigator.ControlsEnabled = false;
    this.TrinketsUINavigator.ControlsEnabled = false;
    float Timer = 0.0f;
    while ((double) (Timer += Time.deltaTime) > 0.5)
      yield return (object) null;
    this.WeaponsUINavigator.ControlsEnabled = true;
    this.CursesUINavigator.ControlsEnabled = true;
    this.TrinketsUINavigator.ControlsEnabled = true;
  }

  public void OnClose()
  {
    System.Action callBack = this.CallBack;
    if (callBack != null)
      callBack();
    GameManager.GetInstance().CameraSetOffset(new Vector3(0.0f, 0.0f, 0.0f));
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public void OnDisable()
  {
    this.WeaponsUINavigator.OnChangeSelection -= new UINavigator.ChangeSelection(this.OnChangeWeaponSelection);
    this.WeaponsUINavigator.OnButtonDown -= new UINavigator.ButtonDown(this.OnWeaponButtonDown);
    this.WeaponsUINavigator.OnClose -= new UINavigator.Close(this.OnClose);
    this.CursesUINavigator.OnChangeSelection -= new UINavigator.ChangeSelection(this.OnChangeCursesSelection);
    this.CursesUINavigator.OnButtonDown -= new UINavigator.ButtonDown(this.OnCursesButtonDown);
    this.CursesUINavigator.OnClose -= new UINavigator.Close(this.OnClose);
    this.TrinketsUINavigator.OnChangeSelection -= new UINavigator.ChangeSelection(this.OnChangeTrinketsSelection);
    this.TrinketsUINavigator.OnButtonDown -= new UINavigator.ButtonDown(this.OnTrinketsButtonDown);
    this.TrinketsUINavigator.OnClose -= new UINavigator.Close(this.OnClose);
  }

  public void CreateSoul(Buttons CurrentButton, UIWeaponCardSoul.CardComplete Callback)
  {
    (CurrentButton as UICardManager.ButtonsUICardManager).uICardManagerCard.UnlockProgressWait += 0.1f;
    UIWeaponCardSoul component = UnityEngine.Object.Instantiate<GameObject>(this.SoulPrefab, Vector3.zero, Quaternion.identity, this.transform).GetComponent<UIWeaponCardSoul>();
    Vector3 position = CurrentButton.Button.GetComponent<RectTransform>().position;
    component.Play(CurrentButton, this.SoulSpawnPoint.position, position);
    --Inventory.Souls;
    this.SoulsCount.text = "<sprite name=\"icon_spirits\"> " + Inventory.Souls.ToString();
    component.OnCardComplete += Callback;
  }

  public void OnCardCompleteWeapon(Buttons CurrentButton)
  {
    (CurrentButton as UICardManager.ButtonsUICardManager).Card.UnlockProgress += 0.1f;
    this.UpdateCard(CurrentButton as UICardManager.ButtonsUICardManager);
    if ((double) (CurrentButton as UICardManager.ButtonsUICardManager).Card.UnlockProgress < 1.0)
      return;
    this.StartCoroutine((IEnumerator) this.UnlockCard(CurrentButton as UICardManager.ButtonsUICardManager, this.WeaponsUINavigator));
  }

  public void OnCardCompleteCurses(Buttons CurrentButton)
  {
    (CurrentButton as UICardManager.ButtonsUICardManager).Card.UnlockProgress += 0.1f;
    this.UpdateCard(CurrentButton as UICardManager.ButtonsUICardManager);
    if ((double) (CurrentButton as UICardManager.ButtonsUICardManager).Card.UnlockProgress < 1.0)
      return;
    this.StartCoroutine((IEnumerator) this.UnlockCard(CurrentButton as UICardManager.ButtonsUICardManager, this.CursesUINavigator));
  }

  public void OnCardCompleteTrinkets(Buttons CurrentButton)
  {
    (CurrentButton as UICardManager.ButtonsUICardManager).Card.UnlockProgress += 0.1f;
    this.UpdateCard(CurrentButton as UICardManager.ButtonsUICardManager);
    if ((double) (CurrentButton as UICardManager.ButtonsUICardManager).Card.UnlockProgress < 1.0)
      return;
    this.StartCoroutine((IEnumerator) this.UnlockCard(CurrentButton as UICardManager.ButtonsUICardManager, this.TrinketsUINavigator));
  }

  public void SpawnWeaponCards()
  {
    int num = -1;
    while (++num <= 3)
      UnityEngine.Object.Instantiate<GameObject>(this.BlankCardPrefab, (Transform) this.WeaponRT);
    this.WeaponsUINavigator.OnChangeSelection += new UINavigator.ChangeSelection(this.OnChangeWeaponSelection);
    this.WeaponsUINavigator.OnDeselect += new UINavigator.Deselect(this.OnDeselect);
    this.WeaponsUINavigator.OnButtonDown += new UINavigator.ButtonDown(this.OnWeaponButtonDown);
    this.WeaponsUINavigator.OnClose += new UINavigator.Close(this.OnClose);
    this.WeaponsUINavigator.setDefault();
  }

  public void OnDeselect(Buttons Button)
  {
    (Button as UICardManager.ButtonsUICardManager).uICardManagerCard.Deselected();
  }

  public void OnChangeWeaponSelection(Buttons Button)
  {
    (Button as UICardManager.ButtonsUICardManager).uICardManagerCard.Selected();
    this.DisplayFeature.Selected(Button, "Weapons/" + (Button as UICardManager.ButtonsUICardManager).Card.Type.ToString());
  }

  public void OnWeaponButtonDown(Buttons CurrentButton)
  {
    if ((double) (CurrentButton as UICardManager.ButtonsUICardManager).uICardManagerCard.UnlockProgressWait >= 1.0 || Inventory.Souls <= 0)
      return;
    this.CreateSoul(CurrentButton, new UIWeaponCardSoul.CardComplete(this.OnCardCompleteWeapon));
  }

  public void UpdateCard(UICardManager.ButtonsUICardManager CurrentButton)
  {
    if (CurrentButton.Card.Unlocked)
      return;
    float num1 = (float) UnityEngine.Random.Range(-10, -5);
    float num2 = (float) (130 + UnityEngine.Random.Range(-45, 45));
    CurrentButton.uICardManagerCard.DoShake(num1 + Mathf.Cos(num2 * ((float) Math.PI / 180f)), num1 + Mathf.Sin(num2 * ((float) Math.PI / 180f)));
    CurrentButton.uICardManagerCard.SetCard(CurrentButton.Card);
  }

  public IEnumerator UnlockCard(
    UICardManager.ButtonsUICardManager CurrentButton,
    UINavigator uINavigator)
  {
    uINavigator.ControlsEnabled = false;
    while (CurrentButton.uICardManagerCard.Unlocking)
      yield return (object) null;
    if ((UnityEngine.Object) uINavigator == (UnityEngine.Object) this.WeaponsUINavigator)
      this.DisplayFeature.Selected((Buttons) CurrentButton, "Weapons/" + CurrentButton.Card.Type.ToString());
    else if ((UnityEngine.Object) uINavigator == (UnityEngine.Object) this.CursesUINavigator)
      this.DisplayFeature.Selected((Buttons) CurrentButton, "Curses/" + CurrentButton.Card.Type.ToString());
    else if ((UnityEngine.Object) uINavigator == (UnityEngine.Object) this.TrinketsUINavigator)
      this.DisplayFeature.Selected((Buttons) CurrentButton, TarotCards.Skin(CurrentButton.Card.Type));
    yield return (object) new WaitForSeconds(0.5f);
    uINavigator.ControlsEnabled = true;
  }

  public void SpawnCursesCards()
  {
    int num = -1;
    while (++num <= 3)
      UnityEngine.Object.Instantiate<GameObject>(this.BlankCardPrefab, (Transform) this.CursesRT);
    this.CursesUINavigator.OnChangeSelection += new UINavigator.ChangeSelection(this.OnChangeCursesSelection);
    this.CursesUINavigator.OnButtonDown += new UINavigator.ButtonDown(this.OnCursesButtonDown);
    this.CursesUINavigator.OnDeselect += new UINavigator.Deselect(this.OnDeselect);
    this.CursesUINavigator.OnClose += new UINavigator.Close(this.OnClose);
    this.CursesUINavigator.setDefault();
  }

  public void OnChangeCursesSelection(Buttons Button)
  {
    (Button as UICardManager.ButtonsUICardManager).uICardManagerCard.Selected();
    this.DisplayFeature.Selected(Button, "Curses/" + (Button as UICardManager.ButtonsUICardManager).Card.Type.ToString());
  }

  public void OnCursesButtonDown(Buttons CurrentButton)
  {
    if ((double) (CurrentButton as UICardManager.ButtonsUICardManager).uICardManagerCard.UnlockProgressWait >= 1.0)
      return;
    this.CreateSoul(CurrentButton, new UIWeaponCardSoul.CardComplete(this.OnCardCompleteCurses));
  }

  public void SpawnTrinketsCards()
  {
    int num = -1;
    while (++num < DataManager.Instance.PlayerFoundTrinkets.Count)
    {
      TarotCards Card = TarotCards.Create(DataManager.Instance.PlayerFoundTrinkets[num], true);
      GameObject Button = UnityEngine.Object.Instantiate<GameObject>(this.CardPrefab, (Transform) this.TrinketRT);
      UICardManagerCard component = Button.GetComponent<UICardManagerCard>();
      component.SetSkin(TarotCards.Skin(Card.Type));
      component.SetCard(Card);
      this.TrinketsUINavigator.Buttons.Add((Buttons) new UICardManager.ButtonsUICardManager(Button, num, buttons.HorizontalSelector, num == 0, Card, component));
    }
    while (++num <= DataManager.AllTrinkets.Count)
      UnityEngine.Object.Instantiate<GameObject>(this.BlankCardPrefab, (Transform) this.TrinketRT);
    while (++num <= 3)
      UnityEngine.Object.Instantiate<GameObject>(this.BlankCardPrefab, (Transform) this.TrinketRT);
    this.TrinketsUINavigator.OnChangeSelection += new UINavigator.ChangeSelection(this.OnChangeTrinketsSelection);
    this.TrinketsUINavigator.OnButtonDown += new UINavigator.ButtonDown(this.OnTrinketsButtonDown);
    this.TrinketsUINavigator.OnDeselect += new UINavigator.Deselect(this.OnDeselect);
    this.TrinketsUINavigator.OnClose += new UINavigator.Close(this.OnClose);
    this.TrinketsUINavigator.setDefault();
  }

  public void OnChangeTrinketsSelection(Buttons Button)
  {
    (Button as UICardManager.ButtonsUICardManager).uICardManagerCard.Selected();
    this.DisplayFeature.Selected(Button, TarotCards.Skin((Button as UICardManager.ButtonsUICardManager).Card.Type));
  }

  public void OnTrinketsButtonDown(Buttons CurrentButton)
  {
    if ((double) (CurrentButton as UICardManager.ButtonsUICardManager).uICardManagerCard.UnlockProgressWait >= 1.0)
      return;
    this.CreateSoul(CurrentButton, new UIWeaponCardSoul.CardComplete(this.OnCardCompleteTrinkets));
  }

  public class ButtonsUICardManager : Buttons
  {
    [HideInInspector]
    public UICardManagerCard uICardManagerCard;
    [HideInInspector]
    public TarotCards Card;
    [HideInInspector]
    public SermonsAndRituals.SermonRitualType SermonRitualType;

    public ButtonsUICardManager(
      GameObject Button,
      int Index,
      buttons buttonTypes,
      bool selected,
      TarotCards Card,
      UICardManagerCard uICardManagerCard)
    {
      this.Button = Button;
      this.Index = Index;
      this.buttonTypes = buttonTypes;
      this.selected = selected;
      this.Card = Card;
      this.uICardManagerCard = uICardManagerCard;
    }
  }
}
