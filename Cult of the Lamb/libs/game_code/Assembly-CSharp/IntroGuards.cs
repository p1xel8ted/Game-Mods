// Decompiled with JetBrains decompiler
// Type: IntroGuards
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class IntroGuards : BaseMonoBehaviour
{
  public StateMachine Guard1;
  public StateMachine Guard2;
  public GameObject Wall;
  public GameObject Player;
  public bool Activated;

  public void Start() => this.Wall.SetActive(false);

  public void OnTriggerEnter2D(Collider2D collision)
  {
    if (this.Activated || !collision.gameObject.CompareTag("Player"))
      return;
    this.Player = collision.gameObject;
    this.StartCoroutine(this.FollowPlayer());
    this.Activated = true;
  }

  public IEnumerator FollowPlayer()
  {
    IntroGuards introGuards = this;
    introGuards.Wall.SetActive(true);
    float Progress = 0.0f;
    float Duration = 1.5f;
    Vector3 StartGuard1 = introGuards.Guard1.transform.position;
    Vector3 StartGuard2 = introGuards.Guard2.transform.position;
    Vector3 StartWall = introGuards.Wall.transform.position;
    introGuards.Guard1.CURRENT_STATE = StateMachine.State.Moving;
    introGuards.Guard2.CURRENT_STATE = StateMachine.State.Moving;
    while ((double) (Progress += Time.deltaTime) < (double) Duration)
    {
      introGuards.Guard1.transform.position = Vector3.Lerp(StartGuard1, introGuards.transform.position + Vector3.left * 0.75f, Progress / Duration);
      introGuards.Guard2.transform.position = Vector3.Lerp(StartGuard2, introGuards.transform.position + Vector3.right * 0.75f, Progress / Duration);
      introGuards.Wall.transform.position = Vector3.Lerp(StartWall, introGuards.transform.position, Progress / Duration);
      introGuards.Guard1.facingAngle = 80f;
      introGuards.Guard2.facingAngle = 95f;
      yield return (object) null;
    }
    introGuards.Guard1.CURRENT_STATE = StateMachine.State.Idle;
    introGuards.Guard2.CURRENT_STATE = StateMachine.State.Idle;
  }
}
