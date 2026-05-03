# Beleak Launcher

A fast, lightweight, and clean Minecraft launcher built with **C#**, **.NET**, **Avalonia UI**, and **CmlLib**.

---

## Beleak Launcher

A minimal launcher focused on one goal: launching Minecraft quickly and efficiently without unnecessary complexity.

---

## Screenshots

<img width="836" height="501" alt="Capture d&#39;écran 2026-05-03 133842" src="https://github.com/user-attachments/assets/1832d42d-5dcc-4a9f-8a29-d8c92f38304e" />
<img width="838" height="500" alt="Capture d&#39;écran 2026-05-03 133756" src="https://github.com/user-attachments/assets/2b33da72-b121-4fd5-a4c0-687cb05a1bf6" />


---

## Features

* Fast launching
* Minimal experience with no unnecessary features
* Modern and clean user interface built with Avalonia UI
* Minecraft version selection
* Custom RAM allocation
* One-click play
* Offline account support

---

## Tech Stack

* C# / .NET
* Avalonia UI
* CmlLib

---

## Prerequisites

* .NET SDK (recommended for building from source)

---

## Getting Started

### Clone the Repository

```bash
git clone https://github.com/bestyeti788/Beleak-launcher.git
cd Beleak-launcher
```

---

### Run in Development

```bash
dotnet restore
dotnet run
```

---

### Build for Production

#### Windows

```bash
dotnet publish -c Release -r win-x64 --self-contained -p:PublishSingleFile=true
```

Output:

```
bin/Release/net8.0/win-x64/publish/
```

---

#### Linux

```bash
dotnet publish -c Release -r linux-x64 --self-contained -p:PublishSingleFile=true
```

Output:

```
bin/Release/net8.0/linux-x64/publish/
```

---

## Usage

* Select your Minecraft version
* Adjust RAM settings if needed
* Launch the game
* The launcher handles download and startup automatically

---

## Project Status

Completed project.
Stable and fully functional, with no planned updates.

---

## Contributing

The project is complete, but you are free to fork it, modify it, or use it as a base for your own launcher.

---

## License

MIT License

---

## Support

If you like the project, consider giving it a star.

---

Beleak Launcher — Simple. Fast. No nonsense.
