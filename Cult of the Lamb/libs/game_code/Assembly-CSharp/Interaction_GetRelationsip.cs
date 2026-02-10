// Decompiled with JetBrains decompiler
// Type: Interaction_GetRelationsip
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

#nullable disable
public class Interaction_GetRelationsip : Interaction
{
  public Worshipper playerCharacter;
  public Worshipper c;

  public void Start() => this.c = this.GetComponent<Worshipper>();

  public override void Update() => base.Update();
}
