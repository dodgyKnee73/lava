using FluentAssertions.Execution;
using System;
using System.Linq;

namespace Lava.Domain.Tests;

public static class FluentExtensions
{
  public static bool Satisfy<T>(this T assertableObject, Action<T> assertion)
  {
    using var scope = new AssertionScope();
    assertion(assertableObject);
    return !scope.Discard().Any();
  }
}