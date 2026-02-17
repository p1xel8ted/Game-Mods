// Decompiled with JetBrains decompiler
// Type: TestingCustomRoom
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5F70CF1F-EE8D-4EAB-9CF8-16424448359F
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
public class TestingCustomRoom : BaseMonoBehaviour
{
  public bool AutoPlacePlayer = true;
  public GameObject PlayerPrefab;
  public GameObject Player;
  public StateMachine PlayerState;
  public Door North;
  public Door East;
  public Door South;
  public Door West;

  public void Start()
  {
    if (!this.AutoPlacePlayer)
      return;
    this.StartCoroutine((IEnumerator) this.PlacePlayerRoutine());
  }

  public IEnumerator PlacePlayerRoutine()
  {
    yield return (object) new WaitForEndOfFrame();
    this.PlacePlayer();
  }

  public void PlacePlayer()
  {
    Time.timeScale = 1f;
    this.GetDoors();
    if ((Object) this.Player == (Object) null)
    {
      this.Player = Object.Instantiate<GameObject>(this.PlayerPrefab, Vector3.zero, Quaternion.identity, GameObject.FindGameObjectWithTag("Unit Layer").transform);
      this.PlayerState = this.Player.GetComponent<StateMachine>();
    }
    this.Player.transform.position = Vector3.zero;
    if ((Object) this.South != (Object) null)
    {
      this.Player.transform.position = this.South.transform.position + Vector3.up * 0.5f;
      this.PlayerState.facingAngle = 90f;
    }
    else if ((Object) this.North != (Object) null)
    {
      this.Player.transform.position = this.North.transform.position + Vector3.down * 0.5f;
      this.PlayerState.facingAngle = 270f;
    }
    else if ((Object) this.West != (Object) null)
    {
      this.Player.transform.position = this.West.transform.position + Vector3.right * 0.5f;
      this.PlayerState.facingAngle = 0.0f;
    }
    else if ((Object) this.East != (Object) null)
    {
      this.Player.transform.position = this.East.transform.position + Vector3.left * 0.5f;
      this.PlayerState.facingAngle = 180f;
    }
    GameManager.GetInstance().CameraSnapToPosition(this.Player.transform.position);
    GameManager.GetInstance().AddPlayerToCamera();
  }

  public void GetDoors()
  {
    this.North = GameObject.FindGameObjectWithTag("North Door")?.GetComponent<Door>();
    this.East = GameObject.FindGameObjectWithTag("East Door")?.GetComponent<Door>();
    this.South = GameObject.FindGameObjectWithTag("South Door")?.GetComponent<Door>();
    this.West = GameObject.FindGameObjectWithTag("West Door")?.GetComponent<Door>();
  }
}
