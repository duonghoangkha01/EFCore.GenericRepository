# Gemini Subagent Integration - Quick Reference

## Tổng Quan (Overview)

Tích hợp Claude Code (Opus 4.5) với Gemini CLI để tận dụng điểm mạnh của cả hai AI models:

- **Claude Code (Opus 4.5)**: Chính thất - Strategic planning, architecture, complex reasoning
- **Gemini (2.0/3.0 Pro)**: Ái phi - Tactical implementation, UI/UX, detail work

## Setup Nhanh (Quick Setup)

### 1. Cài Đặt Gemini CLI

```bash
# Cài đặt Gemini CLI (tùy package manager)
npm install -g @google/generative-ai-cli

# Hoặc sử dụng package khác tùy platform
```

### 2. Đăng Nhập và Cấu Hình

```bash
# Login lần đầu
gemini login

# Bật beta preview mode để dùng Gemini 2.0/3.0 Pro
gemini config set beta-preview true

# Verify setup
gemini --version
```

### 3. Test Gemini CLI

```bash
# Test basic
gemini -p "Hello, write a simple C# class"

# Test với model cụ thể
gemini -m gemini-2.0-flash-exp -p "Explain async/await in C#"

# Test với pipe
echo "Generate a repository pattern class" | gemini
```

## Headless Mode Commands

### Claude Code Headless
```bash
claude -p "your prompt" --allowedTools Read,Write,Edit
```

### Gemini CLI Headless
```bash
# Direct prompt
gemini -p "your prompt"

# Pipe input
cat prompt.md | gemini

# With model selection
gemini -m gemini-2.0-flash-exp -p "prompt"

# Output to file
gemini -p "prompt" > output.md
```

## Workflow Patterns

### Pattern 1: Plan → Implement
**Use case**: Feature implementation, refactoring

```
┌─────────────────────┐
│ Claude Opus 4.5     │
│ - Analyze PRD       │
│ - Create plan       │
│ - Define structure  │
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│ Gemini 3 Pro        │
│ - Implement code    │
│ - Follow plan       │
│ - Generate files    │
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│ Claude Opus 4.5     │
│ - Review code       │
│ - Integrate         │
│ - Refine            │
└─────────────────────┘
```

### Pattern 2: Plan → Style → Implement (Cost-Efficient)
**Use case**: UI features, tiết kiệm quota

```
┌─────────────────────┐
│ Claude Opus 4.5     │
│ - Strategic plan    │
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│ Gemini 3 Pro        │
│ - Add style guide   │
│ - Define animations │
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│ Claude Haiku 4.5    │
│ - Implement         │
│ - Lower cost        │
└─────────────────────┘
```

### Pattern 3: UI Debugging with Antigravity
**Use case**: Frontend debugging, visual fixes

```
┌─────────────────────────┐
│ Claude Code VS Code Ext │
│ + Antigravity           │
└──────────┬──────────────┘
           │
           ▼
┌─────────────────────┐
│ Claude Opus 4.5     │
│ - Debug issues      │
│ - Identify fixes    │
└──────────┬──────────┘
           │
           ▼
┌─────────────────────┐
│ Gemini 3 Pro        │
│ - Implement fixes   │
│ - Update UI         │
└─────────────────────┘
```

## Context Handoff Template

### ✅ Good Handoff (Detailed)

```markdown
Gemini, please implement [Feature Name] based on this plan:

## Context
- File: [path/to/file.cs]
- Pattern: [design pattern being used]
- Base class/Interface: [inheritance info]
- Dependencies: [relevant packages/services]

## Current Code Structure
```csharp
// Existing code that provides context
public class ExistingClass
{
    // Relevant methods/properties
}
```

## Task
Implement the following:
1. [Specific requirement 1]
2. [Specific requirement 2]
3. [Specific requirement 3]

## Style Guide
- [Code style requirement 1]
- [Code style requirement 2]
- [Naming convention]
- [Documentation requirement]

## Constraints
- [Technical constraint 1]
- [Technical constraint 2]
- [Performance/security considerations]

## Expected Output
Provide complete [language] implementation in a code block with:
- [Output requirement 1]
- [Output requirement 2]
```

### ❌ Poor Handoff (Vague)

```markdown
Gemini, implement the repository.
```

**Why this fails:**
- No context about project structure
- No specific requirements
- No style guidelines
- No expected output format
- Gemini has separate context, doesn't see Claude's conversation

## Gemini Model Selection

| Model | Use Case | Characteristics |
|-------|----------|----------------|
| `gemini-2.0-flash-exp` | Quick code gen, simple tasks | Fast, low cost, good for straightforward implementation |
| `gemini-2.0-flash-thinking-exp` | Complex logic, reasoning | Better reasoning, handles complex requirements |
| `gemini-exp-1206` | Latest features | Experimental, cutting-edge capabilities |

### Usage Examples

