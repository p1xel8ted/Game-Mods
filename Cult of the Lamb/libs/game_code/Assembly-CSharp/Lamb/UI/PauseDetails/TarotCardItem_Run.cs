// Decompiled with JetBrains decompiler
// Type: Lamb.UI.PauseDetails.TarotCardItem_Run
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
namespace Lamb.UI.PauseDetails;

public class TarotCardItem_Run : TarotCardItemBase
{
  public void Start() => this.TarotCard.Spine.color = new Color(1f, 1f, 1f, 1f);
}
