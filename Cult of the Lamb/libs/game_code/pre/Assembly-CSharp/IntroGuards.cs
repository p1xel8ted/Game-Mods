// Decompiled with JetBrains decompiler
// Type: IntroGuards
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class IntroGuards : BaseMonoBehaviour
{
  public StateMachine Guard1;
  public StateMachine Guard2;
  public GameObject Wall;
  private GameObject Player;
  private bool Activated;

  private void Start() => this.Wall.SetActive(false);

  private void OnTriggerEnter2D(Collider2D collision)
  {
    if (this.Activated || !(collision.gameObject.tag == "Player"))
      return;
    this.Player = collision.gameObject;
    this.StartCoroutine((IEnumerator) this.FollowPlayer());
    this.Activated = true;
  }

  private IEnumerator FollowPlayer()
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