```bash
# Quick implementation
gemini -m gemini-2.0-flash-exp -p "$(cat task.md)"

# Complex logic
gemini -m gemini-2.0-flash-thinking-exp -p "$(cat complex_task.md)"

# Latest features
gemini -m gemini-exp-1206 -p "$(cat advanced_task.md)"
```

## Integration từ Claude Code

### Step 1: Gather Context (Claude)

```bash
# Claude Code uses Read, Glob, Grep tools
# Example: Reading existing repository files
```

### Step 2: Prepare Prompt File (Claude)

```bash
cat > /tmp/gemini_prompt.md <<'EOF'
# Task: Implement UserRepository

## Context
[Detailed context here]

## Requirements
[Specific requirements]

## Expected Output
[Format specification]
EOF
```

### Step 3: Invoke Gemini CLI (Claude)

```bash
# Option 1: Direct execution
gemini -m gemini-2.0-flash-exp -p "$(cat /tmp/gemini_prompt.md)"

# Option 2: With output capture
gemini -m gemini-2.0-flash-exp -p "$(cat /tmp/gemini_prompt.md)" > /tmp/gemini_response.md

# Option 3: Pipe input
cat /tmp/gemini_prompt.md | gemini -m gemini-2.0-flash-exp > /tmp/gemini_response.md
```

### Step 4: Process Response (Claude)

```bash
# Claude reads and integrates Gemini's response
# Presents to user with clear attribution
```

### Step 5: Cleanup

```bash
rm /tmp/gemini_prompt.md /tmp/gemini_response.md
```

## Best Practices

### 1. Phân Chia Trách Nhiệm Rõ Ràng

- **Claude Opus 4.5**:
  - ✅ PRD analysis
  - ✅ System architecture
  - ✅ Complex logic design
  - ✅ Planning & orchestration

- **Gemini 2.0/3.0**:
  - ✅ Code implementation
  - ✅ UI/UX development
  - ✅ Styling & animations
  - ✅ Detail-oriented tasks

### 2. Context Handoff

- ✅ **ALWAYS include detailed context** - Gemini không thấy history của Claude
- ✅ **Be explicit** - Specify exactly what to implement
- ✅ **Provide constraints** - Style guides, patterns, limitations
- ✅ **Request structured output** - Ask for specific format
- ❌ **NEVER assume shared context**
- ❌ **NEVER be vague** - "Fix this" won't work

### 3. Giảm Ảo Giác (Hallucination)

Gemini có xu hướng ảo giác cao, để giảm thiểu:

```markdown
## Constraints (Important!)
- Use ONLY these methods: [list exact methods]
- Follow THIS exact pattern: [code example]
- DO NOT add features beyond: [scope]
- Reference these files: [specific files]
```

### 4. Verify và Review

```
Claude Opus 4.5: Plan
        ↓
Gemini: Implement
        ↓
Claude Opus 4.5: Review & refine ← CRITICAL STEP
```

**NEVER skip the review step!**

## Troubleshooting

### Problem: "gemini command not found"

**Solution:**
```bash
# Check if installed
which gemini

# If not found, install
npm install -g @google/generative-ai-cli

# Add to PATH if needed
export PATH=$PATH:/path/to/gemini
```

### Problem: "Authentication required"

**Solution:**
```bash
gemini login
# Follow the authentication flow
```

### Problem: "Model not available"

**Solution:**
```bash
# Enable beta preview
gemini config set beta-preview true

# List available models
gemini models list
```

### Problem: Output too long / truncated

**Solution:**
```bash
# Save to file instead of stdout
gemini -p "prompt" > output.md

# Or increase buffer size in your shell
```

### Problem: Gemini hallucinating / wrong output

**Solution:**
- Provide MORE detailed context
- Add explicit constraints and examples
- Break down into smaller tasks
- Use `gemini-2.0-flash-thinking-exp` for better reasoning

## Example: Complete Workflow

### Scenario: Implement UserRepository với Complex Queries

#### Step 1: User Request
```
User: "Use Gemini to implement UserRepository with email search and active user filtering"
```

#### Step 2: Claude Gathers Context
```bash
# Claude uses Read tool to examine:
# - IRepository interface
# - RepositoryBase implementation
# - Existing repository examples
# - DbContext structure
```

#### Step 3: Claude Prepares Detailed Prompt
```bash
cat > /tmp/gemini_task.md <<'EOF'
# Implement UserRepository Class

## Project Context
- Project: EFCore.GenericRepository
- Framework: .NET 8, Entity Framework Core 8
- Pattern: Generic Repository Pattern
- Base Class: RepositoryBase<User, Guid, AppDbContext>

## Existing Code Structure

### IRepository<T, TKey>
```csharp
public interface IRepository<T, TKey> where T : class
{
    Task<T?> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    // ... other methods
}
```

### RepositoryBase<T, TKey, TContext>
```csharp
public abstract class RepositoryBase<T, TKey, TContext> : IRepository<T, TKey>
    where T : class
    where TContext : DbContext
{
    protected readonly TContext Context;
    protected readonly DbSet<T> DbSet;

    protected RepositoryBase(TContext context)
    {
        Context = context;
        DbSet = context.Set<T>();
    }
    // ... base implementation
}
```

### User Entity
```csharp
public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string Name { get; set; } = null!;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

