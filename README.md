# 🧩 Checkers Game – C# WinForms

A classic **Checkers game** developed using **C# (.NET)** and **WinForms**, featuring a clean separation between the UI and game logic.

## 🛠️ Key Features

- 🎮 **Two-player gameplay** with full rule support: move validation, capturing, and king promotion.
- 🧠 **Game logic encapsulated in a separate DLL**, ensuring a modular and testable architecture.
- 🧩 **WinForms UI layer** interacts with the logic layer using clean **Action-based communication**.
- 🧼 Follows **Object-Oriented Programming principles** for maintainable and extensible design.

## 🧪 Future Improvements

- Add single-player mode with AI opponent
- Highlight possible moves and enforce turn rules
- Improve visual design and add sound effects

## 📁 Project Structure

Checkers-Game/
├── CheckersGame.sln                   # Solution file
├── CheckersGameLogic/                 # Class Library (Game Logic Layer)
│   ├── BoardCell.cs
│   ├── CheckerPiece.cs
│   ├── eBoardSize.cs
│   ├── eGameMode.cs
|   ├── ePlayerTurn.cs
│   ├── ePieceType.cs
|   ├── GameBoard.cs
|   ├── Move.cs
|   ├── Player.cs
|   └── Position.cs
│
├── CheckersUI/                        # WinForms Application (UI Layer)
│   ├── FormGameBoard.cs                    
│   ├── FormGameSettings.cs                    
│   ├── GameAuthenticator.cs   
│   ├── MovingPictureBox.cs
│   ├── WinFormUI.cs                    
│   ├── Program.cs
│   └── Resources/                     # Images, icons, etc.
│
└── README.md

## 🚀 Getting Started

1. Clone the repository:
   ```bash
   git clone https://github.com/DvirKakun/Checkers-Game.git

2. Open the solution in Visual Studio 2019 or later.

3. Build and run the project.
