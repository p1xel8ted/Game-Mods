// Decompiled with JetBrains decompiler
// Type: TempleDecorationsManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Lamb.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

#nullable disable
public class TempleDecorationsManager : MonoBehaviour
{
  public static TempleDecorationsManager Instance;
  public LightingManagerVolume lightingManager;
  public BiomeLightingSettings[] lightingSettings;
  public Light StandardGlassLight;
  public Light StainedGlassSpotlight;
  public Texture[] StainedGlassCookies;
  public Transform LambDecorationPosition;
  [SerializeField]
  public Transform container;
  [SerializeField]
  public TempleDecorationsManager.DecorationSet[] decorations;
  [SerializeField]
  public GameObject spawnAnimationFx;
  public Dictionary<string, AsyncOperationHandle<GameObject>> loadedDecorations = new Dictionary<string, AsyncOperationHandle<GameObject>>();
  public List<GameObject> spawnedDecorations = new List<GameObject>();
  public List<GameObject> decorationsToAnimate = new List<GameObject>();
  public int LAST_DECORATION_LEVEL;
  public float startingSpotAngle;
  public Vector3 startingSpotPosition;
  public float startingSpotIntensity;
  public int cachedDecorationsStyle = -1;
  public List<GameObject> individualDecorationsToAnimate = new List<GameObject>();
  public Coroutine AnimateIndividualDecorationsCoroutine;
  public Vector3 nextDecorationPosition;
  public bool lambInPlace;

  public int DECORATION_LEVEL => DataManager.Instance.TempleLevel;

  public int DECORATION_STYLE => DataManager.Instance.TempleBorder - 100;

  public void Start()
  {
    TempleDecorationsManager.Instance = this;
    this.startingSpotAngle = this.StainedGlassSpotlight.spotAngle;
    this.startingSpotPosition = this.StainedGlassSpotlight.gameObject.transform.position;
    this.startingSpotIntensity = this.StainedGlassSpotlight.intensity;
  }

  public void OnEnable()
  {
    this.cachedDecorationsStyle = Mathf.Max(0, this.DECORATION_STYLE);
    this.LAST_DECORATION_LEVEL = this.DECORATION_LEVEL;
    UICultUpgradesMenuController.OnCultUpgraded += new System.Action(this.RefreshDecorationsAfterCultUpgrade);
  }

  public void OnDisable()
  {
    UICultUpgradesMenuController.OnCultUpgraded -= new System.Action(this.RefreshDecorationsAfterCultUpgrade);
  }

  public void RefreshDecorationsAfterCultUpgrade()
  {
    Debug.Log((object) ("CULT HAS BEEN UPDATED - WAIT UNTIL NO MENUS THEN BEGIN CUTSCENE " + this.DECORATION_LEVEL.ToString()));
    if (this.LAST_DECORATION_LEVEL != 0 && this.LAST_DECORATION_LEVEL < this.DECORATION_LEVEL)
    {
      this.SpawnDecorations(new Action<GameObject>(this.BeginAnimatedIntro));
      this.LAST_DECORATION_LEVEL = this.DECORATION_LEVEL;
    }
    else
      this.SpawnDecorations((Action<GameObject>) null);
  }

  public void OnDestroy() => TempleDecorationsManager.Instance = (TempleDecorationsManager) null;

