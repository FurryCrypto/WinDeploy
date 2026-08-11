# Changelog

## Unreleased

### Added

- Added optional update notifications to the Windows 10/11, Windows 8/8.1, and Windows 7 editions without changing their existing installation workflows or visual designs.
- Added quiet startup checks limited to once every 24 hours, an enabled-by-default automatic-check setting, and a manual **Check for Updates** action.
- Added platform-specific update manifests, semantic-version comparison, real download progress, cancellation, HTTPS enforcement, and mandatory SHA-256 verification before an installer can run.
- Added localized update interface text to all 42 application languages.
- Added Arabic, Hebrew, Persian, Afrikaans, Hungarian, Portuguese, Czech, Cyrillic Uyghur, Turkish, Thai, Korean, Japanese, Georgian, Azerbaijani, Traditional Chinese, Norwegian Nynorsk, Kyrgyz, Italian, Romanian, and Icelandic to both the application and installer in every supported edition.
- Renamed the existing Norwegian entry to Norwegian Bokmål and retained the existing Spanish and Simplified Chinese translations.
- Added right-to-left application layout handling for Arabic, Hebrew, and Persian.

## Latest release — 2026-08-10

### Branding

- Renamed the product to **ESD Installer** across the Windows 10/11, Windows 8/8.1, and Windows 7 editions.
- Renamed application and worker executables, assemblies, namespaces, projects, settings paths, logs, installer identities, shortcuts, and release artifacts consistently.

### Fixed

- Fixed the Windows 10/11 **Review Installation** button so it navigates reliably and records navigation failures in the log.
- Fixed the Windows 10/11 installer omitting the elevated installation worker required to perform deployment.
- Fixed overlapping Back, selected-disk summary, and Next controls on the Windows 8/8.1 destination page.
- Fixed Windows 8/8.1 WIM/ESD inspection failures caused by incorrect native wimlib architecture packaging.
- Added the required down-level Universal CRT files to the Windows 8/8.1 installer to prevent missing `api-ms-win-crt-*.dll` startup errors.
- Fixed clipped headings in localized setup and uninstall wizard pages.

### Languages added

The following languages were added to both the application and installer in every supported edition:

- Norwegian
- Finnish
- Swedish
- Mongolian (Cyrillic)
- Armenian
- Kazakh
- Bashkir
- Tatar
- Crimean Tatar
- Abkhazian
- Ossetian
