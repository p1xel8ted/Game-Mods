// Decompiled with JetBrains decompiler
// Type: MouseManager
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 1F1BB429-82E6-41C3-9004-EF845C927D09
// Assembly location: F:\OneDrive\Development\Game-Mods\Cult of the Lamb\libs\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class MouseManager : BaseMonoBehaviour
{
  public TargetMarker target;
  public static MouseManager Instance;
  public LineRenderer lineRenderer;
  public Transform lineStartPosition;
  public bool showLine;
  public bool dragCam;
  public Vector3 oldPos;
  public Vector3 panOrigin;
  public Vector3 panTargetPosition;
  public Vector3 camZoom;
  public float camZoomTarget;

  public void Start()
  {
    MouseManager.Instance = this;
    this.lineRenderer = this.GetComponent<LineRenderer>();
    this.lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
    this.lineRenderer.startWidth = 0.05f;
    this.lineRenderer.endWidth = 0.05f;
    this.lineRenderer.positionCount = 2;
    this.lineRenderer.enabled = false;
    this.panTargetPosition = Camera.main.transform.position;
    this.camZoom.z = 0.0f;
  }

  public void Update()
  {
    if (this.showLine)
    {
      if ((Object) this.lineStartPosition == (Object) null)
      {
        this.showLine = false;
      }
      else
      {
        this.lineRenderer.SetPosition(0, this.lineStartPosition.position);
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition with
        {
          z = -Camera.main.transform.position.z
        });
        this.lineRenderer.SetPosition(1, worldPoint);
        MouseManager.Instance.target.reveal(worldPoint);
      }
    }
    if (Input.GetMouseButtonDown(1))
    {
      this.dragCam = true;
      this.oldPos = Camera.main.transform.transform.position - this.camZoom;
      this.panOrigin = Camera.main.ScreenToViewportPoint(Input.mousePosition);
    }
    if (Input.GetMouseButton(1))
      this.panTargetPosition = this.oldPos + -(Camera.main.ScreenToViewportPoint(Input.mousePosition) - this.panOrigin) * -Camera.main.transform.position.z;
    if (Input.GetMouseButtonUp(1))
      this.dragCam = false;
    if ((double) Input.GetAxis("Mouse ScrollWheel") > 0.0)
      ++this.camZoomTarget;
    if ((double) Input.GetAxis("Mouse ScrollWheel") < 0.0)
      --this.camZoomTarget;
    if ((double) this.camZoomTarget < -4.0)
      this.camZoomTarget = -4f;
    if ((double) this.camZoomTarget > 2.0)
      this.camZoomTarget = 2f;
    this.camZoom.z += (float) (((double) this.camZoomTarget - (double) this.camZoom.z) / 5.0);
    Vector3 position = Camera.main.transform.position;
    Camera.main.transform.position = position + (this.panTargetPosition + this.camZoom - position) / 3f;
  }

  public void OnDestroy() => MouseManager.Instance = (MouseManager) null;

  public static MouseManager getInstance() => MouseManager.Instance;

  public static void placeTarget(Vector3 position)
  {
    if ((Object) MouseManager.Instance == (Object) null)
      return;
    MouseManager.Instance.target.reveal(position);
  }

  public static void ShowLine(Transform position)
  {
    if ((Object) MouseManager.Instance == (Object) null)
      return;
    MouseManager.Instance.lineRenderer.enabled = true;
    MouseManager.Instance.lineStartPosition = position;
    MouseManager.Instance.showLine = true;
  }

  public static void HideLine()
  {
    if ((Object) MouseManager.Instance == (Object) null)
      return;
    MouseManager.Instance.showLine = false;
    MouseManager.Instance.lineRenderer.enabled = false;
  }
}
