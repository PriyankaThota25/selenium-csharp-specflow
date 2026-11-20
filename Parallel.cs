using NUnit.Framework;

// Each .feature file (NUnit fixture) runs in parallel; scenarios inside one feature stay sequential.
// Every scenario gets its own BoDi container, WebDriver and recorder, so no state is shared between threads.
[assembly: Parallelizable(ParallelScope.Fixtures)]
[assembly: LevelOfParallelism(4)]
