﻿using System;
using System.Runtime.CompilerServices;

namespace ObserverableFunctionApp.Abstractions;

public interface ILogScopeBuilder
{
    ILogScopeBuilder AddToScope<TValue>(TValue value, [CallerArgumentExpression("value")] string argumentExpression = "");

    IDisposable BeginScope();

    IDisposable BeginScope(string message, params object?[] args);
}
