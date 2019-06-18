// Copyright (c) 2016 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit.Abstractions;

    public class ListLogger
    {
        private readonly List<string> _log = new List<string>(8);

        private readonly object _lock = new object();

        private readonly ITestOutputHelper _output;

        public ListLogger(ITestOutputHelper output)
        {
            _output = output ?? throw new ArgumentNullException(nameof(output));
        }

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

                    throw new ArgumentOutOfRangeException(
                        nameof(index),
                        $"There are '{_log.Count} logs but the index '{index}' was expected. " +
                        $"{string.Join(Environment.NewLine, _log.Prepend("Logs:"))}");
                }
            }
        }

        public string First()
        {
            lock (_lock)
            {
                return this[0];
            }
        }

        public string Last()
        {
            lock (_lock)
            {
                return this[_log.Count - 1];
            }
        }

        public void Add(string message)
        {
            _output.WriteLine(message);
            lock (_lock)
            {
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
}
