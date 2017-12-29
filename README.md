## Overview

*NOTE*: This engine has only been tested with Unity 5.6.1f1!

This is a basic RPG engine for Unity I created completely from scratch for my entry in a 2 week game jam at https://xavi-smith.itch.io/resist 

The original engine and project was lost, and so I decided to remake and improve upon the project in my free time over the past few months and make it open source on GitHub as a project for myself, and will be updating it every so often. My goal is to eventually have it function as a free standalone RPG engine for Unity!

The general working of the engine will be described below, but in short it is split into 2 sections, *Overworld* and *Battle*. If you want a general idea of how the code works, the gist of battling is outlined in FightManager.cs, PlayerManager.cs, Fighter.cs, PlayerFighter.cs, and EnemyManager.cs. Menus are mostly handled through MenuManager.cs, Menu.cs, and TextManager.cs. 

Credits to Cabeeno Rossley ( https://freesound.org/people/Cabeeno%20Rossley/ ) for sounds and KenneyNL ( http://www.kenney.nl/ ) for the sprites.


## Features
-Dialogue and dialogue triggers
-Battle Menus and basic enemy AI
-Gaining gold and experience to level up, and learn new moves at specific levels
-Hp, Mp, Atk, Def, SpAtk, SpDef, Speed, and status effects for characters
-Easily customizable attacks with different damage modifiers, stat comparisons (e.g. atk vs def, or speed vs spDef), crit chances, and mp costs
-Audio, Input, and Text handling
-Prepackaged art and sound assets with Prefabs to help you get started. 
-Tooltips for easier learning
-A ready-to-go tutorial.

## Changelog 
v1.1
-Actions are now their own class rather than delegates, as it was a pain and ugly to add new types of actions. Made all associated changes to code
-Fighter stats (hp, mp, speed, attack, defence etc) now work like a "dictionary" by matching string names to their stats using GetStat(), GetStatRaw() and SetStat(). This allows attacks to be set more easily (pass in what stats to check against each other by a string)
-TakeAction now uses IsAssignableFrom instead of IsSubclassOf
-TextMenuManager now does the work of advancing flavor text boxes etc. 
-BattlePositions are handled by a manager
-Every Fighter has a Basic Attack
-Every fighter has a unique ID.
-Removed the Struct "Option" in MenuOption script and moved its variables to MenuOption
-Players' skills are now dynamically populated at the beginning of their turn through MenuManager, rather than having been hardcoded
-MenuManager now handles switching to the menu to select target for an action
-Fighters are actually sorted by speed now
-Consolidated Removing Players and Enemies from the fight in FightManager.cs
-"[Name] went down!" and "Critical Hit!" are now displayed on taking down anyone in battle/landing a crit respectively
-Game Over text before transition
-Level up formulae now takes into account individual stat modifiers to customize levelling up (e.g. tanks can level up def 2x as high, but spAtk 0.3x)
-Tooltips for PlayerFighter's LevelUpModifiers, and Fighter.cs's Basic Attack variable.
-Consolidated Losing/Gaining hp and mana into UpdateHealth and UpdateMana respectively
-Stat modifiers stored in a list that gets reset at the end of every battle
-Fight Manager resets state after a battle + removes children
-Skills are learnt upon levelling up (NOTE: make sure they don't have the same script name!)
-All audio is now handled by an audio manager class with one source for music, and another for sound effects.
-Skill menu dynamically populates amount of cursor options as well.
-TextManager has been split into a DialogueManager and BattleTextManager
-DialogueTriggers for forced and playerPrompted dialogue consolidated into one class
-Added a tutorial level and basic README

## Setup

This section will very briefly go over setting up your own RPG area with battles, recruitable party members, and signs/dialogue triggers. An example has been set up in the Overworld Scene, where interacting with any of the enemies toward the left will take you to a battle.

1. Drag a Player prefab from Prefabs->Characters->Player into your scene. The player contains a Rigidbody2D, BoxCollider2D, and PlayerMovement script to allow the player to move them using the WASD keys. For now you can ignore every variable other than Move Speed.
2. The Main Camera follows the Player using CameraScroll.cs (Drag your Player GameObject in to the Player variable so the camera knows what to follow). You can check Cam Lock for any instances where you just want a stationary camera.
3. Drag the Canvas prefab in from Prefabs->UI->Canvas. This includes a Dialogue Manager that handles all text for you
4. Drag the AudioManager (self explanatory), FightManager, GameManager, PlayerManager, and OverworldManager prefabs in.
5. Populate the _Active Areas_ variable of GameManager.cs with all the scenes you plan to have in your project (whenever you add a scene to your build, make sure to update your GameManager as well!)
6. Point the FightManager to your PlayerManager prefab (drag and drop to the Player Manager variable of FightManager)
7. Add the Battle scene to your build
8. Add any EnemyFighter prefabs you want under AreaEnemies for your OverworldManager object/PlayerFighter prefabs you want under PlayerManager to add to your starting party
9. Populate your area with signs/backgrounds etc and you're good to go!


## TODO

-Update Readme
-More Sounds
-Resetting Positions after fights
-Give enemies more attacks
-Cursors cleanup
-Testing ModifierAttacks
-Recovery outside of battle
-Saving
-More Tooltips
-Clean up finding the back button for SetSkill() in MenuManager
-Make back buttons special + dynamic Menus

Depending on the size of the project, if it is small and simple enough the reference docs can be added to the README. For medium size to larger projects it is important to at least provide a link to where the API reference docs live.

## Tests

Describe and show how to run the tests with code examples.

## Contributors

Let people know how they can dive into the project, include important links to things like issue trackers, irc, twitter accounts if applicable.

## License

A short snippet describing the license (MIT, Apache, etc.)
