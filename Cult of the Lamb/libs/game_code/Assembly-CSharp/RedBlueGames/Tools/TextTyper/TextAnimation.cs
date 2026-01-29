// Decompiled with JetBrains decompiler
// Type: RedBlueGames.Tools.TextTyper.TextAnimation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

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
  public int firstCharToAnimate;
  [Tooltip("0-based index of the last printable character that should be animated")]
  [SerializeField]
  public int lastCharToAnimate;
  [Tooltip("If true, animation will begin playing immediately on Awake")]
  [SerializeField]
  public bool playOnAwake;
  public const float frameRate = 30f;
  public static float timeBetweenAnimates = 0.0333333351f;
  public float lastAnimateTime;
  public TextMeshProUGUI textComponent;
  public TMP_TextInfo textInfo;
  public TMP_MeshInfo[] cachedMeshInfo;

  public int FirstCharToAnimate => this.firstCharToAnimate;

  public int LastCharToAnimate => this.lastCharToAnimate;

  public TextMeshProUGUI TextComponent
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

  public virtual void Awake() => this.enabled = this.playOnAwake;

  public virtual void Start()
  {
    this.TextComponent.ForceMeshUpdate(false, false);
    this.lastAnimateTime = float.MinValue;
  }

  public virtual void OnEnable()
  {
    TMPro_EventManager.TEXT_CHANGED_EVENT.Add(new Action<UnityEngine.Object>(this.OnTMProChanged));
  }

  public virtual void OnDisable()
  {
    TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(new Action<UnityEngine.Object>(this.OnTMProChanged));
    this.TextComponent.ForceMeshUpdate(false, false);
  }

  public virtual void Update()
  {
    if ((double) Time.time <= (double) this.lastAnimateTime + (double) TextAnimation.timeBetweenAnimates)
      return;
    this.AnimateAllChars();
  }

  public abstract void Animate(
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

  public void ApplyChangesToMesh()
  {
    for (int index = 0; index < this.textInfo.meshInfo.Length; ++index)
    {
      this.textInfo.meshInfo[index].mesh.vertices = this.textInfo.meshInfo[index].vertices;
      this.TextComponent.UpdateGeometry(this.textInfo.meshInfo[index].mesh, index);
    }
  }

  public void OnTMProChanged(UnityEngine.Object obj)
  {
    if (!(obj == (UnityEngine.Object) this.TextComponent))
      return;
    this.CacheTextMeshInfo();
  }
}
