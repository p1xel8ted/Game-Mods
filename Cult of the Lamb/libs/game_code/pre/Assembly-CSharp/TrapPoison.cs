// Decompiled with JetBrains decompiler
// Type: TrapPoison
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

#nullable disable
public class TrapPoison : BaseMonoBehaviour
{
  public Collider2D collider;
  public Transform graphic;
  [SerializeField]
  private ParticleSystem particleSystem;
  public float lifetime;
  public float spawnDuration = 0.25f;
  public float despawnDuration = 0.25f;
  [Space]
  [SerializeField]
  private Sprite[] sprites;
  public Health.Team team;
  private float spawnTimestamp;
  private bool isSpawning;
  private bool isDespawning;
  private List<Health> victims = new List<Health>();
  private static List<TrapPoison> activePoison = new List<TrapPoison>();
  public bool useMeshGoop;
  [SerializeField]
  private Mesh[] goopMeshes;
  public MeshRenderer meshRenderer;
  public MeshFilter meshFilter;

  public static void CreatePoison(Vector3 position, int amount, float radius, Transform parent)
  {
    for (int index = 0; index < amount; ++index)
      Addressables.InstantiateAsync((object) "Assets/Prefabs/Enemies/Misc/Trap Poison.prefab", position + (Vector3) Random.insideUnitCircle * radius, Quaternion.identity, parent);
  }

  private void OnEnable()
  {
    if (this.goopMeshes.Length != 0)
      this.meshFilter.mesh = this.goopMeshes[Random.Range(0, this.goopMeshes.Length)];
    this.meshRenderer.gameObject.SetActive(true);
    this.graphic.DOComplete();
    this.graphic.transform.localScale = Vector3.zero;
    this.graphic.transform.DOScale(1f, 0.25f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.OutBack);
    this.isSpawning = true;
    this.isDespawning = false;
    this.collider.enabled = false;
    this.graphic.transform.Rotate(0.0f, 0.0f, Random.Range(0.0f, 360f));
    if ((bool) (Object) GameManager.GetInstance())
      this.spawnTimestamp = GameManager.GetInstance().CurrentTime - Random.Range(0.1f, 0.3f);
    TrapPoison.activePoison.Add(this);
  }

  private void OnDisable() => Object.Destroy((Object) this.gameObject);

  private void Update()
  {
    if (this.isSpawning)
    {
      if ((double) GameManager.GetInstance().TimeSince(this.spawnTimestamp) < (double) this.spawnDuration)
        return;
      this.isSpawning = false;
      this.collider.enabled = true;
    }
    if ((Object) PlayerFarming.Instance != (Object) null && (LetterBox.IsPlaying || PlayerFarming.Instance._state.CURRENT_STATE == StateMachine.State.CustomAnimation))
      this.DespawnPoison();
    if (!this.isDespawning && (double) GameManager.GetInstance().TimeSince(this.spawnTimestamp) >= (double) this.spawnDuration + (double) this.lifetime)
      this.DespawnPoison();
    if (!this.isDespawning || (double) GameManager.GetInstance().TimeSince(this.spawnTimestamp) < (double) this.spawnDuration + (double) this.lifetime + (double) this.despawnDuration)
      return;
    Object.Destroy((Object) this.gameObject);
  }

  private void DespawnPoison()
  {
    if ((bool) (Object) this.particleSystem)
      this.particleSystem.Pause();
    TrapPoison.activePoison.Remove(this);
    this.isDespawning = true;
    this.collider.enabled = false;
    this.graphic.DOComplete();
    this.graphic.transform.localScale = Vector3.one;
    this.graphic.transform.DOScale(0.0f, 0.5f).SetEase<TweenerCore<Vector3, Vector3, VectorOptions>>(Ease.InSine);
  }

  private void OnTriggerEnter2D(Collider2D collider)
  {
    if (collider.gameObject.layer != 14 && collider.gameObject.layer != 9)
      return;
    Health component = collider.GetComponent<Health>();
    if (!((Object) component != (Object) null) || component.team == this.team || component.team == Health.Team.PlayerTeam && (Object) PlayerFarming.Instance != (Object) null && (LetterBox.IsPlaying || PlayerFarming.Instance._state.CURRENT_STATE == StateMachine.State.CustomAnimation))
      return;
    component.AddPoison(this.gameObject);
  }

  private void OnTriggerExit2D(Collider2D collider)
  {
    Health component = collider.GetComponent<Health>();
    if (!((Object) component != (Object) null) || component.team == this.team)
      return;
    component.RemovePoison();
  }

  public static void RemoveAllPoison()
  {
    for (int index = TrapPoison.activePoison.Count - 1; index >= 0; --index)
    {
      if ((Object) TrapPoison.activePoison[index] != (Object) null)
        TrapPoison.activePoison[index].DespawnPoison();
    }
    PlayerFarming.Instance.health.ClearPoison();
  }
}
