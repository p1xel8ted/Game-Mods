// Decompiled with JetBrains decompiler
// Type: CustomInteractionHint
// Assembly: Assembly-CSharp, Version=11.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 015C13E4-E5D0-4A69-A75A-A5E4923AD5DC
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class CustomInteractionHint : MonoBehaviour
{
  public string hint;

  public bool has_hint => !string.IsNullOrEmpty(this.hint);

  public string GetHint() => this.hint;
}
