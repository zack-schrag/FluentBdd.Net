using System.Text;
using Stubble.Core.Builders;
using Stubble.Core.Settings;

namespace FluentBdd.Net;

public class MarkdownFormatter : IResultFormatter
{
    private readonly string? _resultsPath;

    public MarkdownFormatter(string? resultsPath = null)
    {
        _resultsPath = resultsPath;
    }

    public void Output(Feature feature)
    {
        var stubble = new StubbleBuilder().Build();

        var output = stubble.Render(ResultTemplate, feature, new RenderSettings
        {
            SkipHtmlEncoding = true
        });

        var cleanedName = feature.Name.ToLower().Replace(" ", "_");
        var outputFile = _resultsPath ?? $"{cleanedName}.md";
        File.WriteAllText(outputFile, output);
    }

    private const string ResultTemplate = @"
# Feature: {{Name}}
{{Description}}

{{#ScenarioResults}}
## Scenario: {{ScenarioName}}
{{ScenarioDescription}}
{{#ScenarioStepResults}}
<details>
  <summary>{{#Passed}}✅{{/Passed}}{{#Failed}}❌{{/Failed}}<strong>{{StepType}}</strong> {{StepDescription}}</summary>
{{#HasError}}

  ```bash
{{ErrorMessage}}
  ```
{{/HasError}}
</details>
{{/ScenarioStepResults}}
<br />

{{/ScenarioResults}}

";
}
