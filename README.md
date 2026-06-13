# EDR
An open source project that might eventually be a complete EDR platform across Windows, macOS and Linux.

## Status
[![SonarQube](https://github.com/jcapellman/EDR/actions/workflows/SonarQube.yml/badge.svg)](https://github.com/jcapellman/EDR/actions/workflows/SonarQube.yml)
[![CodeQL](https://github.com/jcapellman/EDR/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/jcapellman/EDR/actions/workflows/codeql-analysis.yml)

## Technology Stack
- .NET 9.0
- C# with nullable reference types enabled
- NLog for logging
- AWS S3 SDK for cloud storage
- WET.lib for Windows ETW monitoring

## Development Items Remaining
* ~~Unit Test Coverage~~ (Partially completed)
* ~~Logging~~ (Completed - NLog integrated)
* Abstractions for different operating systems (Currently Windows-only via ETW)
* GitHub Action Release Packaging
