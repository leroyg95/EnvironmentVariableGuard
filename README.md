# EnvironmentVariablesGuard 🛡️

A lightweight, zero-dependency .NET helper library to strictly validate required environment variables at application startup.

[![NuGet Version](https://img.shields.io/nuget/v/EnvironmentVariablesGuard.svg)](https://www.nuget.org/packages/EnvironmentVariablesGuard)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

---

## Why EnvironmentVariablesGuard?

Many .NET applications rely heavily on environment variables for configuration (database connection strings, API keys, secrets). However, missing or misconfigured variables are often only discovered *at runtime* when a specific service or feature is triggered. 

**EnvironmentVariablesGuard** ensures your application **fails fast** at startup with clear, descriptive error messages if any required environment variables are missing or invalid.

---

## Installation

Install the package via the .NET CLI:

```bash
dotnet add package EnvironmentVariablesGuard