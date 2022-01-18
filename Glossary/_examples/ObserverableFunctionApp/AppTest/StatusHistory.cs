using System.Collections.Generic;

namespace ObserverableFunctionApp.AppTest;

public record StatusHistory(
    Status CurrentStatus,
    List<Status> History);
