## 🔄 Project Awareness & Context

- **Always review `architecture.md`** before starting work to understand architecture, goals, and constraints.
- **Check or update `TASK.md`** before beginning new tasks. If the task isn’t listed, add it with a short description and today’s date.
- **Maintain consistent file structure and naming conventions** as outlined in `architecture.md`.
- **Use `appsettings.Development.json` or `.env` files** for secrets or configuration during local development.

---

## 🧱 Code Structure & Modularity

- Follow **Clean Architecture** layering:
  - `Domain/` — business rules, value objects, aggregates
  - `Application/` — use cases, interfaces, validations
  - `Infrastructure/` — repositories, external integrations
  - `Presentation/` — FastEndpoints-based API definitions
- **Keep code files under 500 lines**. If approaching that limit, refactor by splitting logic into services, helpers, or handlers.
- Group related files by feature or responsibility (e.g., Basket, Quote, Checkout).
- **Avoid deep nesting** and aim for readable, modular code.

---

## ✅ Task Completion

- **Mark tasks as complete in `TASK.md`** immediately after finishing them.
- Add new sub-tasks or TODOs discovered during work to a “Discovered During Work” section in `TASK.md`.

---

## 📎 Style & Conventions

- Use **C# .NET 8**
- **PascalCase** for class names, public properties, and methods.
- **camelCase** for private fields and local variables.
- **Namespaces** use file based naming based on the containing folder.
- Use `record` for immutable data structures where appropriate.
- **Use records for immutable data models** where applicable.
- Use FluentValidation for complex business rules (if required).
- Use JSON schemas where provided to validate structure before persistence
- **Use dependency injection** for all service dependencies.
- Use XML documentation comments for public members:
  ```csharp
  /// <summary>
  /// Adds a quote to the specified basket.
  /// </summary>
  /// <param name="basketId">The ID of the basket.</param>
  /// <param name="quote">The quote to add.</param>
  /// <returns>The updated basket object.</returns>
  public async Task<Basket> AddQuoteAsync(Guid basketId, Quote quote)
  {
      ...
  }
  ```
- **Inline comment guidance**:
  - For non-obvious logic, use `// Reason:` to explain why the logic exists.
- Format code using `.editorconfig` settings and enforce with `dotnet format`.
- Results from API calls should return the following json regardless of the http code:
```json
{
  "Result": "Response data or null as appropriate",
  "Error": "Meaningful error message or null as appropriate"
}

---


## CLI Tool Guidance
- CLI should be implemented in C#

- Use the CLI to:
  - Seed test data to the services
  - Call service endpoints for testing purposes
  - Ensure CLI and API use consistent data models and serialization rules

---

## Design Philosophy
- Be pragmatic over dogmatic — don't over-engineer
- Keep the domain model rich and expressive
- Favour clear, testable logic over strict pattern enforcement
- Avoid introducing infrastructure concerns into domain or application layers
---

## 📚 Documentation & Explainability

- **Update `README.md`** whenever:
  - New features are added
  - Dependencies change
  - Setup steps are modified
- **Update `architecture.md`** whenever a change you make affects the solution's architecture.
- Keep code understandable for a mid-level .NET developer.
- **GEMINI.md** If you think it necessary create `GEMINI.md` files in project sub-folders to assist you in understanding and working with the codebase.

---

## 🧪 Testing & Reliability

- Use **MSTest** for unit testing.
- Create at least:
  - 1 success path test
  - 1 edge case
  - 1 failure case
- Place all tests in a `/tests` folder mirroring the application structure.
- Use **fakes**, not mocks.
- Do not aim for 100% coverage, but cover all critical logic paths.
- Update tests if logic changes.

---

## 🔐 Security & Environment

- Use **JWT** for authentication on API endpoints.
- Validate claims to enforce broker/admin access and ownership logic.
- Never commit secrets; use `.env` or `appsettings.Development.json` locally.

---


## 🧠 AI Assistant Rules

- **Confirm file and namespace paths** before using them.

- **Clarify, Don't Assume:** If a requirement is ambiguous or context is missing, your first action is to ask for clarification.

- **No Hallucinations:** Do not invent types, libraries, functions, or file paths. If you need a tool you don't have, state what you need and why.

- **Plan Before You Code:** For any non-trivial task, first outline your implementation plan in a list or with pseudocode. We will approve it before you write the final code.

- **Explain the "Why":** For complex or non-obvious blocks of code, add a `# WHY:` comment explaining the reasoning behind the implementation choice.