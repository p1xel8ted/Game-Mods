// Decompiled with JetBrains decompiler
// Type: Interaction_PlaceName
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using I2.Loc;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_PlaceName : BaseMonoBehaviour
{
  [TermsPopup("")]
  public string PlaceName;
  public float Radius = 3f;
  private GameObject Player;

  private void Start() => this.StartCoroutine((IEnumerator) this.UpdateRoutine());

  private IEnumerator UpdateRoutine()
  {
    Interaction_PlaceName interactionPlaceName = this;
    while ((Object) (interactionPlaceName.Player = GameObject.FindGameObjectWithTag("Player")) == (Object) null)
      yield return (object) null;
    while ((Object) interactionPlaceName.Player != (Object) null && (double) Vector3.Distance(interactionPlaceName.transform.position, interactionPlaceName.Player.transform.position) > (double) interactionPlaceName.Radius)
      yield return (object) null;
    if (!((Object) interactionPlaceName.Player == (Object) null))
      HUD_DisplayName.Play(interactionPlaceName.PlaceName);
  }

  private void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position, this.Radius, Color.white);
  }
}
