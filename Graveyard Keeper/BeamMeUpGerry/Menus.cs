namespace BeamMeUpGerry;

public static class Menus
{
    private static Dictionary<string, Action> MenuActions => new()
    {
        {Language.GetTranslation(Language.Terms.Page1), () => ShowMultiAnswer(LocationLists.Locations[0])},
        {Language.GetTranslation(Language.Terms.Page2), () => ShowMultiAnswer(LocationLists.Locations[1])},
        {Language.GetTranslation(Language.Terms.Page3), () => ShowMultiAnswer(LocationLists.Locations[2])},
        {Language.GetTranslation(Language.Terms.Page4), () => ShowMultiAnswer(LocationLists.Locations[3])},
        {Language.GetTranslation(Language.Terms.Page5), () => ShowMultiAnswer(LocationLists.Locations[4])},
        {Language.GetTranslation(Language.Terms.Page6), () => ShowMultiAnswer(LocationLists.Locations[5])},
        {Language.GetTranslation(Language.Terms.Page7), () => ShowMultiAnswer(LocationLists.Locations[6])},
        {Language.GetTranslation(Language.Terms.Page8), () => ShowMultiAnswer(LocationLists.Locations[7])},
        {Language.GetTranslation(Language.Terms.Page9), () => ShowMultiAnswer(LocationLists.Locations[8])},
        {Language.GetTranslation(Language.Terms.Page10), () => ShowMultiAnswer(LocationLists.Locations[9])},
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