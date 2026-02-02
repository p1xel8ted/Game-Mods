// Decompiled with JetBrains decompiler
// Type: EnemyRoundsBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public abstract class EnemyRoundsBase : BaseMonoBehaviour
{
  public static EnemyRoundsBase Instance;
  public System.Action actionCallback;
  public bool combatBegan;
  [CompilerGenerated]
  public int \u003CTotalRounds\u003Ek__BackingField;
  [CompilerGenerated]
  public int \u003CCurrentRound\u003Ek__BackingField;
  [CompilerGenerated]
  public bool \u003CCompleted\u003Ek__BackingField;
  [CompilerGenerated]
  public float \u003CSpawnDelay\u003Ek__BackingField;
  public bool showRoundsUI = true;
  [SerializeField]
  public SpriteRenderer[] roundIndicators;

  public virtual int TotalRounds => this.\u003CTotalRounds\u003Ek__BackingField;

  public virtual int CurrentRound => this.\u003CCurrentRound\u003Ek__BackingField;

  public bool Completed
  {
    get => this.\u003CCompleted\u003Ek__BackingField;
    set => this.\u003CCompleted\u003Ek__BackingField = value;
  }

  public float SpawnDelay
  {
    get => this.\u003CSpawnDelay\u003Ek__BackingField;
    set => this.\u003CSpawnDelay\u003Ek__BackingField = value;
  }

  public static event EnemyRoundsBase.RoundEvent OnRoundStart;

  public event EnemyRoundsBase.EnemyEvent OnEnemySpawned;

  public virtual void Awake()
  {
  }

  public void OnEnable() => EnemyRoundsBase.Instance = this;

  public void OnDisable()
  {
    if (!((UnityEngine.Object) EnemyRoundsBase.Instance == (UnityEngine.Object) this))
      return;
    EnemyRoundsBase.Instance = (EnemyRoundsBase) null;
  }

  public void BeginCombat() => this.BeginCombat(false, (System.Action) null);

  public virtual void BeginCombat(bool showRoundsUI, System.Action ActionCallback)
  {
    this.showRoundsUI = showRoundsUI;
    this.combatBegan = true;
    this.actionCallback += ActionCallback;
    this.actionCallback += new System.Action(this.CompletedRounds);
    AudioManager.Instance.SetMusicCombatState();
    if (!((UnityEngine.Object) Interaction_Chest.Instance != (UnityEngine.Object) null))
      return;
    GameManager.GetInstance().AddToCamera(Interaction_Chest.Instance.gameObject);
    Interaction_Chest.Instance.Delay = 3f;
  }

  public void RoundStarted(int round, int totalRounds)
  {
    EnemyRoundsBase.RoundEvent onRoundStart = EnemyRoundsBase.OnRoundStart;
    if (onRoundStart != null)
      onRoundStart(round, totalRounds);
    for (int index = 0; index < this.roundIndicators.Length; ++index)
    {
      if (!this.roundIndicators[index].gameObject.activeSelf)
      {
        this.roundIndicators[index].color = new Color(this.roundIndicators[index].color.r, this.roundIndicators[index].color.g, this.roundIndicators[index].color.b, 0.0f);
        this.roundIndicators[index].DOFade(1f, 0.5f);
      }
      this.roundIndicators[index].gameObject.SetActive(index <= round - 1);
    }
  }

  public void CompletedRounds()
  {
    for (int index = 0; index < this.roundIndicators.Length; ++index)
    {
      if (!this.roundIndicators[index].gameObject.activeSelf)
      {
        this.roundIndicators[index].color = new Color(this.roundIndicators[index].color.r, this.roundIndicators[index].color.g, this.roundIndicators[index].color.b, 0.0f);
        this.roundIndicators[index].DOFade(1f, 0.5f);
      }
      this.roundIndicators[index].gameObject.SetActive(true);
    }
    RoomLockController.RoomCompleted(true, false);
    if (!((UnityEngine.Object) Interaction_Chest.Instance != (UnityEngine.Object) null))
      return;
    Interaction_Chest.Instance.Delay = 0.0f;
    GameManager.GetInstance().RemoveFromCamera(this.gameObject);
  }

  public virtual void AddEnemyToRound(Health e)
  {
    if (!(bool) (UnityEngine.Object) e.GetComponent<UnitObject>())
      return;
    EnemyRoundsBase.EnemyEvent onEnemySpawned = this.OnEnemySpawned;
    if (onEnemySpawned == null)
      return;
    onEnemySpawned(e.GetComponent<UnitObject>());
  }

  public delegate void RoundEvent(int currentRound, int maxRounds);

  public delegate void EnemyEvent(UnitObject enemy);
}
