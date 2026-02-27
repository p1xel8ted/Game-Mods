// Decompiled with JetBrains decompiler
// Type: DeadWorshipper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5ECA9E40-DF29-464B-A6ED-FE41BA24084E
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

#nullable disable
public class DeadWorshipper : BaseMonoBehaviour
{
  public static List<DeadWorshipper> DeadWorshippers = new List<DeadWorshipper>();
  public SkeletonAnimation Spine;
  public Structure Structure;
  public ParticleSystem RottenParticles;
  public FollowerInfo followerInfo;
  public GameObject freezingParticle;
  public GameObject[] Flowers;
  public GameObject ItemIndicator;
  public GameObject Rotted;
  public bool PlayAnimation;
  public string DieAnimation = "die";
  public string DeadAnimation = "dead";
  public bool rotSequencePlayed;

  public StructuresData StructureInfo => this.Structure.Structure_Info;

  public bool IsRotten
  {
    get
    {
      return this.StructureInfo != null && this.StructureInfo.Rotten && !this.StructureInfo.BodyWrapped;
    }
  }

  public bool ShowWidget
  {
    get
    {
      return this.StructureInfo != null && this.followerInfo != null && !this.StructureInfo.BodyWrapped && !this.followerInfo.FrozeToDeath;
    }
  }

  public void Data() => Debug.Log((object) ("PlayAnimation " + this.PlayAnimation.ToString()));

  public void Start()
  {
    DeadWorshipper.DeadWorshippers.Add(this);
    StructureManager.OnStructureMoved += new StructureManager.StructureChanged(this.StructureModified);
    StructureManager.OnStructureRemoved += new StructureManager.StructureChanged(this.StructureModified);
  }

  public void OnEnable()
  {
    TimeManager.OnNewDayStarted += new System.Action(this.OnNewDayStarted);
    this.Structure.OnBrainAssigned += new System.Action(this.OnBrainAssigned);
    this.HideBody();
    if (this.Structure.Brain == null)
      return;
    this.OnBrainAssigned();
  }

  public void OnDestroy()
  {
    DeadWorshipper.DeadWorshippers.Remove(this);
    StructureManager.OnStructureMoved -= new StructureManager.StructureChanged(this.StructureModified);
    StructureManager.OnStructureRemoved -= new StructureManager.StructureChanged(this.StructureModified);
  }

