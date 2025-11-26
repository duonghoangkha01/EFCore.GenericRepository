---
name: gemini-subagent-skill
description: Invokes Gemini AI as a specialized subagent for code analysis, documentation generation, architecture review, and design decisions. Activates when you request Gemini assistance, need specialized AI analysis, want alternative perspectives on implementation, or require Gemini-specific capabilities for code review and technical writing.
allowed-tools: Read, Glob, Grep, WebFetch, Bash
---

# Gemini Subagent Skill

## Purpose
This skill enables Claude Code to delegate specialized tasks to Google's Gemini AI as a subagent, providing an additional AI perspective for code analysis, documentation generation, architectural decisions, and implementation strategies.

## When to Use This Skill

Activate this skill when you need:
- **Alternative AI perspectives**: Get Gemini's unique insights on code patterns and design
- **Code analysis**: Deep analysis of code quality, patterns, and potential improvements
- **Documentation generation**: Create comprehensive API docs, guides, and technical specifications
- **Architecture review**: Get design pattern recommendations and structural improvements
- **Implementation strategies**: Explore alternative approaches to solving problems
- **Code quality reviews**: Security checks, performance analysis, and best practices
- **Technical writing**: Generate clear, well-structured technical documentation

## How It Works

### Step 1: Prepare the Context
The skill gathers relevant information from your repository:
- Identifies files and code relevant to your request
- Collects current implementation details
- Understands the project structure and patterns

### Step 2: Format the Request
Creates a structured request for Gemini including:
- Specific task description
- Code context from your repository
- Analysis requirements and goals
- Any reference materials or templates

### Step 3: Invoke Gemini
Communicates with Gemini AI to:
- Submit the formatted request
- Provide necessary code context
- Specify the type of analysis or generation needed

### Step 4: Process and Integrate Results
Gemini's response is processed and presented:
- Code suggestions are clearly marked and explained
- Documentation is properly formatted in Markdown
- Analysis findings are organized and highlighted
- Recommendations include detailed rationale
- Integration points are identified

## Supported Task Types

### Code Analysis
```
"Use Gemini to analyze the Repository.cs implementation for design patterns"
"Ask Gemini to review our query building logic for performance issues"
```

### Documentation Generation
```
"Have Gemini create API documentation for our generic repository"
"Ask Gemini to write a comprehensive guide for using our pagination system"
```

### Architecture Review
```
"Request Gemini's perspective on our repository pattern implementation"
"Ask Gemini to suggest improvements to our database abstraction layer"
```

### Implementation Strategies
```
"Ask Gemini for alternative approaches to implementing soft delete functionality"
"Have Gemini suggest different patterns for handling complex queries"
```

### Code Quality Assessment
```
"Use Gemini to perform a security review of our data access layer"
"Ask Gemini to analyze potential performance bottlenecks in our query system"
```

## Usage Examples

### Example 1: Architecture Review
```
User: "Use the Gemini subagent to review our repository pattern architecture"

The skill will:
1. Read relevant repository pattern files
2. Analyze the current implementation structure
3. Submit to Gemini with context about EF Core and repository patterns
4. Return Gemini's architectural recommendations
```

### Example 2: Documentation Generation
```
User: "Ask Gemini to create comprehensive documentation for our IRepository interface"

The skill will:
1. Read the IRepository interface definition
2. Collect usage examples from the codebase
3. Request Gemini to generate detailed API documentation
4. Format the documentation for integration into your project
```

### Example 3: Code Quality Review
```
User: "Have Gemini analyze our query methods for security vulnerabilities"

The skill will:
1. Identify all query-related methods
2. Extract implementation details
3. Ask Gemini to perform security analysis
4. Present findings with severity levels and remediation steps
```

## Integration Notes

### Gemini CLI Setup (Headless Mode)

This skill uses Gemini CLI in headless mode to integrate with Google's Gemini models.

**Initial Setup:**
1. Install Gemini CLI: `npm install -g @google/generative-ai-cli` (or appropriate package)
2. Login to Gemini CLI: `gemini login`
3. Enable beta preview mode for Gemini 2.0/3.0 Pro access:
   ```bash
   gemini config set beta-preview true
   ```
4. Verify setup: `gemini --version`

**Headless Mode Commands:**
- Basic prompt: `gemini -p "your prompt here"`
- Pipe input: `cat prompt.md | gemini`
- With model selection: `gemini -m gemini-2.0-flash-exp -p "prompt"`

### Claude Code + Gemini Integration Pattern

**Recommended Workflow:**

