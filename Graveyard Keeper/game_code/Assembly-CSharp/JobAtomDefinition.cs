// Decompiled with JetBrains decompiler
// Type: JobAtomDefinition
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;

#nullable disable
[Serializable]
public class JobAtomDefinition : BalanceBaseObject
{
  public string action;
  public string target;
  public string location;
  public string job;
  public int time;
  public int anim;
  public BearCode code = new BearCode();
  public BearLogicExpression condition = new BearLogicExpression();
}
