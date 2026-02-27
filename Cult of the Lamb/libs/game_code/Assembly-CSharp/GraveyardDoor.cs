// Decompiled with JetBrains decompiler
// Type: GraveyardDoor
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using MMTools;
using UnityEngine;

#nullable disable
public class GraveyardDoor : MonoBehaviour
{
  [SerializeField]
  public string sceneToLoad;
  public bool Used;

  public void OnTriggerEnter2D(Collider2D collision)
  {
    if (!(bool) (UnityEngine.Object) collision.gameObject.GetComponent<PlayerFarming>() || this.Used)
      return;
    this.Used = true;
    MMTransition.StopCurrentTransition();
    MMTransition.Play(MMTransition.TransitionType.ChangeRoomWaitToResume, MMTransition.Effect.BlackFade, this.sceneToLoad, 1f, "", (System.Action) null);
  }
}
