// Decompiled with JetBrains decompiler
// Type: Interaction_ExhumeSpiritShrine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using Lamb.UI;
using Lamb.UI.FollowerSelect;
using Spine.Unity;
using src.Extensions;
using src.UINavigator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_ExhumeSpiritShrine : Interaction
{
  [SerializeField]
  public SkeletonAnimation spine;
  [SerializeField]
  public Transform lootPosition;
  [SerializeField]
  public GameObject godTearPrefab;
  [SerializeField]
  public GameObject sinPrefab;
  public ParticleSystem summonParticles;
  public ParticleSystem eyeParticles1;
  public ParticleSystem eyeParticles2;
  public ParticleSystem leafParticles;
  public ParticleSystem followerParticles;
  public GameObject redEyes;
  public List<InventoryItem> commonLoot = new List<InventoryItem>()
  {
    new InventoryItem(InventoryItem.ITEM_TYPE.BLACK_GOLD, 10),
    new InventoryItem(InventoryItem.ITEM_TYPE.LOG, 5),
    new InventoryItem(InventoryItem.ITEM_TYPE.STONE, 3),
    new InventoryItem(InventoryItem.ITEM_TYPE.GIFT_SMALL, 1)
  };
  public List<InventoryItem> rareLoot = new List<InventoryItem>()
  {
    new InventoryItem(InventoryItem.ITEM_TYPE.GOD_TEAR, 1),
    new InventoryItem(InventoryItem.ITEM_TYPE.PLEASURE_POINT, 1),
    new InventoryItem(InventoryItem.ITEM_TYPE.GOLD_REFINED, 5),
    new InventoryItem(InventoryItem.ITEM_TYPE.STONE_REFINED, 3),
    new InventoryItem(InventoryItem.ITEM_TYPE.LOG_REFINED, 3),
    new InventoryItem(InventoryItem.ITEM_TYPE.GIFT_MEDIUM, 1)
  };
  public List<FollowerInfo> targetFollowers = new List<FollowerInfo>();
  public Coroutine exhumeRoutine;
  public Coroutine skipRoutine;

  public override void GetLabel()
  {
    base.GetLabel();
    if (DataManager.Instance.Followers_Dead.Count > 0)
      this.Label = LocalizationManager.GetTranslation("Interactions/ReleaseSpirit");
    else
      this.Label = "";
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.StartCoroutine((IEnumerator) this.SacrificeFollowerRoutine());
  }

  public IEnumerator SacrificeFollowerRoutine()
  {
    Interaction_ExhumeSpiritShrine exhumeSpiritShrine = this;
    TimeManager.PauseGameTime = true;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(exhumeSpiritShrine.state.gameObject);
    exhumeSpiritShrine.leafParticles.Play();
    yield return (object) new WaitForSeconds(0.5f);
    int num = 100;
    List<FollowerSelectEntry> followerSelectEntries = new List<FollowerSelectEntry>();
    foreach (FollowerInfo followerInfo in DataManager.Instance.Followers_Dead)
    {
      if (!followerInfo.FrozeToDeath || !StructureManager.HasFollowerDeadWorshipper(followerInfo.ID))
      {
        followerSelectEntries.Add(new FollowerSelectEntry(followerInfo));
        if (followerSelectEntries.Count >= num)
          break;
      }
    }
    UIExhumeSpiritsMenuController followerSelectInstance = MonoSingleton<UIManager>.Instance.ExhumeSpriritMenuTemplate.Instantiate<UIExhumeSpiritsMenuController>();
    MonoSingleton<UINavigatorNew>.Instance.AllowInputOnlyFromPlayer = exhumeSpiritShrine._playerFarming;
    followerSelectInstance.VotingType = TwitchVoting.VotingType.RITUAL_ASCEND;
    followerSelectInstance.Show(followerSelectEntries, followerSelectionType: UpgradeSystem.Type.Ritual_Ascend, showDefaultInfoCard: true);
    followerSelectInstance.OnFollowersChosen += (System.Action<List<FollowerInfo>>) (followerInfos =>
    {
      AudioManager.Instance.PlayOneShot("event:/ritual_sacrifice/select_follower");
      this.exhumeRoutine = this.StartCoroutine((IEnumerator) this.ExhumeSpiritIE(followerInfos));
    });
    UIExhumeSpiritsMenuController spiritsMenuController1 = followerSelectInstance;
    spiritsMenuController1.OnHidden = spiritsMenuController1.OnHidden + (System.Action) (() => followerSelectInstance = (UIExhumeSpiritsMenuController) null);
    UIExhumeSpiritsMenuController spiritsMenuController2 = followerSelectInstance;
    spiritsMenuController2.OnCancel = spiritsMenuController2.OnCancel + (System.Action) (() =>
    {
      GameManager.GetInstance().OnConversationEnd();
      TimeManager.PauseGameTime = false;
    });
  }

  public IEnumerator ExhumeSpiritIE(List<FollowerInfo> infos)
  {
    Interaction_ExhumeSpiritShrine exhumeSpiritShrine = this;
    exhumeSpiritShrine.targetFollowers.Clear();
    exhumeSpiritShrine.targetFollowers.AddRange((IEnumerable<FollowerInfo>) infos);
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = true;
    exhumeSpiritShrine.skipRoutine = exhumeSpiritShrine.StartCoroutine((IEnumerator) exhumeSpiritShrine.WaitForSkip());
    yield return (object) new WaitForSeconds(0.5f);
    List<Structures_Crypt> crypts = StructureManager.GetAllStructuresOfType<Structures_Crypt>();
    List<Structures_Morgue> morgues = StructureManager.GetAllStructuresOfType<Structures_Morgue>();
    foreach (FollowerInfo info in infos)
    {
      FollowerBrain.SetFollowerCostume(exhumeSpiritShrine.spine.Skeleton, info, forceUpdate: true);
      exhumeSpiritShrine.redEyes.SetActive(true);
      exhumeSpiritShrine.eyeParticles1.Play();
      exhumeSpiritShrine.eyeParticles2.Play();
      exhumeSpiritShrine.followerParticles.Play();
      for (int index = DataManager.Instance.Followers_Dead.Count - 1; index >= 0; --index)
      {
        if (DataManager.Instance.Followers_Dead[index].ID == info.ID)
          DataManager.Instance.Followers_Dead.RemoveAt(index);
      }
      DataManager.Instance.Followers_Dead_IDs.Remove(info.ID);
      for (int index = 0; index < crypts.Count; ++index)
      {
        if (crypts[index].Data.MultipleFollowerIDs.Contains(info.ID))
          crypts[index].Data.MultipleFollowerIDs.Remove(info.ID);
      }
      for (int index = 0; index < morgues.Count; ++index)
      {
        if (morgues[index].Data.MultipleFollowerIDs.Contains(info.ID))
          morgues[index].Data.MultipleFollowerIDs.Remove(info.ID);
      }
      exhumeSpiritShrine.summonParticles.Play();
      exhumeSpiritShrine.spine.gameObject.SetActive(true);
      AudioManager.Instance.PlayOneShot("event:/doctrine_stone/doctrine_shake");
      CameraManager.instance.ShakeCameraForDuration(0.5f, 0.5f, 2.35f);
      exhumeSpiritShrine.spine.AnimationState.SetAnimation(0, "fox-sacrifice-shrine", false);
      yield return (object) new WaitForSeconds(2.56f);
      exhumeSpiritShrine.spine.gameObject.SetActive(false);
      yield return (object) exhumeSpiritShrine.StartCoroutine((IEnumerator) exhumeSpiritShrine.GiveLootIE(false));
      exhumeSpiritShrine.targetFollowers.Remove(info);
      exhumeSpiritShrine.redEyes.SetActive(false);
    }
    exhumeSpiritShrine.StopCoroutine(exhumeSpiritShrine.skipRoutine);
    GameManager.GetInstance().OnConversationEnd(false);
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = false;
    TimeManager.PauseGameTime = false;
    exhumeSpiritShrine.OnInteract(exhumeSpiritShrine.state);
  }

  public IEnumerator GiveLootIE(bool wasSkipped)
  {
    Interaction_ExhumeSpiritShrine exhumeSpiritShrine = this;
    yield return (object) null;
    if ((double) UnityEngine.Random.value <= 0.5)
    {
      if ((double) UnityEngine.Random.value > 0.20000000298023224)
      {
        InventoryItem inventoryItem = exhumeSpiritShrine.commonLoot[UnityEngine.Random.Range(0, exhumeSpiritShrine.commonLoot.Count)];
        for (int index = 0; index < inventoryItem.quantity; ++index)
          InventoryItem.Spawn((InventoryItem.ITEM_TYPE) inventoryItem.type, 1, exhumeSpiritShrine.lootPosition.position);
      }
      else
      {
        InventoryItem inventoryItem;
        InventoryItem.ITEM_TYPE type;
        do
        {
          inventoryItem = exhumeSpiritShrine.rareLoot[UnityEngine.Random.Range(0, exhumeSpiritShrine.rareLoot.Count)];
          type = (InventoryItem.ITEM_TYPE) inventoryItem.type;
        }
        while (type == InventoryItem.ITEM_TYPE.GOD_TEAR && !DataManager.Instance.OnboardedGodTear || type == InventoryItem.ITEM_TYPE.PLEASURE_POINT && !DataManager.Instance.PleasureEnabled);
        if (!DataManager.Instance.OnboardedRefinery)
        {
          inventoryItem.type = 44;
          inventoryItem.quantity = 1;
        }
        if (type != InventoryItem.ITEM_TYPE.GOD_TEAR && type != InventoryItem.ITEM_TYPE.PLEASURE_POINT)
        {
          for (int index = 0; index < inventoryItem.quantity; ++index)
            InventoryItem.Spawn((InventoryItem.ITEM_TYPE) inventoryItem.type, 1, exhumeSpiritShrine.lootPosition.position);
        }
        else
        {
          if (!wasSkipped)
            exhumeSpiritShrine.StopCoroutine(exhumeSpiritShrine.skipRoutine);
          LetterBox.Instance.HideSkipPrompt();
          switch (type)
          {
            case InventoryItem.ITEM_TYPE.GOD_TEAR:
              yield return (object) exhumeSpiritShrine.StartCoroutine((IEnumerator) exhumeSpiritShrine.GiveGodTearIE());
              break;
            case InventoryItem.ITEM_TYPE.PLEASURE_POINT:
              yield return (object) exhumeSpiritShrine.StartCoroutine((IEnumerator) exhumeSpiritShrine.GiveSinIE());
              break;
          }
          if (!wasSkipped)
            exhumeSpiritShrine.skipRoutine = exhumeSpiritShrine.StartCoroutine((IEnumerator) exhumeSpiritShrine.WaitForSkip());
        }
      }
    }
  }

  public IEnumerator WaitForSkip()
  {
    Interaction_ExhumeSpiritShrine exhumeSpiritShrine = this;
    LetterBox.Instance.ShowSkipPrompt();
    yield return (object) null;
    while (!InputManager.Gameplay.GetAttackButtonDown(exhumeSpiritShrine._playerFarming))
      yield return (object) null;
    CameraManager.instance.Stopshake();
    exhumeSpiritShrine.StopCoroutine(exhumeSpiritShrine.exhumeRoutine);
    exhumeSpiritShrine.spine.gameObject.SetActive(false);
    List<Structures_Crypt> crypts = StructureManager.GetAllStructuresOfType<Structures_Crypt>();
    List<Structures_Morgue> morgues = StructureManager.GetAllStructuresOfType<Structures_Morgue>();
    for (int i = 0; i < exhumeSpiritShrine.targetFollowers.Count; ++i)
    {
      for (int index = DataManager.Instance.Followers_Dead.Count - 1; index >= 0; --index)
      {
        if (DataManager.Instance.Followers_Dead[index].ID == exhumeSpiritShrine.targetFollowers[i].ID)
          DataManager.Instance.Followers_Dead.RemoveAt(index);
      }
      DataManager.Instance.Followers_Dead_IDs.Remove(exhumeSpiritShrine.targetFollowers[i].ID);
      for (int index = 0; index < crypts.Count; ++index)
      {
        if (crypts[index].Data.MultipleFollowerIDs.Contains(exhumeSpiritShrine.targetFollowers[i].ID))
          crypts[index].Data.MultipleFollowerIDs.Remove(exhumeSpiritShrine.targetFollowers[i].ID);
      }
      for (int index = 0; index < morgues.Count; ++index)
      {
        if (morgues[index].Data.MultipleFollowerIDs.Contains(exhumeSpiritShrine.targetFollowers[i].ID))
          morgues[index].Data.MultipleFollowerIDs.Remove(exhumeSpiritShrine.targetFollowers[i].ID);
      }
      yield return (object) exhumeSpiritShrine.StartCoroutine((IEnumerator) exhumeSpiritShrine.GiveLootIE(true));
    }
    GameManager.GetInstance().OnConversationEnd(false);
    MonoSingleton<UIManager>.Instance.ForceBlockMenus = false;
    exhumeSpiritShrine.OnInteract(exhumeSpiritShrine.state);
  }

  public IEnumerator GiveGodTearIE()
  {
    Interaction_ExhumeSpiritShrine exhumeSpiritShrine = this;
    GameManager.GetInstance().OnConversationNew();
    GameObject godTear = UnityEngine.Object.Instantiate<GameObject>(exhumeSpiritShrine.godTearPrefab, exhumeSpiritShrine.transform.position - Vector3.forward * 2f, Quaternion.identity, exhumeSpiritShrine.transform.parent);
    godTear.transform.localScale = Vector3.zero;
    godTear.transform.DOScale(Vector3.one, 1.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    GameManager.GetInstance().OnConversationNext(godTear, 6f);
    AudioManager.Instance.PlayOneShot("event:/Stings/global_faith_up", godTear);
    AudioManager.Instance.PlayOneShot("event:/Stings/generic_positive", godTear);
    AudioManager.Instance.PlayOneShot("event:/player/float_follower", godTear);
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
    yield return (object) new WaitForSeconds(1.5f);
    PlayerSimpleInventory component = exhumeSpiritShrine.state.gameObject.GetComponent<PlayerSimpleInventory>();
    godTear.transform.DOMove(new Vector3(component.ItemImage.transform.position.x, component.ItemImage.transform.position.y, -1f), 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
    yield return (object) new WaitForSeconds(0.25f);
    exhumeSpiritShrine.playerFarming.state.CURRENT_STATE = StateMachine.State.FoundItem;
    AudioManager.Instance.PlayOneShot("event:/Stings/Choir_mid", exhumeSpiritShrine.transform.position);
    Inventory.AddItem(119, 1);
    yield return (object) new WaitForSeconds(1.25f);
    UnityEngine.Object.Destroy((UnityEngine.Object) godTear.gameObject);
    GameManager.GetInstance().OnConversationEnd(false);
  }

  public IEnumerator GiveSinIE()
  {
    Interaction_ExhumeSpiritShrine exhumeSpiritShrine = this;
    GameManager.GetInstance().OnConversationNew();
    GameObject sin = UnityEngine.Object.Instantiate<GameObject>(exhumeSpiritShrine.sinPrefab, exhumeSpiritShrine.transform.position - Vector3.forward * 2f, Quaternion.identity, exhumeSpiritShrine.transform.parent);
    sin.transform.localScale = Vector3.zero;
    sin.transform.DOScale(Vector3.one, 1.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    GameManager.GetInstance().OnConversationNext(sin, 6f);
    AudioManager.Instance.PlayOneShot("event:/Stings/global_faith_up", sin);
    AudioManager.Instance.PlayOneShot("event:/Stings/generic_positive", sin);
    AudioManager.Instance.PlayOneShot("event:/player/float_follower", sin);
    CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
    yield return (object) new WaitForSeconds(1.5f);
    PlayerSimpleInventory component = exhumeSpiritShrine.state.gameObject.GetComponent<PlayerSimpleInventory>();
    sin.transform.DOMove(new Vector3(component.ItemImage.transform.position.x, component.ItemImage.transform.position.y, -1f), 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
    yield return (object) new WaitForSeconds(0.25f);
    exhumeSpiritShrine.playerFarming.state.CURRENT_STATE = StateMachine.State.FoundItem;
    AudioManager.Instance.PlayOneShot("event:/Stings/Choir_mid", exhumeSpiritShrine.transform.position);
    Inventory.AddItem(154, 1);
    yield return (object) new WaitForSeconds(1.25f);
    UnityEngine.Object.Destroy((UnityEngine.Object) sin.gameObject);
    GameManager.GetInstance().OnConversationEnd(false);
  }
}
