using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;

namespace OK.Mobisis
{
    public static class Helper
    {        
        public static async Task StartTask()
        {
            var taskName = "UpdatePeriodTask";

            var backgroundAccessStatus = await BackgroundExecutionManager.RequestAccessAsync();

            if (backgroundAccessStatus == BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity ||
                backgroundAccessStatus == BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity)
            {
                foreach (var task in BackgroundTaskRegistration.AllTasks)
                {
                    if (task.Value.Name == taskName)
                        return;
                }

                var taskBuilder = new BackgroundTaskBuilder();
                taskBuilder.Name = taskName;
                taskBuilder.TaskEntryPoint = typeof(OK.Mobisis.BackgroundTasks.UpdatePeriodTask).FullName;
                taskBuilder.SetTrigger(new TimeTrigger(20, false));

                var registration = taskBuilder.Register();
            }
        }

        public static async Task EndTask()
        {
            var taskName = "UpdatePeriodTask";

            var backgroundAccessStatus = await BackgroundExecutionManager.RequestAccessAsync();

            if (backgroundAccessStatus == BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity ||
                backgroundAccessStatus == BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity)
            {
                foreach (var task in BackgroundTaskRegistration.AllTasks)
                {
                    if (task.Value.Name == taskName)
                        task.Value.Unregister(true);
                }
            }
        }
    }
}
