// Copyright (c) 2016-2022 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy;

using Xunit.Abstractions;

public class ListLogger
{
    private readonly List<string> _log = new List<string>(8);

    private readonly object _lock = new object();

    public ListLogger(ITestOutputHelper output)
    {
        Output = output ?? throw new ArgumentNullException(nameof(output));
    }

    public ITestOutputHelper Output { get; }

    public bool Disabled { get; private set; }

    public int Count
    {
        get
        {
            lock (_lock)
            {
                return _log.Count;
            }
        }
    }

    public string this[int index]
    {
        get
        {
            lock (_lock)
            {
                if (index >= 0 && index < _log.Count)
                {
                    return _log[index];
                }

                string message =
                    $"There are '{_log.Count} logs but the index '{index}' was expected. " +
                    $"{string.Join(Environment.NewLine, _log.Prepend("Logs:"))}";
                throw new ArgumentOutOfRangeException(nameof(index), message);
            }
        }
    }

    public bool Disable(bool disable = true)
    {
        lock (_lock)
        {
            bool oldValue = Disabled;
            Disabled = disable;
            return oldValue;
        }
    }

    public string First() => this[0];

    public string Last()
    {
        lock (_lock)
        {
            return this[_log.Count - 1];
        }
    }

    public void Add(string message)
    {
        lock (_lock)
        {
            if (Disabled)
                return;

            Output.WriteLine(message);
            _log.Add(message);
        }
    }

    public IReadOnlyList<string> GetLog()
    {
        lock (_lock)
        {
            return _log.ToList();
        }
    }
}
