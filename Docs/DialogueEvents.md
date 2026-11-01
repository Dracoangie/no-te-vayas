# Dialogue events

Development names and code are in English. Player-facing dialogue is in Spanish.
The configured lines follow pages 3 and 4 of the supplied Unity technical brief.

## Components

- `DialogueUI` only displays queued text and fades it in and out.
- `DialogueTrigger` stores one line and exposes `PlayDialogue()` for a UnityEvent callback.
- `playOnEnter` enables activation by the player's collider. Turn it off for interaction and progression callbacks.
- `showOnce` prevents repeated playback for that component during the current scene session. A scene reload resets it.
- `On Dialogue` sends the text and duration to `DialogueUI.ShowDialogue` using the dynamic string/float event entry.

There are no custom Tools menu commands. Reuse the prefabs in `Assets/Prefabs/Dialogue` or duplicate a configured event in the scene.

## NightHouse configuration

All configured objects are under `Dialogue`.

| Object | Required caller | Repeat behavior |
| --- | --- | --- |
| `DialogueTrigger - Arrival` | Player appears inside the entrance trigger | Once |
| `DialogueEvent - BoxExamined` | Box inspection begins | Repeatable |
| `DialogueEvent - ExitLocked` | Exit interaction while the key has not been collected | Repeatable |
| `DialogueEvent - PuzzleFailed` | The third symbol completes an incorrect attempt | Repeatable |
| `DialogueEvent - KeyCollected` | The player explicitly picks up the key | Once |
| `DialogueEvent - Ending` | The exit sequence reaches the mother's line after key collection | Once |

The arrival trigger is wired to play immediately at the player spawn. The other five events are configured and connected to the dialogue UI, with their colliders disabled. Connect the relevant interaction or progression UnityEvent to their `PlayDialogue()` method when implementing those mechanics. Entering the living room or kitchen does not play a clue or start a scare.

The ending event first calls `DialogueUI.ClearDialogue()` and then shows its line, so old feedback cannot delay it. The ending presentation must attribute the line to the mother, close the door, fade to black, show `Fin`, and offer `Reiniciar` and `Salir`; those systems are outside this dialogue-only change.

## Progression rules for future connections

- Reading or entering the kitchen must not fire the footsteps/chair event. Only closing the kitchen note for the first time may do so.
- That event needs two seconds of silence, three hallway footsteps, and a chair move while the chair is out of view. Re-reading either note must stay possible.
- Solving the box does not mean the key has been collected. Only an explicit pickup may unlock the exit.
- The exit sequence must guard against repeated interaction and disable player movement and interaction during the ending.
- Restart should reload the gameplay scene and reset the puzzle, notes, chair, key, door and progression flags.

## Add another dialogue event

1. Duplicate a configured manual event and keep `playOnEnter` off.
2. Edit `dialogue`, `duration`, and `showOnce` in the Inspector.
3. Connect the gameplay object's UnityEvent to the new object's `DialogueTrigger.PlayDialogue()`.
4. Keep its `On Dialogue` callback connected to the shared `DialogueUI.ShowDialogue` dynamic event.

The dialogue layer does not validate the puzzle or grant keys. Those conditions belong to the interaction and progression components that invoke it.

## Validation

The runtime assembly compiles without errors. The six configured lines, replay settings, activation modes, UI callbacks and scene object IDs were checked against the supplied brief. End-to-end gameplay checks still require the note, puzzle, pickup and exit mechanics to be implemented and connected.
