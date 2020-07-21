# Atomization

> **Update:**
> 
> The main architecture of *UI Engine* project is almost finished, and therefore I can continue writing this game. 

## Introduction

This is a game in which players can play as roles in a politics simulation. 

In this game (simulation), domestic stability is the key to excel. As soon as one of the opponents' stability reduces to zero, that player can no longer impose any influences on the game (game over). 

____
## Current Progress

The overall architecture is completed. All of the game features will be added in the form of plug-ins in the future. 

# Todo

- [x] Re-write the in-game event system and the event propagation process
- [x] Implement a new task sequence. All commands given by the player will take effect after go through a task management sequence. 
- [x] Convert the game object architecture from inheritance oriented to component oriented. 
- [x] Modularize the main indices, i.e., each index will become an independent game object, in order to simulate the intricacies between enterprises-governments and/or government departments. 

## Schedule

### 8/1/20

Finish the coding part of the test package. 

### 8/10/20

Finish the debugging of the test package

### 8/15/20

Finalize miscellaneous adjustments of the architecture

