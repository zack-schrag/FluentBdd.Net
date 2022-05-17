using System;
using FluentBdd.Net;

namespace Xunit.Samples;

public class MyFeatureFixture : IDisposable
{
    public MyFeatureFixture()
    {
        Feature = new Feature("My feature", description: "foo bar my feature is cool");
    }

    public Feature Feature { get; set; }

    public void Dispose()
    {
        Feature.Output();
    }
}
