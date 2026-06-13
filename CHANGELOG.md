# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

### Changed
- **Upgraded to .NET 9.0** from .NET 7.0 (latest LTS version)
- **Updated NuGet packages** to latest stable versions:
  - AWSSDK.S3 3.7.410.5 (from 4.0.24.3)
  - NLog 5.3.4 (from 6.1.3)
  - Microsoft.NET.ILLink packages to 9.0.0
- **Replaced deprecated `ReaderWriterLock`** with `ReaderWriterLockSlim` in LocalStorage
- **Improved disposal patterns** - Added IDisposable implementation to:
  - CollectorRunner
  - BaseStorageType (and all implementations)
  - LocalStorage (with proper lock disposal)
  - AWSS3Storage (with timer and resource cleanup)
- **Fixed AWS region bug** - Now uses configured region instead of hard-coded USEast2
- **Made timer configurable** in AWSS3Storage via `UploadIntervalMs` configuration property
- **Enhanced error handling** - Try-catch in event processing with proper logging
- **Added null safety checks** throughout codebase
- **Improved async patterns** - Better handling of async operations
- **Added comprehensive XML documentation** to all public APIs
- **Improved logging** - Using logger instead of Console.WriteLine for errors
- **Code formatting** - Consistent style and removed inconsistent indentation
- **Updated GitHub workflows** to use .NET 9.0

### Added
- XML documentation comments for better IntelliSense support
- Disposal pattern implementation across storage backends
- Configurable upload interval for AWS S3 storage
- Better error logging with structured messages
- Startup/shutdown messages in Program.cs

### Fixed
- AWS S3 region now respects configuration value
- File.Move overwrite parameter prevents exceptions when archiving
- Proper resource cleanup on application shutdown
- Lock usage in LocalStorage (using ReaderWriterLockSlim correctly)
- Nullable warnings with proper null checks

### Security
- Enabled latest language features with LangVersion property
- Maintained security settings (disabled unsafe serialization, UTF-7)
