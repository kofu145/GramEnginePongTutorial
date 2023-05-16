# Welcome to GramEngine!
In order to start developing with GramEngine, let's go over some basic concepts integral to the engine.  
This guide will go over each core aspect of the engine, explaining how they work and how they relate to each other.

THIS PAGE IS A WORK IN PROGRESS

# EC Pattern

This engine is based off of an EC system, derived from a full [ECS](https://en.wikipedia.org/wiki/Entity_component_system).

Entities represent containers, or identifiers for collections of data. Each entity would act to simulate a gameobject in your world.

A component would represent objects that store and manipulate data, based on a certain category - these are what entities are made of. 

For instance, you could have a player entity with a Sprite component, and a Player Input component. The Sprite component would store texture data to be rendered and render it, where a Player Input component would store information about keybinds and how to process them.





## note: this next section isn't really important for following the pong tutorial, just a nice to know thing

# States
Usually in game development, each section of a game is divided into portions we refer to as "Game States". In essence, this means our game is separated into a sequence of states that represent every important section of our game. Let's go over this in practice.

### Example Game Loop with States

When we start up our example game, we load into a start menu with options for starting the game, settings, etc. From there, clicking start would bring us to the beginning of our game. While we play our game, we could pause anytime, then resume or return to the initial start screen. Upon beating the game, we could be led to a credits screen where we view our results. All of these different sections could be represented in different "worlds" like this:

-   Start Menu State
-   Main Game State
-   Settings State
-   Pause State
-   End State

![](https://camo.githubusercontent.com/ebc21efa2b2a8640df4db8d68e4454aa5a7fa0f6fb12e0271970bddc909b2178/68747470733a2f2f692e696d6775722e636f6d2f47655271597a762e706e67)

