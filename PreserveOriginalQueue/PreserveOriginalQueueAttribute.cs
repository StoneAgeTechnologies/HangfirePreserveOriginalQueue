using Hangfire.Common;
using Hangfire.States;
using Hangfire.Storage;

namespace PreserveOriginalQueue
{
    public class PreserveOriginalQueueAttribute : JobFilterAttribute, IApplyStateFilter
    {
        public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            var enqueuedState = context.NewState as EnqueuedState;

            if (Not_Scheduling_Background_Job(enqueuedState)) return;

            var originalQueue = Get_Original_Queue(context);
            if (Original_Queue_Specified(originalQueue))
            {
                Set_Queue(enqueuedState, originalQueue);
            }
            else
            {
                Queue_For_First_Time(context, enqueuedState);
            }
        }

        private static string Get_Original_Queue(ApplyStateContext context)
        {
            var queueNameJson = context.Connection.GetJobParameter(
                context.BackgroundJob.Id,
                "OriginalQueue");
            var originalQueue = JobHelper.FromJson<string>(queueNameJson);
            return originalQueue;
        }

        public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
        {
            // empty due to inheritance 
        }

        private static void Set_Queue(EnqueuedState enqueuedState, string originalQueue)
        {
            enqueuedState.Queue = originalQueue;
        }

        private static void Queue_For_First_Time(ApplyStateContext context, EnqueuedState enqueuedState)
        {
            context.Connection.SetJobParameter(
                context.BackgroundJob.Id,
                "OriginalQueue",
                JobHelper.ToJson(enqueuedState.Queue));
        }

        private static bool Original_Queue_Specified(string originalQueue)
        {
            return originalQueue != null;
        }

        private static bool Not_Scheduling_Background_Job(EnqueuedState enqueuedState)
        {
            return enqueuedState == null;
        }
    }
}