1. **Claude Code (Opus 4.5) as Primary Agent:**
   - PRD analysis and thinking through solutions
   - System architecture and design decisions
   - High-level planning and guide generation
   - Complex logic and workflow orchestration

2. **Gemini (2.0/3.0 Pro) as Implementation Subagent:**
   - Code implementation from Claude's plans
   - UI/UX implementation and styling
   - Animation and visual enhancements
   - Detail-oriented implementation tasks

**Example Workflows:**

**Workflow 1: Plan → Implement**
```
Claude Opus 4.5: Analyze PRD, create implementation plan
        ↓
Gemini 3 Pro: Implement based on plan
        ↓
Claude Opus 4.5: Review and integrate
```

**Workflow 2: Plan → Style → Implement (Cost-Efficient)**
```
Claude Opus 4.5: Create implementation plan
        ↓
Gemini 3 Pro: Add style guide and animations
        ↓
Claude Haiku 4.5: Implement with reduced cost
```

**Workflow 3: UI Debugging with Antigravity**
```
Claude Code for VS Code Extension → Antigravity
        ↓
Claude Opus 4.5: Debug and fix UI issues
        ↓
Gemini 3 Pro: Implement fixes
```

### Invoking Gemini from Claude Code

When this skill is activated, Claude Code will:

1. **Gather Context**: Read relevant files and code
2. **Prepare Prompt**: Create detailed prompt with context
3. **Invoke Gemini CLI**:
   ```bash
   gemini -m gemini-2.0-flash-exp -p "$(cat <<'EOF'
   Context: [code context here]

   Task: [specific task description]

   Requirements:
   - [requirement 1]
   - [requirement 2]

   Please provide implementation in Markdown format.
   EOF
   )"
   ```
4. **Process Response**: Parse Gemini's output and integrate
5. **Handoff**: Provide detailed context back to main agent

### Context Handoff Best Practices

**Critical: Subagents have separate context!**

When handing off to/from Gemini:
- ✅ **Include detailed context**: File paths, code snippets, requirements
- ✅ **Specify exact task**: Be explicit about what Gemini should do
- ✅ **Provide constraints**: Style guides, patterns to follow, limitations
- ✅ **Request structured output**: Ask for specific format (Markdown, code blocks)
- ❌ **Don't assume shared context**: Gemini doesn't see Claude's conversation history
- ❌ **Don't be vague**: "Fix this" won't work; specify what and how

**Good Handoff Example:**
```
Gemini, please implement the UserRepository class based on this plan:

Context:
- File: src/Repositories/UserRepository.cs
- Pattern: Generic Repository Pattern with EF Core
- Base class: RepositoryBase<User, Guid, AppDbContext>

Requirements:
1. Implement IUserRepository interface
2. Add method: Task<User> GetByEmailAsync(string email)
3. Add method: Task<IEnumerable<User>> GetActiveUsersAsync()
4. Follow existing code style in RepositoryBase.cs
5. Include XML documentation comments

Constraints:
- Use async/await pattern
- Include CancellationToken parameters
- Handle null cases appropriately

Please provide the complete implementation.
```

**Poor Handoff Example:**
```
Gemini, implement the user repository.
```

### Response Format
All Gemini responses are:
- Formatted in Markdown for readability
- Structured with clear sections
- Include code examples where applicable
- Provide actionable recommendations

### Error Handling
If Gemini is unavailable or encounters errors:
- The skill will inform you of the issue
- Gracefully falls back to standard Claude analysis
- Suggests alternative approaches if needed

### Antigravity Integration

For UI debugging and development:
1. Install Claude Code for VS Code extension
2. Connect to Antigravity workspace
3. Use Gemini for UI implementation and fixes
4. Debug visually in Antigravity's preview environment

## Best Practices

1. **Be Specific**: Provide clear, specific requests for better Gemini analysis
2. **Context Matters**: Mention relevant files or components for focused analysis
3. **Iterative Refinement**: Use Gemini's feedback to refine implementations
4. **Combine Perspectives**: Leverage both Claude and Gemini insights for comprehensive solutions

## Related Tools

- Use standard Claude Code capabilities for general development tasks
- Activate this skill specifically when you want Gemini's unique perspective
- Combine with other skills for multi-perspective analysis

## Practical Implementation Guide

### How Claude Code Invokes Gemini

When you activate this skill, Claude Code will use bash commands to invoke Gemini CLI:

**Step-by-step Process:**

1. **Prepare Context File** (temporary):
   ```bash
   cat > /tmp/gemini_prompt.md <<'EOF'
   # Task: [Your specific task]

   ## Context
   [Relevant code, file paths, and background]

   ## Requirements
   - Requirement 1
   - Requirement 2

   ## Expected Output
   Please provide [specific format requested]
   EOF
   ```

