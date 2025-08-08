## Purpose
Template optimized for AI agents to implement features with sufficient context and self-validation capabilities to achieve working code through iterative refinement.

## Core Principles
1. **Context is King**: Include ALL necessary documentation, examples, and caveats
2. **Validation Loops**: Provide executable tests/lints the AI can run and fix
3. **Information Dense**: Use keywords and patterns from the codebase
4. **Progressive Success**: Start simple, validate, then enhance
5. **Global rules**: Be sure to follow all rules in CLAUDE.md

---

## Goal
[What needs to be built - be specific about the end state and desires]

## Why
- [Business value and user impact]
- [Integration with existing features]
- [Problems this solves and for whom]

## What
[User-visible behavior and technical requirements]

### Success Criteria
- [ ] [Specific measurable outcomes]

## All Needed Context

### Documentation & References (list all context needed to implement the feature)
```yaml
# MUST READ - Include these in your context window
- url: [Official API docs URL]
  why: [Specific sections/methods you'll need]

- file: [path/to/example.py]
  why: [Pattern to follow, gotchas to avoid]

- doc: [Library documentation URL]
  section: [Specific section about common pitfalls]
  critical: [Key insight that prevents common errors]

- docfile: [PRPs/ai_docs/file.md]
  why: [docs that the user has pasted in to the project]

```

### Current Codebase tree (run `tree` in the root of the project) to get an overview of the codebase
```bash

```

### Desired Codebase tree with files to be added and responsibility of file
```bash

```

### Known Gotchas of our codebase & Library Quirks
```python
# CRITICAL: [Library name] requires [specific setup]
# Example: FastAPI requires async functions for endpoints
# Example: This ORM doesn't support batch inserts over 1000 records
# Example: We use pydantic v2 and
```

## Implementation Blueprint

### Data models and structure

Create the core data models, we ensure type safety and consistency.
```csharp
// Examples:
// - C# records or classes for Basket, Quote, Customer
// - Clean Architecture: Domain entities and value objects
// - DTOs for FastEndpoints requests/responses
// - JSON schema validation (if relevant)


```

### list of tasks to be completed to fullfill the PRP in the order they should be completed

```yaml
Task 1:
MODIFY src/Services/ExistingModule.cs:
  - FIND pattern: "class ExistingModule"
  - INJECT logic after constructor declaration
  - PRESERVE method signatures and public interfaces

CREATE src/Features/NewFeature/NewFeatureCommand.cs:
  - DEFINE a record or class with appropriate request properties
  - USE PascalCase for all members
  - MARK immutable where possible

CREATE src/Features/NewFeature/NewFeatureHandler.cs:
  - IMPLEMENT business logic for the new feature
  - INJECT dependencies via constructor
  - FOLLOW existing error-handling and logging patterns

MODIFY src/Endpoints/EndpointRegistry.cs:
  - REGISTER new endpoint route for NewFeature
  - USE route pattern consistent with existing endpoints

CREATE src/Endpoints/NewFeatureEndpoint.cs:
  - DEFINE a FastEndpoint that handles the new feature
  - ACCEPT NewFeatureRequest, return NewFeatureResponse
  - APPLY authorization attributes as required

CREATE tests/Features/NewFeatureTests.cs:
  - TEST happy path scenario
  - TEST invalid input scenario
  - TEST external dependency failure

CREATE src/Infrastructure/EventWriter.cs (if not already existing):
  - DEFINE reusable service for writing event envelopes
  - FOLLOW append-only rules
  - SERIALIZE payload using configured JSON settings
...(...)

Task N:
...

```


### Per task pseudocode as needed added to each task
```csharp

// Task 1
// Pseudocode (do not write full code)
public async Task<IResult> HandleAsync(AddQuoteRequest req, CancellationToken ct)
{
    // Validate JWT claims and ownership
    // Load basket from MongoDB
    // Check editability
    // Add quote to quotes array
    // Save basket
    // Emit BasketUpdated event with QuoteAdded
    // Return response DTO
}
```

