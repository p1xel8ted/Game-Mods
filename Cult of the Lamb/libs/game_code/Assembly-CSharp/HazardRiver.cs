// Decompiled with JetBrains decompiler
// Type: HazardRiver
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 023F7ED3-0437-4ADB-A778-0C302DE53340
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FMOD.Studio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class HazardRiver : BaseMonoBehaviour
{
  [SerializeField]
  public Transform riverPathTransform;
  [SerializeField]
  public float waterSpeed = 1f;
  [Range(0.0f, 1f)]
  public float _waterSpeedMultiplier = 1f;
  public LineRenderer riverPath;
  public LineRenderer bankPath;
  public LineRenderer bankEdgePath;
  public WaterInteractionRiver waterInteractionRiver;
  public bool _riverDriedUp;
  public bool generateEdgeDressing;
  public GameObject edgeDressingPrefab;
  public float dressingGap = 0.75f;
  public float dressingScaleMin = 0.8f;
  public float dressingScaleRange = 0.4f;
  public float dressingXRange = 0.1f;
  public float dressingYRange = 0.1f;
  public Vector3 dressingOffset = Vector3.zero;
  public EventInstance loopedSound;
  public HazardRiver.RiverPathPosition[] riverPathPositions;
  public Animator waterScrollAnimator;
  public float colliderWidthMultiplier = 0.6f;
  public static int AnimatedOffsetUVX1 = Shader.PropertyToID("AnimatedOffsetUV_X_1");
  public float MaxDistanceSFX = 15f;
  public float soundOffset;
  public bool foundPlayer;
  public bool inTrigger;
  public bool playerInWater;
  public float SFXIntensity;

  public float waterSpeedMultiplier
  {
    get => this._waterSpeedMultiplier;
    set
    {
      if ((double) value != (double) this._waterSpeedMultiplier)
        this.riverPath.materials[2].SetFloat(HazardRiver.AnimatedOffsetUVX1, (float) ((double) value * (double) this.waterSpeed * -5.0));
      this._waterSpeedMultiplier = value;
    }
  }

  public bool riverDriedUp
  {
    get => this._riverDriedUp;
    set
    {
      this._riverDriedUp = value;
      if (!this._riverDriedUp)
        this.riverPath.materials[2].SetFloat(HazardRiver.AnimatedOffsetUVX1, 0.0f);
      else
        this.riverPath.materials[2].SetFloat(HazardRiver.AnimatedOffsetUVX1, this.waterSpeed * -5f);
    }
  }

  public void OnEnable()
  {
    if (this.riverDriedUp)
      return;
    this.StartCoroutine((IEnumerator) this.WaitForPlayerLoop());
  }

  public void OnDisable() => AudioManager.Instance.StopLoop(this.loopedSound);

  public void Awake()
  {
    if ((UnityEngine.Object) this.bankPath != (UnityEngine.Object) null)
      this.bankPath.receiveShadows = true;
    if ((UnityEngine.Object) this.bankEdgePath != (UnityEngine.Object) null)
      this.bankEdgePath.receiveShadows = true;
    if ((UnityEngine.Object) this.riverPath == (UnityEngine.Object) null)
      this.riverPath = this.riverPathTransform.GetComponent<LineRenderer>();
    this.riverPathPositions = new HazardRiver.RiverPathPosition[this.riverPath.positionCount];
    List<Vector2> vector2List = new List<Vector2>();
    for (int index = this.riverPath.positionCount - 1; index >= 0; --index)
    {
      HazardRiver.RiverPathPosition riverPathPosition = new HazardRiver.RiverPathPosition();
      riverPathPosition.id = index;
      riverPathPosition.position = this.riverPath.GetPosition(index);
      if (index < this.riverPath.positionCount - 1)
        riverPathPosition.direction = Vector3.Normalize(this.riverPathPositions[index + 1].position - riverPathPosition.position);
      this.riverPathPositions[index] = riverPathPosition;
    }
    this.riverPath.materials[2].SetFloat(HazardRiver.AnimatedOffsetUVX1, this.waterSpeed * -5f);
  }

  public void FixedUpdate()
  {
    if (this.inTrigger || !this.foundPlayer)
      return;
    this.waterSpeedMultiplier = !PlayerRelic.TimeFrozen ? 1f : 0.0f;
    this.soundOffset = !this.playerInWater ? 0.25f : 0.0f;
    if (!((UnityEngine.Object) PlayerFarming.Instance != (UnityEngine.Object) null))
      return;
    float num = (float) ((double) PlayerFarming.GetClosestPlayerDist(this.transform.position) / (double) this.MaxDistanceSFX * -1.0 + 1.0);
    if ((double) num >= (double) this.MaxDistanceSFX)
      return;
    this.SFXIntensity = num - this.soundOffset;
    AudioManager.Instance.SetEventInstanceParameter(this.loopedSound, "river_intensity", this.SFXIntensity);
  }

  public void Start()
  {
    RoomLockController.OnRoomCleared += new RoomLockController.RoomEvent(this.RoomLockController_OnRoomCleared);
  }

  public IEnumerator WaitForPlayerLoop()
  {
    HazardRiver hazardRiver = this;
    while ((UnityEngine.Object) PlayerFarming.Instance == (UnityEngine.Object) null)
      yield return (object) null;
    hazardRiver.loopedSound = AudioManager.Instance.CreateLoop("event:/atmos/misc/river_loop", hazardRiver.gameObject, true);
    hazardRiver.foundPlayer = true;
  }

  public void OnDestroy()
  {
    AudioManager.Instance.StopLoop(this.loopedSound);
    RoomLockController.OnRoomCleared -= new RoomLockController.RoomEvent(this.RoomLockController_OnRoomCleared);
  }

  public void RoomLockController_OnRoomCleared()
  {
    this.inTrigger = false;
    AudioManager.Instance.SetEventInstanceParameter(this.loopedSound, "river_intensity", 0.0f);
    this.riverPath.DOKill();
    DOTween.To((DOGetter<float>) (() => this.riverPath.widthMultiplier), (DOSetter<float>) (x => this.riverPath.widthMultiplier = x), 0.0f, 2f).SetEase<TweenerCore<float, float, FloatOptions>>(Ease.InQuart).OnComplete<TweenerCore<float, float, FloatOptions>>((TweenCallback) (() =>
    {
      this.riverDriedUp = true;
      UnityEngine.Object.Destroy((UnityEngine.Object) this.waterInteractionRiver);
      AudioManager.Instance.StopLoop(this.loopedSound);
    }));
  }

  public void OnTriggerExit2D(Collider2D collision)
  {
    if (this.riverDriedUp)
      return;
    UnitObject component = collision.GetComponent<UnitObject>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || !((UnityEngine.Object) component.health != (UnityEngine.Object) null) || component.health.team != Health.Team.PlayerTeam)
      return;
    this.playerInWater = false;
  }

  public void OnTriggerEnter2D(Collider2D collision)
  {
    if (this.riverDriedUp)
      return;
    UnitObject component = collision.GetComponent<UnitObject>();
    if (!((UnityEngine.Object) component != (UnityEngine.Object) null) || component.health.team != Health.Team.PlayerTeam)
      return;
    this.playerInWater = true;
  }

  public void OnTriggerStay2D(Collider2D collision)
  {
    if (this.riverDriedUp)
      return;
    UnitObject unit = collision.GetComponent<UnitObject>();
    if (!((UnityEngine.Object) unit != (UnityEngine.Object) null) || unit.isFlyingEnemy)
      return;
    HazardRiver.RiverPathPosition[] array = ((IEnumerable<HazardRiver.RiverPathPosition>) this.riverPathPositions).OrderBy<HazardRiver.RiverPathPosition, float>((Func<HazardRiver.RiverPathPosition, float>) (rp => Vector3.Distance(rp.position, unit.transform.position))).ToArray<HazardRiver.RiverPathPosition>();
    HazardRiver.RiverPathPosition riverPathPosition1 = array[0];
    HazardRiver.RiverPathPosition riverPathPosition2 = array[1];
    HazardRiver.RiverPathPosition riverPathPosition3 = riverPathPosition1.id >= riverPathPosition2.id ? riverPathPosition2 : riverPathPosition1;
    unit.transform.position += riverPathPosition3.direction * (this.waterSpeed * 3f * this._waterSpeedMultiplier) * Time.deltaTime;
  }

  [CompilerGenerated]
  public float \u003CRoomLockController_OnRoomCleared\u003Eb__38_0()
  {
    return this.riverPath.widthMultiplier;
  }

  [CompilerGenerated]
  public void \u003CRoomLockController_OnRoomCleared\u003Eb__38_1(float x)
  {
    this.riverPath.widthMultiplier = x;
  }

  [CompilerGenerated]
  public void \u003CRoomLockController_OnRoomCleared\u003Eb__38_2()
  {
    this.riverDriedUp = true;
    UnityEngine.Object.Destroy((UnityEngine.Object) this.waterInteractionRiver);
    AudioManager.Instance.StopLoop(this.loopedSound);
  }

  public struct RiverPathPosition
  {
    public Vector3 position;
    public Vector3 direction;
    public float distance;
    public int id;
  }
}
