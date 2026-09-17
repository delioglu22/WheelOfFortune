# Wheel Of Fortune

A 2D mobile-style mini-game built with Unity, where players test their luck by spinning a dynamic wheel. Progress through zones, collect valuable loot, and avoid the dreaded bomb to keep your inventory safe!

## 📸 Screenshots

The same scene captured at the three target aspect ratios. No UI element is stretched and nothing is clipped — every panel is anchored rather than positioned by pixel offsets.

**20:9**

![20:9](docs/screenshots/20.9%20Ratio.png)

**16:9**

![16:9](docs/screenshots/16.9%20Ratio.png)

**4:3**

![4:3](docs/screenshots/4.3%20Ratio.png)

## 🎮 Gameplay Features

*   **Dynamic Zone Progression:** 
    *   Advance through zones with each successful spin.
    *   **Safe Zones:** Every 5th zone is a guaranteed safe spin (no bombs).
    *   **Super Zones:** Every 30th zone offers high-tier loot.
    *   A sliding zone bar shows the zones you passed, the one you are on and the ones ahead, highlighting the next safe and super milestones.
*   **Interactive Wheel Mechanics:** 
    *   Smooth, physics-like spinning animations powered by DOTween: a short wind-up, a long spin and a settle at the end.
    *   The indicator is kicked aside on every slice it passes and springs back.
    *   Procedurally generated wheel slices using ScriptableObjects for easy data management.
*   **Inventory & Loot System:** 
    *   Real-time inventory tracking.
    *   The reward pops out of the wheel, bursts into a handful of icons that fly to their own inventory row, and the counter tweens up to the new total.
    *   Weapon rewards are collected as skin points, each skin keeping its own pool.
*   **Risk & Reward Penalty (Bomb):** 
    *   Landing on a bomb forces the player to make a choice: Give up (lose all current loot and reset) or Revive (spend resources to continue).
*   **Responsive UI Architecture:** 
    *   Fully adaptable UI layouts supporting various mobile aspect ratios including `16:9`, `20:9`, and `4:3`.
    *   Locked to landscape, with a Canvas Scaler set to scale with screen size.

## 🛠 Technologies & Tools

*   **Engine:** Unity (2021.3+)
*   **Language:** C#
*   **UI:** Unity uGUI & TextMeshPro
*   **Animation:** DOTween (for polished UI and wheel rotations)
*   **Optimization:** A Sprite Atlas packs the UI sprites into a single page to keep UI draw calls batched.

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
