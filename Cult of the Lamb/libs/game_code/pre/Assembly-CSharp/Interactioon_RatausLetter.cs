// Decompiled with JetBrains decompiler
// Type: Interactioon_RatausLetter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

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

  protected override void OnEnable()
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

  private IEnumerator GiveKeyPieceRoutine()
  {
    Interactioon_RatausLetter interactioonRatausLetter = this;
    yield return (object) null;
    TarotCustomTarget tarotCustomTarget = TarotCustomTarget.Create(interactioonRatausLetter.transform.position, PlayerFarming.Instance.transform.position + Vector3.back * 0.5f, 1f, TarotCards.Card.Hearts2, (System.Action) (() => GameManager.GetInstance().OnConversationEnd()));
    GameManager.GetInstance().OnConversationNew();
    GameManager.GetInstance().OnConversationNext(tarotCustomTarget.gameObject, 6f);
  }
}
