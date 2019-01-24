# HangfirePreserveOriginalQueue
A scheduled task attribute to address a rescheduling bug in HangFire where it places the rescheduled job into the default queue instead of the one it was originally executed on. 

## Useage

Annotate your scheduled task class with the attribute
```
[PreserveOriginalQueue]
public class UserSyncTask : IUserSyncTask{
   ....
}
```

And when configuring hangfire add the filter
```
 GlobalConfiguration.Configuration.UseFilter(new PreserveOriginalQueueAttribute());
```
