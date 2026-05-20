# Wheel Of Fortune

A 2D mobile-style mini-game built with Unity, where players test their luck by spinning a dynamic wheel. Progress through zones, collect valuable loot, and avoid the dreaded bomb to keep your inventory safe!

## 🎮 Gameplay Features

*   **Dynamic Zone Progression:** 
    *   Advance through zones with each successful spin.
    *   **Safe Zones:** Every 5th zone is a guaranteed safe spin (no bombs).
    *   **Super Zones:** Every 30th zone offers high-tier loot.
    *   UI dynamically tracks and displays your next milestone goal.
*   **Interactive Wheel Mechanics:** 
    *   Smooth, physics-like spinning animations powered by DOTween.
    *   Procedurally generated wheel slices using ScriptableObjects for easy data management.
*   **Inventory & Loot System:** 
    *   Real-time inventory tracking.
    *   Collected items are beautifully stacked and displayed in the UI.
*   **Risk & Reward Penalty (Bomb):** 
    *   Landing on a bomb forces the player to make a choice: Give up (lose all current loot and reset) or Revive (spend resources to continue).
*   **Responsive UI Architecture:** 
    *   Fully adaptable UI layouts supporting various mobile aspect ratios including `16:9`, `20:9`, and `4:3`.

## 🛠 Technologies & Tools

*   **Engine:** Unity (2021.3+)
*   **Language:** C#
*   **UI:** Unity uGUI & TextMeshPro
*   **Animation:** DOTween (for polished UI and wheel rotations)

## 🚀 Getting Started

### Prerequisites
*   Unity Editor 2021.3 or higher.
*   DOTween package installed in the project.

### Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/delioglu22/WheelOfFortune.git
   ```
2. Open the project via Unity Hub.
3. Open the `SampleScene` located in `Assets/Scenes`.
4. Press **Play** in the editor to start spinning!

## 📱 Build & Release

To build the APK for Android devices:
1. Go to `File > Build Settings`.
2. Select **Android** and click **Switch Platform**.
3. Click **Build** and save the `.apk` file.
4. You can find the latest stable `.apk` in the [Releases](../../releases) section of this repository.

---