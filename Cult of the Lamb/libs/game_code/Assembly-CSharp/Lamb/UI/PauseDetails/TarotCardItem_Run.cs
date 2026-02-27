// Decompiled with JetBrains decompiler
// Type: Lamb.UI.PauseDetails.TarotCardItem_Run
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Lamb.UI.PauseDetails;

public class TarotCardItem_Run : TarotCardItemBase
{
  public void Start() => this.TarotCard.Spine.color = new Color(1f, 1f, 1f, 1f);
}
