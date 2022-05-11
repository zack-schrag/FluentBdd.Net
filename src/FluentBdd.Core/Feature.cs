using System.Reflection.PortableExecutable;
using System.Text;
using Stubble.Core.Builders;
using Stubble.Core.Settings;

namespace FluentBdd.Core;

public class Feature
{
    public List<IScenarioResult> ScenarioResults { get; }
    public string Name { get; }
    public string? Description { get; }
    public List<string> _tags;


    public Feature(string name, string? description = null)
    {
        Name = name;
        Description = description;
        _tags = new List<string>();
        ScenarioResults = new List<IScenarioResult>();
    }

    public void AddTag(string tag)
    {
        _tags.Add(tag);
    }

    public void AddScenarioResult(IScenarioResult result)
    {
        ScenarioResults.Add(result);
    }

    public void Output(string? resultsPath = null)
    {
        var stubble = new StubbleBuilder().Build();

        using StreamReader streamReader = new("result_template.md", Encoding.UTF8);
        var output = stubble.Render(streamReader.ReadToEnd(), this, new RenderSettings
        {
            SkipHtmlEncoding = true
        });

        var cleanedName = Name.ToLower().Replace(" ", "_");
        var outputFile = resultsPath ?? $"{cleanedName}.md";
        File.WriteAllText(outputFile, output);
    }
}
