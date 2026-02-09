// Decompiled with JetBrains decompiler
// Type: SmartCondition
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
public abstract class SmartCondition : MonoBehaviour
{
  public abstract bool CheckCondition();

  public virtual string GetName()
  {
    return this.GetType().Name.Replace(nameof (SmartCondition), "").Trim('_');
  }
}
