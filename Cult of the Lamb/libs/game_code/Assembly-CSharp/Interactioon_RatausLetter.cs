// Decompiled with JetBrains decompiler
// Type: Interactioon_RatausLetter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interactioon_RatausLetter : Interaction_SimpleConversation
{
  public Interaction_KeyPiece KeyPiecePrefab;
  public bool GiveKeyPiece = true;

  public override void UpdateLocalisation()
  {
    base.UpdateLocalisation();
    this.sLabel = ScriptLocalization.Interactions.ReadLetter;
  }

  public override void OnEnable()
  {
    this.UpdateLocalisation();
    if (!DataManager.Instance.RatauKilled)
      this.gameObject.SetActive(false);
    else
      base.OnEnable();
  }

  public override void DoCallBack()
  {
    base.DoCallBack();
    if (!this.GiveKeyPiece)
      return;
    this.StartCoroutine((IEnumerator) this.GiveKeyPieceRoutine());
  }

  public IEnumerator GiveKeyPieceRoutine()
  {
    Interactioon_RatausLetter interactioonRatausLetter = this;
    yield return (object) null;
    if ((UnityEngine.Object) interactioonRatausLetter.playerFarming == (UnityEngine.Object) null)
      interactioonRatausLetter.playerFarming = PlayerFarming.Instance;
    TarotCustomTarget tarotCustomTarget = TarotCustomTarget.Create(interactioonRatausLetter.transform.position, interactioonRatausLetter.playerFarming.transform.position + Vector3.back * 0.5f, 1f, TarotCards.Card.Hearts2, (System.Action) (() => GameManager.GetInstance().OnConversationEnd()));
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(tarotCustomTarget.gameObject, 6f);
  }
}
