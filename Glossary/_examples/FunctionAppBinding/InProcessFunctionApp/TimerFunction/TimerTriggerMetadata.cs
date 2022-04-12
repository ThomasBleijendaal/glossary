using Microsoft.Azure.WebJobs.Script.Description;
using Newtonsoft.Json;

namespace InProcessFunctionApp.TimerFunction;

internal class TimerTriggerMetadata
{
    public TimerTriggerMetadata(string scheduleExpression)
    {
        ScheduleExpression = scheduleExpression;
    }

    [JsonProperty("name")]
    public string Name { get; set; } = "timer";

    [JsonProperty("type")]
    public string Type { get; set; } = "timerTrigger";

    [JsonProperty("direction")]
    public BindingDirection Direction { get; set; } = BindingDirection.In;

    [JsonProperty("schedule")]
    public string ScheduleExpression { get; set; }
}
