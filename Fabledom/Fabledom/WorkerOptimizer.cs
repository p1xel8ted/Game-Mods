using System.Collections;
using Nielsen;

namespace Fabledom;

internal enum WorkerOptimizeTrigger
{
    Disabled,
    SeasonChange,
    NewBuilding,
    All
}

internal static class WorkerOptimizer
{
    private const int SwapsPerFrame = 5;
    private static bool IsRunning;

    private static string WorkplaceName(WorkPlace wp) => wp.worldObject.data.title.GetLocalizedString();

    private static string WorkerName(Worker w) => w.unit.GetLocalizedBirthName();

    internal static IEnumerator OptimizeCoroutine()
    {
        if (IsRunning)
        {
            Plugin.Log.LogInfo("Worker optimization already running, skipping");
            yield break;
        }

        IsRunning = true;

        Plugin.Log.LogInfo("=== Worker Optimization Started ===");

        // Collect all employed workers with housing (skip fairies)
        var employed = new List<Worker>();
        foreach (var worker in Worker.Instances)
        {
            if (worker.fablingClass == FablingClass.FAIRY) continue;
            if (!worker.HasWorkPlace()) continue;
            if (worker.resident == null || !worker.resident.HasHousing()) continue;
            employed.Add(worker);
        }

        // Collect unemployed workers with housing
        var unemployed = new List<Worker>();
        foreach (var worker in Worker.Instances)
        {
            if (worker.fablingClass == FablingClass.FAIRY) continue;
            if (worker.HasWorkPlace()) continue;
            if (worker.resident == null || !worker.resident.HasHousing()) continue;
            unemployed.Add(worker);
        }

        Plugin.Log.LogInfo($"Found {employed.Count} employed, {unemployed.Count} unemployed with housing");

        if (employed.Count == 0)
        {
            Plugin.Log.LogInfo("No eligible workers to optimize");
            Plugin.Log.LogInfo("=== Worker Optimization Complete ===");
            IsRunning = false;
            yield break;
        }

        // Phase 1: Compute unemployed replacements
        Plugin.Log.LogInfo("--- Phase 1: Unemployed Replacements ---");
        var replacements = new List<(Worker oldWorker, Worker newWorker, WorkPlace workplace)>();
        var changed = true;
        while (changed)
        {
            changed = false;
            for (var i = employed.Count - 1; i >= 0; i--)
            {
                var worker = employed[i];
                if (!worker.HasWorkPlace()) continue;

                var wp = worker.workPlace;
                var currentDist = worker.GetDistanceBetweenHomeAndWork(wp);

                Worker bestUnemployed = null;
                var bestDist = currentDist;

                foreach (var candidate in unemployed)
                {
                    if (!wp.gridObject.wo.data.workPlaceFablingClasses.Contains(candidate.fablingClass)) continue;
                    var candidateDist = candidate.GetDistanceBetweenHomeAndWork(wp);
                    if (candidateDist < bestDist)
                    {
                        bestDist = candidateDist;
                        bestUnemployed = candidate;
                    }
                }

                if (bestUnemployed == null) continue;

                replacements.Add((worker, bestUnemployed, wp));
                unemployed.Remove(bestUnemployed);
                unemployed.Add(worker);
                employed.Remove(worker);
                employed.Add(bestUnemployed);

                changed = true;
                break;
            }
        }

        // Phase 2: Compute pairwise swaps
        Plugin.Log.LogInfo("--- Phase 2: Pairwise Swaps ---");
        var swaps = new List<(Worker workerA, Worker workerB, WorkPlace wpX, WorkPlace wpY, float saved)>();
        changed = true;
        while (changed)
        {
            changed = false;
            for (var i = 0; i < employed.Count; i++)
            {
                for (var j = i + 1; j < employed.Count; j++)
                {
                    var workerA = employed[i];
                    var workerB = employed[j];

                    if (!workerA.HasWorkPlace() || !workerB.HasWorkPlace()) continue;

                    var wpX = workerA.workPlace;
                    var wpY = workerB.workPlace;
                    if (wpX == wpY) continue;

                    if (!wpY.gridObject.wo.data.workPlaceFablingClasses.Contains(workerA.fablingClass)) continue;
                    if (!wpX.gridObject.wo.data.workPlaceFablingClasses.Contains(workerB.fablingClass)) continue;

                    var currentTotal = workerA.GetDistanceBetweenHomeAndWork(wpX) + workerB.GetDistanceBetweenHomeAndWork(wpY);
                    var swappedTotal = workerA.GetDistanceBetweenHomeAndWork(wpY) + workerB.GetDistanceBetweenHomeAndWork(wpX);

                    if (swappedTotal >= currentTotal) continue;

                    swaps.Add((workerA, workerB, wpX, wpY, currentTotal - swappedTotal));

                    // Simulate the swap in our local tracking
                    workerA.workPlace = wpY;
                    workerB.workPlace = wpX;

                    changed = true;
                    break;
                }

                if (changed) break;
            }
        }

        // Restore original workplaces before applying (simulation changed them)
        // We need to undo the simulated swaps in reverse order
        for (var i = swaps.Count - 1; i >= 0; i--)
        {
            var (workerA, workerB, wpX, wpY, _) = swaps[i];
            workerA.workPlace = wpX;
            workerB.workPlace = wpY;
        }

        // Apply Phase 1 — spread across frames
        var appliedThisFrame = 0;
        var phase1Applied = 0;
        foreach (var (oldWorker, newWorker, workplace) in replacements)
        {
            Plugin.Log.LogInfo($"  {WorkerName(newWorker)} (unemployed, dist: {newWorker.GetDistanceBetweenHomeAndWork(workplace):F0}) replaces {WorkerName(oldWorker)} (dist: {oldWorker.GetDistanceBetweenHomeAndWork(workplace):F0}) at {WorkplaceName(workplace)}");

            workplace.RemoveWorker(oldWorker);
            newWorker.StartProfession(workplace);
            phase1Applied++;
            appliedThisFrame++;

            if (appliedThisFrame >= SwapsPerFrame)
            {
                appliedThisFrame = 0;
                yield return null;
            }
        }

        if (phase1Applied == 0)
        {
            Plugin.Log.LogInfo("  No beneficial unemployed replacements found");
        }

        // Apply Phase 2 — spread across frames
        var phase2Applied = 0;
        foreach (var (workerA, workerB, wpX, wpY, saved) in swaps)
        {
            Plugin.Log.LogInfo($"  Swap: {WorkerName(workerA)} ({WorkplaceName(wpX)} -> {WorkplaceName(wpY)}) <-> {WorkerName(workerB)} ({WorkplaceName(wpY)} -> {WorkplaceName(wpX)}) [saved: {saved:F0}]");

            wpX.RemoveWorker(workerA);
            wpY.RemoveWorker(workerB);
            workerA.StartProfession(wpY);
            workerB.StartProfession(wpX);
            phase2Applied++;
            appliedThisFrame++;

            if (appliedThisFrame >= SwapsPerFrame)
            {
                appliedThisFrame = 0;
                yield return null;
            }
        }

        if (phase2Applied == 0)
        {
            Plugin.Log.LogInfo("  No beneficial swaps found");
        }

        var totalSwaps = phase1Applied + phase2Applied;
        Plugin.Log.LogInfo($"=== Worker Optimization Complete: {totalSwaps} total changes ({phase1Applied} replacements, {phase2Applied} swaps) ===");

        if (Plugin.OptimizeNotifications.Value)
        {
            ShowModNotification(totalSwaps > 0
                ? $"Workers optimized: {totalSwaps} changes ({phase1Applied} replacements, {phase2Applied} swaps)"
                : "Workers already optimal \u2014 no changes needed");
        }

        IsRunning = false;
    }

    internal static void ShowModNotification(string text)
    {
        var container = UIManager.Instance.notificationContainer;
        if (container.pool.childCount <= 0)
        {
            container.grid.GetChild(0).SetParent(container.pool);
        }

        var notifTransform = container.pool.GetChild(0);
        var notif = notifTransform.GetComponent<Nielsen.Notification>();

        // Create minimal NotificationData so the fade timer works
        var data = ScriptableObject.CreateInstance<NotificationData>();
        data.showTime = 5f;
        data.icon = "";
        data.hasActionButton = false;
        data.hasSound = false;
        notif.data = data;

        notif.message.text = text;
        notif.iconChar.text = "";
        notif.hasLifetime = true;
        notif.unscaledTimeAlive = 0f;
        notif.isFading = false;
        notif.actionButton.gameObject.SetActive(false);
        if (notif.hasDate)
        {
            notif.date.text = DateTimeManager.Instance.GetFullDateString();
        }
        notifTransform.gameObject.SetActive(true);
        notifTransform.SetParent(container.grid);
    }
}
