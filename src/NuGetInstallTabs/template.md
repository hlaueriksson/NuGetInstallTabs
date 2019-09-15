| NuGet            |       | [![{{package}}][1]][2]                                 {{ }} |
| :--------------- | ----: | :-------------------------------------------{{-}} |
| Package Manager  | `PM>` | `Install-Package {{package}} -Version {{version}}`                 |
| .NET CLI         | `>`   | `dotnet add package {{package}} --version {{version}}`             |
| PackageReference |       | `<PackageReference Include="{{package}}" Version="{{version}}" />` |
| Paket CLI        | `>`   | `paket add {{package}} --version {{version}}`                      |

[1]: https://img.shields.io/nuget/v/{{package}}.svg?label={{package}}
[2]: https://www.nuget.org/packages/{{package}}