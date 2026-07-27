# 🚀 GitHub User Activity CLI

> A fast and lightweight command-line tool built with **C# and .NET 10** to display the recent public activity of any GitHub user.

This project is a solution to the [roadmap.sh GitHub User Activity challenge](https://roadmap.sh/projects/github-user-activity).

---

## ✨ Features

- ⚡ Fetches recent activity from the GitHub REST API.
- 📊 Summarizes commits, issues, stars, forks, and other events.

---

## 🛠️ Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- Git
- Internet connection

---

## 📥 Installation

```bash
git clone https://github.com/YOUR_USERNAME/github-user-activity.git
cd github-user-activity
dotnet restore
```

---

## ▶️ Usage

Start the application:

```bash
dotnet run
```

Search for a GitHub user:

```text
github-activity octocat
```

Example output:

```text
Recent activity for octocat:

- Pushed 3 commits to octocat/Hello-World
- Opened an issue in octocat/Spoon-Knife
- Starred github/gitignore
```

Use `exit` to close the application.

