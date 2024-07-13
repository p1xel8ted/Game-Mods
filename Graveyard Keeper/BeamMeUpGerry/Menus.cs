namespace BeamMeUpGerry;

public static class Menus
{
    private readonly static Dictionary<string, Action> MenuActions = new()
    {
        {strings.Page_1, () => ShowMultiAnswer(LocationLists.Locations[0])},
        {strings.Page_2, () => ShowMultiAnswer(LocationLists.Locations[1])},
        {strings.Page_3, () => ShowMultiAnswer(LocationLists.Locations[2])},
        {strings.Page_4, () => ShowMultiAnswer(LocationLists.Locations[3])},
        {strings.Page_5, () => ShowMultiAnswer(LocationLists.Locations[4])},
        {strings.Page_6, () => ShowMultiAnswer(LocationLists.Locations[5])},
        {strings.Page_7, () => ShowMultiAnswer(LocationLists.Locations[6])},
        {strings.Page_8, () => ShowMultiAnswer(LocationLists.Locations[7])},
        {strings.Page_9, () => ShowMultiAnswer(LocationLists.Locations[8])},
        {strings.Page_10, () => ShowMultiAnswer(LocationLists.Locations[9])},
        {Constants.Cancel, Helpers.EnablePlayerControl}
    };

    internal static void ShowMultiAnswer(List<AnswerVisualData> answers)
    {
        Helpers.DisablePlayerControl();

        MainGame.me.player.ShowMultianswer(answers, MenuOnOnChosen, talker: MainGame.me.player);
    }

    private static void MenuOnOnChosen(string chosen)
    {
        if (MenuActions.TryGetValue(chosen, out var action))
        {
            action.Invoke();
            return;
        }

        var allLocations = new[] {LocationLists.AllLocations};
        var chosenLocation = allLocations.SelectMany(list => list).FirstOrDefault(loc => loc.zone == chosen);

        if (chosenLocation != null)
        {
            if (chosenLocation.defaultLocation)
            {
                Teleport.TryTeleport(chosenLocation);
            }
            else
            {
                if (Helpers.HasTheMoney())
                {
                    Teleport.TryTeleport(chosenLocation);
                }
            }
        }

        Helpers.EnablePlayerControl();
    }
}