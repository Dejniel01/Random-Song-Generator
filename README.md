# Random Song Generator
This is a simple .NET console app made to get to know https://random-words-api.vercel.app/ API and "MusicBrainz" API. It generates user-defined number of random English words and searches the MusicBrainz database to match recordings for each of them.

# Getting the code
Since it is a public repository, you can easily `git clone` it using HTTPS/SSH or, alternatively,  fork the repository here on GitHub.

# Building the code
The code was written in C# 8.0 and requires that version (or newer).
It is also necessary to link 2 external packages: 

* RestSharp (used version 107.3.0)
* MetaBrainz.MusicBrainz by Zastai (used version 5.0.0)

If you open the solution file in Visual Studio they should get installed, as they are specified in the project file. Otherwise, use NuGet or any other package manager of your choice to add them manually.

# Using the app
After starting the app it shall ask how many songs are requested. When provided with the correct value (beetwen 5 and 20 by default, can be changed in the corresponding constants) it will start to generate data. This process may take a while due to limited number of MusicBrainz queries per second. After that it should display the results to console. 
