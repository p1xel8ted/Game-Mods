// Decompiled with JetBrains decompiler
// Type: Interaction_RatauShrine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using UnityEngine;

#nullable disable
public class Interaction_RatauShrine : Interaction
{
  public DevotionCounterOverlay devotionCounterOverlay;
  public GameObject ReceiveSoulPosition;
  public Structure Structure;
  public SpriteXPBar XPBar;
  public string sString;
  public bool Activating;
  public float Delay;
  public float Distance;
  public float DistanceToTriggerDeposits = 5f;

  public Structures_Shrine_Ratau StructureBrain => this.Structure.Brain as Structures_Shrine_Ratau;

  public void Start()
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

  public void OnStructuresPlaced()
  {
    this.UpdateBar();
    StructureManager.OnStructuresPlaced -= new StructureManager.StructuresPlaced(this.OnStructuresPlaced);
  }

  public void OnBrainAssigned()
  {
    if ((double) this.StructureBrain.Data.LastPrayTime == -1.0)
    {
      this.StructureBrain.SoulCount = this.StructureBrain.SoulMax;
      this.StructureBrain.Data.LastPrayTime = TimeManager.TotalElapsedGameTime + this.StructureBrain.TimeBetweenSouls;
    }
    Structures_Shrine_Ratau structureBrain = this.StructureBrain;
    structureBrain.OnSoulsGained = structureBrain.OnSoulsGained + new System.Action<int>(this.OnSoulsGained);
    this.UpdateBar();
  }

  public new void OnDestroy()
  {
    if (this.StructureBrain == null)
      return;
    Structures_Shrine_Ratau structureBrain = this.StructureBrain;
    structureBrain.OnSoulsGained = structureBrain.OnSoulsGained - new System.Action<int>(this.OnSoulsGained);
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
      string str = (GameManager.HasUnlockAvailable() ? 1 : (DataManager.Instance.DeathCatBeaten ? 1 : 0)) != 0 ? "<sprite name=\"icon_spirits\">" : "<sprite name=\"icon_blackgold\">";
      this.Interactable = this.StructureBrain.SoulCount > 0;
      if (LocalizeIntegration.IsArabic())
      {
        string[] strArray = new string[8]
        {
          this.sString,
          " ",
          str,
          " ",
          null,
          null,
          null,
          null
        };
        int num = this.StructureBrain.SoulMax;
        strArray[4] = LocalizeIntegration.ReverseText(num.ToString());
        strArray[5] = " / ";
        num = this.StructureBrain.SoulCount;
        strArray[6] = LocalizeIntegration.ReverseText(num.ToString());
        strArray[7] = StaticColors.GreyColorHex;
        this.Label = string.Concat(strArray);
      }
      else
      {
        string[] strArray = new string[8]
        {
          this.sString,
          " ",
          str,
          " ",
          null,
          null,
          null,
          null
        };
        int num = this.StructureBrain.SoulCount;
        strArray[4] = num.ToString();
        strArray[5] = StaticColors.GreyColorHex;
        strArray[6] = " / ";
        num = this.StructureBrain.SoulMax;
        strArray[7] = num.ToString();
        this.Label = string.Concat(strArray);
      }
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

  public void OnSoulsGained(int count) => this.UpdateBar();

  public void UpdateBar()
  {
    if ((UnityEngine.Object) this.XPBar == (UnityEngine.Object) null || this.StructureBrain == null)
      return;
    this.XPBar.UpdateBar(Mathf.Clamp((float) this.StructureBrain.SoulCount / (float) this.StructureBrain.SoulMax, 0.0f, 1f));
  }

  public override void Update()
  {
    base.Update();
    if ((UnityEngine.Object) this.playerFarming == (UnityEngine.Object) null)
      return;
    this.Distance = Vector3.Distance(this.transform.position, this.playerFarming.transform.position);
    if (this.Activating && (this.StructureBrain.SoulCount <= 0 || InputManager.Gameplay.GetInteractButtonUp() || (double) this.Distance > (double) this.DistanceToTriggerDeposits))
      this.Activating = false;
    if ((double) (this.Delay -= Time.deltaTime) < 0.0 && this.Activating)
    {
      if ((GameManager.HasUnlockAvailable() ? 1 : (DataManager.Instance.DeathCatBeaten ? 1 : 0)) != 0)
        SoulCustomTarget.Create(this.playerFarming.gameObject, this.ReceiveSoulPosition.transform.position, Color.white, new System.Action(this.GivePlayerSoul));
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

  public void GivePlayerSoul() => this.playerFarming.GetSoul(1);
}
