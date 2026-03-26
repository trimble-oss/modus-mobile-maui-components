# Code Scanning Setup Guide

This document explains the CodeQL code scanning configuration for this repository.

## Current Configuration

This repository uses a **custom CodeQL workflow** (`.github/workflows/codeql-analysis.yml`) that provides thorough analysis with the `security-and-quality` query suite.

### Workflow Details

- **Triggers**: Pushes and pull requests to `develop`/`main`, monthly scheduled scan, manual dispatch
- **Runner**: `ubuntu-latest`
- **Build target**: `net9.0-android` (compatible with Ubuntu runners)
- **Query suite**: `security-and-quality` (more thorough than default)

## Previous Issue: Stale Configuration

The original workflow file (`codeql.yml`) was **manually disabled** in GitHub Actions UI. A manually disabled workflow cannot be re-enabled through code changes alone — modifying the file content has no effect on the disabled state. This caused the code scanning status page to show a stale configuration error.

The fix was to rename the workflow file from `codeql.yml` to `codeql-analysis.yml`. GitHub Actions identifies workflows by their file path, so the renamed file is treated as a new (active) workflow.

## Managing Duplicate Scanning Configurations

If both the custom workflow and GitHub's **Default Setup** are active, you may see duplicate scan results. To avoid this:

1. Go to **Settings** > **Code security** > **Code scanning**
2. Disable the **Default setup** since the custom workflow provides more thorough analysis

## Why a Custom Build Is Required

CodeQL's **Autobuild** step cannot build .NET MAUI projects because:

- The .NET SDK is not set up in the runner by default
- MAUI workloads (required for building MAUI projects) are not installed
- Autobuild does not automatically install platform-specific workloads

The custom workflow fixes these issues by:

- Adding a `setup-dotnet` step for .NET 9
- Installing the `android` and `maui-android` workloads
- Replacing Autobuild with an explicit `dotnet build` targeting `net9.0-android`

## Verifying Code Scanning Compliance

To verify your code scanning setup is working correctly:

1. Go to **Security** > **Code scanning** > **Tool status**
2. Confirm CodeQL shows a **green status** with recent scan results
3. Ensure there is only **one active configuration** (either default setup or custom workflow, not both)
4. Check that scheduled scans are running (the custom workflow runs monthly on the 1st)

## Dependencies for the Custom Workflow

The custom CodeQL workflow requires:

| Dependency | Purpose |
|---|---|
| `actions/checkout@v5` | Check out the repository code |
| `actions/setup-dotnet@v4` | Install .NET 9 SDK |
| `github/codeql-action/init@v4` | Initialize CodeQL analysis |
| `github/codeql-action/analyze@v4` | Run CodeQL analysis and upload results |

These are kept up to date by Dependabot (configured in `.github/dependabot.yml`).
