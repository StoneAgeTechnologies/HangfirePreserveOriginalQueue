# HangfirePreserveOriginalQueue
A scheduled task attribute to address a rescheduling bug in HangFire where it places the rescheduled job into the default queue instead of the one it was originally executed on. 

## Usage

Annotate your scheduled task class with the attribute
```
[PreserveOriginalQueue]
public class UserSyncTask : IUserSyncTask{
   ....
}
```

And then add the filter to your Hangfire configuration
```
 GlobalConfiguration.Configuration.UseFilter(new PreserveOriginalQueueAttribute());
```
