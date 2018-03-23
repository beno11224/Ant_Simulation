# Ant_Simulation
A Model of 'ant's to test Swarm Intelligence ideas
Based off of an area (Gameboard) for the Ants to move around in. Ants have a 'Home' (represented by a green square) and 'Goals' (yellow pixels).
Contains a couple of core classes, and a couple of example classes:
### Core:
* [GameBoard] Main class, array of Floortile. Contains data and associated methods (such as step and addPheremone)]
* [FloorTile] Each pixel in the Gameboard bitmap represents a 'FloorTile'. Stores what type of tile this is and what that tile can do.
* [Ant] Abstract(ish) class to build other ants from.
* [Pheremone] Object that is created by an ant to give data to other ants. Pheremone has a value (max resolution 256) (the blue value of its colour). Pheremones have a decay rate.
### Example:
* [Normal_Ant] Example of how a normal ant might move and collect 'goal'/points to take back to the homesquare.