  public void SpawnDecorations(Action<GameObject> callback, bool forceReload = false)
  {
    Debug.Log((object) "Reloading all decorations on Decorations style change");
    int style = Mathf.Max(0, this.DECORATION_STYLE);
    if (forceReload || this.cachedDecorationsStyle != style)
      this.UnloadDecorations();
    this.cachedDecorationsStyle = style;
    this.lightingManager.LightingSettings = this.lightingSettings[style];
    this.lightingManager.gameObject.SetActive(false);
    this.lightingManager.gameObject.SetActive(true);
    if (style > 1)
    {
      Debug.Log((object) ("DECORATION LEVEL: " + style.ToString()));
      this.StainedGlassSpotlight.cookie = this.StainedGlassCookies[style];
      if (style > 1)
      {
        this.StainedGlassSpotlight.gameObject.transform.position = new Vector3(this.StainedGlassSpotlight.gameObject.transform.position.x, this.StainedGlassSpotlight.gameObject.transform.position.y, -0.12f);
        this.StainedGlassSpotlight.spotAngle = 174.2f;
        this.StainedGlassSpotlight.intensity = style != 3 ? this.startingSpotIntensity : 3f;
      }
      else
      {
        this.StainedGlassSpotlight.gameObject.transform.position = this.startingSpotPosition;
        this.StainedGlassSpotlight.spotAngle = this.startingSpotAngle;
      }
      this.StandardGlassLight.gameObject.SetActive(false);
      this.StainedGlassSpotlight.gameObject.SetActive(false);
      this.StainedGlassSpotlight.gameObject.SetActive(true);
    }
    Interaction_TempleAltar.Instance.SetAltarStyle(style);
    int loadedCount = 0;
    for (int level = 0; level < this.DECORATION_LEVEL; ++level)
    {
      string path = this.GetDecorationAsset(style, level);
      if (this.loadedDecorations.ContainsKey(path))
      {
        loadedCount++;
      }
      else
      {
        AsyncOperationHandle<GameObject> asyncOperationHandle = Addressables.LoadAssetAsync<GameObject>((object) path);
        this.loadedDecorations.Add(path, asyncOperationHandle);
        asyncOperationHandle.Completed += (Action<AsyncOperationHandle<GameObject>>) (obj =>
        {
          this.loadedDecorations[path] = obj;
          ++loadedCount;
          if (loadedCount < this.DECORATION_LEVEL)
            return;
          for (int index = 0; index < this.loadedDecorations.Values.Count; ++index)
          {
            GameObject result = this.loadedDecorations.Values.ElementAt<AsyncOperationHandle<GameObject>>(index).Result;
            if (!this.spawnedDecorations.Contains(result))
            {
              GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(result, this.container);
              this.spawnedDecorations.Add(result);
              if (index >= this.loadedDecorations.Count - 1)
              {
                Action<GameObject> action = callback;
                if (action != null)
                  action(gameObject);
                this.lightingManager.gameObject.SetActive(true);
                this.StainedGlassSpotlight.gameObject.SetActive(true);
              }
            }
          }
        });
      }
    }
  }

  public void BeginAnimatedIntro(GameObject resultToAnimate)
  {
    this.individualDecorationsToAnimate.Clear();
    this.RecursivelyFindChildDecorationParts(resultToAnimate, 2);
    if (this.AnimateIndividualDecorationsCoroutine != null)
      this.StopCoroutine(this.AnimateIndividualDecorationsCoroutine);
    this.AnimateIndividualDecorationsCoroutine = this.StartCoroutine((IEnumerator) this.AnimateIndividualDecorations());
  }

  public void RecursivelyFindChildDecorationParts(GameObject decoration, int maxDepth, int depth = 0)
  {
    int num = 1;
    Renderer component = decoration.GetComponent<Renderer>();
    if (!(bool) (UnityEngine.Object) component)
      num = 0;
    bool flag = false;
    if (decoration.CompareTag("Decoration"))
    {
      flag = true;
      depth = maxDepth;
    }
    if (flag || (bool) (UnityEngine.Object) component && decoration.activeSelf && !this.individualDecorationsToAnimate.Contains(decoration))
    {
      this.individualDecorationsToAnimate.Add(decoration);
      decoration.SetActive(false);
    }
    if (depth >= maxDepth)
      return;
    for (int index = 0; index < decoration.transform.childCount; ++index)
      this.RecursivelyFindChildDecorationParts(decoration.transform.GetChild(index).gameObject, maxDepth, depth + num);
  }

