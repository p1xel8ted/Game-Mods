using UnityEngine;
using UnityEngine.SceneManagement;

namespace AnAlchemicalCollection;

public class TimePatches : MonoBehaviour
{
    private const float DefaultHourPerSec = 45f;
    public void Start()
    {
        Plugin.L("TimePatches.Start",true);;
    }


    public void ResetValues()
    {
        if(TimeManager.timeManagerSetting == null)
        {
            Plugin.L("TimeManager.timeManagerSetting is null",true);
            return;
        }
        TimeManager.timeManagerSetting.hourPerSec = DefaultHourPerSec;
    }

    public void UpdateValues()
    {
        if(TimeManager.timeManagerSetting == null)
        {
            Plugin.L("TimeManager.timeManagerSetting is null",true);
            return;
        }
        TimeManager.timeManagerSetting.hourPerSec = Mathf.RoundToInt(DefaultHourPerSec * Plugin.TimeMultiplier.Value);
        Plugin.L($"TimeManager.hourPerSec set to {TimeManager.timeManagerSetting.hourPerSec}",true);

    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        UpdateValues();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
        ResetValues();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
        UpdateValues();
    }
}