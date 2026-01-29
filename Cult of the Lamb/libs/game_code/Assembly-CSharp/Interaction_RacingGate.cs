// Decompiled with JetBrains decompiler
// Type: Interaction_RacingGate
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using FMOD.Studio;
using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_RacingGate : Interaction
{
  public const int RACE_GATES_TO_PASS = 5;
  public static System.Action OnStartRace;
  public static System.Action<int> OnGatePassed;
  public static System.Action<float> OnFinishRace;
  public static List<Interaction_RacingGate> RacingGates = new List<Interaction_RacingGate>();
  public static HashSet<Interaction_RacingGate> FinishedRaceGatesLamb = new HashSet<Interaction_RacingGate>();
  public static HashSet<Interaction_RacingGate> FinishedRaceGatesGoat = new HashSet<Interaction_RacingGate>();
  public static float raceTimer = 0.0f;
  public static bool isRaceActive = false;
  public Vector3 startPos;
  public static int raceTicker = 0;
  public static EventInstance raceLoopInstance;
  public string raceLoopSFX = "event:/dlc/animal/shared/race_amb_loop";
  public StructureBrain _StructureInfo;
  public Structure Structure;
  [SerializeField]
  public GameObject front;
  [SerializeField]
  public GameObject[] startFlags;
  [SerializeField]
  public GameObject[] endFlags;
  [SerializeField]
  public Sprite defaultSprite;
  [SerializeField]
  public Sprite lambSprite;
  [SerializeField]
  public Sprite goatSprite;
  [SerializeField]
  public SpriteRenderer[] winnerIcons;
  [SerializeField]
  public GameObject confettiLamb;
  [SerializeField]
  public GameObject confettiGoat;
  [SerializeField]
  public GameObject confettiFinish;
  public PlacementObject placementObject;
  public Coroutine coroutine;

  public static bool CanRace => Interaction_RacingGate.RacingGates.Count >= 5;

  public static float RaceTimer => Mathf.Clamp(Interaction_RacingGate.raceTimer, 0.0f, 9999.99f);

  public static bool IsRaceActive => Interaction_RacingGate.isRaceActive;

  public StructureBrain Brain
  {
    get
    {
      if (this._StructureInfo == null)
        this._StructureInfo = this.Structure.Brain;
      return this._StructureInfo;
    }
    set => this._StructureInfo = value;
  }

  public GateType gateType
  {
    get => this.Brain != null ? this.Brain.Data.GateType : GateType.Default;
    set => this.Brain.Data.GateType = value;
  }

  public void Awake()
  {
    this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    if (this.Brain != null)
      this.OnBrainAssigned();
    foreach (SpriteRenderer winnerIcon in this.winnerIcons)
      winnerIcon.sprite = this.defaultSprite;
    this.placementObject = this.GetComponentInParent<PlacementObject>();
  }

  public override void OnEnable()
  {
    base.OnEnable();
    Interaction_RacingGate.RacingGates.Add(this);
  }

  public override void OnDisable()
  {
    base.OnDisable();
    Interaction_RacingGate.RacingGates.Remove(this);
    this.FinishRace(PlayerFarming.Instance);
    Interaction_RacingGate.raceTimer = 0.0f;
    Interaction_RacingGate.isRaceActive = false;
    Interaction_RacingGate.FinishedRaceGatesLamb.Clear();
    Interaction_RacingGate.FinishedRaceGatesGoat.Clear();
  }

  public override void OnDestroy()
  {
    base.OnDestroy();
    Interaction_RacingGate.RacingGates.Remove(this);
    this.FinishRace(PlayerFarming.Instance);
    Interaction_RacingGate.raceTimer = 0.0f;
    Interaction_RacingGate.isRaceActive = false;
    if ((UnityEngine.Object) this.Structure != (UnityEngine.Object) null)
      this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    if (this.Brain == null || !this.Brain.Data.Destroyed || this.Brain.Data.Type != StructureBrain.TYPES.RACING_GATE || Interaction_RacingGate.CanRace)
      return;
    foreach (Interaction_RacingGate racingGate in Interaction_RacingGate.RacingGates)
    {
      if (racingGate.gateType == GateType.Start || racingGate.gateType == GateType.End)
      {
        racingGate.gateType = GateType.Default;
        racingGate.SetFlags(racingGate.gateType);
      }
    }
  }

  public void OnBrainAssigned()
  {
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    this.SetFlags(this.gateType);
    this.startPos = new Vector3(this.Brain.Data.Position.x + this.Brain.Data.Offset.x, this.Brain.Data.Position.y + this.Brain.Data.Offset.y, Mathf.Min(0.0f, this.Brain.Data.Position.z));
    this.transform.position = new Vector3(this.startPos.x, this.startPos.y, this.startPos.z - UnityEngine.Random.Range(-0.05f, 0.05f));
  }

  public override void Update()
  {
    base.Update();
    this.HandleRaceTimer();
    if (this.gateType != GateType.Start || !Interaction_RacingGate.isRaceActive)
      return;
    for (int index = 0; index < PlayerFarming.players.Count && !PlayerFarming.players[index].IsRidingAnimal(); ++index)
      this.FinishRace(PlayerFarming.players[index]);
  }

  public override void GetLabel()
  {
    this.GetSecondaryLabel();
    if (!Interaction_RacingGate.CanRace)
    {
      this.Label = string.Format(LocalizationManager.GetTranslation("Interactions/MissingGates"), (object) Interaction_RacingGate.RacingGates.Count, (object) 5);
      this.Interactable = false;
    }
    else
    {
      if (this.gateType == GateType.Start)
        this.Label = ScriptLocalization.Interactions.RemoveStart;
      else if (this.gateType == GateType.End)
        this.Label = ScriptLocalization.Interactions.RemoveFinish;
      else if (!Interaction_RacingGate.IsAnyGateType(GateType.Start))
        this.Label = ScriptLocalization.Interactions.SetStart;
      else if (!Interaction_RacingGate.IsAnyGateType(GateType.End))
        this.Label = ScriptLocalization.Interactions.SetFinish;
      else
        this.Label = "";
      this.Interactable = true;
    }
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    if (this.gateType == GateType.Start || this.gateType == GateType.End)
    {
      this.gateType = GateType.Default;
      AudioManager.Instance.PlayOneShot("event:/dlc/animal/shared/race_line_remove", this.transform.position);
    }
    else if (!Interaction_RacingGate.IsAnyGateType(GateType.Start))
    {
      this.gateType = GateType.Start;
      AudioManager.Instance.PlayOneShot("event:/dlc/animal/shared/race_line_set", this.transform.position);
    }
    else if (!Interaction_RacingGate.IsAnyGateType(GateType.End))
    {
      this.gateType = GateType.End;
      AudioManager.Instance.PlayOneShot("event:/dlc/animal/shared/race_line_set", this.transform.position);
    }
    this.SetFlags(this.gateType);
    this.HasChanged = true;
  }

  public void OnTriggerEnter2D(Collider2D collision)
  {
    PlayerFarming component = collision.gameObject.GetComponent<PlayerFarming>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || !component.IsRidingAnimal())
      return;
    this.TryStartRace();
    if (Interaction_RacingGate.isRaceActive && !this.GetFinishedRacingGates(component).Contains(this) && (this.gateType != GateType.End || this.GetFinishedRacingGates(component).Count >= Interaction_RacingGate.RacingGates.Count - 1))
    {
      foreach (SpriteRenderer winnerIcon in this.winnerIcons)
        winnerIcon.sprite = !component.isLamb || component.IsGoat ? this.goatSprite : this.lambSprite;
      ObjectPool.Spawn(!component.isLamb || component.IsGoat ? this.confettiGoat : this.confettiLamb, this.transform.parent, this.transform.position + Vector3.back * 1.5f + Vector3.up);
      AudioManager.Instance.PlayOneShot("event:/dlc/animal/shared/race_gate_passthrough", this.transform.position);
      if (this.gateType == GateType.Default)
        this.front.transform.DOShakeScale(0.3f, 0.2f);
      else
        this.front.transform.DOShakeScale(0.5f, 0.4f);
      Interaction_RacingGate.OnGatePassed(this.GetFinishedRacingGates(component).Count);
      this.GetFinishedRacingGates(component).Add(this);
      this.FinishRace(component, false);
    }
    this.coroutine = this.StartCoroutine((IEnumerator) this.DelayIE());
  }

  public IEnumerator DelayIE()
  {
    yield return (object) new WaitForSeconds(5f);
    foreach (SpriteRenderer winnerIcon in this.winnerIcons)
      winnerIcon.sprite = this.defaultSprite;
    this.coroutine = (Coroutine) null;
  }

  public void TryStartRace()
  {
    if (this.gateType != GateType.Start || !Interaction_RacingGate.IsAnyGateType(GateType.End) || Interaction_RacingGate.isRaceActive)
      return;
    AudioManager.Instance.PlayOneShot("event:/dlc/animal/shared/race_start");
    Interaction_RacingGate.isRaceActive = true;
    Interaction_RacingGate.raceTimer = 0.0f;
    Interaction_RacingGate.raceTicker = 0;
    System.Action onStartRace = Interaction_RacingGate.OnStartRace;
    if (onStartRace != null)
      onStartRace();
    Interaction_RacingGate.raceLoopInstance = AudioManager.Instance.CreateLoop(this.raceLoopSFX, true);
  }

  public void HandleRaceTimer()
  {
    if (this.gateType != GateType.Start || !Interaction_RacingGate.isRaceActive)
      return;
    if ((double) Interaction_RacingGate.raceTimer == 0.0)
      AudioManager.Instance.PlayOneShot("event:/dlc/animal/shared/race_timer_tick");
    Interaction_RacingGate.raceTimer += Time.deltaTime;
    if (Mathf.FloorToInt(Interaction_RacingGate.raceTimer) <= Interaction_RacingGate.raceTicker)
      return;
    Interaction_RacingGate.raceTicker = Mathf.FloorToInt(Interaction_RacingGate.raceTimer);
    AudioManager.Instance.PlayOneShot("event:/dlc/animal/shared/race_timer_tick");
  }

  public void FinishRace(PlayerFarming player, bool checkFail = true)
  {
    if (!Interaction_RacingGate.IsRaceActive)
      return;
    if (this.gateType == GateType.End && this.GetFinishedRacingGates(player).Count >= Interaction_RacingGate.RacingGates.Count)
    {
      AudioManager.Instance.StopLoop(Interaction_RacingGate.raceLoopInstance);
      AudioManager.Instance.PlayOneShot("event:/dlc/animal/shared/race_finish");
      ObjectPool.Spawn(this.confettiFinish, this.transform.parent, this.transform.position + Vector3.back * 1.5f + Vector3.up);
      System.Action<float> onFinishRace = Interaction_RacingGate.OnFinishRace;
      if (onFinishRace != null)
        onFinishRace(Interaction_RacingGate.raceTimer);
      this.GetFinishedRacingGates(player).Clear();
      Interaction_RacingGate.raceTimer = 0.0f;
      Interaction_RacingGate.isRaceActive = false;
      GameManager.GetInstance().WaitForSeconds(5f, (System.Action) (() =>
      {
        if (PlayerFarming.players.Count < 2)
          return;
        Interaction_RacingGate.FinishedRaceGatesLamb.Clear();
        Interaction_RacingGate.FinishedRaceGatesGoat.Clear();
      }));
    }
    else
    {
      if (!checkFail)
        return;
      AudioManager.Instance.StopLoop(Interaction_RacingGate.raceLoopInstance);
      System.Action<float> onFinishRace = Interaction_RacingGate.OnFinishRace;
      if (onFinishRace != null)
        onFinishRace(float.PositiveInfinity);
      this.GetFinishedRacingGates(player).Clear();
      Interaction_RacingGate.raceTimer = 0.0f;
      Interaction_RacingGate.isRaceActive = false;
    }
  }

  public void SetFlags(GateType setGateType)
  {
    switch (setGateType)
    {
      case GateType.Default:
        for (int index = 0; index < this.startFlags.Length; ++index)
          this.startFlags[index].SetActive(false);
        for (int index = 0; index < this.endFlags.Length; ++index)
          this.endFlags[index].SetActive(false);
        break;
      case GateType.Start:
        for (int index = 0; index < this.startFlags.Length; ++index)
          this.startFlags[index].SetActive(true);
        break;
      case GateType.End:
        for (int index = 0; index < this.endFlags.Length; ++index)
          this.endFlags[index].SetActive(true);
        break;
    }
    BiomeConstants.Instance.EmitSmokeExplosionVFX(this.transform.position);
  }

  public static bool IsAnyGateType(GateType gateType)
  {
    foreach (Interaction_RacingGate racingGate in Interaction_RacingGate.RacingGates)
    {
      if (racingGate.gateType == gateType)
        return true;
    }
    return false;
  }

  public HashSet<Interaction_RacingGate> GetFinishedRacingGates(PlayerFarming player)
  {
    return !((UnityEngine.Object) player == (UnityEngine.Object) null) && !((UnityEngine.Object) player == (UnityEngine.Object) PlayerFarming.players[0]) ? Interaction_RacingGate.FinishedRaceGatesGoat : Interaction_RacingGate.FinishedRaceGatesLamb;
  }
}
