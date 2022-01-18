using System;

namespace ObserverableFunctionApp.AppTest;

public record Status(
    string Name,
    CheckStatus CheckStatus,
    CheckType CheckType,
    DateTime DateTime,
    string Message);
