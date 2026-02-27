// Decompiled with JetBrains decompiler
// Type: FollowerSkinShop
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using Lamb.UI;
using src.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FollowerSkinShop : Interaction
{
  public bool Activated;
  public GameObject g;
  [SerializeField]
  public SpriteRenderer Shrine;
  [SerializeField]
  public GameObject Portal;
  [SerializeField]
  public GameObject Symbol;
  [SerializeField]
  public GameObject CoinTarget;
  [SerializeField]
  public GameObject LightingVolumeManager;
  public FollowerLocation location = FollowerLocation.None;
  public List<WorshipperData.SkinAndData> Skins;
  public List<WorshipperData.SkinAndData> SkinsAvailable;
  public InventoryItem.ITEM_TYPE Cost = InventoryItem.ITEM_TYPE.BLACK_GOLD;
  public bool noSkins;

  public void Start()
  {
    this.Portal.SetActive(false);
    this.Symbol.SetActive(false);
    this.LightingVolumeManager.SetActive(false);
  }

  public void Setup()
  {
    this.location = PlayerFarming.Location;
    this.Skins = WorshipperData.Instance.GetSkinsFromFollowerLocation(PlayerFarming.Location);
    this.CheckSkinAvailability();
  }

  public override void OnEnableInteraction()
  {
    base.OnEnableInteraction();
    this.CheckSkinAvailability();
  }

  public void disableBuilding()
  {
    if (!((UnityEngine.Object) this.Shrine != (UnityEngine.Object) null))
      return;
    this.Shrine.color = new Color(0.5f, 0.5f, 0.5f, 1f);
  }

  public void CheckSkinAvailability()
  {
    this.SkinsAvailable.Clear();
    if (this.Skins == null)
      return;
    foreach (WorshipperData.SkinAndData skin in this.Skins)
    {
      if (!DataManager.GetFollowerSkinUnlocked(skin.Skin[0].Skin) && !skin.Skin[0].Skin.Contains("Boss") && !DataManager.OnBlackList(skin.Skin[0].Skin))
        this.SkinsAvailable.Add(skin);
    }
    if (this.SkinsAvailable.Count > 0)
      return;
    this.disableBuilding();
  }

  public int GetCost()
  {
    switch (this.location)
    {
      case FollowerLocation.HubShore:
        return 10;
      case FollowerLocation.Dungeon_Decoration_Shop1:
        return 30;
      case FollowerLocation.Dungeon_Location_3:
        return 40;
      case FollowerLocation.Sozo_Cave:
        return 20;
      case FollowerLocation.DLC_ShrineRoom:
        int[] numArray = new int[13]
        {
          3,
          3,
          5,
          5,
          10,
          10,
          10,
          10,
          15,
          15,
          15,
          15,
          20
        };
        int index = Mathf.Clamp(DataManager.Instance.WoolhavenSkinsPurchased, 0, numArray.Length - 1);
        return numArray[index];
      default:
        return 3;
    }
  }

  public override void OnSecondaryInteract(StateMachine state)
  {
    base.OnSecondaryInteract(state);
    state.CURRENT_STATE = StateMachine.State.InActive;
    HUD_Manager.Instance.Hide(false, 0);
    Time.timeScale = 0.0f;
    UIFollowerFormsMenuController followerFormsMenuInstance = MonoSingleton<UIManager>.Instance.FollowerFormsMenuTemplate.Instantiate<UIFollowerFormsMenuController>();
    followerFormsMenuInstance.Show();
    UIFollowerFormsMenuController formsMenuController = followerFormsMenuInstance;
    formsMenuController.OnHidden = formsMenuController.OnHidden + (System.Action) (() =>
    {
      Time.timeScale = 1f;
      this.HasChanged = true;
      PlayerFarming.SetStateForAllPlayers();
      this.Activated = false;
      this.SecondaryInteractable = true;
      HUD_Manager.Instance.Show(0);
      followerFormsMenuInstance = (UIFollowerFormsMenuController) null;
    });
  }

  public override void GetSecondaryLabel()
  {
    base.GetSecondaryLabel();
    this.SecondaryLabel = ScriptLocalization.UI_FollowerSkinUnlock.UnlockedForms;
  }

  public override void GetLabel()
  {
    if (this.SkinsAvailable.Count <= 0)
    {
      this.Interactable = false;
      this.noSkins = true;
      this.Label = ScriptLocalization.Interactions.SoldOut;
    }
    else
      this.Label = string.Join(" ", ScriptLocalization.UI_FollowerSkinUnlock.BuyForm, CostFormatter.FormatCost(this.Cost, this.GetCost()));
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (this.noSkins)
      this.playerFarming.indicator.PlayShake();
    else if (this.GetCost() <= Inventory.GetItemQuantity((int) this.Cost) && !this.Activated)
    {
      AudioManager.Instance.PlayOneShot("event:/shop/buy", this.transform.position);
      this.Activated = true;
      this.Interactable = false;
      this.StartCoroutine(this.GiveSkin());
    }
    else
    {
      AudioManager.Instance.PlayOneShot("event:/ui/negative_feedback", this.transform.position);
      this.playerFarming.indicator.PlayShake();
    }
  }

  public IEnumerator GiveSkin()
  {
    FollowerSkinShop followerSkinShop = this;
    GameManager.GetInstance().OnConversationNew();
    followerSkinShop.playerFarming.GoToAndStop(followerSkinShop.transform.position - Vector3.up, followerSkinShop.gameObject, true, true, new System.Action(followerSkinShop.\u003CGiveSkin\u003Eb__22_0));
    GameManager.GetInstance().CameraSetTargetZoom(8f);
    followerSkinShop.LightingVolumeManager.SetActive(true);
    yield return (object) new WaitForSeconds(1f);
    followerSkinShop.LightingVolumeManager.SetActive(true);
    followerSkinShop.Symbol.SetActive(true);
    followerSkinShop.Symbol.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.0f);
    followerSkinShop.Symbol.GetComponent<SpriteRenderer>().DOFade(1f, 0.5f);
    for (int i = 0; i < followerSkinShop.GetCost(); ++i)
    {
      AudioManager.Instance.PlayOneShot("event:/followers/pop_in", followerSkinShop.gameObject);
      ResourceCustomTarget.Create(followerSkinShop.CoinTarget, followerSkinShop.playerFarming.transform.position, followerSkinShop.Cost, (System.Action) null);
      Inventory.ChangeItemQuantity((int) followerSkinShop.Cost, -1);
      if (followerSkinShop.GetCost() == 10 || followerSkinShop.GetCost() == 20)
        yield return (object) new WaitForSeconds(UnityEngine.Random.Range(0.05f, 0.1f));
      else
        yield return (object) new WaitForSeconds(UnityEngine.Random.Range(0.005f, 0.25f));
    }
    CameraManager.instance.ShakeCameraForDuration(0.1f, 1f, 1f);
    MMVibrate.Haptic(MMVibrate.HapticTypes.MediumImpact);
    followerSkinShop.Portal.SetActive(true);
    followerSkinShop.Symbol.GetComponent<SpriteRenderer>().DOFade(0.0f, 1f);
    followerSkinShop.Portal.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.0f);
    followerSkinShop.Portal.GetComponent<SpriteRenderer>().DOFade(0.0f, 0.3f);
    followerSkinShop.Portal.transform.localScale = new Vector3(1f, 0.0f, 1f);
    followerSkinShop.Portal.transform.DOScaleY(1f, 0.25f);
    AudioManager.Instance.PlayOneShot("event:/small_portal/open", followerSkinShop.gameObject);
    yield return (object) new WaitForSeconds(0.5f);
    AudioManager.Instance.PlayOneShot("event:/Stings/Choir_mid", followerSkinShop.gameObject);
    FollowerSkinCustomTarget.Create(followerSkinShop.gameObject.transform.position + new Vector3(0.0f, 0.0f, -1f), followerSkinShop.playerFarming.gameObject.transform.position, 2f, followerSkinShop.SkinsAvailable[UnityEngine.Random.Range(0, followerSkinShop.SkinsAvailable.Count)].Skin[0].Skin, new System.Action(followerSkinShop.PickedUp));
    BiomeConstants.Instance.EmitSmokeExplosionVFX(followerSkinShop.transform.position);
    RumbleManager.Instance.Rumble();
    yield return (object) new WaitForSeconds(0.5f);
    followerSkinShop.LightingVolumeManager.SetActive(false);
  }

  public void PickedUp()
  {
    this.SecondaryInteractable = true;
    this.Activated = false;
    this.Interactable = true;
    this.Portal.transform.DOScaleY(0.0f, 0.33f);
    AudioManager.Instance.PlayOneShot("event:/small_portal/close", this.gameObject);
    this.Symbol.SetActive(false);
    this.LightingVolumeManager.SetActive(false);
    GameManager.GetInstance().CameraResetTargetZoom();
    PlayerFarming.SetStateForAllPlayers();
    this.HasChanged = true;
    GameManager.GetInstance().OnConversationEnd();
    if (this.location == FollowerLocation.DLC_ShrineRoom)
      ++DataManager.Instance.WoolhavenSkinsPurchased;
    this.CheckSkinAvailability();
  }

  public override void Update()
  {
    base.Update();
    if ((UnityEngine.Object) this.playerFarming == (UnityEngine.Object) null || this.location != FollowerLocation.None)
      return;
    this.Setup();
  }

  [CompilerGenerated]
  public void \u003CGiveSkin\u003Eb__22_0()
  {
    this.playerFarming.Spine.AnimationState.SetAnimation(0, "idle", true);
    this.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
  }
}
