# Changelog

All notable changes to this project will be documented in this file.  

This format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and this project adheres to [Semantic Versioning](https://semver.org/).

## 📘 Versions

- [v1.1.0](#-release-v110--native-aot-support--continuous-verification)
- [v1.0.0](#-release-v100--full-api-coverage--stable-sdk-foundation)

## 🚀 Release v1.1.0 — Native AOT Support & Continuous Verification

**Release Date:** 2026-07-21

This release adds Native AOT and trimming compatibility without changing the public API. JSON serialization now uses source-generated metadata for every API request and response type, while dedicated smoke and CI coverage verify the complete native publishing path.

---

### Added

- Added Native AOT and trimming compatibility through source-generated `System.Text.Json` metadata.
- Added a self-contained AOT smoke application and release-workflow verification for request serialization and response deserialization.
- Added continuous integration checks for multi-target builds, unit tests, and `linux-x64` Native AOT publishing on pull requests and pushes to `master`.

---

### 📦 Notes

Applications can opt into Native AOT by setting `<PublishAot>true</PublishAot>` in the executable project. Applications that do not enable Native AOT continue to use Banned.Bangumi as a regular managed .NET library.

## 🚀 Release v1.0.0 — Full API Coverage & Stable SDK Foundation

**Release Date:** 2026-07-18

This initial stable release provides a modern, strongly typed .NET SDK for the Bangumi OpenAPI. It covers every operation in the bundled specification and establishes a resource-oriented public API, consistent asynchronous behavior, and a tested HTTP foundation.

---

### ✨ Added

* **Complete Bangumi OpenAPI Coverage**
  - Implemented all 56 operations from the bundled `dist.json` specification.
  - Added dedicated services for calendars, subjects, episodes, characters, persons, users, collections, revisions, and indices.
  - Added subject, character, and person search APIs to their corresponding resource services.

* **Unified Client and HTTP Infrastructure**
  - Added `BangumiClient` as the single public entry point for all resource services.
  - Added `BangumiClientOptions` for configuring the API address, User-Agent, access token, timeout, proxy, and caller-provided `HttpClient`.
  - Added built-in `IWebProxy` support for SDK-managed HTTP clients without requiring a custom `HttpClient`.
  - Added no-authentication, optional-authentication, and required-authentication request modes.
  - Added consistent API, authentication, and rate-limit exceptions.
  - Added cancellation support to every public network operation.

* **Strongly Typed Public Models**
  - Added resource-specific request, response, search, collection, revision, and index models.
  - Added the shared `PagedResult<T>` model for paginated responses.
  - Added explicit NSFW search filtering through `NsfwFilterMode`.
  - Added redirect-target `Uri` results for subject images, character images, person images, and user avatars.

* **Documentation and Packaging**
  - Added Simplified Chinese and English XML documentation for all public APIs.
  - Added English and Simplified Chinese usage guides with authentication, pagination, and error-handling examples.
  - Added upstream attribution and project independence notices.
  - Added NuGet metadata and GitHub Actions publishing for package version `1.0.0`.

* **Tests and Framework Support**
  - Added controlled `HttpMessageHandler` tests that do not depend on the live Bangumi API.
  - Added coverage for request paths, methods, queries, JSON bodies, headers, authentication, cancellation, errors, deserialization, and `HttpClient` ownership.
  - Added opt-in live API integration tests with environment-based User-Agent, proxy, and access-token configuration.
  - Added support for .NET 8, .NET 9, and .NET 10.

---

### 📦 Notes

This is the first stable release of Banned.Bangumi. Every public network operation is asynchronous, accepts a `CancellationToken`, and is available through a resource service exposed by `BangumiClient`.