2. **Invoke Gemini CLI**:
   ```bash
   # Option 1: Direct prompt
   gemini -m gemini-2.0-flash-exp -p "$(cat /tmp/gemini_prompt.md)"

   # Option 2: Pipe input
   cat /tmp/gemini_prompt.md | gemini -m gemini-2.0-flash-exp

   # Option 3: With output capture
   gemini -m gemini-2.0-flash-exp -p "$(cat /tmp/gemini_prompt.md)" > /tmp/gemini_response.md
   ```

3. **Parse and Integrate Response**:
   - Read Gemini's output from stdout or file
   - Extract code blocks and recommendations
   - Present to user with clear attribution

### Example: Full Integration Flow

```bash
# 1. Claude Code gathers context
# (using Read, Glob, Grep tools)

# 2. Create detailed prompt for Gemini
cat > /tmp/gemini_context.md <<'EOF'
# Implement UserRepository Class

## Context
Project: EFCore.GenericRepository
Pattern: Generic Repository Pattern
Base Class: RepositoryBase<User, Guid, AppDbContext>

## Current Code Structure
```csharp
// From RepositoryBase.cs
public abstract class RepositoryBase<T, TKey, TContext>
    where T : class
    where TContext : DbContext
{
    protected readonly TContext Context;
    // ... existing implementation
}
```

## Task
Implement UserRepository that extends RepositoryBase with:
1. GetByEmailAsync(string email, CancellationToken ct)
2. GetActiveUsersAsync(CancellationToken ct)
3. XML documentation comments
4. Proper async/await patterns

## Style Guide
- Use nullable reference types
- Include CancellationToken in all async methods
- Follow C# naming conventions
- Add comprehensive XML docs

## Expected Output
Provide complete C# class implementation in a code block.
EOF

# 3. Invoke Gemini
gemini -m gemini-2.0-flash-exp -p "$(cat /tmp/gemini_context.md)" > /tmp/gemini_output.md

# 4. Claude Code processes response
# (reads /tmp/gemini_output.md and presents to user)

# 5. Cleanup
rm /tmp/gemini_context.md /tmp/gemini_output.md
```

### Model Selection Guide

Choose the right Gemini model for your task:

| Model | Best For | Speed | Cost |
|-------|----------|-------|------|
| `gemini-2.0-flash-exp` | Quick code generation, simple tasks | Fast | Low |
| `gemini-2.0-flash-thinking-exp` | Complex logic, reasoning tasks | Medium | Medium |
| `gemini-exp-1206` | Experimental features, latest capabilities | Medium | Medium |

**Usage:**
```bash
# Flash for quick implementation
gemini -m gemini-2.0-flash-exp -p "prompt"

# Thinking for complex logic
gemini -m gemini-2.0-flash-thinking-exp -p "prompt"
```

### Troubleshooting

**Issue: "gemini command not found"**
- Solution: Install Gemini CLI or ensure it's in PATH

**Issue: "Authentication required"**
- Solution: Run `gemini login` first

**Issue: "Model not available"**
- Solution: Enable beta preview: `gemini config set beta-preview true`

**Issue: Context too large**
- Solution: Reduce context, focus on relevant code only

**Issue: Gemini output is hallucinatory**
- Solution: Provide more detailed constraints and examples in prompt

## Version History

- **1.0.0** (2025-11-26): Initial Gemini subagent integration
  - Basic code analysis support
  - Documentation generation capabilities
  - Architecture review functionality
  - Code quality assessment features
  - Headless CLI integration via bash commands
  - Context handoff best practices
  - Antigravity debugging workflow

## Notes

This skill is designed to complement Claude Code's native capabilities by providing access to Gemini's unique strengths in code analysis and generation. Use it when you want a different AI perspective or when Gemini's specific capabilities (like certain language specializations or analysis techniques) would be beneficial for your task.

**Key Principles:**
- **Claude Code (Opus 4.5)** = Strategic planning, architecture, complex reasoning
- **Gemini (2.0/3.0)** = Tactical implementation, UI/UX, detail work
- **Always provide detailed context** when handing off to Gemini
- **Gemini doesn't hallucinate less**, so provide clear constraints

To invoke this skill, simply mention "Gemini" or "subagent" in your requests, or explicitly ask to "use the Gemini subagent skill" for your task.

### Quick Start

1. Setup Gemini CLI: `gemini login && gemini config set beta-preview true`
2. Test: `gemini -p "Hello, write a simple C# class"`
3. Use in Claude Code: "Ask Gemini to implement [feature]"
4. Review and integrate Gemini's output
