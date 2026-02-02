// Decompiled with JetBrains decompiler
// Type: PalworldBlacksmith
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using Lamb.UI;
using Lamb.UI.BuildMenu;
using MMTools;
using Spine;
using Spine.Unity;
using src.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class PalworldBlacksmith : MonoBehaviour
{
  [SerializeField]
  public SkeletonAnimation egg;
  [SerializeField]
  public SkeletonAnimation portalSpine;
  [SerializeField]
  public GameObject podium;
  [SerializeField]
  public GameObject purchaseBark;
  [SerializeField]
  public SimpleBark cantAffordBark;
  [SerializeField]
  public Interaction_SimpleConversation[] convos;
  public string followerName;
  public string followerSkin;
  public int followerID;
  public FollowerSpecialType followerSpecial;
  public List<string> skins = new List<string>()
  {
    "PalworldOne",
    "PalworldThree",
    "PalworldFour",
    "PalworldFive"
  };
  public List<StructureBrain.TYPES> decos = new List<StructureBrain.TYPES>()
  {
    StructureBrain.TYPES.DECORATION_PALWORLD_LAMB,
    StructureBrain.TYPES.DECORATION_PALWORLD_LANTERN,
    StructureBrain.TYPES.DECORATION_PALWORLD_PLANT,
    StructureBrain.TYPES.DECORATION_PALWORLD_STATUE,
    StructureBrain.TYPES.DECORATION_PALWORLD_TREE
  };
  public EventInstance receiveLoop;

  public void Awake()
  {
    DataManager.Instance.ForcePalworldEgg = false;
    foreach (Component convo in this.convos)
      convo.gameObject.SetActive(false);
    this.convos[DataManager.Instance.PalworldEggsCollected].gameObject.SetActive(true);
    List<string> stringList = new List<string>();
    for (int index = 0; index < this.skins.Count; ++index)
    {
      if (!DataManager.Instance.PalworldEggSkinsGiven.Contains(this.skins[index]))
        stringList.Add(this.skins[index]);
    }
    this.followerSkin = stringList[UnityEngine.Random.Range(0, stringList.Count)];
    switch (this.followerSkin)
    {
      case "PalworldOne":
        this.followerName = "Chillet";
        this.followerID = 100005;
        this.followerSpecial = FollowerSpecialType.Palworld_Frozen;
        break;
      case "PalworldTwo":
        this.followerName = "Daedream";
        this.followerID = 100001;
        this.followerSpecial = FollowerSpecialType.Palworld_Dark;
        break;
      case "PalworldThree":
        this.followerName = "Depresso";
        this.followerID = 100003;
        this.followerSpecial = FollowerSpecialType.Palworld_Dark;
        break;
      case "PalworldFour":
        this.followerName = "Anubis";
        this.followerID = 100004;
        this.followerSpecial = FollowerSpecialType.Palworld_Rocky;
        break;
      case "PalworldFive":
        this.followerName = "Quivern";
        this.followerID = 100002;
        this.followerSpecial = FollowerSpecialType.Palworld_Dragon;
        break;
    }
    this.egg.Skeleton.SetSkin($"Eggs/{this.followerSpecial}");
    if (this.egg.AnimationState == null)
      return;
    this.egg.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.AnimationState_Event);
  }

  public void OnDestroy()
  {
    if ((UnityEngine.Object) AudioManager.Instance != (UnityEngine.Object) null)
      AudioManager.Instance.StopLoop(this.receiveLoop);
    if (this.egg.AnimationState == null)
      return;
    this.egg.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.AnimationState_Event);
  }

  public void GiveEgg() => this.StartCoroutine((IEnumerator) this.GiveEggIE());

  public IEnumerator GiveEggIE()
  {
    PalworldBlacksmith palworldBlacksmith = this;
    palworldBlacksmith.cantAffordBark.Close();
    palworldBlacksmith.cantAffordBark.gameObject.SetActive(false);
    MMConversation.ClearConversation();
    Inventory.ChangeItemQuantity(InventoryItem.ITEM_TYPE.BLACK_GOLD, -50 * (DataManager.Instance.PalworldEggsCollected + 1));
    FollowerInfo followerInfo = FollowerInfo.NewCharacter(FollowerLocation.Base, palworldBlacksmith.followerSkin);
    followerInfo.TraitsSet = true;
    followerInfo.Traits.Clear();
    followerInfo.ID = palworldBlacksmith.followerID;
    followerInfo.Name = palworldBlacksmith.followerName;
    followerInfo.Special = palworldBlacksmith.followerSpecial;
    DataManager.Instance.Followers_Recruit.Add(followerInfo);
    ++DataManager.Instance.PalworldEggsCollected;
    DataManager.Instance.PalworldEggSkinsGiven.Add(palworldBlacksmith.followerSkin);
    DataManager.Instance.PalworldSkinsGivenLocations.Add(PlayerFarming.Location);
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(palworldBlacksmith.egg.gameObject, 7f);
    for (int i = 0; i < 5; ++i)
    {
      ResourceCustomTarget.Create(palworldBlacksmith.gameObject, PlayerFarming.Instance.transform.position, InventoryItem.ITEM_TYPE.BLACK_GOLD, (System.Action) null);
      yield return (object) new WaitForSeconds(0.2f);
    }
    yield return (object) new WaitForSeconds(1f);
    palworldBlacksmith.podium.transform.DOScale(0.0f, 0.5f);
    palworldBlacksmith.egg.transform.parent = (Transform) null;
    AudioManager.Instance.PlayOneShot("event:/dialogue/followers/positive_acknowledge", palworldBlacksmith.gameObject);
    AudioManager.Instance.PlayOneShot("event:/player/receive_animation_start", PlayerFarming.Instance.gameObject);
    palworldBlacksmith.receiveLoop = AudioManager.Instance.CreateLoop("event:/player/receive_animation_loop", PlayerFarming.Instance.gameObject, true);
    palworldBlacksmith.egg.AnimationState.SetAnimation(0, "Egg/convert", false);
    palworldBlacksmith.portalSpine.gameObject.SetActive(true);
    palworldBlacksmith.portalSpine.AnimationState.SetAnimation(0, "convert-short", false);
    CameraManager.shakeCamera(0.5f, (float) UnityEngine.Random.Range(0, 360));
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    yield return (object) new WaitForSeconds(PlayerFarming.Instance.simpleSpineAnimator.Animate("specials/special-activate-long", 0, true).Animation.Duration - 1f);
    AudioManager.Instance.PlayOneShot("event:/player/receive_animation_end", PlayerFarming.Instance.gameObject);
    int num = (int) palworldBlacksmith.receiveLoop.stop(STOP_MODE.ALLOWFADEOUT);
    palworldBlacksmith.egg.gameObject.SetActive(false);
    GameManager.GetInstance().OnConversationEnd();
    if (DataManager.Instance.PalworldEggsCollected >= 4)
      palworldBlacksmith.convos[palworldBlacksmith.convos.Length - 1].Play();
    else
      palworldBlacksmith.purchaseBark.gameObject.SetActive(true);
  }

  public void AnimationState_Event(TrackEntry trackEntry, Spine.Event e)
  {
    if (!(e.Data.Name == "Audio/egg_bounce"))
      return;
    AudioManager.Instance.PlayOneShot("event:/material/egg_bounce", this.egg.gameObject);
  }

  public void GiveDecos() => this.StartCoroutine((IEnumerator) this.GiveDecosIE());

  public IEnumerator GiveDecosIE()
  {
    PalworldBlacksmith palworldBlacksmith = this;
    List<FoundItemPickUp> d = new List<FoundItemPickUp>();
    for (int i = 0; i < palworldBlacksmith.decos.Count; ++i)
    {
      AudioManager.Instance.PlayOneShot("event:/chests/chest_item_spawn", palworldBlacksmith.transform.position);
      FoundItemPickUp component = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.FOUND_ITEM_DECORATION, 1, palworldBlacksmith.transform.position).GetComponent<FoundItemPickUp>();
      component.DecorationType = palworldBlacksmith.decos[i];
      d.Add(component);
      yield return (object) new WaitForSeconds(0.1f);
    }
    yield return (object) new WaitForSeconds(1f);
    foreach (FoundItemPickUp foundItemPickUp in d)
    {
      FoundItemPickUp deco = foundItemPickUp;
      deco.transform.DOMove(PlayerFarming.Instance.transform.position, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack).OnComplete<TweenerCore<Vector3, Vector3, VectorOptions>>((TweenCallback) (() => UnityEngine.Object.Destroy((UnityEngine.Object) deco.gameObject)));
    }
    yield return (object) new WaitForSeconds(0.5f);
    for (int index = 0; index < d.Count; ++index)
    {
      StructuresData.CompleteResearch(palworldBlacksmith.decos[index]);
      StructuresData.SetRevealed(palworldBlacksmith.decos[index]);
    }
    bool wait = true;
    if (palworldBlacksmith.decos.Count > 0)
    {
      Time.timeScale = 0.0f;
      UIBuildMenuController buildMenuController = MonoSingleton<UIManager>.Instance.BuildMenuTemplate.Instantiate<UIBuildMenuController>();
      buildMenuController.Show(palworldBlacksmith.decos);
      UIBuildMenuController buildMenuController1 = buildMenuController;
      buildMenuController1.OnHidden = buildMenuController1.OnHidden + (System.Action) (() =>
      {
        wait = false;
        buildMenuController = (UIBuildMenuController) null;
      });
      while (wait)
        yield return (object) null;
    }
    Time.timeScale = 1f;
    GameManager.GetInstance().OnConversationEnd();
    palworldBlacksmith.purchaseBark.gameObject.SetActive(true);
  }
}
