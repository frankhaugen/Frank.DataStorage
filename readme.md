# Frank.DataStorage
A set of nugets to allow for abstraction of whatever data you are storing and help you to keep your code clean and testable.

___
[![GitHub License](https://img.shields.io/github/license/frankhaugen/Frank.DataStorage)](LICENSE)
[![NuGet](https://img.shields.io/nuget/v/Frank.DataStorage.svg)](https://www.nuget.org/packages/Frank.DataStorage)
[![NuGet](https://img.shields.io/nuget/dt/Frank.DataStorage.svg)](https://www.nuget.org/packages/Frank.DataStorage)

![GitHub contributors](https://img.shields.io/github/contributors/frankhaugen/Frank.DataStorage)
![GitHub Release Date - Published_At](https://img.shields.io/github/release-date/frankhaugen/Frank.DataStorage)
![GitHub last commit](https://img.shields.io/github/last-commit/frankhaugen/Frank.DataStorage)
![GitHub commit activity](https://img.shields.io/github/commit-activity/m/frankhaugen/Frank.DataStorage)
![GitHub pull requests](https://img.shields.io/github/issues-pr/frankhaugen/Frank.DataStorage)
![GitHub issues](https://img.shields.io/github/issues/frankhaugen/Frank.DataStorage)
![GitHub closed issues](https://img.shields.io/github/issues-closed/frankhaugen/Frank.DataStorage)
___

## Installation

### NuGet

```bash
dotnet add package Frank.DataStorage
```

## Concepts

### IRepository<T>

This is the main interface that you will use to interact with your data. It is a generic interface that takes a type parameter that is the type of the data you are storing. This interface has a number of methods that you can use to interact with your data.