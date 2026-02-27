// Decompiled with JetBrains decompiler
// Type: Interaction_Villager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

#nullable disable
public class Interaction_Villager : Interaction
{
  private WorshipperInfoManager character;

  private void Start() => this.character = this.GetComponent<WorshipperInfoManager>();

  private new void Update()
  {
    if (!(this.Label == "") || this.character.v_i == null)
      return;
    this.Label = $"{this.character.v_i.Name} ({(object) this.character.v_i.Age})";
  }
}
