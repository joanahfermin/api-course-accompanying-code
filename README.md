# api-course-accompanying-code

The source code for the **API course** on [joanahfermin.com](https://joanahfermin.com/courses/api/): a step-by-step tutorial that builds a web API in C# and .NET, with a small web front end on top.

The course builds one project, an online bookshop called **BookShop**. This repository holds the code as it stands after each lesson.

> Work in progress. The code for the first lessons is here now, and the rest follows in order.

## How the code is organised

Every lesson that changes the code has its own folder, named `lesson15`, `lesson16` and so on. The number is the lesson number on the website. Each folder is a complete solution as it stands after that lesson, so you can open it and run it on its own.

```text
api-course-accompanying-code/
├── lesson15/
│   ├── BookShop.slnx
│   └── src/
├── lesson16/
├── lesson17/
└── ...
```

Lessons that only explain an idea and change no code have no folder, so some numbers are skipped.

To see exactly what a lesson changed, compare its folder with the one before:

```bash
diff -r lesson18 lesson19
```

## What you need

- The [.NET 10 SDK](https://dotnet.microsoft.com/download)
- Optional: [DB Browser for SQLite](https://sqlitebrowser.org/), to look inside the database

## Open a folder and run it

Pick the folder of the lesson you want. Open its `BookShop.slnx` in Visual Studio, or work in a terminal from inside the folder:

```bash
cd lesson19
dotnet build
```

From lesson 17 on, the API uses a SQLite database. The database file is not in the repository. Create it with the lesson's migration command. Run `dotnet build` first: on a fresh copy, `dotnet ef` fails (`NETSDK1004`, "Assets file not found") until a build has restored the packages.

```bash
dotnet ef database update --project src/BookShop.Data --startup-project src/BookShop.Api
```

(This needs the `dotnet-ef` tool: `dotnet tool install --global dotnet-ef --version 10.0.15`. The lessons explain it.)

Run the API, then open the address it prints, for example `http://localhost:5027/books`. From lesson 16 on, `/scalar` is a page where you can try the endpoints.

```bash
dotnet run --project src/BookShop.Api
```

Run the tests (from lesson 18 on):

```bash
dotnet test
```

The port number differs on every machine. Use the one your terminal shows.

From lesson 33 on, the web pages are in `src/BookShop.Api/wwwroot` and the API serves them: open `http://localhost:5027/` (your port) in a browser. From lesson 37 on, the API makes login tokens. The signing key in `appsettings.Development.json` is a made-up key for practice, which is fine only because it signs nothing real. Never reuse it, and never commit a real key.

## Lessons

| Folder | Lesson |
|---|---|
| [`lesson15`](lesson15) | [Create the Solution and the API Project](https://joanahfermin.com/courses/api/create-the-solution/) |
| [`lesson16`](lesson16) | [The First Endpoint and Swagger](https://joanahfermin.com/courses/api/first-endpoint-and-swagger/) |
| [`lesson17`](lesson17) | [A Data Project and the First Table with EF Core](https://joanahfermin.com/courses/api/ef-core-and-sqlite/) |
| [`lesson18`](lesson18) | [The Books Service](https://joanahfermin.com/courses/api/the-books-service/) |
| [`lesson19`](lesson19) | [Read Books: GET All and GET One](https://joanahfermin.com/courses/api/read-books/) |
| [`lesson21`](lesson21) | [Errors the Professional Way](https://joanahfermin.com/courses/api/errors-the-professional-way/) |
| [`lesson22`](lesson22) | [Create a Book: POST](https://joanahfermin.com/courses/api/create-a-book/) |
| [`lesson23`](lesson23) | [Update and Delete: PUT and DELETE](https://joanahfermin.com/courses/api/update-and-delete/) |
| [`lesson24`](lesson24) | [Business Rules and Meaningful Errors](https://joanahfermin.com/courses/api/business-rules-and-meaningful-errors/) |
| [`lesson26`](lesson26) | [The Stock Report](https://joanahfermin.com/courses/api/the-stock-report/) |
| [`lesson27`](lesson27) | [Records](https://joanahfermin.com/courses/api/records/) |
| [`lesson28`](lesson28) | [Small C# Shortcuts](https://joanahfermin.com/courses/api/small-csharp-shortcuts/) |
| [`lesson29`](lesson29) | [DTOs for the Books Endpoints](https://joanahfermin.com/courses/api/dtos-for-the-books-endpoints/) |
| [`lesson30`](lesson30) | [More Validation Rules](https://joanahfermin.com/courses/api/more-validation-rules/) |
| [`lesson31`](lesson31) | [Where Business Rules Live](https://joanahfermin.com/courses/api/where-business-rules-live/) |
| [`lesson33`](lesson33) | [The First Web Page](https://joanahfermin.com/courses/api/the-first-web-page/) |
| [`lesson34`](lesson34) | [The Books Screen](https://joanahfermin.com/courses/api/the-books-screen/) |
| [`lesson35`](lesson35) | [The Report Screen](https://joanahfermin.com/courses/api/the-report-screen/) |
| [`lesson37`](lesson37) | [The Login Endpoint](https://joanahfermin.com/courses/api/the-login-endpoint/) |
| [`lesson38`](lesson38) | [Protect Endpoints and Roles](https://joanahfermin.com/courses/api/protect-endpoints-and-roles/) |
| [`lesson39`](lesson39) | [The Login Page](https://joanahfermin.com/courses/api/the-login-page/) |
| [`lesson40`](lesson40) | [Sending the Token](https://joanahfermin.com/courses/api/sending-the-token/) |
| [`lesson41`](lesson41) | [Logging](https://joanahfermin.com/courses/api/logging/) |
| [`lesson42`](lesson42) | [The Trace Id](https://joanahfermin.com/courses/api/the-trace-id/) |
| [`lesson43`](lesson43) | [CORS](https://joanahfermin.com/courses/api/cors/) |

## Back to the course

Read the lessons at [joanahfermin.com/courses/api](https://joanahfermin.com/courses/api/).

## Licence

MIT. See [LICENSE](LICENSE).
