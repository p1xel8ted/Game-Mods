// Decompiled with JetBrains decompiler
// Type: Interaction_PlaceName
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using I2.Loc;
using System.Collections;
using UnityEngine;

#nullable disable
public class Interaction_PlaceName : BaseMonoBehaviour
{
  [TermsPopup("")]
  public string PlaceName;
  public float Radius = 3f;
  public GameObject Player;

  public void Start() => this.StartCoroutine(this.UpdateRoutine());

  public IEnumerator UpdateRoutine()
  {
    Interaction_PlaceName interactionPlaceName = this;
    while ((Object) (interactionPlaceName.Player = GameObject.FindGameObjectWithTag("Player")) == (Object) null)
      yield return (object) null;
    while ((Object) interactionPlaceName.Player != (Object) null && (double) Vector3.Distance(interactionPlaceName.transform.position, interactionPlaceName.Player.transform.position) > (double) interactionPlaceName.Radius)
      yield return (object) null;
    if (!((Object) interactionPlaceName.Player == (Object) null))
      HUD_DisplayName.Play(interactionPlaceName.PlaceName);
  }

  public void OnDrawGizmos()
  {
    Utils.DrawCircleXY(this.transform.position, this.Radius, Color.white);
  }
}
