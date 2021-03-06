# This is a drill.
This is my attempt at the [Social Network programming exercise](EXERCISE.md). It is written in C#.
Most probably you won't use it for anything else than being a learning/teaching material.  
Anyway - if you find this useful - please shoot me a note.  
PRs/issues welcome.

# Building

[![Build Status](https://travis-ci.org/cyplo/social_network.svg?branch=master)](https://travis-ci.org/cyplo/social_network)
[![Build status](https://ci.appveyor.com/api/projects/status/s73wngt2k6bcf58v/branch/master?svg=true)](https://ci.appveyor.com/project/cyplo/social-network/branch/master)


## Requirements

* Mono 4.2 / .Net framework 4.5

## Building on Linux

    nuget restore
    xbuild /p:Configuration=Releas

## Building on Windows

    nuget restore
    msbuild /p:Configuration=Release

# Testing

There are features tests, written using [`behave`](http://pythonhosted.org/behave/).  
Then there are unit tests, I'm using NUnit here.

## Feature tests

### Requirements
* python3

    `pip3 install behave`

### Running feature tests

    behave

### Running unit tests under Linux

    mono ./packages/NUnit.ConsoleRunner.3.4.1/tools/nunit3-console.exe SocialNetworkTests/bin/x86/Release/SocialNetworkTests.dll

### Running unit tests under Windows

     packages\NUnit.ConsoleRunner.3.4.1\tools\nunit3-console.exe SocialNetworkTests\bin\x86\Release\SocialNetworkTests.dll

### A note on the choice of the feature testing framework
I knew I needed some testing framework that would allow me to write high level tests and launch the final executable against them. This would allow me to mimic the interaction the user has with the binary without skipping layers and calling the C# code directly.
I went through `SpecFlow`, `cucumber` + `aruba`, `behave` and `behave` + `cli-bdd`.  

`SpecFlow` - I don't need to drive C# code from the feature tests, so IMHO no need for the overhead. No out-of-the-box support for portable CLI tests. Writing the bridge myself was deemed too cumbersome.
  
`cucumber` + `aruba` - a promising combo, however I had problems extracting process' status reliably and writing my own `mini-aruba` is a bit too much.  

`behave` + `cli-bdd` - a direct port of `cucumber` + `aruba` to python. `cli-bdd` has same problems `aruba` has - mainly it's quite hard to extract the current status of the process. However, writing a mini thing that talks to the CLI directly in `behave` steps is quite easy as python has an excellent multiplatform support for managing processes.   

`behave` + minimal custom code for testing it is then.
