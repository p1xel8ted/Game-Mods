// Decompiled with JetBrains decompiler
// Type: AuraSubcomponentBase
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using System;

#nullable disable
public abstract class AuraSubcomponentBase
{
  public string aura_id;
  [NonSerialized]
  public AuraDefinition _aura;

  public AuraDefinition aura
  {
    get => this._aura ?? (this._aura = GameBalance.me.GetData<AuraDefinition>(this.aura_id));
  }

  public virtual void Update()
  {
  }
}
