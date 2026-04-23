# Contributing to the Spidra .NET SDK

Thanks for taking the time to contribute. This document covers how to set up the project locally, run tests, and submit changes.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download) or later
- A Spidra API key for running integration tests (get one at [app.spidra.io](https://app.spidra.io))

## Setup

```bash
git clone https://github.com/spidra-io/spidra-dotnet.git
cd spidra-dotnet
dotnet restore
```

## Building

```bash
dotnet build
```

## Running tests

```bash
dotnet test
```

For integration tests that hit the live API, set your key first:

```bash
export SPIDRA_API_KEY=your-api-key
dotnet test --filter Category=Integration
```

## Code style

This project follows standard C# conventions enforced by the .editorconfig. Run a format check before submitting:

```bash
dotnet format --verify-no-changes
```

## Submitting changes

1. Fork the repository and create a branch from `main`.
2. Make your changes. If you are adding a feature, add tests.
3. Ensure `dotnet build` and `dotnet test` both pass.
4. Run `dotnet format` to clean up formatting.
5. Open a pull request with a clear description of what changed and why.

## Reporting bugs

Open an issue at [github.com/spidra-io/spidra-dotnet/issues](https://github.com/spidra-io/spidra-dotnet/issues) with steps to reproduce, the SDK version, and the .NET version you are running.

## Questions

For API questions or general usage help, see the [Spidra documentation](https://docs.spidra.io) or reach out at [spidra.io](https://spidra.io).
