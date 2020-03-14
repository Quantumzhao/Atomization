# Atomization

> **Update:**
> 
> The main architecture of *UI Engine* project is almost finished, and therefore I can continue writing this game. 

## Introduction

This is a game in which players can play as roles in a political gamming. In this game (simulation), players are able to gain advantages through manipulating their nuclear weapons and enact corresponding domestic and diplomatic policies. 

In this game (simulation), domestic stability is the key to excel. As soon as one of the opponents' stability reduces to zero, that player can no longer impose any influences on the game (game over). 

Commencing a total nuclear is a choice, but it is a risky move which could trigger unbearable nuclear revenge or MAD strikes. 

____

This game (simulator) is not adapted from any existed historical events. The developer (me) is not responsible for any action made by the players, nor its consequences. 

____
## Current Progress

Currently, I am rewriting the underlying architecture of this game, since the previous one is too clumsy and awkward. The version after this major refactor would be quite different from the old one. 

# Todo

- [x] Re-write the in-game event system and the event propagation process
- [x] Implement a new task sequence. All commands given by the player will take effect after go through a task management sequence. 
- [ ] More complicated deployment process
- [ ] Re-write the whole UI using *UI Engine*. 

___
**This game is still under development, and tons of bugs exist. If you have any problems and ideas about the source code, it is welcomed to be posted in ISSUES**