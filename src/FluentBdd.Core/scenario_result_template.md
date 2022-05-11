### Scenario: {{ScenarioName}}
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
