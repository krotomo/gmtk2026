
# 🎯 [[Fool's Errand]] — Game Design Document

## 1. Executive Summary
* **One-Sentence Pitch (The Hook):** An unprepared diver attempts to swim his way through aquatic caves to glory.
* **Core Pillars:** 
	* **Pillar 1:** Complex movement
	* **Pillar 2:** Uniquely challenging level design
	* **Pillar 3:** Competent physics simulation
* **Comp Titles:** [[QWOP]] meets [[Dave the Diver]].

## 2. Gameplay & Mechanics
### Core Game Loop
1. **Step 1:** Control your body to swim through parts of the cave
2. **Step 2:** Find or establish air pockets
3. **Step 3:** Gradually learn the layout to plan more efficient movement

### Player Controls & Movement
* **Input Mapping:** 
	* Keyboard/Mouse
		* Q - Kick with left flipper
		* E - Kick with right flipper
		* Scroll wheel - Switch between glowstick colors
		* Mouse position - Point flashlight
		* Left click - Orient body towards cursor.
		* Right click - Toss glowstick
* **Movement Mechanics:** 
		* I.K. Movement of ragdoll limbs
		* Force applied to player body by kicking

### Combat / Interaction Systems
* Currently the player cannot attack/interact with anything

---

## 3. World & Narrative
* **Setting:** In the ocean, in a submerged cave system.
* **The Story Arc:** The story begins with the diver jumping off a boat, which they have forgotten to put the anchor down for, which then drifts away. The diver, having forgotten his oxygen tank, then decides to brave the cave system relying only on his own lung capacity. If, after many trials, he makes it to the deepest chamber, he finds a half-buried pineapple.
* **Key Characters:**
	* [[Character - Nautilus Dumas]]
* **Key Locations:** 
	* [[Location - The Cave]]


---

## 4. Art & Audio Style
### Visual Direction
* **Style:** (e.g., Stylized low-poly, pixel art, hyper-realistic)
* **Moodboard:** Link to your local asset folder or embed external mood boards.

### Audio & Music
* **Sound Effects (SFX):** Audio cues for actions like jumps, hits, or menu UI clicks.
* **Soundtrack:** Desired musical genres, tempos, and soundscapes per zone.

---

## 5. Technical & Progression Systems
### Game Systems / Variables
* This game functions primarily as an exploration of player control mechanics, with environmental challenges.
	* The player object will be an active 2d ragdoll, moved by using inverse kinematics.
	* The player will attempt to orient their body towards the cursor on left click.
	* Using the left/right flipper actions will attempt a "kick" animation with that leg, which, depending on range of motion and body position, will apply force to the player's body.
	* The player will have an "air" meter which ticks down over time. The more the player moves, the faster it depletes. If the player finds an air pocket, and places their character's head into it, they will be able to replenish their air. If they run out of air, they drown, and get a game over.
* Link to technical architectures: [[Fool's Errand - Technical Architecture]]

### Meta-Progression & Economy
* How does the player permanently power up?
* Progression loops, skill trees, or unlockable tiers.

---

## 6. Project Roadmap & Production
* **MVP Scope (Minimum Viable Product):** The bare minimum features needed for a fun test loop.
* **Milestones:**
	* [ ] Greybox prototype complete.
	* [ ] Core movement mechanics polished.
	* [ ] Vertical slice (1 playable level with art/audio).
	* [ ] Alpha / Beta / Release.