  public IEnumerator AnimateIndividualDecorations()
  {
    TempleDecorationsManager decorationsManager = this;
    Debug.Log((object) "Animating new upgrade in");
    yield return (object) new WaitForSeconds(0.1f);
    yield return (object) new WaitForSeconds(0.25f);
    Interaction_TempleAltar.Instance.FrontWall.SetActive(false);
    PlayerFarming.Instance.GoToAndStop(decorationsManager.LambDecorationPosition.position + Vector3.up, ChurchFollowerManager.Instance.RitualCenterPosition.gameObject, GoToCallback: new System.Action(decorationsManager.\u003CAnimateIndividualDecorations\u003Eb__35_0));
    while (!decorationsManager.lambInPlace)
      yield return (object) new WaitForEndOfFrame();
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    PlayerFarming.Instance.simpleSpineAnimator.Animate("sermons/sermon-start", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("sermons/sermon-loop", 0, true, 0.0f);
    AudioManager.Instance.PlayOneShot("event:/rituals/generic_start_ritual");
    yield return (object) new WaitForSeconds(0.25f);
    decorationsManager.lambInPlace = false;
    for (int i = 0; i < decorationsManager.individualDecorationsToAnimate.Count; ++i)
    {
      GameObject decoration = decorationsManager.individualDecorationsToAnimate[i];
      if (!((UnityEngine.Object) decoration == (UnityEngine.Object) null))
      {
        GameManager.GetInstance().RemoveAllFromCamera();
        GameManager.GetInstance().AddPlayerToCamera();
        GameManager.GetInstance().AddToCamera(decoration.gameObject);
        yield return (object) new WaitForSeconds(0.3f);
        decoration.gameObject.SetActive(true);
        Vector3 localScale1 = decoration.transform.localScale;
        decoration.transform.localScale = Vector3.zero;
        decoration.transform.DOScale(localScale1, 1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBounce);
        GameObject gameObject1 = UnityEngine.Object.Instantiate<GameObject>(decorationsManager.spawnAnimationFx, decorationsManager.container);
        gameObject1.transform.position = decoration.transform.position;
        gameObject1.SetActive(true);
        UnityEngine.Object.Destroy((UnityEngine.Object) gameObject1, 3f);
        AudioManager.Instance.PlayOneShot("event:/relics/lightning/lightning_impact", gameObject1.transform.position);
        CameraManager.shakeCamera(3f, Utils.GetAngle(decorationsManager.transform.position, decoration.transform.position));
        if (i < decorationsManager.individualDecorationsToAnimate.Count - 1)
        {
          GameObject gameObject2 = decorationsManager.individualDecorationsToAnimate[i + 1];
          if (!((UnityEngine.Object) gameObject2 == (UnityEngine.Object) null))
          {
            if ((double) Vector3.Magnitude(gameObject2.transform.position - decoration.transform.position) < 0.125)
            {
              Vector3 localScale2 = gameObject2.transform.localScale;
              gameObject2.transform.localScale = Vector3.zero;
              gameObject2.transform.DOScale(localScale2, 1.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutBounce);
              gameObject2.SetActive(true);
              ++i;
            }
          }
          else
            continue;
        }
        decoration = (GameObject) null;
      }
    }
    CultFaithManager.AddThought(Thought.Cult_BaseUpgraded);
    FaithBarFake.Play(FollowerThoughts.GetData(Thought.Cult_BaseUpgraded).Modifier);
    yield return (object) new WaitForSeconds(1f);
    PlayerFarming.Instance.simpleSpineAnimator.Animate("sermons/sermon-stop", 0, false);
    PlayerFarming.Instance.simpleSpineAnimator.AddAnimate("idle", 0, true, 0.0f);
    PlayerFarming.SetStateForAllPlayers();
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    GameManager.GetInstance().RemoveAllFromCamera();
    GameManager.GetInstance().AddPlayerToCamera();
    UICultUpgradeProgress.showCultUpgradeProgressSequence = true;
    Interaction_TempleAltar.Instance.OnInteract(Interaction_TempleAltar.Instance.state);
    Interaction_TempleAltar.Instance.GetComponent<Collider2D>().enabled = true;
  }

  public void UnloadDecorations()
  {
    for (int index = this.transform.childCount - 1; index >= 0; --index)
    {
      if ((UnityEngine.Object) this.transform.GetChild(index) != (UnityEngine.Object) null)
        UnityEngine.Object.Destroy((UnityEngine.Object) this.transform.GetChild(index).gameObject);
    }
    foreach (AsyncOperationHandle<GameObject> handle in this.loadedDecorations.Values)
    {
      if (handle.IsValid() && (UnityEngine.Object) handle.Result != (UnityEngine.Object) null)
        Addressables.Release<GameObject>(handle);
    }
    for (int index = 0; index < this.spawnedDecorations.Count; ++index)
    {
      if ((UnityEngine.Object) this.spawnedDecorations[index] != (UnityEngine.Object) null)
        this.spawnedDecorations[index].transform.DOKill();
    }
    this.spawnedDecorations.Clear();
    this.individualDecorationsToAnimate.Clear();
    this.decorationsToAnimate.Clear();
    this.loadedDecorations.Clear();
    GameManager.GetInstance().CameraSetOffset(Vector3.zero);
    GameManager.GetInstance().RemoveAllFromCamera();
    GameManager.GetInstance().AddPlayerToCamera();
  }

  public TempleDecorationsManager.DecorationSet GetDecorationSet(int style)
  {
    if (style > this.decorations.Length - 1)
    {
      Debug.Log((object) $"Style was out of range, setting back style level max in array {style.ToString()} / {this.decorations.Length.ToString()}");
      style = this.decorations.Length - 1;
    }
    return this.decorations[style];
  }

  public string GetDecorationAsset(int style, int level)
  {
    return this.GetDecorationSet(style).Decorations[level];
  }

  [CompilerGenerated]
  public void \u003CAnimateIndividualDecorations\u003Eb__35_0()
  {
    PlayerFarming.Instance.state.transform.DOMove(this.LambDecorationPosition.position + Vector3.up, 0.1f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InOutSine).SetUpdate<TweenerCore<Vector3, Vector3, VectorOptions>>(true);
    Interaction_TempleAltar.Instance.FrontWall.SetActive(true);
    this.lambInPlace = true;
  }

  [Serializable]
  public struct DecorationSet
  {
    public List<string> Decorations;
  }
}
