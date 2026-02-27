// Decompiled with JetBrains decompiler
// Type: RedBlueGames.Tools.TextTyper.TextAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using System;
using TMPro;
using UnityEngine;

#nullable disable
namespace RedBlueGames.Tools.TextTyper;

[RequireComponent(typeof (TextMeshProUGUI))]
public abstract class TextAnimation : MonoBehaviour
{
  [Tooltip("0-based index of the first printable character that should be animated")]
  [SerializeField]
  private int firstCharToAnimate;
  [Tooltip("0-based index of the last printable character that should be animated")]
  [SerializeField]
  private int lastCharToAnimate;
  [Tooltip("If true, animation will begin playing immediately on Awake")]
  [SerializeField]
  private bool playOnAwake;
  private const float frameRate = 30f;
  private static readonly float timeBetweenAnimates = 0.0333333351f;
  private float lastAnimateTime;
  private TextMeshProUGUI textComponent;
  private TMP_TextInfo textInfo;
  private TMP_MeshInfo[] cachedMeshInfo;

  protected int FirstCharToAnimate => this.firstCharToAnimate;

  protected int LastCharToAnimate => this.lastCharToAnimate;

  private TextMeshProUGUI TextComponent
  {
    get
    {
      if ((UnityEngine.Object) this.textComponent == (UnityEngine.Object) null)
        this.textComponent = this.GetComponent<TextMeshProUGUI>();
      return this.textComponent;
    }
  }

  public void SetCharsToAnimate(int firstChar, int lastChar)
  {
    this.firstCharToAnimate = firstChar;
    this.lastCharToAnimate = lastChar;
  }

  public void CacheTextMeshInfo()
  {
    this.textInfo = this.TextComponent.textInfo;
    this.cachedMeshInfo = this.textInfo.CopyMeshInfoVertexData();
  }

  protected virtual void Awake() => this.enabled = this.playOnAwake;

  protected virtual void Start()
  {
    this.TextComponent.ForceMeshUpdate(false, false);
    this.lastAnimateTime = float.MinValue;
  }

  protected virtual void OnEnable()
  {
    TMPro_EventManager.TEXT_CHANGED_EVENT.Add(new Action<UnityEngine.Object>(this.OnTMProChanged));
  }

  protected virtual void OnDisable()
  {
    TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(new Action<UnityEngine.Object>(this.OnTMProChanged));
    this.TextComponent.ForceMeshUpdate(false, false);
  }

  protected virtual void Update()
  {
    if ((double) Time.time <= (double) this.lastAnimateTime + (double) TextAnimation.timeBetweenAnimates)
      return;
    this.AnimateAllChars();
  }

  protected abstract void Animate(
    int characterIndex,
    out Vector2 translation,
    out float rotation,
    out float scale);

  public void AnimateAllChars()
  {
    this.lastAnimateTime = Time.time;
    int characterCount = this.textInfo.characterCount;
    if (characterCount == 0)
      return;
    for (int characterIndex = 0; characterIndex < characterCount; ++characterIndex)
    {
      if (characterIndex >= this.firstCharToAnimate && characterIndex <= this.lastCharToAnimate)
      {
        TMP_CharacterInfo tmpCharacterInfo = this.textInfo.characterInfo[characterIndex];
        if (tmpCharacterInfo.isVisible)
        {
          int materialReferenceIndex = tmpCharacterInfo.materialReferenceIndex;
          int vertexIndex = tmpCharacterInfo.vertexIndex;
          Vector3[] vertices1 = this.cachedMeshInfo[materialReferenceIndex].vertices;
          Vector3 vector3 = (Vector3) (Vector2) ((vertices1[vertexIndex] + vertices1[vertexIndex + 2]) / 2f);
          Vector3[] vertices2 = this.textInfo.meshInfo[materialReferenceIndex].vertices;
          vertices2[vertexIndex] = vertices1[vertexIndex] - vector3;
          vertices2[vertexIndex + 1] = vertices1[vertexIndex + 1] - vector3;
          vertices2[vertexIndex + 2] = vertices1[vertexIndex + 2] - vector3;
          vertices2[vertexIndex + 3] = vertices1[vertexIndex + 3] - vector3;
          Vector2 translation;
          float rotation;
          float scale;
          this.Animate(characterIndex, out translation, out rotation, out scale);
          Matrix4x4 matrix4x4 = Matrix4x4.TRS((Vector3) translation, Quaternion.Euler(0.0f, 0.0f, rotation), scale * Vector3.one);
          vertices2[vertexIndex] = matrix4x4.MultiplyPoint3x4(vertices2[vertexIndex]);
          vertices2[vertexIndex + 1] = matrix4x4.MultiplyPoint3x4(vertices2[vertexIndex + 1]);
          vertices2[vertexIndex + 2] = matrix4x4.MultiplyPoint3x4(vertices2[vertexIndex + 2]);
          vertices2[vertexIndex + 3] = matrix4x4.MultiplyPoint3x4(vertices2[vertexIndex + 3]);
          vertices2[vertexIndex] += vector3;
          vertices2[vertexIndex + 1] += vector3;
          vertices2[vertexIndex + 2] += vector3;
          vertices2[vertexIndex + 3] += vector3;
        }
      }
    }
    this.ApplyChangesToMesh();
  }

  private void ApplyChangesToMesh()
  {
    for (int index = 0; index < this.textInfo.meshInfo.Length; ++index)
    {
      this.textInfo.meshInfo[index].mesh.vertices = this.textInfo.meshInfo[index].vertices;
      this.TextComponent.UpdateGeometry(this.textInfo.meshInfo[index].mesh, index);
    }
  }

  private void OnTMProChanged(UnityEngine.Object obj)
  {
    if (!(obj == (UnityEngine.Object) this.TextComponent))
      return;
    this.CacheTextMeshInfo();
  }
}
