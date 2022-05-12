// Copyright (c) 2016-2022 James Skimming. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using BenchmarkDotNet.Running;

////BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);

BenchmarkRunner.Run(typeof(Program).Assembly);
////Summary summary =
////    BenchmarkRunner.Run<IncompleteResultTaskAsynchronousInterceptorBenchmarks>();
