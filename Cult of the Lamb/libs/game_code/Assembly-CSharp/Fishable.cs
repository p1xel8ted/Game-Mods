// Decompiled with JetBrains decompiler
// Type: Fishable
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 74784EE5-FB9D-47CB-98C9-77A69FCC35F7
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class Fishable : BaseMonoBehaviour
{
  [SerializeField]
  public SpriteRenderer spriteRender;
  [SerializeField]
  public float fishMoveSpeed = 0.15f;
  [SerializeField]
  public float fishRotationSpeed = 0.35f;
  [SerializeField]
  public float targetHookMoveSpeed;
  [Space]
  [SerializeField]
  [Range(0.0f, 1f)]
  public float hookTargetAngle;
  [SerializeField]
  public float hookTargetingDistance;
  [SerializeField]
  public float hookSplashDistance = 3f;
  [SerializeField]
  public float biteHookDistance = 0.3f;
  [SerializeField]
  public float fishInterestMaxDistance;
  public StateMachine.State state;
  public Interaction_Fishing fishing;
  [CompilerGenerated]
  public Interaction_Fishing.FishType \u003CFishType\u003Ek__BackingField;
  public Vector3 originalCenterPoint;
  public float moveSpeed;
  public float rotationSpeed;
  public const float moveRadius = 2.5f;
  public float moveTimer;
  public Vector2 timeInterval = new Vector2(1f, 3f);
  public Vector3 targetPosition;
  public Vector3 targetLookAt;
  public Vector3 playerPosition;
  public bool chasing;
  public int playerIndex = -1;

  public Interaction_Fishing.FishType FishType
  {
    get => this.\u003CFishType\u003Ek__BackingField;
    set => this.\u003CFishType\u003Ek__BackingField = value;
  }

  public InventoryItem.ITEM_TYPE ItemType
  {
    get => this.FishType.Type;
    set => this.FishType.Type = value;
  }

  public int Quantity => this.FishType.Quantity;

  public void Configure(Interaction_Fishing.FishType fishType, Interaction_Fishing fishingParent)
  {
    this.FishType = fishType;
    this.fishing = fishingParent;
    this.fishing.OnCasted += new Interaction_Fishing.FishEvent(this.HookLanded);
    this.transform.localScale = Vector3.one * Random.Range(fishType.Scale.x, fishType.Scale.y);
    this.originalCenterPoint = new Vector3(fishingParent.transform.position.x, this.transform.position.y, 0.0f);
    this.state = this.GetComponent<StateMachine>().CURRENT_STATE;
    this.moveSpeed = this.fishMoveSpeed;
    this.rotationSpeed = this.fishRotationSpeed;
  }

  public void OnDestroy()
  {
    if (!((Object) this.fishing != (Object) null))
      return;
    this.fishing.OnCasted -= new Interaction_Fishing.FishEvent(this.HookLanded);
  }

  public void OnDisable()
  {
    if (!((Object) this.fishing != (Object) null))
      return;
    this.fishing.OnCasted -= new Interaction_Fishing.FishEvent(this.HookLanded);
  }

  public void HookLanded()
  {
    int playerIndex1 = -1;
    float num1 = float.PositiveInfinity;
    for (int playerIndex2 = 0; playerIndex2 < PlayerFarming.players.Count; ++playerIndex2)
    {
      float num2 = Vector3.Distance(this.fishing.FishingHooks[playerIndex2].position, this.transform.position);
      if ((double) num2 < (double) this.hookSplashDistance && this.fishing.IsClosestFish(playerIndex2, this) && (double) num2 < (double) num1)
      {
        playerIndex1 = playerIndex2;
        num1 = num2;
      }
    }
    if (playerIndex1 == -1)
      return;
    this.TargetedHook(playerIndex1);
  }

  public void FadeIn()
  {
    this.spriteRender.material.color = new Color(this.spriteRender.material.color.r, this.spriteRender.material.color.g, this.spriteRender.material.color.b, 0.0f);
    this.spriteRender.material.DOFade(1f, 1f);
  }

  public void Update()
  {
    if (this.state == StateMachine.State.Idle)
    {
      float? currentTime = GameManager.GetInstance()?.CurrentTime;
      float moveTimer = this.moveTimer;
      if ((double) currentTime.GetValueOrDefault() > (double) moveTimer & currentTime.HasValue)
        this.SetRandomTargetPosition(2.5f);
    }
    if (this.state == StateMachine.State.Attacking)
    {
      this.transform.position = this.fishing.FishingHooks[this.playerIndex].position;
      this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(0.0f, 0.0f, Utils.GetAngle(this.playerPosition, this.transform.position)), this.rotationSpeed * Time.deltaTime);
    }
    else
    {
      this.transform.position = Vector3.Lerp(this.transform.position, this.targetPosition, this.moveSpeed * (this.fishInterestMaxDistance - Vector3.Distance(this.transform.position, this.targetPosition)) * Time.deltaTime);
      this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.Euler(0.0f, 0.0f, Utils.GetAngle(this.targetLookAt, this.transform.position)), this.rotationSpeed * Time.deltaTime);
    }
    if (!((Object) this.fishing != (Object) null))
      return;
    for (int playerIndex = 0; playerIndex < PlayerFarming.players.Count; ++playerIndex)
    {
      if (this.fishing.States[playerIndex] == StateMachine.State.Reeling && this.state != StateMachine.State.Attacking)
      {
        float num = Vector3.Distance(this.transform.position, this.fishing.FishingHooks[playerIndex].position);
        if ((double) num < (double) this.hookTargetingDistance)
          this.TargetedHook(playerIndex);
        else if (this.chasing && this.playerIndex == playerIndex)
        {
          this.targetPosition = this.fishing.FishingHooks[playerIndex].position;
          this.targetLookAt = this.fishing.FishingHooks[playerIndex].position;
        }
        if ((double) num < (double) this.biteHookDistance && this.playerIndex == playerIndex)
        {
          this.Hooked();
          break;
        }
        if ((double) num > (double) this.fishInterestMaxDistance && this.fishing.FishChasing && this.playerIndex == playerIndex)
        {
          this.fishing.FishChasing = false;
          this.chasing = false;
          this.playerIndex = -1;
          this.SetRandomTargetPosition(2.5f);
          this.moveSpeed = this.fishMoveSpeed;
        }
      }
    }
  }

  public void TargetedHook(int playerIndex)
  {
    this.playerIndex = playerIndex;
    this.targetLookAt = this.fishing.FishingHooks[playerIndex].position;
    this.targetPosition = this.fishing.FishingHooks[playerIndex].position;
    this.fishing.FishChasing = true;
    this.chasing = true;
    this.moveSpeed = this.targetHookMoveSpeed;
  }

  public void Hooked()
  {
    this.fishing.FishOn(this.playerIndex, this);
    this.state = StateMachine.State.Attacking;
    this.playerPosition = this.fishing.PlayerPosition;
  }

  public void Spooked()
  {
    if (this.chasing)
    {
      this.fishing.FishChasing = false;
      this.chasing = false;
    }
    this.SetRandomTargetPosition(10f);
    this.moveSpeed = 1f;
    this.rotationSpeed = 1f;
    this.transform.DOScale(0.0f, 2f);
    this.state = StateMachine.State.Fleeing;
    this.playerIndex = -1;
  }

  public void SetRandomTargetPosition(float r)
  {
    this.targetPosition = this.originalCenterPoint + (Vector3) (Random.insideUnitCircle * r);
    this.targetLookAt = this.targetPosition;
    this.moveTimer = GameManager.GetInstance().CurrentTime + Random.Range(this.timeInterval.x, this.timeInterval.y);
  }
}
