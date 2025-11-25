# Contributing to EFCore.GenericRepository

Thank you for your interest in contributing to EFCore.GenericRepository! This document provides guidelines and instructions for contributing.

## Code of Conduct

This project follows a Code of Conduct. By participating, you are expected to uphold this code. Please report unacceptable behavior to the project maintainers.

## How Can I Contribute?

### Reporting Bugs

Before creating bug reports, please check the existing issues to avoid duplicates. When creating a bug report, include as many details as possible:

- **Use a clear and descriptive title**
- **Describe the exact steps to reproduce the problem**
- **Provide specific examples** - include code samples
- **Describe the behavior you observed** and what you expected to see
- **Include your environment details** (OS, .NET version, EF Core version, etc.)

### Suggesting Enhancements

Enhancement suggestions are tracked as GitHub issues. When creating an enhancement suggestion:

- **Use a clear and descriptive title**
- **Provide a detailed description** of the suggested enhancement
- **Explain why this enhancement would be useful**
- **Include code examples** if applicable

### Pull Requests

1. **Fork the repository** and create your branch from `develop`
2. **Follow the coding standards** (see below)
3. **Add tests** for your changes
4. **Ensure all tests pass** (`dotnet test`)
5. **Update documentation** if needed
6. **Write a clear commit message**
7. **Submit the pull request** to the `develop` branch

## Development Setup

### Prerequisites

- .NET 8 SDK
- Visual Studio 2022, VS Code, or Rider
- Git

### Getting Started

```bash
# Clone the repository
git clone https://github.com/duonghoangkha01/EFCore.GenericRepository.git
cd EFCore.GenericRepository

# Restore dependencies
dotnet restore

# Build the solution
dotnet build

# Run tests
dotnet test
```

## Coding Standards

### C# Style Guide

- Follow [C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- Use meaningful variable and method names
- Keep methods small and focused
- Write self-documenting code
- Add comments for complex logic

### Code Organization

- Place classes in appropriate namespaces
- One class per file
- File name should match the class name
- Use proper folder structure (see project organization)

### XML Documentation

All public APIs must have XML documentation:

```csharp
/// <summary>
/// Brief description of what this method does.
/// </summary>
/// <param name="paramName">Description of the parameter.</param>
/// <returns>Description of the return value.</returns>
/// <exception cref="ExceptionType">When this exception is thrown.</exception>
public ReturnType MethodName(ParamType paramName)
{
    // Implementation
}
```

### Testing

- Write unit tests for all public methods
- Use meaningful test names: `MethodName_Scenario_ExpectedBehavior`
- Follow AAA pattern (Arrange, Act, Assert)
- Aim for 80%+ code coverage

Example:
```csharp
[Fact]
public void GetById_ExistingId_ReturnsEntity()
{
    // Arrange
    var entity = new TestEntity { Id = 1 };
    // ... setup

    // Act
    var result = repository.GetById(1);

    // Assert
    Assert.NotNull(result);
    Assert.Equal(1, result.Id);
}
```

## Git Workflow

### Branch Naming

- `feature/your-feature-name` - for new features
- `bugfix/issue-number-description` - for bug fixes
- `docs/description` - for documentation updates
- `refactor/description` - for refactoring
- `test/description` - for test additions

### Commit Messages

Follow the [Conventional Commits](https://www.conventionalcommits.org/) specification:

```
<type>(<scope>): <subject>

<body>

<footer>
```

Types:
- `feat`: A new feature
- `fix`: A bug fix
- `docs`: Documentation changes
- `style`: Code style changes (formatting, etc.)
- `refactor`: Code refactoring
- `test`: Adding or updating tests
- `chore`: Maintenance tasks

Examples:
```
feat(repository): add sorting support with OrderBy methods

docs(readme): update installation instructions

fix(soft-delete): correct query filter for ISoftDeletable entities
```

### Pull Request Process

1. Update the CHANGELOG.md with details of your changes
2. Update the README.md if you change functionality
3. The PR will be merged once it has been reviewed and approved
4. Ensure all CI checks pass before requesting review

## Project Structure

```
src/
  EFCore.GenericRepository/
    Entities/          - Base entity classes and interfaces
    Repositories/      - Repository interfaces and implementations
      Interfaces/
    UnitOfWork/        - Unit of Work pattern implementation
    Extensions/        - Extension methods

tests/
  EFCore.GenericRepository.Tests/  - Unit tests

samples/
  EFCore.GenericRepository.Sample/ - Sample applications
```

## Review Process

- All submissions require review
- Reviewers will check:
  - Code quality and standards
  - Test coverage
  - Documentation
  - Performance implications
  - Breaking changes

## License

By contributing, you agree that your contributions will be licensed under the MIT License.

## Questions?

Feel free to open an issue with the `question` label, or reach out to the maintainers.

## Recognition

Contributors will be recognized in the project README and release notes.

Thank you for contributing to EFCore.GenericRepository! 🎉
