// Decompiled with JetBrains decompiler
// Type: StealDevotionShrine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using I2.Loc;
using MMTools;
using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class StealDevotionShrine : MonoBehaviour
{
  public Health Health;
  public SkeletonAnimation Spine;
  public EventInstance shrineRebuildInstanceSFX;
  public bool BossBeatn;
  public bool droppedStone;
  public GameObject VortexObject;
  public GameObject ContainerToHide;

  public void OnEnable()
  {
    this.Health.OnHit += new Health.HitAction(this.OnHit);
    this.Health.OnDie += new Health.DieAction(this.OnDie);
    this.Health.OnPoisonedHit += new Health.HitAction(this.OnHit);
    this.Health.OnBurnHit += new Health.HitAction(this.OnHit);
    LocationManager.OnPlayerLocationSet += new System.Action(this.OnPlayerLocationSet);
    this.Spine.AnimationState.Event += new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
  }

  public void OnDisable()
  {
    this.Health.OnHit -= new Health.HitAction(this.OnHit);
    this.Health.OnPoisonedHit -= new Health.HitAction(this.OnHit);
    this.Health.OnBurnHit -= new Health.HitAction(this.OnHit);
    this.Health.OnDie -= new Health.DieAction(this.OnDie);
    this.Spine.AnimationState.Event -= new Spine.AnimationState.TrackEntryEventDelegate(this.HandleEvent);
    LocationManager.OnPlayerLocationSet -= new System.Action(this.OnPlayerLocationSet);
    AudioManager.Instance.StopOneShotInstanceEarly(this.shrineRebuildInstanceSFX, STOP_MODE.IMMEDIATE);
  }

  public void OnPlayerLocationSet()
  {
    LocationManager.OnPlayerLocationSet -= new System.Action(this.OnPlayerLocationSet);
    this.BossBeatn = DataManager.Instance.DungeonCompleted(PlayerFarming.Location, GameManager.Layer2);
    string str = "";
    switch (PlayerFarming.Location)
    {
      case FollowerLocation.Dungeon1_1:
        str = "1";
        break;
      case FollowerLocation.Dungeon1_2:
        str = "2";
        break;
      case FollowerLocation.Dungeon1_3:
        str = "3";
        break;
      case FollowerLocation.Dungeon1_4:
        str = "4";
        break;
      case FollowerLocation.Dungeon1_5:
        str = "dungeon5";
        break;
      case FollowerLocation.Dungeon1_6:
        str = "dungeon6_1";
        break;
    }
    this.Spine.skeleton.SetSkin(str + (!this.BossBeatn || DungeonSandboxManager.Active ? "" : "_defeated") + (!DataManager.Instance.DeathCatBeaten || DungeonSandboxManager.Active ? "" : "_2"));
    this.Spine.skeleton.SetSlotsToSetupPose();
    this.Spine.AnimationState.Apply(this.Spine.skeleton);
  }

  public void HandleEvent(TrackEntry trackEntry, Spine.Event e)
  {
    if (e.Data.Name == "piece")
    {
      AudioManager.Instance.PlayOneShot("event:/material/stone_impact", this.gameObject);
    }
    else
    {
      if (!(e.Data.Name == "final_piece"))
        return;
      AudioManager.Instance.PlayOneShot("event:/building/finished_stone", this.gameObject);
    }
  }

  public void OnHit(
    GameObject attacker,
    Vector3 attacklocation,
    Health.AttackTypes attacktype,
    bool frombehind)
  {
    int message = Mathf.FloorToInt((float) ((1.0 - (double) this.Health.HP / (double) this.Health.totalHP) * 4.0));
    this.Spine.AnimationState.SetAnimation(0, message.ToString(), false);
    Debug.Log((object) message);
  }

  public void OnDie(
    GameObject attacker,
    Vector3 attacklocation,
    Health victim,
    Health.AttackTypes attacktype,
    Health.AttackFlags attackflags)
  {
    if (DungeonSandboxManager.Active)
      PlayerFarming.ReloadAllFaith();
    if (!this.BossBeatn)
    {
      if (!this.droppedStone)
      {
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.STONE, UnityEngine.Random.Range(1, 3), this.transform.position);
        this.droppedStone = true;
      }
      GameManager.GetInstance().StartCoroutine((IEnumerator) this.ReformRoutine());
    }
    else if (!DungeonSandboxManager.Active)
      this.StartCoroutine((IEnumerator) this.FindLegendaryWeapon((System.Action) (() =>
      {
        this.GetComponent<CircleCollider2D>().enabled = false;
        this.ContainerToHide.SetActive(false);
        this.VortexObject.SetActive(true);
        if (!DataManager.Instance.HealingLeshyQuestActive && !DataManager.Instance.HealingHeketQuestActive && !DataManager.Instance.HealingKallamarQuestActive && !DataManager.Instance.HealingShamuraQuestActive)
          return;
        string str1 = "Leshy";
        string str2 = ScriptLocalization.NAMES_CultLeaders.Dungeon1;
        int num = 99990;
        if (!DataManager.Instance.HealingLeshyQuestActive)
        {
          if (DataManager.Instance.HealingHeketQuestActive)
          {
            str1 = "Heket";
            num = 99991;
            str2 = ScriptLocalization.NAMES_CultLeaders.Dungeon2;
          }
          else if (DataManager.Instance.HealingKallamarQuestActive)
          {
            str1 = "Kallamar";
            num = 99992;
            str2 = ScriptLocalization.NAMES_CultLeaders.Dungeon3;
          }
          else if (DataManager.Instance.HealingShamuraQuestActive)
          {
            str1 = "Shamura";
            num = 99993;
            str2 = ScriptLocalization.NAMES_CultLeaders.Dungeon4;
          }
        }
        this.VortexObject.GetComponent<BossPortalEffect>().PortalInteraction.Interactable = false;
        GameObject gameObject = (GameObject) null;
        foreach (GameObject demon in Demon_Arrows.Demons)
        {
          if (demon.GetComponent<Demon>().FollowerInfo.ID == num)
            gameObject = demon.gameObject;
        }
        List<ConversationEntry> Entries = new List<ConversationEntry>()
        {
          new ConversationEntry(gameObject.gameObject, "Conversation_NPC/NoPortal/" + str1)
        };
        foreach (ConversationEntry conversationEntry in Entries)
        {
          conversationEntry.CharacterName = str2;
          conversationEntry.followerID = num;
        }
        MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) null));
      })));
    this.Spine.AnimationState.SetAnimation(0, "4", false);
    AudioManager.Instance.PlayOneShot("event:/player/Curses/explosive_shot", this.transform.position);
    BiomeConstants.Instance.EmitSmokeExplosionVFX(this.transform.position);
    CameraManager.shakeCamera(5f);
  }

  public IEnumerator ReformRoutine()
  {
    StealDevotionShrine stealDevotionShrine = this;
    yield return (object) stealDevotionShrine.StartCoroutine((IEnumerator) stealDevotionShrine.FindLegendaryWeapon());
    yield return (object) null;
    stealDevotionShrine.Spine.AnimationState.SetAnimation(0, "4", false);
    yield return (object) new WaitForSeconds(2f);
    if (PlayerFarming.Location == FollowerLocation.Dungeon1_6 || PlayerFarming.Location == FollowerLocation.Dungeon1_5)
    {
      AudioManager.Instance.StopOneShotInstanceEarly(stealDevotionShrine.shrineRebuildInstanceSFX, STOP_MODE.IMMEDIATE);
      stealDevotionShrine.shrineRebuildInstanceSFX = AudioManager.Instance.PlayOneShotWithInstanceCleanup("event:/dlc/env/yngya_shrine/dungeon_rebuild", stealDevotionShrine.transform);
    }
    stealDevotionShrine.Spine.AnimationState.SetAnimation(0, "reform", false);
    stealDevotionShrine.Spine.AnimationState.AddAnimation(0, "0", false, 0.0f);
    yield return (object) new WaitForSeconds(6.133333f);
    if ((UnityEngine.Object) stealDevotionShrine.Health != (UnityEngine.Object) null)
    {
      stealDevotionShrine.Health.HP = stealDevotionShrine.Health.totalHP;
      stealDevotionShrine.Health.enabled = true;
    }
  }

  public IEnumerator FindLegendaryWeapon(System.Action callback = null)
  {
    StealDevotionShrine stealDevotionShrine = this;
    if (!DataManager.Instance.OnboardedLegendaryWeapons || DataManager.Instance.LegendaryWeaponsUnlockOrder.Contains(EquipmentType.Sword_Legendary) || PlayerFarming.Location != FollowerLocation.Dungeon1_5 && PlayerFarming.Location != FollowerLocation.Dungeon1_6)
    {
      System.Action action = callback;
      if (action != null)
        action();
    }
    else
    {
      GameManager.GetInstance().OnConversationNew();
      PickUp legendarySword = InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BROKEN_WEAPON_SWORD, 1, stealDevotionShrine.transform.position + Vector3.back, 0.0f);
      Interaction_BrokenWeapon legendarySwordInteraction = legendarySword.GetComponent<Interaction_BrokenWeapon>();
      legendarySwordInteraction.SetWeapon(EquipmentType.Sword_Legendary);
      legendarySword.enabled = false;
      legendarySword.transform.localScale = Vector3.zero;
      legendarySword.transform.DOScale(Vector3.one, 1.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
      GameManager.GetInstance().OnConversationNext(legendarySword.gameObject, 6f);
      AudioManager.Instance.PlayOneShot("event:/player/float_follower", legendarySword.gameObject);
      CameraManager.instance.ShakeCameraForDuration(0.4f, 0.5f, 0.3f);
      yield return (object) new WaitForSeconds(1.5f);
      PlayerSimpleInventory component = PlayerFarming.Instance.state.gameObject.GetComponent<PlayerSimpleInventory>();
      legendarySword.transform.DOMove(new Vector3(component.ItemImage.transform.position.x, component.ItemImage.transform.position.y, -1f), 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutSine);
      yield return (object) new WaitForSeconds(0.25f);
      PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.FoundItem;
      AudioManager.Instance.PlayOneShot("event:/Stings/Choir_mid", stealDevotionShrine.transform.position);
      Inventory.AddItem(InventoryItem.ITEM_TYPE.LEGENDARY_WEAPON_FRAGMENT, 1);
      DataManager.Instance.AddLegendaryWeaponToUnlockQueue(EquipmentType.Sword_Legendary);
      DataManager.Instance.FoundLegendarySword = true;
      legendarySwordInteraction.StartBringWeaponToBlacksmithObjective();
      yield return (object) new WaitForSeconds(1.25f);
      UnityEngine.Object.Destroy((UnityEngine.Object) legendarySword.gameObject);
      GameManager.GetInstance().OnConversationEnd();
      System.Action action = callback;
      if (action != null)
        action();
    }
  }

  [CompilerGenerated]
  public void \u003COnDie\u003Eb__12_0()
  {
    this.GetComponent<CircleCollider2D>().enabled = false;
    this.ContainerToHide.SetActive(false);
    this.VortexObject.SetActive(true);
    if (!DataManager.Instance.HealingLeshyQuestActive && !DataManager.Instance.HealingHeketQuestActive && !DataManager.Instance.HealingKallamarQuestActive && !DataManager.Instance.HealingShamuraQuestActive)
      return;
    string str1 = "Leshy";
    string str2 = ScriptLocalization.NAMES_CultLeaders.Dungeon1;
    int num = 99990;
    if (!DataManager.Instance.HealingLeshyQuestActive)
    {
      if (DataManager.Instance.HealingHeketQuestActive)
      {
        str1 = "Heket";
        num = 99991;
        str2 = ScriptLocalization.NAMES_CultLeaders.Dungeon2;
      }
      else if (DataManager.Instance.HealingKallamarQuestActive)
      {
        str1 = "Kallamar";
        num = 99992;
        str2 = ScriptLocalization.NAMES_CultLeaders.Dungeon3;
      }
      else if (DataManager.Instance.HealingShamuraQuestActive)
      {
        str1 = "Shamura";
        num = 99993;
        str2 = ScriptLocalization.NAMES_CultLeaders.Dungeon4;
      }
    }
    this.VortexObject.GetComponent<BossPortalEffect>().PortalInteraction.Interactable = false;
    GameObject gameObject = (GameObject) null;
    foreach (GameObject demon in Demon_Arrows.Demons)
    {
      if (demon.GetComponent<Demon>().FollowerInfo.ID == num)
        gameObject = demon.gameObject;
    }
    List<ConversationEntry> Entries = new List<ConversationEntry>()
    {
      new ConversationEntry(gameObject.gameObject, "Conversation_NPC/NoPortal/" + str1)
    };
    foreach (ConversationEntry conversationEntry in Entries)
    {
      conversationEntry.CharacterName = str2;
      conversationEntry.followerID = num;
    }
    MMConversation.Play(new ConversationObject(Entries, (List<MMTools.Response>) null, (System.Action) null));
  }
}
