// Decompiled with JetBrains decompiler
// Type: MegaSlash
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 75F2F530-4272-42C6-BFDD-6995B78CAB72
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class MegaSlash : BaseMonoBehaviour, ISpellOwning
{
  [SerializeField]
  public GameObject symbol;
  [SerializeField]
  public GameObject pivot;
  [SerializeField]
  public SpriteRenderer renderer;
  [SerializeField]
  public GameObject collider;
  [SerializeField]
  public float minScale = 0.3f;
  [SerializeField]
  public float maxScale = 0.75f;
  [SerializeField]
  public AnimationCurve scaleCurve = new AnimationCurve(new Keyframe[2]
  {
    new Keyframe(0.0f, 0.0f),
    new Keyframe(1f, 1f)
  });
  [SerializeField]
  public MeshRenderer fireWallRenderer;
  public MeshRenderer fireWallLightingRenderer;
  [SerializeField]
  public AnimationCurve fireWallSpreadCurve = new AnimationCurve(new Keyframe[3]
  {
    new Keyframe(0.0f, 0.0f),
    new Keyframe(0.5f, 0.5f),
    new Keyframe(1f, 1f)
  });
  [SerializeField]
  public MeshRenderer groundScorchRenderer;
  [SerializeField]
  public AnimationCurve groundScorchCleanupCurve = new AnimationCurve(new Keyframe[2]
  {
    new Keyframe(0.666f, 0.0f),
    new Keyframe(1f, 1f)
  });
  [SerializeField]
  public float duration = 0.75f;
  public DamageCollider damageCollider;
  public GameObject owner;
  public float damageMultiplier = 1f;
  public Coroutine slashRoutine;

  public void OnEnable()
  {
    if (this.slashRoutine == null)
      return;
    Object.Destroy((Object) this.gameObject);
  }

  public void DefaultSizedButton() => this.Play(1f);

  public void Play(float norm, bool ignorePlayer = false)
  {
    if ((Object) this.fireWallRenderer == (Object) null || (Object) this.groundScorchRenderer == (Object) null)
      return;
    PlayerFarming farmingComponent = PlayerFarming.GetPlayerFarmingComponent(this.owner);
    this.damageCollider = this.collider.GetComponent<DamageCollider>();
    this.damageCollider.Damage = EquipmentManager.GetCurseData(farmingComponent.currentCurse).Damage * PlayerSpells.GetCurseDamageMultiplier(farmingComponent) * this.damageMultiplier;
    this.damageCollider.DestroyBullets = true;
    this.damageCollider.IgnorePlayer = ignorePlayer;
    if (farmingComponent.currentCurse == EquipmentType.MegaSlash_Ice)
      this.damageCollider.AttackFlags = Health.AttackFlags.Ice;
    else if (farmingComponent.currentCurse == EquipmentType.MegaSlash_Charm)
      this.damageCollider.AttackFlags = Health.AttackFlags.Charm;
    else if (farmingComponent.currentCurse == EquipmentType.MegaSlash_Flame)
      this.damageCollider.AttackFlags = Health.AttackFlags.Burn;
    this.StopAllCoroutines();
    this.slashRoutine = this.StartCoroutine((IEnumerator) this.SlashRoutine(norm));
    Debug.Log((object) ("Size: " + norm.ToString()));
  }

  public IEnumerator SlashRoutine(float norm)
  {
    MegaSlash megaSlash = this;
    megaSlash.symbol.SetActive(true);
    megaSlash.pivot.SetActive(false);
    megaSlash.symbol.SetActive(false);
    megaSlash.pivot.SetActive(true);
    megaSlash.fireWallLightingRenderer = megaSlash.fireWallRenderer.transform.GetChild(0).GetComponent<MeshRenderer>();
    float timer = 0.0f;
    bool isActive = false;
    while ((double) timer <= (double) megaSlash.duration)
    {
      timer += Time.deltaTime;
      float time = Mathf.Clamp01(timer / megaSlash.duration);
      if ((double) time >= 0.5 && !isActive)
      {
        isActive = true;
        megaSlash.collider.SetActive(false);
      }
      else
        megaSlash.damageCollider.TriggerCheckCollision();
      float num = Mathf.Lerp(megaSlash.minScale, megaSlash.maxScale, norm);
      megaSlash.pivot.transform.localScale = Vector3.Lerp(new Vector3(0.0f, 0.0f, 1f), new Vector3(num, num, 1f), megaSlash.scaleCurve.Evaluate(time));
      float b = Mathf.Lerp(0.35f, 1f, norm);
      megaSlash.groundScorchRenderer.material.SetFloat("_BurnPos", Mathf.Lerp(0.0f, b, megaSlash.scaleCurve.Evaluate(time)));
      megaSlash.groundScorchRenderer.material.SetFloat("_SpreadThreshold", megaSlash.fireWallSpreadCurve.Evaluate(time));
      megaSlash.groundScorchRenderer.material.SetFloat("_ScorchPos", megaSlash.groundScorchCleanupCurve.Evaluate(time));
      Vector2 vector2 = new Vector2(megaSlash.pivot.transform.localScale.x, megaSlash.pivot.transform.localScale.z) / new Vector2(1f, 1f);
      megaSlash.fireWallRenderer.material.SetVector("_NoiseInvScale", (Vector4) vector2);
      megaSlash.fireWallRenderer.material.SetFloat("_SpreadThreshold", megaSlash.fireWallSpreadCurve.Evaluate(time));
      megaSlash.fireWallRenderer.material.SetFloat("_DeformHeight", time);
      megaSlash.fireWallLightingRenderer.material = megaSlash.fireWallRenderer.material;
      megaSlash.fireWallLightingRenderer.material.SetColor("_Color", Color.white);
      yield return (object) null;
    }
    megaSlash.slashRoutine = (Coroutine) null;
    megaSlash.pivot.SetActive(false);
    Object.Destroy((Object) megaSlash.gameObject);
  }

  public GameObject GetOwner() => this.owner;

  public void SetOwner(GameObject owner) => this.owner = owner;

  public void SetDamageMultiplier(float multiplier = 1f) => this.damageMultiplier = multiplier;
}
