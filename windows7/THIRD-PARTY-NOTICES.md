# Third-party notices

The Windows 7 edition of ESD Installer uses the following separately licensed components. They remain the property of their respective authors.

- **ManagedWimLib 2.6.0** and the included **wimlib 1.14.4** native libraries, Copyright Hajin Jang and Eric Biggers/contributors. Licensed under LGPL-3.0-or-later. Source and license: https://github.com/ied206/ManagedWimLib and https://wimlib.net/
- **DiscUtils 0.16.13** (`DiscUtils.Core`, `DiscUtils.Streams`, `DiscUtils.Iso9660`, and `DiscUtils.Udf`), Copyright Kenneth Bell and contributors. Licensed under the MIT License. Source and license: https://github.com/DiscUtils/DiscUtils
- **Joveler.DynLoader**, Copyright Joveler and contributors. Licensed under the MIT License. Source and license: https://github.com/ied206/Joveler.DynLoader
- Microsoft .NET compatibility libraries distributed through NuGet retain their own license terms and notices.

ESD Installer dynamically loads `libwim-15.dll`; the library files are kept separate so recipients can replace them with a compatible build as permitted by the LGPL.
