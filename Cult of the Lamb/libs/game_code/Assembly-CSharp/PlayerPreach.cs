// Decompiled with JetBrains decompiler
// Type: PlayerPreach
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B4944960-D044-4E12-B091-6A0422C77B16
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class PlayerPreach : BaseMonoBehaviour
{
  public PlayerFarming playerFarming;
  public StateMachine state;
  public float Timer;
  public List<Worshipper> Worshippers;

  public void Start()
  {
    this.playerFarming = this.GetComponent<PlayerFarming>();
    this.state = this.GetComponent<StateMachine>();
  }

  public void Update()
  {
    switch (this.state.CURRENT_STATE)
    {
      case StateMachine.State.Idle:
      case StateMachine.State.Moving:
        using (List<Worshipper>.Enumerator enumerator = Worshipper.worshippers.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            Worshipper current = enumerator.Current;
            if (!current.GivenPreachSoul && (double) Vector3.Distance(this.transform.position, current.transform.position) < 2.0)
            {
              current.GivenPreachSoul = true;
              current.StartWorship(this.gameObject);
              CameraManager.shakeCamera(0.1f, Utils.GetAngle(this.transform.position, current.transform.position));
            }
          }
          break;
        }
    }
  }

  public IEnumerator Preach()
  {
    PlayerPreach playerPreach = this;
    playerPreach.Timer = 0.0f;
    playerPreach.state.CURRENT_STATE = StateMachine.State.Preach;
    playerPreach.Worshippers = new List<Worshipper>();
    foreach (Worshipper worshipper in Worshipper.worshippers)
    {
      if ((double) Vector3.Distance(playerPreach.transform.position, worshipper.transform.position) < 3.0)
      {
        worshipper.StartWorship(playerPreach.gameObject);
        playerPreach.Worshippers.Add(worshipper);
      }
    }
    yield return (object) new WaitForSeconds(0.5f);
    if (playerPreach.Worshippers.Count > 0)
      CameraManager.shakeCamera(0.3f, (float) UnityEngine.Random.Range(0, 360));
    foreach (Worshipper worshipper in playerPreach.Worshippers)
    {
      if (!worshipper.GivenPreachSoul)
        SoulCustomTarget.Create(playerPreach.gameObject, worshipper.transform.position, Color.black, (System.Action) null);
      worshipper.GivenPreachSoul = true;
    }
    yield return (object) new WaitForSeconds(2f);
    foreach (Worshipper worshipper in playerPreach.Worshippers)
      worshipper.EndWorship();
    playerPreach.state.CURRENT_STATE = StateMachine.State.Idle;
  }
}
