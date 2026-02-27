# 🕹️ P_Arcade

## 📘 Overview

**P_Arcade** is a C# console application that lets you play multiple games inside a single interface.
It includes navigation menus, high‑score tracking, and an easy‑to-use launcher, all running directly in the terminal.

---

## ✨ Features

- 🎮 **Multiple built‑in games**
- 🏆 **High score system** (for supported games)
- 💾 **Persistent score saving** using XML files
  - Saved next to the executable as: `GAMENAME_highscores.xml`
- 🎨 Console‑based UI with keyboard navigation

---

## 🚀 Getting Started

### 1️⃣ Install Requirements

You’ll need **Visual Studio** with the _.NET Desktop Development_ workload installed.

### 2️⃣ Open the Project

- Navigate to the `P_Arcade` folder
- Open the solution file: `P_Arcade.sln`

### 3️⃣ Run the Program

Press **Ctrl + F5** in Visual Studio.

This builds and launches the application.
The executable can also be found here: `P_Arcade/bin/Debug`

---

## 🎯 How to Use

### 📂 Menu Navigation

- Use **Up / Down Arrow Keys** to browse games
- Press **Enter** to launch the selected game

### 🔢 Option Selection

- Every menu allows selecting items by pressing the corresponding **number key**, seen on the left of the item

---

## 📄 Notes

- All scores are stored locally
- Each game can have its own separate score file if it supports high scores
- The application runs entirely in the console. No external libraries required

---
