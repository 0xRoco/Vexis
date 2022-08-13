using System;

namespace Vexis.Client.Core;

public abstract class LazySingletonBase<T> where T : class, new()
{
    private static readonly Lazy<T> Lazy = new(() => new T());
    public static T Instance => Lazy.Value;
}