## Task Requirements

Implement `UserRepository` class that:

1. Extends `RepositoryBase<User, Guid, AppDbContext>`
2. Implements `IUserRepository` interface
3. Add method: `Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)`
   - Should be case-insensitive email search
   - Return null if not found
4. Add method: `Task<IEnumerable<User>> GetActiveUsersAsync(CancellationToken cancellationToken = default)`
   - Filter by IsActive == true
   - Order by CreatedAt descending
5. Add method: `Task<PagedResult<User>> GetActiveUsersPaginatedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)`
   - Combine pagination with active user filtering
   - Use existing PagedResult<T> class

## Code Style Requirements

- Use nullable reference types (enabled in project)
- Include XML documentation comments for all public methods
- Follow C# naming conventions (PascalCase for methods, camelCase for parameters)
- Use async/await pattern correctly
- Include CancellationToken parameter in all async methods
- Handle null cases appropriately
- Use LINQ with Entity Framework Core best practices

## Example XML Documentation Format
```csharp
/// <summary>
/// [Brief description]
/// </summary>
/// <param name="[paramName]">[Parameter description]</param>
/// <param name="cancellationToken">Cancellation token</param>
/// <returns>[Return description]</returns>
```

## Constraints

- DO NOT modify base class or interfaces
- DO NOT add unnecessary dependencies
- DO NOT use raw SQL queries - use LINQ only
- MUST use async/await for all database operations
- MUST pass CancellationToken to EF Core methods
- MUST NOT expose IQueryable - return materialized results

## Expected Output

Provide TWO code blocks:

1. **IUserRepository.cs** - Interface definition
2. **UserRepository.cs** - Complete implementation

Format as:
```csharp
// IUserRepository.cs
[full interface code]
```

```csharp
// UserRepository.cs
[full implementation code]
```

## Additional Notes

- Repository will be registered in DI container as scoped service
- AppDbContext is already configured with User entity
- No need for error handling beyond standard EF Core exceptions
EOF

# Step 4: Invoke Gemini
gemini -m gemini-2.0-flash-exp -p "$(cat /tmp/gemini_task.md)" > /tmp/gemini_output.md
```

#### Step 4: Claude Processes Response

```bash
# Claude reads /tmp/gemini_output.md
# Extracts code blocks
# Validates against requirements
# Presents to user
```

#### Step 5: User Reviews and Claude Integrates

```
Claude: "Here's Gemini's implementation for UserRepository:

[Presents code]

Would you like me to:
1. Create these files in your project
2. Review the implementation for improvements
3. Ask Gemini to make any adjustments
"
```

## Tips & Tricks

### 1. Tối Ưu Chi Phí (Cost Optimization)

```
Complex Task:
├─ Claude Opus 4.5: Architecture & Planning (expensive but necessary)
├─ Gemini 3 Pro: Detail work (moderate cost)
└─ Claude Haiku 4.5: Simple implementation (cheap)

Use Haiku for final implementation if Gemini provided good style guide!
```

### 2. Parallel Processing

```bash
# Run multiple Gemini tasks in parallel for independent components
gemini -p "$(cat task1.md)" > output1.md &
gemini -p "$(cat task2.md)" > output2.md &
gemini -p "$(cat task3.md)" > output3.md &
wait
```

### 3. Iterative Refinement

```
Round 1: Claude → Gemini → Review
Round 2: Claude refines prompt → Gemini implements → Review
Round 3: Final touches by Claude
```

### 4. Use Templates

Create reusable prompt templates:

```bash
# ~/.claude/templates/gemini-implement-class.md
# Template for class implementation
# Replace {{CLASS_NAME}}, {{REQUIREMENTS}}, etc.
```

## Antigravity Integration

### Setup

1. Install Claude Code for VS Code extension
2. Open project in Antigravity
3. Extension auto-connects to workspace

### Workflow

```
Code in Antigravity
        ↓
Claude Code debugs visually
        ↓
Gemini implements fixes
        ↓
Preview in Antigravity
```

### Benefits

- Visual debugging for UI issues
- Real-time preview of Gemini's implementations
- Faster iteration cycle

## Conclusion

**Key Takeaways:**

1. ✅ Claude Opus 4.5 = Strategy, Gemini = Execution
2. ✅ ALWAYS provide detailed context to Gemini
3. ✅ Review Gemini's output - don't trust blindly
4. ✅ Use right model for right task
5. ✅ Iterative refinement works better than one-shot

**Remember:**
> "Subagents have separate context!" - Always include full context in handoffs.

---

**Version**: 1.0.0
**Last Updated**: 2025-11-26
**Author**: Claude Code Gemini Subagent Skill
