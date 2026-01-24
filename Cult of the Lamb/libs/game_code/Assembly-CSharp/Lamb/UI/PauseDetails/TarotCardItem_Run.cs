// Decompiled with JetBrains decompiler
// Type: Lamb.UI.PauseDetails.TarotCardItem_Run
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A2AB015A-5AB3-4BBD-8AD6-CE3D7C83DC19
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Lamb.UI.PauseDetails;

public class TarotCardItem_Run : TarotCardItemBase
{
  public void Start() => this.TarotCard.Spine.color = new Color(1f, 1f, 1f, 1f);
}
