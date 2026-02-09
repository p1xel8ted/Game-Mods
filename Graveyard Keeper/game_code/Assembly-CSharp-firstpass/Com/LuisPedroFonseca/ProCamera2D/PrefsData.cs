// Decompiled with JetBrains decompiler
// Type: Com.LuisPedroFonseca.ProCamera2D.PrefsData
// Assembly: Assembly-CSharp-firstpass, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD927277-3A17-461A-93C2-E51B5C84C57C
// Assembly location: F:\OneDrive\Development\Game-Mods\Graveyard Keeper\libs\Assembly-CSharp-firstpass.dll

using UnityEngine;

#nullable disable
namespace Com.LuisPedroFonseca.ProCamera2D;

public static class PrefsData
{
  public static string NumericBoundariesColorKey = "Numeric Boundaries";
  public static Color NumericBoundariesColorValue = Color.white;
  public static string TargetsMidPointColorKey = "Targets Mid Point";
  public static Color TargetsMidPointColorValue = Color.yellow;
  public static string InfluencesColorKey = "Influences Sum";
  public static Color InfluencesColorValue = Color.red;
  public static string ShakeInfluenceColorKey = "Shake Influence";
  public static Color ShakeInfluenceColorValue = Color.red;
  public static string OverallOffsetColorKey = "Overall Offset";
  public static Color OverallOffsetColorValue = Color.yellow;
  public static string CamDistanceColorKey = "Camera Distance Limit";
  public static Color CamDistanceColorValue = Color.red;
  public static string CamTargetPositionColorKey = "Camera Target Position";
  public static Color CamTargetPositionColorValue = new Color(0.3f, 0.3f, 0.1f);
  public static string CamTargetPositionSmoothedColorKey = "Camera Target Position Smoothed";
  public static Color CamTargetPositionSmoothedColorValue = new Color(0.5f, 0.3f, 0.1f);
  public static string CurrentCameraPositionColorKey = "Current Camera Position";
  public static Color CurrentCameraPositionColorValue = new Color(0.8f, 0.3f, 0.1f);
  public static string CameraWindowColorKey = "Camera Window";
  public static Color CameraWindowColorValue = Color.red;
  public static string ForwardFocusColorKey = "Forward Focus";
  public static Color ForwardFocusColorValue = Color.red;
  public static string ZoomToFitColorKey = "Zoom To Fit";
  public static Color ZoomToFitColorValue = Color.magenta;
  public static string BoundariesTriggerColorKey = "Trigger Boundaries";
  public static Color BoundariesTriggerColorValue = new Color(Color.cyan.r, Color.cyan.g, Color.cyan.b, 0.3f);
  public static string InfluenceTriggerColorKey = "Trigger Influence";
  public static Color InfluenceTriggerColorValue = new Color(Color.cyan.r, Color.cyan.g, Color.cyan.b, 0.3f);
  public static string ZoomTriggerColorKey = "Trigger Zoom";
  public static Color ZoomTriggerColorValue = new Color(Color.cyan.r, Color.cyan.g, Color.cyan.b, 0.3f);
  public static string TriggerShapeColorKey = "Trigger Shape";
  public static Color TriggerShapeColorValue = new Color(Color.cyan.r, Color.cyan.g, Color.cyan.b, 0.3f);
  public static string RailsColorKey = "Rails";
  public static Color RailsColorValue = Color.white;
  public static float RailsSnapping = 0.1f;
  public static string PanEdgesColorKey = "Pan Edges";
  public static Color PanEdgesColorValue = Color.red;
  public static string RoomsColorKey = "Rooms";
  public static Color RoomsColorValue = Color.red;
  public static float RoomsSnapping = 0.1f;
}
