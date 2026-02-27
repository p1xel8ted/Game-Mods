// Decompiled with JetBrains decompiler
// Type: Interaction_RatauShrine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using UnityEngine;

#nullable disable
public class Interaction_RatauShrine : Interaction
{
  public DevotionCounterOverlay devotionCounterOverlay;
  public GameObject ReceiveSoulPosition;
  public Structure Structure;
  public SpriteXPBar XPBar;
  private string sString;
  private GameObject Player;
  private bool Activating;
  private float Delay;
  private float Distance;
  public float DistanceToTriggerDeposits = 5f;

  public Structures_Shrine_Ratau StructureBrain => this.Structure.Brain as Structures_Shrine_Ratau;

  private void Start()
  {
    this.UpdateLocalisation();
    this.ContinuouslyHold = true;
  }

  public override void OnEnableInteraction()
  {
    DataManager.Instance.ShrineLevel = 1;
    base.OnEnableInteraction();
    StructureManager.OnStructuresPlaced += new StructureManager.StructuresPlaced(this.OnStructuresPlaced);
    this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    if (this.Structure.Brain != null)
      this.OnBrainAssigned();
    this.UpdateBar();
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
  }

  private void OnStructuresPlaced()
  {
    this.UpdateBar();
    StructureManager.OnStructuresPlaced -= new StructureManager.StructuresPlaced(this.OnStructuresPlaced);
  }

  private void OnBrainAssigned()
  {
    Debug.Log((object) ("StructureBrain.Data.LastPrayTime: " + (object) this.StructureBrain.Data.LastPrayTime));
    if ((double) this.StructureBrain.Data.LastPrayTime == -1.0)
    {
      this.StructureBrain.SoulCount = this.StructureBrain.SoulMax;
      this.StructureBrain.Data.LastPrayTime = TimeManager.TotalElapsedGameTime + this.StructureBrain.TimeBetweenSouls;
    }
    this.StructureBrain.OnSoulsGained += new System.Action<int>(this.OnSoulsGained);
    this.UpdateBar();
  }

  private new void OnDestroy()
  {
    if (this.StructureBrain == null)
      return;
    this.StructureBrain.OnSoulsGained -= new System.Action<int>(this.OnSoulsGained);
  }

  public override void GetLabel()
  {
    if (this.StructureBrain == null)
    {
      Debug.Log((object) "STRUCTRUE BRAIN IS NULL!");
      this.Label = "";
    }
    else
    {
      string str = GameManager.HasUnlockAvailable() ? "<sprite name=\"icon_spirits\">" : "<sprite name=\"icon_blackgold\">";
      this.Interactable = this.StructureBrain.SoulCount > 0;
      this.Label = $"{this.sString} {str} {(object) this.StructureBrain.SoulCount}{StaticColors.GreyColorHex} / {(object) this.StructureBrain.SoulMax}";
    }
  }

  public override void OnInteract(StateMachine state)
  {
    if (this.Activating)
      return;
    base.OnInteract(state);
    this.Activating = true;
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sString = ScriptLocalization.Interactions.ReceiveDevotion;
  }

  private void OnSoulsGained(int count) => this.UpdateBar();

  private void UpdateBar()
  {
    if ((UnityEngine.Object) this.XPBar == (UnityEngine.Object) null || this.StructureBrain == null)
      return;
    this.XPBar.UpdateBar(Mathf.Clamp((float) this.StructureBrain.SoulCount / (float) this.StructureBrain.SoulMax, 0.0f, 1f));
  }

  private new void Update()
  {
    if ((UnityEngine.Object) (this.Player = GameObject.FindWithTag("Player")) == (UnityEngine.Object) null)
      return;
    this.GetLabel();
    this.Distance = Vector3.Distance(this.transform.position, this.Player.transform.position);
    if (this.Activating && (this.StructureBrain.SoulCount <= 0 || InputManager.Gameplay.GetInteractButtonUp() || (double) this.Distance > (double) this.DistanceToTriggerDeposits))
      this.Activating = false;
    if ((double) (this.Delay -= Time.deltaTime) < 0.0 && this.Activating)
    {
      if (GameManager.HasUnlockAvailable())
        SoulCustomTarget.Create(PlayerFarming.Instance.gameObject, this.ReceiveSoulPosition.transform.position, Color.white, new System.Action(this.GivePlayerSoul));
      else
        InventoryItem.Spawn(InventoryItem.ITEM_TYPE.BLACK_GOLD, 1, this.transform.position + Vector3.back, 0.0f).SetInitialSpeedAndDiraction(8f + UnityEngine.Random.Range(-0.5f, 1f), (float) (270 + UnityEngine.Random.Range(-90, 90)));
      --this.StructureBrain.SoulCount;
      this.Delay = 0.2f;
      this.UpdateBar();
    }
    if (this.StructureBrain == null || (double) this.StructureBrain.Data.LastPrayTime == -1.0 || (double) TimeManager.TotalElapsedGameTime <= (double) this.StructureBrain.Data.LastPrayTime || this.StructureBrain.SoulCount >= this.StructureBrain.SoulMax)
      return;
    this.HasChanged = true;
    this.StructureBrain.SoulCount = Mathf.Clamp(this.StructureBrain.SoulCount + (1 + Mathf.FloorToInt((TimeManager.TotalElapsedGameTime - this.StructureBrain.Data.LastPrayTime) / this.StructureBrain.TimeBetweenSouls)), 0, this.StructureBrain.SoulMax);
    this.StructureBrain.Data.LastPrayTime = TimeManager.TotalElapsedGameTime + this.StructureBrain.TimeBetweenSouls;
  }

  private void GivePlayerSoul() => PlayerFarming.Instance.GetSoul(1);
}
