# api-course-accompanying-code

The source code for the **API course** on [joanahfermin.com](https://joanahfermin.com/courses/api/): a step-by-step tutorial that builds a web API in C# and .NET, with a small web front end on top.

The course builds one project, an online bookshop called **BookShop**. This repository holds the code as it stands after each lesson.

> Work in progress. The code for the first lessons is being added now, and the rest follows in order.

## How the code is organised

Every lesson that changes the code has its own folder, named `lesson15`, `lesson16` and so on. The number is the lesson number on the website. Each folder is a complete solution as it stands after that lesson, so you can open it and run it on its own.

```text
api-course-accompanying-code/
├── lesson15/
│   └── BookShop.slnx
├── lesson16/
│   └── BookShop.slnx
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

## Lessons

| Folder | Lesson |
|---|---|
| _Coming soon_ | |

## Back to the course

Read the lessons at [joanahfermin.com/courses/api](https://joanahfermin.com/courses/api/).
