# Contributing to this project

Thank you for taking the time to contribute to Sdl3Sharp! Every bit of help is appreciated.

The following is a set of guidelines for contributing to this project. These are guidelines, not hard rules, so use your best judgment and feel free to propose changes to this document itself via a pull request.

> [!IMPORTANT]
> By contributing to this project, you agree to abide by the [Code of Conduct](CODE_OF_CONDUCT.md). Contributions will not be accepted if they violate the Code of Conduct in any form. This includes contributions from individuals whose public history of interactions with others, whether related to this project or not, shows violations of the Code of Conduct, unless they have since publicly demonstrated sufficient remorse and a credible change in behavior.

---

## Table of contents

- [How to contribute](#how-to-contribute)
  - [Reporting bugs](#reporting-bugs)
  - [Suggesting features](#suggesting-features)
  - [Submitting changes](#submitting-changes)
- [Guidelines for contributions](#guidelines-for-contributions)
  - [Coding conventions](#coding-conventions)
  - [A note on AI-assisted contributions](#a-note-on-ai-assisted-contributions)

---

## How to contribute

### Reporting bugs

Found something that doesn't work as expected? Search the [issue tracker](../../issues) for both open and closed issues first. Your bug may already be known or fixed. If nothing relevant comes up, open a new issue and include:

- A clear and descriptive title.
- A concise description of the problem and what you expected to happen instead.
- Reliable steps to reproduce the issue.
- Your environment: OS, .NET version, the version of Sdl3Sharp you encountered the issue with, and anything else that seems relevant.
- Any relevant logs, stack traces, or code snippets.

### Suggesting features

Have an idea for something new or an improvement to existing behavior? Search the [issue tracker](../../issues) for both open and closed issues first, in case it has already been discussed. If nothing relevant comes up, open a new issue and include:

- What you would like to see and why it would be useful.
- Any alternatives you have considered.
- Rough thoughts on how it could be implemented, if you have them.

### Submitting changes

Ready to contribute a fix, improvement, or new feature? Search the [issue tracker](../../issues) for both open and closed issues first. For anything substantial, consider opening an issue before writing code so the direction can be discussed. Once you are ready:

1. Fork the repository and create a branch with a descriptive name.
2. Make your changes, keeping the scope of the pull request focused on one logical change.
3. Open a pull request against the `main` branch. The pull request, as well as any related issues, should be well structured and detailed enough for a reviewer to understand what changed and why. Include a reference to any related issue where applicable.

## Guidelines for contributions

### Coding conventions

This project is written almost entirely in C#. There are no strictly enforced style rules, but please orient your code around the [Microsoft C# identifier naming conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/identifier-names) and the [Microsoft C# coding conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions) as a baseline.

A few things to keep in mind:

- Private fields use the `m` prefix (e.g., `mSomeField`).
- Code should be easy to follow for an onlooker. Avoid unnecessarily obscure constructs.

### A note on AI-assisted contributions

The use of AI tools, including LLMs and other AI-driven code generation tools, is **permitted**.

**For code contributions**, every individual contribution that was produced with the help of AI, fully or partially, must be clearly marked as such, for example in a code comment near the relevant section. Please also indicate the degree of AI involvement, for example `// 100% AI-generated`, `// AI-assisted`, or `// AI-suggested, heavily modified`.

**For non-code contributions** such as documentation, AI use does not need to be noted on a per-contribution basis. If you routinely use AI for such contributions, please make that known somewhere, for example in your pull request description, and give a rough sense of how much your contributions rely on AI.

In either case, contributions that appear to be derived from sources with licenses incompatible with this project's [MIT License](LICENSE.md) will not be accepted.

---

Thank you for reading through these guidelines and for your interest in contributing to this project. It is greatly appreciated. &#x2764;&#xFE0F;
