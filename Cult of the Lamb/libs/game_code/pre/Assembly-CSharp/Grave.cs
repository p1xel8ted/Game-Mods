// Decompiled with JetBrains decompiler
// Type: Grave
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using I2.Loc;
using Lamb.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Grave : Interaction
{
  public static List<Grave> Graves = new List<Grave>();
  public Structure Structure;
  private Structures_Grave _StructureInfo;
  public GameObject EmptyGraveGameObject;
  public GameObject FullGraveGameObject;
  public GameObject Flowers;
  public SpriteXPBar XPBar;
  private string sString;
  public bool PlayerGotBody;
  private bool Activating;
  private float DistanceToTriggerDeposits = 3f;
  private float Delay;

  public StructuresData StructureInfo => this.Structure.Structure_Info;

  public Structures_Grave StructureBrain
  {
    set => this._StructureInfo = value;
    get
    {
      if (this._StructureInfo == null)
        this._StructureInfo = this.Structure.Brain as Structures_Grave;
      return this._StructureInfo;
    }
  }

  public override void OnEnableInteraction()
  {
    this.XPBar.gameObject.SetActive(false);
    Grave.Graves.Add(this);
    this.SetGameObjects();
    this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    StructureManager.OnStructureRemoved += new StructureManager.StructureChanged(this.OnStructureRemoved);
    TimeManager.OnNewPhaseStarted += new System.Action(this.UpdateStructure);
    if (this.StructureInfo == null || FollowerManager.GetDeadFollowerInfoByID(this.StructureInfo.FollowerID) == null || !FollowerManager.GetDeadFollowerInfoByID(this.StructureInfo.FollowerID).HadFuneral)
      return;
    if (this.StructureBrain != null && this.StructureBrain.Data != null && (double) this.StructureBrain.Data.LastPrayTime == -1.0)
    {
      this.StructureBrain.SoulCount = this.StructureBrain.SoulMax;
      this.StructureBrain.Data.LastPrayTime = TimeManager.TotalElapsedGameTime + this.StructureBrain.TimeBetweenSouls;
    }
    this.UpdateBar();
  }

  private void UpdateBar()
  {
    if ((UnityEngine.Object) this.XPBar == (UnityEngine.Object) null || this.StructureBrain == null)
      return;
    this.XPBar.gameObject.SetActive(true);
    this.XPBar.UpdateBar(Mathf.Clamp((float) this.StructureBrain.SoulCount / (float) this.StructureBrain.SoulMax, 0.0f, 1f));
  }

  private void OnBrainAssigned()
  {
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    if (this.Structure.Type == global::StructureBrain.TYPES.GRAVE2)
      this.StructureBrain.UpgradedGrave = true;
    if (this.StructureInfo.FollowerID != -1 && FollowerManager.GetDeadFollowerInfoByID(this.StructureInfo.FollowerID) == null)
      this.StructureInfo.FollowerID = -1;
    if (FollowerManager.GetDeadFollowerInfoByID(this.StructureInfo.FollowerID) != null && FollowerManager.GetDeadFollowerInfoByID(this.StructureInfo.FollowerID).HadFuneral)
    {
      this.StructureBrain.OnSoulsGained += new System.Action<int>(this.OnSoulsGained);
      if ((double) this.StructureBrain.Data.LastPrayTime == -1.0)
      {
        this.StructureBrain.SoulCount = this.StructureBrain.SoulMax;
        this.StructureBrain.Data.LastPrayTime = TimeManager.TotalElapsedGameTime + this.StructureBrain.TimeBetweenSouls;
      }
      this.UpdateBar();
      this.UpdateStructure();
    }
    this.SetGameObjects();
  }

  private void OnSoulsGained(int count) => this.UpdateBar();

  private void OnStructureRemoved(StructuresData structure)
  {
    StructureManager.OnStructureRemoved -= new StructureManager.StructureChanged(this.OnStructureRemoved);
  }

  public override void OnDisableInteraction()
  {
    Grave.Graves.Remove(this);
    StructureManager.OnStructureRemoved -= new StructureManager.StructureChanged(this.OnStructureRemoved);
  }

  private void Start()
  {
    this.SetGameObjects();
    this.UpdateLocalisation();
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sString = ScriptLocalization.Interactions.HereLies;
  }

  public void SetGameObjects()
  {
    if (this.StructureInfo == null)
      return;
    if (this.StructureInfo.FollowerID == -1 || FollowerManager.GetDeadFollowerInfoByID(this.StructureInfo.FollowerID) == null)
    {
      this.EmptyGraveGameObject.SetActive(true);
      this.FullGraveGameObject.SetActive(false);
    }
    else
    {
      this.EmptyGraveGameObject.SetActive(false);
      this.FullGraveGameObject.SetActive(true);
      if (FollowerManager.GetDeadFollowerInfoByID(this.StructureInfo.FollowerID) != null && FollowerManager.GetDeadFollowerInfoByID(this.StructureInfo.FollowerID).HadFuneral)
      {
        if (!this.Flowers.activeSelf)
          this.Flowers.transform.DOPunchScale(Vector3.one * 0.2f, 0.25f);
        this.Flowers.SetActive(true);
      }
      else
        this.Flowers.SetActive(false);
    }
  }

  public override void GetLabel()
  {
    this.Interactable = false;
    if (this.Activating)
      this.Label = string.Empty;
    else if (this.StructureInfo.FollowerID == -1)
    {
      if (PlayerFarming.Instance.state.CURRENT_STATE == StateMachine.State.Idle_CarryingBody || PlayerFarming.Instance.state.CURRENT_STATE == StateMachine.State.Moving_CarryingBody)
      {
        this.Label = ScriptLocalization.Interactions.BuryBody;
        this.PlayerGotBody = true;
      }
      else
      {
        this.Label = string.Empty;
        this.PlayerGotBody = false;
      }
    }
    else
    {
      FollowerInfo followerInfoById = FollowerManager.GetDeadFollowerInfoByID(this.StructureInfo.FollowerID);
      if (followerInfoById != null && followerInfoById.HadFuneral)
      {
        this.XPBar.gameObject.SetActive(true);
        this.Label = $"{ScriptLocalization.Interactions.ReceiveDevotion} {(GameManager.HasUnlockAvailable() ? "<sprite name=\"icon_spirits\">" : "<sprite name=\"icon_blackgold\">")} {(object) this._StructureInfo.SoulCount}{StaticColors.GreyColorHex} / {(object) this.StructureBrain.SoulMax}";
        this.Interactable = true;
      }
      else
        this.Label = this.HereLiesText(followerInfoById);
    }
  }

  public override void OnBecomeCurrent()
  {
    base.OnBecomeCurrent();
    FollowerInfo followerInfoById = FollowerManager.GetDeadFollowerInfoByID(this.StructureInfo.FollowerID);
    if (followerInfoById == null)
    {
      MonoSingleton<Indicator>.Instance.ShowTopInfo(string.Empty);
    }
    else
    {
      if (!followerInfoById.HadFuneral)
        return;
      MonoSingleton<Indicator>.Instance.ShowTopInfo(this.HereLiesText(followerInfoById));
    }
  }

  private string HereLiesText(FollowerInfo f)
  {
    return $"<sprite name=\"img_SwirleyLeft\"> {this.sString} <color=yellow>{f.Name}</color> <sprite name=\"img_SwirleyRight\">";
  }

  public override void OnBecomeNotCurrent()
  {
    base.OnBecomeNotCurrent();
    MonoSingleton<Indicator>.Instance.HideTopInfo();
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (this.StructureBrain.SoulCount > 0)
    {
      if (this.Activating)
        return;
      this.StartCoroutine((IEnumerator) this.GiveReward());
    }
    else
      MonoSingleton<Indicator>.Instance.PlayShake();
  }

  private IEnumerator GiveReward()
  {
    Grave grave = this;
    Debug.Log((object) ("_StructureInfo.SoulCount: " + grave._StructureInfo.SoulCount.ToString().Colour(Color.yellow)));
    grave.Activating = true;
    for (int i = 0; i < grave._StructureInfo.SoulCount; ++i)
    {
      if (GameManager.HasUnlockAvailable())
        SoulCustomTarget.Create(PlayerFarming.Instance.gameObject, grave.transform.position, Color.white, (System.Action) (() => PlayerFarming.Instance.GetSoul(1)));
      else
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1, grave.transform.position + Vector3.back, 0.0f).SetInitialSpeedAndDiraction(8f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
      float num = Mathf.Clamp((float) (grave._StructureInfo.SoulCount - i) / (float) grave.StructureBrain.SoulMax, 0.0f, 1f);
      grave.XPBar.UpdateBar(num);
      yield return (object) new WaitForSeconds(0.1f);
    }
    grave._StructureInfo.SoulCount = 0;
    grave.XPBar.UpdateBar(0.0f);
    grave.Activating = false;
    grave.HasChanged = true;
  }

  private void UpdateStructure()
  {
    if (this.StructureBrain == null || (double) this.StructureBrain.Data.LastPrayTime == -1.0 || (double) TimeManager.TotalElapsedGameTime <= (double) this.StructureBrain.Data.LastPrayTime || this.StructureBrain.SoulCount >= this.StructureBrain.SoulMax)
      return;
    this.HasChanged = true;
    this.StructureBrain.SoulCount = Mathf.Clamp(this.StructureBrain.SoulCount + (1 + Mathf.FloorToInt((TimeManager.TotalElapsedGameTime - this.StructureBrain.Data.LastPrayTime) / this.StructureBrain.TimeBetweenSouls)), 0, this.StructureBrain.SoulMax);
    this.StructureBrain.Data.LastPrayTime = TimeManager.TotalElapsedGameTime + this.StructureBrain.TimeBetweenSouls;
  }

  public void SpawnZombie(FollowerInfo deadBody)
  {
    if (deadBody == null)
      return;
    this.StartCoroutine((IEnumerator) this.SpawnZombieIE(deadBody));
  }

  private IEnumerator SpawnZombieIE(FollowerInfo f)
  {
    yield return (object) new WaitForSeconds(UnityEngine.Random.Range(5f, 10f));
    FollowerBrain resurrectingFollower = FollowerBrain.GetOrCreateBrain(f);
    resurrectingFollower.ResetStats();
    resurrectingFollower.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    resurrectingFollower.Location = FollowerLocation.Base;
    resurrectingFollower.DesiredLocation = FollowerLocation.Base;
    resurrectingFollower.LastPosition = this.StructureInfo.Position;
    resurrectingFollower.CurrentTask.Arrive();
    Follower revivedFollower = FollowerManager.CreateNewFollower(resurrectingFollower._directInfoAccess, this.StructureInfo.Position);
    revivedFollower.State.CURRENT_STATE = StateMachine.State.CustomAnimation;
    revivedFollower.Spine.gameObject.SetActive(false);
    yield return (object) new WaitForSeconds(1f);
    revivedFollower.Spine.gameObject.SetActive(true);
    revivedFollower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ManualControl());
    double num = (double) revivedFollower.SetBodyAnimation("Sermons/sermon-heal", false);
    revivedFollower.AddBodyAnimation("Insane/be-weird", false, 0.0f);
    revivedFollower.AddBodyAnimation("Insane/idle-insane", true, 0.0f);
    this.StructureInfo.FollowerID = -1;
    this.SetGameObjects();
    yield return (object) new WaitForSeconds(4.5f);
    revivedFollower.Brain.ApplyCurseState(Thought.Zombie);
    resurrectingFollower.HardSwapToTask((FollowerTask) new FollowerTask_Zombie());
    yield return (object) new WaitForSeconds(2f);
    if (DataManager.Instance.TryRevealTutorialTopic(TutorialTopic.Zombies))
      MonoSingleton<UIManager>.Instance.ShowTutorialOverlay(TutorialTopic.Zombies);
  }
}
