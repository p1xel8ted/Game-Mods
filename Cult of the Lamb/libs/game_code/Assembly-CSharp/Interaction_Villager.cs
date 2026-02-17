// Decompiled with JetBrains decompiler
// Type: Interaction_Villager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class Interaction_Villager : Interaction
{
  public WorshipperInfoManager character;

  public void Start() => this.character = this.GetComponent<WorshipperInfoManager>();

  public override void Update()
  {
    base.Update();
    if (!(this.Label == "") || this.character.v_i == null)
      return;
    if (LocalizeIntegration.IsArabic())
      this.Label = $"{this.character.v_i.Name} ){LocalizeIntegration.ReverseText(this.character.v_i.Age.ToString())}(";
    else
      this.Label = $"{this.character.v_i.Name} ({this.character.v_i.Age.ToString()})";
  }
}
