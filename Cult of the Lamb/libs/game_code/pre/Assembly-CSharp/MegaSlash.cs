// Decompiled with JetBrains decompiler
// Type: MegaSlash
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System.Collections;
using UnityEngine;

#nullable disable
[ExecuteInEditMode]
public class MegaSlash : BaseMonoBehaviour
{
  [SerializeField]
  private GameObject symbol;
  [SerializeField]
  private GameObject pivot;
  [SerializeField]
  private SpriteRenderer renderer;
  [SerializeField]
  private GameObject collider;
  [SerializeField]
  private float minScale = 0.3f;
  [SerializeField]
  private float maxScale = 0.75f;
  [SerializeField]
  private AnimationCurve scaleCurve = new AnimationCurve(new Keyframe[2]
  {
    new Keyframe(0.0f, 0.0f),
    new Keyframe(1f, 1f)
  });
  [SerializeField]
  private MeshRenderer fireWallRenderer;
  private MeshRenderer fireWallLightingRenderer;
  [SerializeField]
  private AnimationCurve fireWallSpreadCurve = new AnimationCurve(new Keyframe[3]
  {
    new Keyframe(0.0f, 0.0f),
    new Keyframe(0.5f, 0.5f),
    new Keyframe(1f, 1f)
  });
  [SerializeField]
  private MeshRenderer groundScorchRenderer;
  [SerializeField]
  private AnimationCurve groundScorchCleanupCurve = new AnimationCurve(new Keyframe[2]
  {
    new Keyframe(0.666f, 0.0f),
    new Keyframe(1f, 1f)
  });
  [SerializeField]
  private float duration = 0.75f;

  private void DefaultSizedButton() => this.Play(1f);

  public void Play(float norm)
  {
    if ((Object) this.fireWallRenderer == (Object) null || (Object) this.groundScorchRenderer == (Object) null)
      return;
    this.GetComponentInChildren<DamageCollider>().Damage = EquipmentManager.GetCurseData(DataManager.Instance.CurrentCurse).Damage * PlayerSpells.GetCurseDamageMultiplier();
    this.collider.GetComponent<DamageCollider>().DestroyBullets = true;
    this.StopAllCoroutines();
    this.StartCoroutine((IEnumerator) this.SlashRoutine(norm));
    Debug.Log((object) ("Size: " + (object) norm));
  }

  private IEnumerator SlashRoutine(float norm)
  {
    MegaSlash megaSlash = this;
    PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.CustomAnimation;
    megaSlash.symbol.SetActive(true);
    megaSlash.pivot.SetActive(false);
    PlayerFarming.Instance.Spine.AnimationState.SetAnimation(0, "Curses/attack-slash-curse", false);
    PlayerFarming.Instance.Spine.AnimationState.AddAnimation(0, "idle", false, 0.0f);
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
        PlayerFarming.Instance.state.CURRENT_STATE = StateMachine.State.Idle;
        isActive = true;
        megaSlash.collider.SetActive(false);
      }
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
    megaSlash.pivot.SetActive(false);
    Object.Destroy((Object) megaSlash.gameObject);
  }
}
