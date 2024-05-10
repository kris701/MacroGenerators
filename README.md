
<p align="center">
    <img src="https://github.com/kris701/MacroGenerators/assets/22596587/4f77844a-36e8-4ab2-8cf7-5e50d5fd2f12" width="200" height="200" />
</p>

[![Build and Publish](https://github.com/kris701/MacroGenerators/actions/workflows/dotnet-desktop.yml/badge.svg)](https://github.com/kris701/MacroGenerators/actions/workflows/dotnet-desktop.yml)
![Nuget](https://img.shields.io/nuget/v/MacroGenerators)
![Nuget](https://img.shields.io/nuget/dt/MacroGenerators)
![GitHub last commit (branch)](https://img.shields.io/github/last-commit/kris701/MacroGenerators/main)
![GitHub commit activity (branch)](https://img.shields.io/github/commit-activity/m/kris701/MacroGenerators)
![Static Badge](https://img.shields.io/badge/Platform-Windows-blue)
![Static Badge](https://img.shields.io/badge/Platform-Linux-blue)
![Static Badge](https://img.shields.io/badge/Framework-dotnet--8.0-green)

# MacroGenerators
This project is a collection of different macro generators. 
You can run the different macro generators by calling the CLI tool:

```
dotnet run --domain domain.pddl --problem problem.pddl --plans plan1.plan plan2.plan ... --generator Sequential
```

You can also find this project as a package on the [NuGet Package Manager](https://www.nuget.org/packages/MacroGenerators/).

## Sequential Macro Generator

The sequential macro generactor can generate lifted macros based on reoccuring sequences in plans.
It works as follows
1. Find all the possible sequence combinations possible across all the given plans.
2. Sort all sequences by occurence.
3. Remove all sequences where there is no inner-entanglements (e.g. `pick ball1 rooma left`, `pick ball2 rooma right`).
4. Foreach remaining sequence, combine the actions into a macro.
5. Return found macros.

This results in macros that can look like this (for the gripper domain):
```pddl
(:action pick-pick-move-drop-drop
  :parameters (?ball1 ?ball2 ?rooma ?roomb ?left ?right)
  :precondition  
    (and 
      (free ?left) (free ?right)
      (ball ?ball1) (ball ?ball2)
      (room ?rooma) (room ?roomb)
      (gripper ?left) (gripper ?right)
      (at-robby ?rooma) 
      (at ?ball1 ?rooma)
      (at ?ball2 ?rooma)
    )
    :effect 
    (and 
      (at-robby ?roomb)
      (not (at-robby ?rooma))
      (not (at ?ball1 ?rooma))
      (not (at ?ball2 ?rooma))
      (at ?ball1 ?roomb)
      (at ?ball2 ?roomb)
    )
)
```
That effectively takes two balls, moves to the other room and drops them both, consisting of 5 actions in total.

### Examples
To find all macros for a given set plans and a domain:
```csharp
PDDLDecl decl = new PDDLDecl(...)
IMacroGenerator<List<ActionPlan>> generator = new SequentialMacroGenerator(decl);
List<ActionPlan> plans = new List<ActionPlan>(...);
List<ActionDecl> macros = generator.FindMacros(plans);
```

To find top 10 macros for a given set of plans and a domain:
```csharp
PDDLDecl decl = new PDDLDecl(...)
IMacroGenerator<List<ActionPlan>> generator = new SequentialMacroGenerator(decl);
List<ActionPlan> plans = new List<ActionPlan>(...);
List<ActionDecl> macros = generator.FindMacros(plans, 10);
```
