// Decompiled with JetBrains decompiler
// Type: Interaction_DiscipleBoostShrine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using MMTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class Interaction_DiscipleBoostShrine : Interaction
{
  public static List<Interaction_DiscipleBoostShrine> Shrines = new List<Interaction_DiscipleBoostShrine>();
  public Structure Structure;
  [SerializeField]
  public SpriteXPBar XpBar;
  public string sString;
  [SerializeField]
  public GameObject shrineEyes;

  public Structures_Shrine_Passive StructureBrain
  {
    get => this.Structure.Brain as Structures_Shrine_Passive;
  }

  public void Start() => this.UpdateLocalisation();

  public override void OnEnableInteraction()
  {
    if ((UnityEngine.Object) this.shrineEyes != (UnityEngine.Object) null)
      this.shrineEyes.SetActive(false);
    base.OnEnableInteraction();
    Interaction_DiscipleBoostShrine.Shrines.Add(this);
    StructureManager.OnStructuresPlaced += new StructureManager.StructuresPlaced(this.OnStructuresPlaced);
    this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    if (this.Structure.Brain == null)
      return;
    this.OnBrainAssigned();
  }

  public void OnBrainAssigned()
  {
    Structures_Shrine_Passive structureBrain = this.StructureBrain;
    structureBrain.OnSoulsGained = structureBrain.OnSoulsGained + new System.Action<int>(this.OnSoulsGained);
    this.UpdateBar();
  }

  public void OnStructuresPlaced()
  {
    this.UpdateBar();
    DataManager.Instance.ShrineLevel = 1;
  }

  public override void OnDisableInteraction()
  {
    base.OnDisableInteraction();
    Interaction_DiscipleBoostShrine.Shrines.Remove(this);
    StructureManager.OnStructuresPlaced -= new StructureManager.StructuresPlaced(this.OnStructuresPlaced);
    if (this.StructureBrain == null)
      return;
    Structures_Shrine_Passive structureBrain = this.StructureBrain;
    structureBrain.OnSoulsGained = structureBrain.OnSoulsGained - new System.Action<int>(this.OnSoulsGained);
  }

  public override void GetLabel()
  {
    this.Interactable = this.StructureBrain.SoulCount >= this.StructureBrain.SoulMax;
    this.Label = this.Interactable ? this.sString : "";
  }

  public override void OnInteract(StateMachine state)
  {
    base.OnInteract(state);
    this.StartCoroutine((IEnumerator) this.ReceiveBuffIE());
  }

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sString = ScriptLocalization.Interactions.Claim;
  }

  public void OnSoulsGained(int count) => this.UpdateBar();

  public void UpdateBar()
  {
    if (this.StructureBrain == null)
      return;
    float num = Mathf.Clamp((float) this.StructureBrain.SoulCount / (float) this.StructureBrain.SoulMax, 0.0f, 1f);
    if ((UnityEngine.Object) this.shrineEyes != (UnityEngine.Object) null)
    {
      if (this.StructureBrain.SoulCount >= this.StructureBrain.SoulMax)
        this.shrineEyes.SetActive(true);
      else
        this.shrineEyes.SetActive(false);
    }
    this.XpBar.UpdateBar(num);
  }

  public IEnumerator ReceiveBuffIE()
  {
    Interaction_DiscipleBoostShrine discipleBoostShrine = this;
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(discipleBoostShrine.playerFarming.gameObject);
    discipleBoostShrine.playerFarming.GoToAndStop(discipleBoostShrine.transform.position + Vector3.down / 2f);
    yield return (object) new WaitForSeconds(0.5f);
    discipleBoostShrine.playerFarming.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    discipleBoostShrine.playerFarming.simpleSpineAnimator.Animate("float-up-spin", 0, false);
    discipleBoostShrine.playerFarming.simpleSpineAnimator.AddAnimate("floating-land-spin", 0, false, 0.0f);
    discipleBoostShrine.playerFarming.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    yield return (object) new WaitForSeconds(0.75f);
    string term = "";
    if ((double) UnityEngine.Random.value > 0.5)
    {
      double num1 = (double) UnityEngine.Random.value;
      float num2 = UnityEngine.Random.value;
      if (num1 > 0.6600000262260437)
      {
        float num3 = (double) num2 > 0.6600000262260437 ? 2f : 1f;
        if (discipleBoostShrine.playerFarming.isLamb)
          DataManager.Instance.PLAYER_SPIRIT_TOTAL_HEARTS += num3;
        else
          DataManager.Instance.COOP_PLAYER_SPIRIT_TOTAL_HEARTS += num3;
        discipleBoostShrine.playerFarming.health.TotalSpiritHearts += num3;
        BiomeConstants.Instance.EmitHeartPickUpVFX(discipleBoostShrine.playerFarming.CameraBone.transform.position, 0.0f, "red", (double) num2 > 0.6600000262260437 ? "burst_big" : "burst_small");
        term = "UI/DiscipleBonus/SpiritHeart/" + ((double) num2 > 0.6600000262260437 ? "Full" : "Half");
      }
      else
      {
        float num4 = (double) num2 > 0.6600000262260437 ? 2f : 1f;
        if (discipleBoostShrine.playerFarming.isLamb)
          DataManager.Instance.PLAYER_BLUE_HEARTS += num4;
        else
          DataManager.Instance.COOP_PLAYER_BLUE_HEARTS += num4;
        discipleBoostShrine.playerFarming.health.BlueHearts += num4;
        BiomeConstants.Instance.EmitHeartPickUpVFX(discipleBoostShrine.playerFarming.CameraBone.transform.position, 0.0f, "blue", (double) num2 > 0.6600000262260437 ? "burst_big" : "burst_small");
        term = "UI/DiscipleBonus/BlueHeart/" + ((double) num2 > 0.6600000262260437 ? "Full" : "Half");
      }
    }
    else
    {
      float num = UnityEngine.Random.value;
      DataManager.Instance.PLAYER_RUN_DAMAGE_LEVEL += (double) num > 0.6600000262260437 ? 2f : 1f;
      BiomeConstants.Instance.EmitHeartPickUpVFX(discipleBoostShrine.playerFarming.CameraBone.transform.position, 0.0f, "strength", "strength");
      term = "UI/DiscipleBonus/DamageBuff/" + ((double) num > 0.6600000262260437 ? "Big" : "Small");
    }
    AudioManager.Instance.PlayOneShot("event:/followers/love_hearts", discipleBoostShrine.playerFarming.transform.position);
    yield return (object) new WaitForSeconds(1f);
    MMConversation.Play(new ConversationObject(new List<ConversationEntry>()
    {
      new ConversationEntry(discipleBoostShrine.gameObject, $"{LocalizationManager.GetTranslation(term)} {LocalizationManager.GetTranslation("UI/DiscipleBonus/ForNextRun")}")
    }, (List<MMTools.Response>) null, (System.Action) null), false, false, false, false);
    MMConversation.mmConversation.SpeechBubble.ScreenOffset = 200f;
    while (MMConversation.CURRENT_CONVERSATION != null)
      yield return (object) null;
    discipleBoostShrine.StructureBrain.SoulCount = 0;
    discipleBoostShrine.UpdateBar();
    GameManager.GetInstance().OnConversationEnd();
  }
}