  public void OnBrainAssigned()
  {
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
    this.Spine.Initialize(false);
    this.Setup();
    if (!((UnityEngine.Object) BiomeBaseManager.Instance != (UnityEngine.Object) null))
      return;
    PolygonCollider2D collider = BiomeBaseManager.Instance.Room.Pieces[0].Collider;
    if ((Utils.PointWithinPolygon(this.transform.position, collider.GetPath(0)) || collider.OverlapPoint((Vector2) this.transform.position)) && (double) this.transform.position.y <= 1.5)
      return;
    List<StructureBrain> structuresFromRole = StructureManager.GetStructuresFromRole(FollowerRole.Worshipper);
    if (structuresFromRole.Count > 0)
    {
      this.transform.position = (structuresFromRole[0] as Structures_Shrine).Data.Position + (Vector3) UnityEngine.Random.insideUnitCircle.normalized * 3f;
    }
    else
    {
      Vector2 direction = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(0.0f, -1f));
      LayerMask layerMask = (LayerMask) ((int) new LayerMask() | 1 << LayerMask.NameToLayer("Island"));
      this.transform.position = (Vector3) (Physics2D.Raycast((Vector2) Vector3.zero, direction, 1000f, (int) layerMask).point + -direction * UnityEngine.Random.Range(3f, 8f));
    }
  }

  public void StructureModified(StructuresData s)
  {
    if (s == null || this.StructureInfo == null || s.FollowerID != this.StructureInfo.FollowerID)
      return;
    if (s.ID == this.StructureInfo.ID)
    {
      foreach (Structures_Prison structuresPrison in StructureManager.GetAllStructuresOfType<Structures_Prison>())
      {
        if (structuresPrison.Data.FollowerID == this.StructureInfo.FollowerID)
          structuresPrison.Data.FollowerID = -1;
      }
    }
    else
    {
      this.DeadAnimation = "dead";
      this.StructureInfo.Animation = this.DeadAnimation;
      s.FollowerID = -1;
      this.Setup();
    }
  }

  public void Setup()
  {
    this.followerInfo = FollowerManager.GetDeadFollowerInfoByID(this.StructureInfo.FollowerID);
    if (this.followerInfo == null)
      this.followerInfo = FollowerManager.FindFollowerInfo(this.StructureInfo.FollowerID);
    if (this.StructureInfo.FollowerID == -1 || this.followerInfo == null)
    {
      this.gameObject.SetActive(false);
      StructureManager.OnStructureMoved -= new StructureManager.StructureChanged(this.StructureModified);
      StructureManager.OnStructureRemoved -= new StructureManager.StructureChanged(this.StructureModified);
      StructureManager.RemoveStructure(this.Structure.Brain);
    }
    else
    {
      this.freezingParticle.gameObject.SetActive(this.followerInfo.FrozeToDeath);
      this.ItemIndicator.SetActive(!this.followerInfo.FrozeToDeath);
      this.SetOutfit();
      this.Spine.skeleton.ScaleX = (float) this.StructureInfo.Dir;
      this.gameObject.name = "dead body " + this.followerInfo.Name;
      if (this.StructureInfo.Animation != "" && this.StructureInfo.Animation != "dead")
        this.DeadAnimation = this.StructureInfo.Animation;
      if (this.followerInfo.FrozeToDeath)
        this.DeadAnimation = "Freezing/dead";
      this.StructureInfo.Animation = this.DeadAnimation;
      if (this.PlayAnimation)
      {
        this.Spine.AnimationState.SetAnimation(0, this.DieAnimation, false);
        this.Spine.AnimationState.End += new Spine.AnimationState.TrackEntryDelegate(this.AnimationState_End);
        this.Spine.AnimationState.AddAnimation(0, this.DeadAnimation, true, 0.0f);
      }
      else if (this.StructureInfo.BodyWrapped)
      {
        this.Spine.AnimationState.SetAnimation(0, "corpse", true);
        this.WrapBody();
      }
      else if (this.StructureInfo.Rotten)
      {
        if (this.DeadAnimation.Contains("Freezing"))
        {
          this.Spine.AnimationState.SetAnimation(0, "Freezing/dead", true);
          this.ShowBody();
          return;
        }
        if (this.DeadAnimation.Contains("Lightning"))
          this.DeadAnimation = "dead";
        this.Spine.AnimationState.SetAnimation(0, this.DeadAnimation + "-rotten", true);
        this.RottenParticles.gameObject.SetActive(true);
      }
      else
      {
        this.Spine.AnimationState.SetAnimation(0, this.DeadAnimation, true);
        this.RottenParticles.gameObject.SetActive(false);
      }
      if (this.followerInfo.DiedFromRot && !this.StructureInfo.BodyWrapped && PlayerFarming.Location == FollowerLocation.Base)
        this.StartCoroutine(this.RottedIE());
      else
        this.ShowBody();
    }
  }

  public IEnumerator RottedIE()
  {
    if (!this.rotSequencePlayed)
    {
      this.rotSequencePlayed = true;
      this.Spine.gameObject.SetActive(true);
      yield return (object) new WaitForSeconds(1.5f);
      this.Rotted.gameObject.SetActive(true);
      this.Spine.gameObject.SetActive(false);
      Transform transform = this.Rotted.transform;
      Vector3 one = Vector3.one;
      transform.localScale = Vector3.zero;
      DG.Tweening.Sequence sequence = DOTween.Sequence();
      sequence.Append((Tween) transform.DOScale(one * 1.3f, 0.1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutQuad)).Append((Tween) transform.DOScale(one * 0.8f, 0.08f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutQuad)).Append((Tween) transform.DOScale(one * 1.15f, 0.08f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutQuad)).Append((Tween) transform.DOScale(one * 1f, 0.15f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack));
      yield return (object) sequence.WaitForCompletion();
      this.RotDeathSequence();
      yield return (object) new WaitForSeconds(2f);
      this.ItemIndicator.gameObject.SetActive(true);
    }
  }

  public IEnumerator ScaleOverTime(Transform target, Vector3 endScale, float duration)
  {
    Vector3 startScale = target.localScale;
    float time = 0.0f;
    while ((double) time < (double) duration)
    {
      time += Time.deltaTime;
      target.localScale = Vector3.Lerp(startScale, endScale, time / duration);
      yield return (object) null;
    }
    target.localScale = endScale;
  }

  public void ShowIndicator()
  {
    this.ItemIndicator.transform.DOKill();
    this.ItemIndicator.transform.DOScale(Vector3.one * 1.2f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
  }

  public void HideIndicator()
  {
    this.ItemIndicator.transform.DOKill();
    this.ItemIndicator.transform.DOScale(Vector3.zero, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InBack);
  }

  public void SetOutfit()
  {
    if (this.followerInfo == null || (UnityEngine.Object) this.Spine == (UnityEngine.Object) null || this.Spine.Skeleton == null)
      return;
    FollowerBrain.SetFollowerCostume(this.Spine.skeleton, this.followerInfo, forceUpdate: true);
  }

  public void SetOutfit(FollowerOutfitType outfit)
  {
    if (this.followerInfo == null || (UnityEngine.Object) this.Spine == (UnityEngine.Object) null || this.Spine.Skeleton == null)
      return;
    this.followerInfo.Outfit = outfit;
    FollowerBrain.SetFollowerCostume(this.Spine.skeleton, this.followerInfo, forceUpdate: true);
  }

  public void OnNewDayStarted()
  {
    this.Spine.enabled = false;
    this.StartCoroutine(this.WaitRotten());
  }

  public IEnumerator WaitRotten()
  {
    yield return (object) new WaitForSeconds(0.1f);
    if (this.StructureInfo.Rotten && !this.StructureInfo.BodyWrapped)
    {
      this.Spine.enabled = true;
      yield return (object) new WaitForEndOfFrame();
      this.SetRotten();
      yield return (object) new WaitForEndOfFrame();
      this.Spine.enabled = false;
    }
  }

  public IEnumerator DeactivateSpine()
  {
    yield return (object) new WaitForSeconds(2.5f);
    if ((UnityEngine.Object) this.Spine != (UnityEngine.Object) null)
      this.Spine.enabled = false;
  }

  public void SetRotten()
  {
    if (this.StructureInfo.BodyWrapped)
      return;
    Debug.Log((object) "Set Body Rotten");
    if (this.DeadAnimation.Contains("Lightning"))
      this.DeadAnimation = "dead";
    else if (this.DeadAnimation.Contains("Freezing"))
      return;
    this.Spine.AnimationState.SetAnimation(0, this.DeadAnimation + "-rotten", true);
    this.RottenParticles.gameObject.SetActive(true);
  }

  public void WrapBody()
  {
    this.Rotted.gameObject.SetActive(false);
    this.Spine.gameObject.SetActive(true);
    this.Spine.enabled = true;
    this.StructureInfo.BodyWrapped = true;
    this.Spine.AnimationState.SetAnimation(0, "corpse", true);
    this.RottenParticles.gameObject.SetActive(false);
    GameManager.GetInstance().StartCoroutine(this.DeactivateSpine());
  }

  public void HideBody()
  {
    this.Spine.gameObject.SetActive(false);
    this.RottenParticles.gameObject.SetActive(false);
    this.ItemIndicator.SetActive(false);
  }

  public void ShowBody()
  {
    this.ItemIndicator.SetActive(!this.followerInfo.FrozeToDeath);
    this.Spine.gameObject.SetActive(true);
  }

  public void AnimationState_End(TrackEntry trackEntry)
  {
    this.PlayAnimation = false;
    foreach (Follower follower in Follower.Followers)
    {
      if ((double) Vector3.Distance(this.transform.position, this.transform.position) < 12.0 && follower.Brain.Location == this.StructureInfo.Location && (follower.Brain.CurrentTask == null || !follower.Brain.CurrentTask.BlockReactTasks))
        follower.Brain.HardSwapToTask((FollowerTask) new FollowerTask_ReactCorpse(this.StructureInfo.ID));
    }
  }

  public void AddWorshipper() => DeadWorshipper.DeadWorshippers.Add(this);

  public void RemoveWorshipper() => DeadWorshipper.DeadWorshippers.Remove(this);

  public void OnDisable()
  {
    this.Spine.AnimationState.End -= new Spine.AnimationState.TrackEntryDelegate(this.AnimationState_End);
    TimeManager.OnNewDayStarted -= new System.Action(this.OnNewDayStarted);
    this.Structure.OnBrainAssigned -= new System.Action(this.OnBrainAssigned);
  }

  public void BounceOutFromPosition(float speed)
  {
    PickUp component = this.GetComponent<PickUp>();
    component.enabled = true;
    component.Speed = speed;
  }

  public void BounceOutFromPosition(float speed, Vector3 dir)
  {
    PickUp component = this.GetComponent<PickUp>();
    component.enabled = true;
    component.SetInitialSpeedAndDiraction(speed, Utils.GetAngle(Vector3.zero, dir));
  }

  public void RotDeathSequence()
  {
    this.Spine.gameObject.SetActive(false);
    if (PlayerFarming.Location == FollowerLocation.Base)
      BiomeConstants.Instance.EmitSmokeExplosionVFX(this.transform.position);
    PlacementRegion.TileGridTile tileAtWorldPosition = PlacementRegion.Instance.GetClosestTileGridTileAtWorldPosition(this.transform.position);
    if (tileAtWorldPosition == null)
      return;
    List<PlacementRegion.TileGridTile> tiles = new List<PlacementRegion.TileGridTile>();
    this.GetRandomTilesInDirection(tileAtWorldPosition.Position, Vector2Int.up, UnityEngine.Random.Range(3, 6), tiles);
    this.GetRandomTilesInDirection(tileAtWorldPosition.Position, Vector2Int.right, UnityEngine.Random.Range(3, 6), tiles);
    this.GetRandomTilesInDirection(tileAtWorldPosition.Position, Vector2Int.down, UnityEngine.Random.Range(3, 6), tiles);
    this.GetRandomTilesInDirection(tileAtWorldPosition.Position, Vector2Int.left, UnityEngine.Random.Range(3, 6), tiles);
    this.StartCoroutine(this.PlaceRotRubble(tiles));
  }

  public IEnumerator PlaceRotPathing(List<PlacementRegion.TileGridTile> tiles)
  {
    DeadWorshipper deadWorshipper = this;
    tiles = tiles.OrderBy<PlacementRegion.TileGridTile, float>(new Func<PlacementRegion.TileGridTile, float>(deadWorshipper.\u003CPlaceRotPathing\u003Eb__46_0)).ToList<PlacementRegion.TileGridTile>();
    for (int i = 0; i < tiles.Count; ++i)
    {
      if (PathTileManager.Instance.GetTileTypeAtPosition(tiles[i].WorldPosition) == StructureBrain.TYPES.NONE)
      {
        PlacementRegion.Instance.MarkObstructionsForClearing(tiles[i].Position, Vector2Int.one, (StructuresData) null);
        yield return (object) new WaitForSeconds(0.15f);
      }
    }
  }

  public IEnumerator PlaceRotRubble(List<PlacementRegion.TileGridTile> tiles)
  {
    DeadWorshipper deadWorshipper = this;
    tiles = tiles.OrderBy<PlacementRegion.TileGridTile, float>(new Func<PlacementRegion.TileGridTile, float>(deadWorshipper.\u003CPlaceRotRubble\u003Eb__47_0)).ToList<PlacementRegion.TileGridTile>();
    for (int i = 0; i < tiles.Count; ++i)
    {
      deadWorshipper.PlaceRubble(FollowerLocation.Base, tiles[i], PlacementRegion.Instance.structureBrain);
      yield return (object) new WaitForSeconds(0.05f);
    }
  }

  public List<PlacementRegion.TileGridTile> GetRandomTilesInDirection(
    Vector2Int from,
    Vector2Int direction,
    int step,
    List<PlacementRegion.TileGridTile> tiles)
  {
    Vector2Int vector2Int1 = from + direction;
    for (int index = 0; index < step; ++index)
    {
      PlacementRegion.TileGridTile tileGridTile = PlacementRegion.Instance.GetTileGridTile((float) vector2Int1.x, (float) vector2Int1.y);
      if (tileGridTile != null && !tileGridTile.Occupied && !tiles.Contains(tileGridTile))
      {
        tiles.Add(tileGridTile);
        Vector2Int vector2Int2 = new Vector2Int((double) UnityEngine.Random.value < 0.5 ? 1 : -1, (double) UnityEngine.Random.value < 0.5 ? 1 : -1);
        vector2Int1 += new Vector2Int(direction.x + ((double) UnityEngine.Random.value < 0.20000000298023224 ? vector2Int2.x : 0), direction.y + ((double) UnityEngine.Random.value < 0.20000000298023224 ? vector2Int2.y : 0));
      }
      else
        break;
    }
    return tiles;
  }

  public void PlaceRubble(
    FollowerLocation location,
    PlacementRegion.TileGridTile t,
    Structures_PlacementRegion p)
  {
    StructuresData infoByType = StructuresData.GetInfoByType(StructureBrain.TYPES.RUBBLE, 0);
    Vector3 worldPosition = t.WorldPosition;
    infoByType.DontLoadMe = false;
    infoByType.GrowthStage = 1f;
    infoByType.VariantIndex = 1;
    infoByType.PlacementRegionPosition = new Vector3Int((int) p.Data.Position.x, (int) p.Data.Position.y, 0);
    infoByType.GridTilePosition = t.Position;
    PlacementRegion.Instance.MarkObstructionsForClearing(t.Position, Vector2Int.one, (StructuresData) null);
    StructureManager.BuildStructure(location, infoByType, worldPosition, new Vector2Int(1, 1));
  }

  public bool IsWithinScreenView()
  {
    Vector3 screenPoint = CameraManager.instance.CameraRef.WorldToScreenPoint(this.transform.position);
    return (double) screenPoint.x > 0.0 & (double) screenPoint.x < (double) Screen.width && (double) screenPoint.y > 0.0 && (double) screenPoint.y < (double) (Screen.height - 100);
  }

  [CompilerGenerated]
  public float \u003CPlaceRotPathing\u003Eb__46_0(PlacementRegion.TileGridTile x)
  {
    return Vector3.Distance(x.WorldPosition, this.transform.position);
  }

  [CompilerGenerated]
  public float \u003CPlaceRotRubble\u003Eb__47_0(PlacementRegion.TileGridTile x)
  {
    return Vector3.Distance(x.WorldPosition, this.transform.position);
  }
}
