# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

Dates in European way

## [2023.0.1] 11/01/2023
#Fixed Queue.TryDequeue obsolete

## [2023.0.0] 03/01/2023
#Added
- Dequeued items can now be enqueued back with a delay
#Changed
- Component Pools Can Now be created from a prefab

## [2022.3.0] 02/12/2022
#Changed
- Pools no longer are singletons

## [2022.2.5] 18/11/2022
#Fixed
- Now support webGl Audio Pool

## [2022.2.4] 20/10/2022
#Added
- Timers
- Play sound at position
#Fixed
- Singleton reset on application quit (no more reloading the assemblies to reset static instances)

## [2022.2.3] 15/06/2022
#Added
- Normalize ints
#Fixed
- Better BootStrapper
#Optimize
- Version Loading
-- online
-- local
#Remove
- Progressbars when loading
#Renamed
- EditorUtility to EditorUtils To avoid conflicts

## [2022.2.2] 24/05/2022
#Added
- Project Initialization
-- Import Base packages
-- Import specific packages
-- Import 2D Bundle packages
- Boot Strapper

## [2022.2.1] 19/05/2022
#Added
- ExposedScriptableObject attribute
- Settable Sound Parameters

## [2022.2.0] 03/05/2022
#Added
- WebGL Support for game feedbacks (works for single threaded others too)
Now use ExecuteCoroutine(GameObject) in a coroutine.
#Rescoped
- The execute is now protected, use ExecuteAsync(GameObject) Instead.

## [2022.1.0] - 04/04/2022
###Major Update
- Can't update to this version withoud modify somethings
##Fixed
- Audio Parameters
##Changed
- GameFeedbacks are Now Async Methods
##Removed 
- Obsolete AudioManager

## [2022.0.2] - 04/04/2022
##Fixed
- Audio Pool now is in dont destroy on load
- Scene Loader now has an action when scenes are loaded

## [2022.0.1] - 01/04/2022
##Fixed
- Audio Cue now send to right parameter

## [2022.0.0] - 24/03/2022
### Changed
- version is now with year
### Modified
- Audio sound can now loop and be stopped
- Audio parameters can no longer loop (use above)
- Audio cues don't anymore log default error when empty

## [0.1.4] - 8/03/2022
### Modified
- Audio sounds,Cues and drawers
### Added
- Hideif/showif object compatibility
- Sound PArameters Scriptable Objects

## [0.1.3] - 7/03/2022
### Added
- Cinemachine Sample
- Dotween Sample

## [0.1.2] - 7/03/2022
### Fixed
- ClearLog Build Bug

## [0.1.1] - 28/02/2022
### Fixed
- UserPref Creation
- Normalize now normalizes le variable

## [0.1.0] - 22/02/2022
### Added
- Even More Keyboard shortcuts !:  
 ALT+R to reset transform
 ALT+N to reset GameObject name
 ALT+A to apply prefab
 ALT+Ctrl+A to revert prefab
- EditorPrefs to PlayerPrefs to keep parameters inside individual projects
- Better Utility panel
- Better Editor Base Colors

## [0.0.16] - 22/02/2022
### Added
- Keyboard shortcuts : 
 ALT+E to lock/unlock inspector
 ALT+D to debug/normal inspector mode
 ALT+C to clear console

## [0.0.15] - 17/02/2022
### Added
- Normalizable float extension in Etienne.Utils
- CurveCursor drawer and attribute

## [0.0.14] - 30/01/2022
### Added
- Range (with drawer) and MinMaxRange Attribute

## [0.0.13] - 12/01/2022
### Added
- Hide if and Show if attributes

## [0.0.12] - 12/01/2022
### Fixed
- Force debug mode is renamed to readOnly and now works

## [0.0.11] - 09/01/2022
### Added
- Feedback editor attribute and menu
- Skippable feedback

## [0.0.10] - 09/01/2022
### Removed
- HDRP loading compatibilities causing trouble

## [0.0.9] - 09/01/2022
### Added
- Version Checker and Updater

## [0.0.8] - 09/01/2022
### Added
- Version Checker

## [0.0.7] - 08/01/2022
### Added
- Feedback Editor (instantiate, wait, stop time, play sound)

## [0.0.6] - 26-11-2021
### Added
- No multiples camera in HDRP

## [0.0.5] - 21-11-2021
### Added
- Changelog
- loading System

### Changed
- Assembly definitions

### Removed
- Dotween Assembly Reference

### Fixed
- Camera disables when load has finished
