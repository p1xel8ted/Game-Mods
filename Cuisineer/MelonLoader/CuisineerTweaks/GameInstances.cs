using Il2Cpp;
using Il2CppCinemachine;

namespace CuisineerTweaks;

public static class GameInstances
{
    internal static MapTransitionManager MapTransitionManagerInstance => SimpleSingleton<MapTransitionManager>.m_Instance;
    internal static CinemachineVirtualCamera PlayerCameraInstance { get; set; }
    internal static RestaurantExt RestaurantExtInstance { get; set; }
    internal static PlayerRuntimeData PlayerRuntimeDataInstance { get; set; }
    internal static RestaurantDataManager RestaurantDataManagerInstance => SimpleSingleton<RestaurantDataManager>.m_Instance;
    internal static CalendarManager CalendarManagerInstance => SimpleSingleton<CalendarManager>.m_Instance;
    internal static UI_GameplayOptions GameplayOptionsInstance { get; set; }
    internal static TextPopupManager TextPopupManagerInstance => SimpleSingleton<TextPopupManager>.m_Instance;
}