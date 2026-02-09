// Decompiled with JetBrains decompiler
// Type: FishingGUI
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using Fishing;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class FishingGUI : BaseGUI
{
  public const float THROWING_DISTANCE_BAR_FILLING_TIME = 2f;
  public const float THROWING_ENERGY_COST_MODIFICATOR = 2f;
  public const string FISHING_STATE = "fishing_state";
  public const string FISHING_DIST = "fishing_distance";
  public const string START_FISHING_TRIGGER = "start_fishing";
  public const string START_THROWING_TRIGGER = "on_fishing_throw";
  public const string FISH_CATCH_TIME_MLTPLR = "buff_fish_catch_time_mltplr";
  public GameObject bait_choosing_go;
  public GameObject distance_choosing_go;
  public GameObject pulling_go;
  public UIWidget fishing_rod;
  public UIWidget process_back;
  public FishingDistanceChoosingBarGUI distance_choosing_bar;
  public UIProgressBar progress_bar;
  public Transform fish_tf;
  public UILabel bait_name;
  public UILabel bait_decription;
  public BaseItemCellGUI bait_cell_gui;
  public bool taking_out_animation_finished;
  public bool can_take_out;
  public bool is_success_fishing;
  public List<FishWithWeight>[] fishes_with_weights = new List<FishWithWeight>[3];
  public FishingGUI.FishingState _state;
  public float _waiting_for_bite_delay;
  public float _throwing_distance;
  public int _throwing_distance_int;
  public bool _throwing_dist_bar_increasing;
  public List<Item> _avaliable_baits;
  public int _cur_bait_num;
  public FishDefinition _fish_def;
  public Item _fish;
  public FishPreset _fish_preset;
  public float _waiting_for_pulling_time;
  public Item _equipped_fishing_rod;
  public float _screen_k;
  public Transform _fishing_rod_tf;
  public FishingRodLogic _fishing_rod_logic;
  public FishLogic _fish_logic;
  public WorldGameObject _fishing_spot_wgo;
  [CompilerGenerated]
  public ReservoirsDefinition \u003Creservoir_data\u003Ek__BackingField;
  public CustomNetworkAnimatorSync _player_animator;
  public bool is_to_right;
  public UILabel txt_casting_hint;
  public UILabel txt_pull_hint;
  public SimpleUITable bait_text_container;
  public int pull_pos_x_left = -90;
  public int pull_pos_x_right = 90;

  public ReservoirsDefinition reservoir_data
  {
    get => this.\u003Creservoir_data\u003Ek__BackingField;
    set => this.\u003Creservoir_data\u003Ek__BackingField = value;
  }

  public FishingGUI.FishingState state => this._state;

  public override void Init()
  {
    this._fishing_rod_tf = this.fishing_rod.transform;
    base.Init();
  }

  public void Open(WorldGameObject fishing_spot)
  {
    this._equipped_fishing_rod = MainGame.me.player.GetEquippedItem(ItemDefinition.EquipmentType.FishingRod);
    if (this._equipped_fishing_rod == null)
    {
      Debug.LogError((object) "No fishing rod!");
      MainGame.me.player.Say("no_fishing_rod");
    }
    else
    {
      this._fishing_spot_wgo = fishing_spot;
      if ((UnityEngine.Object) this._fishing_spot_wgo == (UnityEngine.Object) null)
      {
        Debug.LogError((object) "Fishing spot is null!");
      }
      else
      {
        this.reservoir_data = GameBalance.me.GetData<ReservoirsDefinition>(this._fishing_spot_wgo.obj_id);
        if (this.reservoir_data == null)
          return;
        this.RecalcAvaliableBaits();
        this.LoadLastBait();
        GUIElements.me.hud.ToolbarSetEnabled(false);
        this.Open(false);
        MainGame.me.player_char.SetAnimationState(CharAnimState.Fishing);
        this._player_animator = MainGame.me.player.components.animator;
        this._player_animator.SetTrigger("start_fishing");
        this._player_animator.SetFloat("fishing_distance", 0.0f);
        this._player_animator.SetInteger("fishing_state", 0);
        GDPoint componentInChildren = this._fishing_spot_wgo.GetComponentInChildren<GDPoint>();
        if ((UnityEngine.Object) componentInChildren != (UnityEngine.Object) null && componentInChildren.direction == Direction.Right)
        {
          MainGame.me.player.gameObject.transform.localScale = new Vector3(-1f, 1f, 1f);
          this.is_to_right = true;
        }
        else
          this.is_to_right = false;
        this.is_success_fishing = false;
        Debug.Log((object) $"#fishing# Started fishing on [{this._fishing_spot_wgo.obj_id}] with rod \"{this._equipped_fishing_rod.id}\", is_to_right = {this.is_to_right.ToString()}");
        this.pulling_go.transform.localPosition = (Vector3) new Vector2(this.is_to_right ? (float) this.pull_pos_x_right : (float) this.pull_pos_x_left, this.pulling_go.transform.localPosition.y);
        this.RedrawSelectedBait();
        this.ChangeState(FishingGUI.FishingState.BaitChoosing);
        this.txt_pull_hint.text = PlatformSpecific.GetInteractionButtonHint(BaseGUI.for_gamepad);
      }
    }
  }

  public static void DrawCanHookFishHint() => MainGame.me.player.DrawFishingPullBubble("pull_fish");

  public void ChangeState(FishingGUI.FishingState target_state)
  {
    Debug.Log((object) $"#fishing# Changing state: \"{this._state.ToString()}\" => \"{target_state.ToString()}\"");
    this._state = target_state;
    this._player_animator.SetInteger("fishing_state", (int) this._state);
    MainGame.me.player.components.interaction.RedrawCurrentInteractiveHint();
    switch (this._state)
    {
      case FishingGUI.FishingState.BaitChoosing:
        this.UpdateFishesWithWeights(false);
        this.distance_choosing_bar.Init();
        this.txt_casting_hint.text = GJL.L("fish_casting_hint", PlatformSpecific.GetInteractionButtonHint(BaseGUI.for_gamepad));
        MainGame.me.player.DrawFishingPullBubble("");
        break;
      case FishingGUI.FishingState.DistanceChoosing:
        this._throwing_distance = 0.0f;
        this._throwing_dist_bar_increasing = true;
        this._player_animator.SetTrigger("on_fishing_throw");
        this.distance_choosing_bar.SetMarkerActive(true);
        this.distance_choosing_bar.SetMarkerPos(0.0f);
        this.txt_casting_hint.text = GJL.L("fish_casting_hint_2", PlatformSpecific.GetInteractionButtonHint(BaseGUI.for_gamepad));
        MainGame.me.player.DrawFishingPullBubble("");
        this.can_take_out = false;
        break;
      case FishingGUI.FishingState.WaitingForBite:
        LazyInput.WaitForRelease(GameKey.MiniGameAction);
        float num = this._equipped_fishing_rod.definition.params_on_use.Get("energy") * -1f;
        if ((double) num > 0.0 && !MainGame.me.player.components.character.player.TrySpendEnergy(num * 2f))
        {
          this._state = FishingGUI.FishingState.None;
          EffectBubblesManager.ShowImmediately(MainGame.me.player.bubble_pos, GJL.L("not_enough_something", "(en)"), EffectBubblesManager.BubbleColor.Energy);
          this.OnPressedBack();
          return;
        }
        FishDefinition randomFish = this.GetRandomFish(out this._waiting_for_bite_delay);
        this.txt_casting_hint.text = "";
        if (randomFish == null)
        {
          this._state = FishingGUI.FishingState.None;
          Debug.Log((object) "#fishing# Nothing to bite here.");
          MainGame.me.player.Say("fishing_something_wrong");
          this.OnPressedBack();
          return;
        }
        MainGame.me.player.DrawFishingPullBubble("");
        this._fish_def = randomFish;
        break;
      case FishingGUI.FishingState.WaitingForPulling:
        LazyInput.WaitForRelease(GameKey.MiniGameAction);
        float f = MainGame.me.player.data.GetParam("buff_fish_catch_time_mltplr");
        this._waiting_for_pulling_time = (double) Mathf.Abs(f) > 0.01 ? this._fish_preset.catch_time * f : this._fish_preset.catch_time;
        this.txt_casting_hint.text = "";
        break;
      case FishingGUI.FishingState.Pulling:
        LazyInput.WaitForRelease(GameKey.MiniGameAction);
        this.txt_casting_hint.text = "";
        this._fish_logic = new FishLogic(this._fish_preset);
        FishingRodPreset preset = Resources.Load<FishingRodPreset>("MiniGames/Fishing/" + this._equipped_fishing_rod.id);
        this._fishing_rod_logic = (UnityEngine.Object) preset != (UnityEngine.Object) null ? new FishingRodLogic(preset) : (FishingRodLogic) null;
        if (this._fishing_rod_logic != null)
        {
          this._screen_k = (float) this.process_back.height / 100f;
          this.fishing_rod.height = Mathf.RoundToInt(this._fishing_rod_logic.rect_size * this._screen_k);
        }
        this._fishing_rod_tf.localPosition = Vector3.zero;
        this.UpdatePulling();
        Sounds.PlaySound("fishing_reel_long");
        MainGame.me.player.DrawFishingPullBubble("bump_fish");
        break;
      case FishingGUI.FishingState.TakingOut:
        this.StopReelSound();
        this.txt_casting_hint.text = "";
        MainGame.me.player.DrawFishingPullBubble("");
        this.taking_out_animation_finished = false;
        if (this._cur_bait_num != -1)
        {
          this.SaveLastBait();
          this.RemoveBait(this._avaliable_baits[this._cur_bait_num]);
        }
        if (this.is_success_fishing)
        {
          MainGame.me.player_char.player.fish.sprite = EasySpritesCollection.GetSprite(this._fish.GetIcon());
          MainGame.me.player_char.player.fishadow.sprite = EasySpritesCollection.GetSprite("fish_shadow");
          break;
        }
        MainGame.me.player_char.player.fish.sprite = (UnityEngine.Sprite) null;
        MainGame.me.player_char.player.fishadow.sprite = (UnityEngine.Sprite) null;
        break;
    }
    this.distance_choosing_go.SetActive(this._state == FishingGUI.FishingState.DistanceChoosing || this._state == FishingGUI.FishingState.BaitChoosing);
    this.pulling_go.SetActive(this._state == FishingGUI.FishingState.Pulling);
    this.bait_choosing_go.SetActive(this._state == FishingGUI.FishingState.BaitChoosing);
  }

  public void StopReelSound() => DarkTonic.MasterAudio.MasterAudio.StopAllOfSound("fishing_reel_long");

  public override void Update()
  {
    base.Update();
    switch (this._state)
    {
      case FishingGUI.FishingState.BaitChoosing:
        this.UpdateBaitChoosing();
        break;
      case FishingGUI.FishingState.DistanceChoosing:
        this.UpdateDistanceChoosing();
        break;
      case FishingGUI.FishingState.WaitingForBite:
        this.UpdateWaitingForBite();
        break;
      case FishingGUI.FishingState.WaitingForPulling:
        this.UpdateWaitingForPulling();
        break;
      case FishingGUI.FishingState.Pulling:
        this.UpdatePulling();
        break;
      case FishingGUI.FishingState.TakingOut:
        this.UpdateTakingOut();
        break;
    }
  }

  public void UpdateBaitChoosing()
  {
    if (LazyInput.GetKeyDown(GameKey.NextTab))
      this.OnNextBait();
    if (LazyInput.GetKeyDown(GameKey.PrevTab))
      this.OnPrevBait();
    if (!LazyInput.GetKeyDown(GameKey.Interaction))
      return;
    this.ChangeState(FishingGUI.FishingState.DistanceChoosing);
  }

  public void UpdateDistanceChoosing()
  {
    if (LazyInput.GetKey(GameKey.Interaction))
    {
      if (this._throwing_dist_bar_increasing)
        this._throwing_distance += Time.deltaTime / 2f;
      else
        this._throwing_distance -= Time.deltaTime / 2f;
      if ((double) this._throwing_distance > 1.0)
      {
        this._throwing_dist_bar_increasing = false;
        this._throwing_distance = 1f;
      }
      else if ((double) this._throwing_distance < 0.0)
      {
        this._throwing_dist_bar_increasing = true;
        this._throwing_distance = 0.0f;
      }
      this.distance_choosing_bar.SetMarkerPos(this._throwing_distance);
    }
    else
    {
      int index = Mathf.CeilToInt(this._throwing_distance * 3f) - 1;
      if (index < 0)
        index = 0;
      if (index > 2)
        index = 2;
      if (this.reservoir_data.dist_avaliables[index] && this.fishes_with_weights[index].Count > 0)
      {
        Debug.Log((object) ("#fishing# Choosed throwing distance: " + (index + 1).ToString()));
        this._throwing_distance_int = index + 1;
        this._player_animator.SetFloat("fishing_distance", (float) this._throwing_distance_int);
        this.ChangeState(FishingGUI.FishingState.WaitingForBite);
      }
      else
      {
        Debug.Log((object) ("#fishing# Can not throw to dist " + (index + 1).ToString()));
        this.ChangeState(FishingGUI.FishingState.BaitChoosing);
      }
    }
  }

  public void UpdateWaitingForBite()
  {
    if (!this.can_take_out)
      return;
    if (LazyInput.GetKeyDown(GameKey.MiniGameAction))
    {
      this.is_success_fishing = false;
      this.ChangeState(FishingGUI.FishingState.TakingOut);
    }
    else
    {
      this._waiting_for_bite_delay -= Time.deltaTime;
      if ((double) this._waiting_for_bite_delay > 0.0)
        return;
      this._fish = new Item(this._fish_def.item_id, 1);
      this._fish_preset = Resources.Load<FishPreset>("MiniGames/Fishing/" + this._fish_def.fish_preset);
      if (!((UnityEngine.Object) this._fish_preset != (UnityEngine.Object) null))
        return;
      this.ChangeState(FishingGUI.FishingState.WaitingForPulling);
    }
  }

  public void UpdateFishesWithWeights(bool update_gui = true)
  {
    for (int index = 0; index < 3; ++index)
      this.fishes_with_weights[index] = new List<FishWithWeight>();
    bool isNight = TimeOfDay.me.is_night;
    string objId = this._fishing_spot_wgo.obj_id;
    string bait = this._cur_bait_num == -1 ? string.Empty : this._avaliable_baits[this._cur_bait_num].id;
    if (!this._equipped_fishing_rod.id.StartsWith("fishing_rod_"))
      Debug.LogError((object) $"Wrong equipped fishing rod: \"{this._equipped_fishing_rod.id}\"");
    string s = this._equipped_fishing_rod.id.Split(new string[1]
    {
      "fishing_rod_"
    }, StringSplitOptions.None)[1];
    int result;
    if (!int.TryParse(s, out result))
    {
      Debug.LogError((object) $"Wrong equipped fishing rod: can not parse rod_lvl \"{s}\"");
    }
    else
    {
      foreach (FishDefinition fishDefinition in GameBalance.me.fishes_data)
      {
        for (int index1 = 0; index1 < 3; ++index1)
        {
          if (this.reservoir_data.dist_avaliables[index1])
          {
            float totalWeight = fishDefinition.GetTotalWeight(objId, isNight, index1 + 1, result, bait);
            bool flag = false;
            if ((double) totalWeight < 0.0099999997764825821)
            {
              float num = 0.0f;
              bool[] flagArray = new bool[2]{ true, false };
              foreach (bool is_night in flagArray)
              {
                for (int rod_lvl = 0; rod_lvl < 3; ++rod_lvl)
                {
                  num += fishDefinition.GetTotalWeight(objId, is_night, index1 + 1, rod_lvl, string.Empty);
                  if ((double) num <= 0.0)
                  {
                    for (int index2 = 0; index2 < GameBalance.me.items_data.Count; ++index2)
                    {
                      ItemDefinition itemDefinition = GameBalance.me.items_data[index2];
                      if (itemDefinition.type == ItemDefinition.ItemType.Bait)
                      {
                        num += fishDefinition.GetTotalWeight(objId, is_night, index1 + 1, rod_lvl, itemDefinition.id);
                        if ((double) num > 0.0)
                          break;
                      }
                    }
                    if ((double) num > 0.0)
                      break;
                  }
                  else
                    break;
                }
                if ((double) num > 0.0)
                  break;
              }
              if ((double) num > 0.0)
                flag = true;
            }
            else
              flag = true;
            if (flag)
              this.fishes_with_weights[index1].Add(new FishWithWeight()
              {
                fish = fishDefinition,
                weight = totalWeight
              });
          }
        }
      }
      if (!update_gui)
        return;
      this.distance_choosing_bar.UpdateBar();
    }
  }

  public FishDefinition GetRandomFish(out float waiting_time)
  {
    this.UpdateFishesWithWeights(false);
    waiting_time = 0.0f;
    float maxInclusive = 0.0f;
    string str1 = this._cur_bait_num == -1 ? string.Empty : this._avaliable_baits[this._cur_bait_num].id;
    List<FishWithWeight> fishesWithWeight = this.fishes_with_weights[this._throwing_distance_int - 1];
    if (fishesWithWeight.Count == 0)
      return (FishDefinition) null;
    bool flag = false;
    string str2 = string.Empty;
    foreach (FishWithWeight fishWithWeight in fishesWithWeight)
    {
      maxInclusive += fishWithWeight.weight;
      if ((double) fishWithWeight.weight > 0.0)
      {
        flag = true;
        str2 = str2 + (string.IsNullOrEmpty(str2) ? "" : ", ") + fishWithWeight.fish.id;
      }
    }
    if (!flag)
      return (FishDefinition) null;
    FishDefinition randomFish = (FishDefinition) null;
    float num1 = UnityEngine.Random.Range(0.0f, maxInclusive);
    float num2 = 0.0f;
    foreach (FishWithWeight fishWithWeight in fishesWithWeight)
    {
      num2 += fishWithWeight.weight;
      if ((double) num2 > (double) num1)
      {
        randomFish = fishWithWeight.fish;
        break;
      }
    }
    if (randomFish == null)
    {
      Debug.LogError((object) "FATAL ERROR while fishing! random_fish not found");
      return (FishDefinition) null;
    }
    if (string.IsNullOrEmpty(str1))
    {
      waiting_time = randomFish.no_bait_mod.wait_time;
    }
    else
    {
      waiting_time = randomFish.no_bait_mod.wait_time;
      foreach (FishDefinition.BaitData baitData in randomFish.baits_mod)
      {
        if (baitData.bait_name == str1)
        {
          waiting_time = baitData.wait_time;
          break;
        }
      }
    }
    waiting_time *= UnityEngine.Random.Range(0.9f, 1.1f);
    Debug.Log((object) $"#fishing# Total_weight={maxInclusive.ToString()}, rand={num1.ToString()} => fish={randomFish.id}, waiting_time={waiting_time.ToString()}\n Available fishes: {{{str2}}}");
    return randomFish;
  }

  public void UpdateWaitingForPulling()
  {
    if ((UnityEngine.Object) this._fish_preset == (UnityEngine.Object) null)
      return;
    if (LazyInput.GetKeyDown(GameKey.MiniGameAction))
      this.ChangeState(FishingGUI.FishingState.Pulling);
    this._waiting_for_pulling_time -= Time.deltaTime;
    if ((double) this._waiting_for_pulling_time >= 0.0)
      return;
    this.ChangeState(FishingGUI.FishingState.WaitingForBite);
  }

  public void UpdatePulling()
  {
    if (this._fishing_rod_logic == null)
      return;
    float rodPos = this._fishing_rod_logic.CalculateRodPos();
    FishLogic.Result fishPos = this._fish_logic.CalculateFishPos(rodPos, this._fishing_rod_logic.rect_size);
    this.progress_bar.value = fishPos.progress;
    this._fishing_rod_tf.localPosition = Vector3.up * (rodPos * this._screen_k);
    this.fish_tf.localPosition = Vector3.up * (fishPos.fish_pos * this._screen_k);
    if (!fishPos.success && !fishPos.fail)
      return;
    this.is_success_fishing = fishPos.success;
    this.ChangeState(FishingGUI.FishingState.TakingOut);
  }

  public void UpdateTakingOut()
  {
    if (!this.taking_out_animation_finished)
      return;
    this.OnPressedBack();
    MainGame.me.player.components.interaction.UpdateNearestHint();
    if (!this.is_success_fishing)
      return;
    MainGame.me.save.achievements.CheckKeyQuests("fishing_success");
    if (MainGame.me.player.AddToInventory(this._fish))
    {
      DropCollectGUI.OnDropCollected(this._fish);
      Sounds.PlaySound("pickup");
    }
    else
      MainGame.me.player.DropItem(this._fish);
    string withoutQualitySuffix = this._fish.definition.GetNameWithoutQualitySuffix();
    string str = $"{this.reservoir_data.id}:{this._throwing_distance_int.ToString()}:{withoutQualitySuffix}";
    Stats.DesignEvent($"Fishing:{(this.reservoir_data == null ? "NULL" : this.reservoir_data.id)}:{(this._fish == null ? "NULL" : this._fish.id)}");
    if (!MainGame.me.save.known_fishes_clear.Contains(withoutQualitySuffix))
    {
      MainGame.me.save.known_fishes_clear.Add(withoutQualitySuffix);
      MainGame.me.save.achievements.CheckKeyQuests("new_fish");
      MainGame.me.save.achievements.CheckKeyQuests("new_fish_" + withoutQualitySuffix);
    }
    if (MainGame.me.save.known_fishes.Contains(str))
      return;
    MainGame.me.save.known_fishes.Add(str);
    CraftDefinition craftDefinition1 = GameBalance.me.GetDataOrNull<CraftDefinition>(withoutQualitySuffix == "fish_frog_green" ? "raw_meat_sliced_from_fish_frog_green" : withoutQualitySuffix + "_fillet");
    if (craftDefinition1 == null)
    {
      foreach (CraftDefinition craftDefinition2 in GameBalance.me.craft_data)
      {
        bool flag1 = false;
        foreach (Item need in craftDefinition2.needs)
        {
          if (need.id == withoutQualitySuffix)
            flag1 = true;
        }
        if (flag1)
        {
          bool flag2 = false;
          foreach (Item obj in craftDefinition2.output)
          {
            if (obj.id.Contains("fillet_fish"))
            {
              craftDefinition1 = craftDefinition2;
              flag2 = true;
              break;
            }
          }
          if (flag2)
            break;
        }
      }
    }
    if (craftDefinition1 == null)
      return;
    MainGame.me.save.unlocked_crafts.Add(craftDefinition1.id);
    MainGame.me.save.unlocked_crafts.Add("t_" + craftDefinition1.id);
  }

  public override void Hide(bool play_hide_sound = true)
  {
    this.StopReelSound();
    MainGame.me.player.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
    MainGame.me.player_char.SetAnimationState(CharAnimState.Idle);
    MainGame.me.player.DrawFishingPullBubble(string.Empty);
    this._state = FishingGUI.FishingState.None;
    GUIElements.me.hud.ToolbarSetEnabled();
    GS.SetPlayerEnable(true, false);
    base.Hide(false);
  }

  public override bool OnPressedBack()
  {
    MainGame.me.player.DrawFishingPullBubble(string.Empty);
    this.OnClosePressed();
    return true;
  }

  public void OnPrevBait()
  {
    if (this._avaliable_baits == null)
      this._avaliable_baits = new List<Item>();
    if (this._avaliable_baits.Count == 0)
    {
      this._cur_bait_num = -1;
    }
    else
    {
      if (this._cur_bait_num == -1)
        this._cur_bait_num += this._avaliable_baits.Count;
      else
        --this._cur_bait_num;
      this.RedrawSelectedBait();
      this.UpdateFishesWithWeights();
    }
  }

  public void OnNextBait()
  {
    if (this._avaliable_baits == null)
      this._avaliable_baits = new List<Item>();
    if (this._avaliable_baits.Count == 0)
    {
      this._cur_bait_num = -1;
    }
    else
    {
      if (this._cur_bait_num == this._avaliable_baits.Count - 1)
        this._cur_bait_num = -1;
      else
        ++this._cur_bait_num;
      this.RedrawSelectedBait();
      this.UpdateFishesWithWeights();
    }
  }

  public void RedrawSelectedBait()
  {
    Item avaliableBait = this._cur_bait_num == -1 ? (Item) null : this._avaliable_baits[this._cur_bait_num];
    this.bait_name.text = GJL.L(avaliableBait == null ? "no_bait" : avaliableBait.definition.GetItemName());
    this.bait_decription.text = GJL.L(avaliableBait == null ? "no_bait_descr" : avaliableBait.definition.GetItemDescription());
    this.bait_cell_gui.DrawItem(avaliableBait ?? Item.empty);
    this.bait_text_container.Reposition();
    Debug.Log((object) $"#fishing# Changed bait to \"{(this._cur_bait_num == -1 ? "no_bate" : this._avaliable_baits[this._cur_bait_num].id)}\"");
  }

  public void RemoveBait(Item bait)
  {
    if (this._avaliable_baits == null)
      this.RecalcAvaliableBaits();
    if (this._avaliable_baits.Count == 0)
      Debug.LogError((object) $"FISHING ERROR: avaliable baits list is empty! Can not remove bait \"{bait.id}\"");
    else if (this._avaliable_baits.Contains(bait))
    {
      if (bait.definition.has_durability && (double) bait.durability > (double) bait.definition.durability_decrease_on_use_speed)
      {
        bait.durability -= bait.definition.durability_decrease_on_use_speed;
        Debug.Log((object) $"#fishing# Removed {bait.definition.durability_decrease_on_use_speed.ToString()} durability from bait \"{bait.id}\" from player inventory.");
      }
      else
      {
        MainGame.me.player.data.RemoveItem(bait, 1);
        this.RecalcAvaliableBaits();
        this._cur_bait_num = -1;
        Debug.Log((object) $"#fishing# Removed bait \"{bait.id}\" from player inventory. New bait: {(this._cur_bait_num == -1 ? "No_Bait" : this._avaliable_baits[this._cur_bait_num].id)}");
      }
    }
    else
      Debug.LogError((object) $"FISHING ERROR: Can not remove bait: _avaliable_baits not contains \"{bait.id}\"");
  }

  public void RecalcAvaliableBaits()
  {
    this._avaliable_baits = new List<Item>();
    string str = string.Empty;
    foreach (Item obj in MainGame.me.player.data.inventory)
    {
      if (obj.definition.type == ItemDefinition.ItemType.Bait)
      {
        str = str + (this._avaliable_baits.Count == 0 ? "" : ", ") + obj.id;
        this._avaliable_baits.Add(obj);
      }
    }
    this._cur_bait_num = -1;
    Debug.Log((object) $"#fishing# Recalculated avaliable baits: count={this._avaliable_baits.Count.ToString()}{(this._avaliable_baits.Count > 0 ? $"\n{{{str}}}" : "")}");
  }

  public void SaveLastBait()
  {
    if (MainGame.me.save.last_bait_baits == null || MainGame.me.save.last_bait_reservoirs == null || MainGame.me.save.last_bait_baits.Count != MainGame.me.save.last_bait_reservoirs.Count)
    {
      Debug.LogError((object) "FATAL ERROR! CALL BULAT! <= SaveLastBait");
      MainGame.me.save.last_bait_baits = new List<string>();
      MainGame.me.save.last_bait_reservoirs = new List<string>();
    }
    int index1 = -1;
    for (int index2 = 0; index2 < MainGame.me.save.last_bait_reservoirs.Count; ++index2)
    {
      if (MainGame.me.save.last_bait_reservoirs[index2] == this.reservoir_data.id)
      {
        index1 = index2;
        break;
      }
    }
    if (index1 == -1)
    {
      MainGame.me.save.last_bait_baits.Add(string.Empty);
      MainGame.me.save.last_bait_reservoirs.Add(this.reservoir_data.id);
      index1 = MainGame.me.save.last_bait_reservoirs.Count - 1;
    }
    MainGame.me.save.last_bait_baits[index1] = this._cur_bait_num >= 0 ? this._avaliable_baits[this._cur_bait_num].id : string.Empty;
  }

  public void LoadLastBait()
  {
    if (MainGame.me.save.last_bait_baits == null || MainGame.me.save.last_bait_reservoirs == null || MainGame.me.save.last_bait_baits.Count != MainGame.me.save.last_bait_reservoirs.Count)
    {
      Debug.LogError((object) "FATAL ERROR! CALL BULAT! <= LoadLastBait");
      MainGame.me.save.last_bait_baits = new List<string>();
      MainGame.me.save.last_bait_reservoirs = new List<string>();
    }
    else
    {
      int index1 = -1;
      for (int index2 = 0; index2 < MainGame.me.save.last_bait_reservoirs.Count; ++index2)
      {
        if (MainGame.me.save.last_bait_reservoirs[index2] == this.reservoir_data.id)
        {
          index1 = index2;
          break;
        }
      }
      if (index1 == -1)
        return;
      string lastBaitBait = MainGame.me.save.last_bait_baits[index1];
      for (int index3 = 0; index3 < this._avaliable_baits.Count; ++index3)
      {
        if (this._avaliable_baits[index3].id == lastBaitBait)
        {
          this._cur_bait_num = index3;
          break;
        }
      }
    }
  }

  public enum FishingState
  {
    None,
    BaitChoosing,
    DistanceChoosing,
    WaitingForBite,
    WaitingForPulling,
    Pulling,
    TakingOut,
  }
}
