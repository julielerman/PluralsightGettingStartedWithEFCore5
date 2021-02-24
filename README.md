# Pluralsight Getting Started With EF Core 5  
Exercise files for my Dec 2020 course, Getting Started with EF Core (5) on Pluralsight  

This course will teach you how to use Entity Framework Core 5 to perform data access in your .NET applications.
  
https://bit.ly/EFCore5Start

Note that the main branch projects target SQL Server and require Windows*. The SQLite branch targets SQLite and can be run on Windows, MacOS or Linux.

*Alternatively, you could use SQL Server in a Docker container on MacOS or Linux. ;)

If you are working in a Next.Tech workspace, you will need to do a few things differently than in the course.  This is a very new setup with the Next.Tech workspaces. I hope that early adopters will give me feedback as issues in the github repository (https://github.com/julielerman/PluralsightGettingStartedWithEFCore5) so that I can fix any confusion. The solutions were not designed for this setup.

The course used Visual Studio on Windows. It targetted SQL Server and when it came to Migrations (used in many modules), I demonstrated EF Core's Powershell migration commands. 

This branch targets SQLite. You'll be using the 
Microsoft.EntityFramework.Sqlite package and you can type the package references directly into the csproj file since there's no Nuget Package Manager available outside of Visual Studio. Many of the solutions already have this in place.

The database connection string points to the parent folder that contains all of the projects for a solution. E.g., if you are in M4 Relationship Mapping / AFTER, the database file will be created in the AFTER folder. You can copy the database from one module's solution to another as you move forward.

In the Next.Tech workspace you will be runnng on Linux and using the EF Core CLI Migrations commands at the command line.

Here are the major differences.


When I demonstrate using the EF Core Migraitons commands, here's how to run their CLI counterparts.

Navigate to the SamuraiApp.Data folder in the solution.
Run the command from there. Migrations files will then go inside that folder. The database will get created outside of that folder and be available to the UI when you run that.

instead of add-migration [migration file name] you can run:
dotnet ef migrations add [migration file name]

Instead of remove-migration, run:
dotnet ef migrations remove

instead of update-database you can run:
dotnet ef database update




