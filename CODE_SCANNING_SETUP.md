# Code Scanning Setup Guide

This document explains how to resolve code scanning configuration issues in this repository and maintain compliance with organizational policies.

## Current Configuration

This repository has two CodeQL code scanning configurations:

1. **Default Setup** (managed by GitHub) — Configured in **Settings > Code security > Code scanning**. This is GitHub's built-in scanning that runs automatically without a workflow file.

2. **Custom Workflow** (`.github/workflows/codeql.yml`) — A workflow file in the repository that provides more control over the scanning process, including the `security-and-quality` query suite.

## Known Issue: Stale Configuration Error

The custom CodeQL workflow (`codeql.yml`) was previously **manually disabled**, which causes the code scanning status page to show an error or stale configuration at:

> **Security** > **Code scanning** > **Tool status** > **CodeQL** > **Configurations**

This happens because GitHub detects the workflow configuration but finds no recent scan results from it.

## How to Resolve

You have two options — choose **one**:

### Option A: Use the Custom Workflow (Recommended)

The custom workflow provides more thorough analysis with the `security-and-quality` query suite.

1. Go to **Actions** > **CodeQL** (the workflow, not the default setup)
2. Click **Enable workflow** to re-enable the disabled `codeql.yml`
3. Optionally, trigger a manual run via **Run workflow** to verify it works
4. Go to **Settings** > **Code security** > **Code scanning** and disable the **Default setup** to avoid duplicate scanning
5. Verify the workflow completes successfully and results appear under **Security** > **Code scanning**

### Option B: Use the Default Setup Only

If you prefer the simpler GitHub-managed scanning:

1. Verify the Default Setup is enabled in **Settings** > **Code security** > **Code scanning**
2. **Delete** the file `.github/workflows/codeql.yml` from the repository to remove the stale configuration
3. Commit and push the deletion
4. The stale configuration error will clear after the next scheduled scan

## Why Was the Workflow Failing?

The original `codeql.yml` used CodeQL's **Autobuild** step, which could not build this .NET MAUI project because:

- The .NET SDK was not set up in the workflow
- MAUI workloads (required for building MAUI projects) were not installed
- Autobuild does not automatically install platform-specific workloads

The updated workflow in this repository fixes these issues by:

- Adding a `setup-dotnet` step for .NET 9
- Installing the `maui-android` workload (compatible with Ubuntu runners)
- Replacing the Autobuild step with an explicit `dotnet build` targeting `net9.0-android`

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
