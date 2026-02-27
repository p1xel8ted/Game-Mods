// Decompiled with JetBrains decompiler
// Type: WeatherController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D4FAC018-F15B-4650-BC23-66B6B15D1655
// Assembly location: G:\CultOfTheLambPreRitualNerf\depots\1313141\21912051\Cult Of The Lamb_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

#nullable disable
public class WeatherController : MonoBehaviour
{
  public static System.Action OnWeatherChanged;
  public WeatherManager weatherManager;
  public int chanceOfRain = 25;
  public int chanceOfWind = 25;
  public bool DisableRainChance;
  public bool DisableWindChance;
  private static WeatherController instance;
  public static bool isRaining;
  public static bool InWeatherOverride;
  private static bool insideBuilding;
  public bool cacheInsideBuilding;
  public static bool IsActive;
  public float startingRainIntensity;
  public float startingWindSpeed;
  public float startingWindDensity;
  private bool weatherChanged;

  public static WeatherController Instance
  {
    get
    {
      if ((UnityEngine.Object) WeatherController.instance == (UnityEngine.Object) null)
      {
        WeatherController weatherController = new WeatherController();
      }
      return WeatherController.instance;
    }
    set => WeatherController.instance = value;
  }

  public static bool InsideBuilding
  {
    get => WeatherController.insideBuilding;
    set
    {
      Debug.Log((object) "Set Inside Building");
      WeatherController.insideBuilding = value;
      if (!((UnityEngine.Object) WeatherController.Instance != (UnityEngine.Object) null))
        return;
      WeatherController.Instance.CheckWeather();
    }
  }

  private void Start() => WeatherController.Instance = this;

  private void RandomWeather() => this.CheckWeather();

  public void SetRain()
  {
    this.weatherManager.RainIntensity = 0.2f;
    WeatherController.IsActive = true;
    WeatherController.isRaining = true;
  }

  public void SetWind()
  {
    this.weatherManager.windSpeed = 5f;
    this.weatherManager.windDensity = 0.66f;
    WeatherController.IsActive = true;
  }

  public void SetWeather()
  {
    this.weatherManager.RainIntensity = 0.2f;
    this.weatherManager.windSpeed = 5f;
    this.weatherManager.windDensity = 0.66f;
    WeatherController.IsActive = true;
    WeatherController.isRaining = true;
  }

  private void OnEnable()
  {
    WeatherController.InsideBuilding = false;
    if (CheatConsole.IN_DEMO)
    {
      TimeManager.OnNewPhaseStarted += new System.Action(this.CheckWeather);
    }
    else
    {
      this.chanceOfRain = 25;
      this.chanceOfWind = 25;
      TimeManager.OnNewDayStarted += new System.Action(this.CheckWeather);
    }
    this.startingRainIntensity = this.weatherManager.RainIntensity;
    this.startingWindSpeed = this.weatherManager.windSpeed;
    this.startingWindDensity = this.weatherManager.windDensity;
  }

  private void OnDisable()
  {
    if (CheatConsole.IN_DEMO)
      TimeManager.OnNewPhaseStarted -= new System.Action(this.CheckWeather);
    else
      TimeManager.OnNewDayStarted -= new System.Action(this.CheckWeather);
  }

  private void OnDestroy() => WeatherController.isRaining = false;

  public void CheckWeather()
  {
    if (WeatherController.InWeatherOverride)
      return;
    if (WeatherController.InsideBuilding)
    {
      Debug.Log((object) "Inside building disable rain & wind");
      this.weatherManager.RainIntensity = 0.0f;
      this.weatherManager.windSpeed = 0.0f;
      this.weatherManager.windDensity = 0.0f;
      this.cacheInsideBuilding = true;
      this.weatherManager.Init();
    }
    else
    {
      if (this.cacheInsideBuilding)
      {
        this.weatherManager.transitionRate = 0.0f;
        this.weatherManager.RainIntensity = this.startingRainIntensity;
        this.weatherManager.windSpeed = this.startingWindSpeed;
        this.weatherManager.windDensity = this.startingWindDensity;
        this.cacheInsideBuilding = false;
      }
      if (this.weatherChanged)
      {
        this.weatherManager.transitionRate = 3f;
        this.weatherManager.RainIntensity = this.startingRainIntensity;
        this.weatherManager.windSpeed = this.startingWindSpeed;
        this.weatherManager.windDensity = this.startingWindDensity;
      }
      this.weatherChanged = false;
      WeatherController.IsActive = false;
      WeatherController.isRaining = false;
      if (!this.DisableRainChance)
      {
        switch (UnityEngine.Random.Range(1, this.chanceOfRain))
        {
          case 16 /*0x10*/:
            this.weatherChanged = true;
            this.weatherManager.RainIntensity = 0.15f;
            WeatherController.IsActive = true;
            Debug.Log((object) "r == 16");
            WeatherController.isRaining = true;
            System.Action onWeatherChanged1 = WeatherController.OnWeatherChanged;
            if (onWeatherChanged1 != null)
            {
              onWeatherChanged1();
              break;
            }
            break;
          case 17:
            this.weatherChanged = true;
            this.weatherManager.RainIntensity = 0.25f;
            WeatherController.IsActive = true;
            Debug.Log((object) "r == 17");
            WeatherController.isRaining = true;
            System.Action onWeatherChanged2 = WeatherController.OnWeatherChanged;
            if (onWeatherChanged2 != null)
            {
              onWeatherChanged2();
              break;
            }
            break;
        }
      }
      if (!this.DisableWindChance)
      {
        switch (UnityEngine.Random.Range(1, this.chanceOfWind))
        {
          case 16 /*0x10*/:
            this.weatherChanged = true;
            this.weatherManager.windSpeed = 5f;
            this.weatherManager.windDensity = 0.15f;
            WeatherController.IsActive = true;
            break;
          case 17:
            this.weatherChanged = true;
            this.weatherManager.windSpeed = 8f;
            this.weatherManager.windDensity = 0.25f;
            WeatherController.IsActive = true;
            break;
        }
      }
      this.weatherManager.Init();
    }
  }
}
