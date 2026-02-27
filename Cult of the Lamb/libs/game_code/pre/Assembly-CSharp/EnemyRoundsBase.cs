// Decompiled with JetBrains decompiler
// Type: EnemyRoundsBase
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using UnityEngine;

#nullable disable
public abstract class EnemyRoundsBase : BaseMonoBehaviour
{
  public static EnemyRoundsBase Instance;
  protected System.Action actionCallback;
  protected bool combatBegan;
  protected bool showRoundsUI = true;
  [SerializeField]
  private SpriteRenderer[] roundIndicators;

  public virtual int TotalRounds { get; }

  public virtual int CurrentRound { get; }

  public bool Completed { get; protected set; }

  public float SpawnDelay { get; set; }

  public static event EnemyRoundsBase.RoundEvent OnRoundStart;

  public event EnemyRoundsBase.EnemyEvent OnEnemySpawned;

  protected virtual void Awake()
  {
  }

  private void OnEnable() => EnemyRoundsBase.Instance = this;

  private void OnDisable()
  {
    if (!((UnityEngine.Object) EnemyRoundsBase.Instance == (UnityEngine.Object) this))
      return;
    EnemyRoundsBase.Instance = (EnemyRoundsBase) null;
  }

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

  private void CompletedRounds()
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