### Integration Points
```yaml
DATABASE:
  - Collection: "baskets" in MongoDB
  - Event log: "basketEvents" in MongoDB

CONFIG:
  - Add values in: appsettings.Development.json
  - Read using: IOptions<AppSettings>

ROUTES:
  - Add FastEndpoint in: src/Presentation/Basket/AddQuoteEndpoint.cs

```

## Validation Loop

### Level 1: Syntax & Style
```bash
# Run these FIRST - fix any errors before proceeding
# Code style
dotnet format --verify-no-changes

# Compilation check
dotnet build --configuration Release

# Unit tests
dotnet test --configuration Release

# Expected: No errors. If errors, READ the error and fix.
```

### Level 2: Unit Tests each new feature/file/function use existing test patterns
```csharp
// File: tests/Application/Basket/AddQuoteTests.cs

[TestClass]
public class AddQuoteTests
{
    [TestMethod]
    public void HappyPath_AddQuote_ReturnsSuccess()
    {
        // Arrange
        var service = new AddQuoteService(fakeBasketRepo, fakeEventWriter);
        var request = new AddQuoteRequest { BasketId = validId, Quote = testQuote };

        // Act
        var result = service.Handle(request, default).Result;

        // Assert
        Assert.IsTrue(result.IsSuccess);
    }

    [TestMethod]
    public void ValidationError_MissingQuoteId_Throws()
    {
        // Arrange
        var service = new AddQuoteService(fakeBasketRepo, fakeEventWriter);
        var invalidRequest = new AddQuoteRequest { BasketId = validId, Quote = new Quote { /* missing fields */ } };

        // Act & Assert
        Assert.ThrowsException<ValidationException>(() => service.Handle(invalidRequest, default).Wait());
    }

    [TestMethod]
    public void ExternalFailure_EventWriterThrows_ReturnsError()
    {
        // Arrange
        var failingEventWriter = new FailingEventWriter(); // fake that throws
        var service = new AddQuoteService(fakeBasketRepo, failingEventWriter);

        // Act
        var result = service.Handle(validRequest, default).Result;

        // Assert
        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("Failed to write event", result.ErrorMessage);
    }
}

```

```bash
# Run and iterate until tests pass
dotnet test --configuration Release --verbosity normal

# If a test fails:
# - Read the full error output
# - Identify the root cause (do not patch with mocks)
# - Fix the logic
# - Re-run tests
```

### Level 3: Integration Test
```bash
# Start the service (from project root)
dotnet run --project src/Presentation/Api --configuration Release

# Then in another terminal, test the endpoint:
curl -X POST https://localhost:5001/baskets/{basketId}/quotes \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <jwt_token>" \
  -d '{
        "quoteId": "abc123",
        "sourceSystem": "QuoteEngineX",
        "vertical": "Waste",
        "price": 50.25,
        "status": "Priced"
      }'

# Expected Response:
# {
#   "status": "success",
#   "message": "Quote added to basket"
# }

# If failing:
# - Check logs (stdout or /logs if persisted)
# - Confirm the basket ID, auth, and input format
```

## Final Validation Checklist
- [ ] All unit tests pass: `dotnet test`
- [ ] No linting errors: `dotnet format --verify-no-changes`
- [ ] Project builds: `dotnet build`
- [ ] Manual endpoint test using CLI or curl
- [ ] Error cases handled gracefully
- [ ] Logs are structured and not noisy
- [ ] README or docs updated if affected

---

## Anti-Patterns to Avoid
- ❌ Don't create new patterns when existing ones work
- ❌ Don't skip validation because "it should work"
- ❌ Don't ignore failing tests - fix them
- ❌ Don't use sync functions in async context
- ❌ Don't hardcode values that should be config
- ❌ Don't catch all exceptions - be specific