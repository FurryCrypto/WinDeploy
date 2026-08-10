# Changelog

## Latest release — 2026-08-10

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
