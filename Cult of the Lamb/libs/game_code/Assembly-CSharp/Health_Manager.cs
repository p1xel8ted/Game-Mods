// Decompiled with JetBrains decompiler
// Type: Health_Manager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 67F01238-B454-48B8-93E4-17A603153F10
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class Health_Manager : MonoBehaviour
{
  [SerializeField]
  public GameObject healthBaseSingle;
  [SerializeField]
  public GameObject healthDungeonSingle;
  [SerializeField]
  public GameObject healthSingleUniversal;
  [SerializeField]
  public GameObject relicUISingle;
  [SerializeField]
  public UI_Transitions transition_Single;
  [SerializeField]
  public HUD_Hearts health_SingleBase;
  [SerializeField]
  public HUD_Hearts health_SingleDungeon;
  [SerializeField]
  public HUD_Hearts base_health_secondPlayer;
  [SerializeField]
  public GameObject healthDungeonTwoPlayer;
  [SerializeField]
  public UI_Transitions transition_TwoPlayer;
  [SerializeField]
  public HUD_Hearts health_firstPlayer;
  [SerializeField]
  public HUD_Hearts health_secondPlayer;
  [SerializeField]
  public GameObject TwoPlayerHudGameObject;

  public void Start() => this.DisableObjects();

  public void HideBars()
  {
    this.transition_TwoPlayer.hideBar();
    this.transition_Single.hideBar();
  }

  public void Hide(bool Snap, int Delay = 1, bool both = false)
  {
    if (CoopManager.CoopActive && LocationManager.IsDungeonActive())
      this.transition_TwoPlayer.MoveBackOutFunction();
    else
      this.transition_Single.MoveBackOutFunction();
  }

  public void Show(int Delay = 1, bool Force = false)
  {
    if (CoopManager.CoopActive && LocationManager.IsDungeonActive())
      this.transition_TwoPlayer.MoveBackInFunction();
    else
      this.transition_Single.MoveBackInFunction();
  }

  public void DisableObjects()
  {
    this.health_SingleBase.gameObject.SetActive(false);
    this.health_SingleDungeon.gameObject.SetActive(false);
    this.healthBaseSingle.gameObject.SetActive(false);
    this.healthDungeonSingle.gameObject.SetActive(false);
    this.relicUISingle.SetActive(false);
    this.healthDungeonTwoPlayer.gameObject.SetActive(false);
    this.healthSingleUniversal.gameObject.SetActive(false);
  }

  public void Init(PlayerFarming p)
  {
    this.DisableObjects();
    if (GameManager.IsDungeon(PlayerFarming.Location))
    {
      if (PlayerFarming.players.Count == 1)
      {
        this.health_SingleDungeon.gameObject.SetActive(true);
        this.relicUISingle.SetActive(true);
        this.healthDungeonSingle.SetActive(true);
        this.health_SingleDungeon.InitDungeon(p);
        Debug.Log((object) "Spawn Dungeon Single Player HUD");
        if (!((UnityEngine.Object) this.TwoPlayerHudGameObject != (UnityEngine.Object) null))
          return;
        this.TwoPlayerHudGameObject.SetActive(false);
      }
      else
      {
        if ((UnityEngine.Object) this.TwoPlayerHudGameObject != (UnityEngine.Object) null)
          this.TwoPlayerHudGameObject.SetActive(true);
        this.healthDungeonTwoPlayer.SetActive(true);
        Debug.Log((object) "Spawn Dungeon Two Player HUD");
        if (PlayerFarming.players.Count <= 0)
          return;
        Debug.Log((object) ("players count: " + PlayerFarming.players.Count.ToString()));
        if (p.isLamb)
        {
          this.health_firstPlayer.gameObject.SetActive(true);
          this.health_firstPlayer.InitDungeon(p);
          Debug.Log((object) "player farming = lamb");
        }
        else
        {
          this.health_secondPlayer.gameObject.SetActive(true);
          this.health_secondPlayer.InitDungeon(p);
        }
      }
    }
    else
    {
      this.healthSingleUniversal.gameObject.SetActive(true);
      this.healthBaseSingle.SetActive(true);
      if (PlayerFarming.players.Count == 1)
      {
        Debug.Log((object) "Spawn Base Single Player HUD");
        this.health_SingleBase.InitBase(p);
      }
      else if (p.isLamb)
        this.health_SingleBase.InitBase(p);
      else
        this.health_secondPlayer.InitBase(p);
    }
  }

  public void ShowSecondPlayerHealth()
  {
    this.base_health_secondPlayer.InitBase(PlayerFarming.players[1]);
    UI_Transitions transition = this.base_health_secondPlayer.HeartIcons[0].transform.parent.parent.GetComponent<UI_Transitions>();
    transition.gameObject.SetActive(true);
    transition.MoveBackOutFunction();
    GameManager.GetInstance().WaitForSeconds(3f, (System.Action) (() =>
    {
      transition.MoveBackInFunction();
      GameManager.GetInstance().WaitForSeconds(2f, (System.Action) (() => transition.gameObject.SetActive(false)));
    }));
  }
}
