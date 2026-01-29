// Decompiled with JetBrains decompiler
// Type: Interaction_Snowman
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using UnityEngine;

#nullable disable
public class Interaction_Snowman : Interaction
{
  [SerializeField]
  public Structure structure;
  [SerializeField]
  public SpriteRenderer spriteRenderer;
  [SerializeField]
  public Sprite[] sprites;

  public void Start()
  {
    this.structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    if (this.structure.Brain != null)
      this.OnBrainAssigned();
    if (SeasonsManager.CurrentSeason == SeasonsManager.Season.Winter)
      return;
    if ((bool) (UnityEngine.Object) this.structure)
      this.structure.RemoveStructure();
    UnityEngine.Object.Destroy((UnityEngine.Object) this.gameObject);
  }

  public override void GetLabel()
  {
    base.GetLabel();
    this.Interactable = false;
    if (this.structure.Brain.Data.Level < 6)
      this.Label = ScriptLocalization.Interactions.ShoddySnowlamb;
    else if (this.structure.Brain.Data.Level <= 9)
      this.Label = ScriptLocalization.Interactions.DecentSnowlamb;
    else
      this.Label = ScriptLocalization.Interactions.PerfectSnowlamb;
  }

  public override void OnDestroy()
  {
    if ((bool) (UnityEngine.Object) this.structure)
      this.structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    base.OnDestroy();
  }

  public void OnBrainAssigned()
  {
    this.spriteRenderer.sprite = this.sprites[this.structure.Brain.Data.VariantIndex];
  }
}
