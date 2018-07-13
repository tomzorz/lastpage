# lastpage

Lastpage is a .net core static website generator based on [Mustachio](https://github.com/wildbit/mustachio/) which is based on [Mustache](https://mustache.github.io/).

![](https://img.shields.io/badge/platform-any-green.svg?longCache=true&style=flat-square) ![](https://img.shields.io/badge/nuget-yes-green.svg?longCache=true&style=flat-square) ![](https://img.shields.io/badge/license-MIT-blue.svg?longCache=true&style=flat-square)

You must have [.NET Core 2.1](https://www.microsoft.com/net/download/windows) or higher installed.

## Try the pre-built `lastpage`

You can quickly install and try [lastpage from nuget.org](https://www.nuget.org/packages/lastpage/) using the following commands:

```console
dotnet tool install -g lastpage
dotnetsay
```

> Note: You may need to open a new command/terminal window the first time you install a tool.

You can uninstall the tool using the following command:

```console
dotnet tool uninstall -g lastpage
```

## How does it work?

The main ingredients are as follows:

The **skeleton** is the... well... the skeleton 😊 of the site, usually containing the html, head and body tags - scripts, styles etc...

**Partials** are common parts that can be inserted into the skeleton or pages - think menus, or a social share widget.

**Pages** are what get embedded in the skeleton after being templated - think "index", "news" or "cat gifs 🐱‍👤".

Lastpage uses 4 custom file extensions: .lpc, .lps, .lpl and .lpe.

* .lpc files are actually json files, containing the configuration for the tool - at the moment the tool looks for "lastpage-config.lpc" where it was started
* the .lps file is the skeleton file, actually html inside - this can use partials in it
* the .lpl files are the partials, these can be templated using .json files with the same name (templates will be applied before embedding)
* the .lpe files are the pages, these are going to be .html files after being optionally templated (by a .json file with the same name) and then embedded in the skeleton

**There is a demo setup in the `demo` folder, check it out!**

## Future features

### Implementing automatic updates based on file changes

This is already seen in some comments in the code, sadly `FileSystemWatcher` needs some taming before primetime in this, causes too many exceptions that can't really be caught.

### Nicer logging

It's fairly barebones at the moment.