// Copyright (c) 2016 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Castle.DynamicProxy
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ListLogger
    {
        private readonly List<string> _log = new List<string>(8);

        private readonly object _lock = new object();

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
                    return _log[index];
                }
            }
        }

        public string First()
        {
            lock (_lock)
            {
                return _log[0];
            }
        }

        public string Last()
        {
            lock (_lock)
            {
                return _log[_log.Count - 1];
            }
        }

        public void Add(string message)
        {
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
