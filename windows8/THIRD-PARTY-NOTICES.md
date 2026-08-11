# Third-party notices

The Windows 8/8.1 edition of ESD Installer uses the following separately licensed components. They remain the property of their respective authors.

- **ManagedWimLib 2.5.3** and the included **wimlib 1.14.3** native libraries, Copyright Hajin Jang and Eric Biggers/contributors. Licensed under LGPL-3.0-or-later. Source and license: https://github.com/ied206/ManagedWimLib and https://wimlib.net/
- **DiscUtils 0.16.13** (`DiscUtils.Core`, `DiscUtils.Streams`, `DiscUtils.Iso9660`, and `DiscUtils.Udf`), Copyright Kenneth Bell and contributors. Licensed under the MIT License. Source and license: https://github.com/DiscUtils/DiscUtils
- **Joveler.DynLoader**, Copyright Joveler and contributors. Licensed under the MIT License. Source and license: https://github.com/ied206/Joveler.DynLoader
- **Json.NET (Newtonsoft.Json) 13.0.3**, Copyright James Newton-King and contributors. Licensed under the MIT License. Source and license: https://github.com/JamesNK/Newtonsoft.Json
- Microsoft .NET compatibility libraries distributed through NuGet retain their own license terms and notices.

ESD Installer dynamically loads `libwim-15.dll`; the library files are kept separate so recipients can replace them with a compatible build as permitted by the LGPL.

- **Microsoft Universal C Runtime 10.0.14393** app-local redistributable files, Copyright Microsoft Corporation. These files are included for Windows 8/8.1 compatibility under the Microsoft Windows SDK redistributable terms.
