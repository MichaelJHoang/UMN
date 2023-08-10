# Assignment 3: Making Beat Saber in Unity

**Due: Tuesday, October 19, 10:00pm CDT**

[Beat Saber](https://beatsaber.com/) is widely considered to be one of the best and most successful virtual reality games. There is a free demo available for the Oculus Quest, which you can find in the official store app. Before beginning this assignment, I suggest that you download this demo or watch some videos on YouTube to familiarize yourself with the gameplay.

In this assignment, you will be implementing Beat Saber's core mechanics. You can use Unity's built-in 3D objects (e.g. cubes) for this assignment, and you do not need to import custom meshes. If you want to make your game more interesting, you are also free to import additional assets or modify the appearance of the scene. Creativity is encouraged! However, note that your grade will be based on the interaction functionality, not the artistic quality of the game.

## Submission Information

You should fill out this information before submitting your assignment.  Make sure to document the name and source of any third party assets such as 3D models, textures, or any other content used that was not solely written by you.  Include sufficient detail for the instructor or TA to easily find them, such as a download link.

Name: Jon-Michael Hoang

UMN Email: hoang339@umn.edu

Third Party Assets:

* afaik, only those from Unity and those given

## Getting Started

Clone the assignment using GitHub Classroom.  The project has been configured for the Oculus Quest, and the [XR Interaction Toolkit](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@1.0/manual/index.html) package has already been imported.  The scene contains the same room with two functional laser swords that are activated using the controller grip buttons, as described in [lecture 9](https://github.com/CSCI-5619-Fall-2021/Lecture-9).

## Rubric

Graded out of 20 points. 

1. Make a single cube that starts at the far end of the room and moves towards the user's start position.  Adjust the size and velocity so that it provides an appropriate challenge. (2)
2. When the user hits a cube with a laser sword, the cube should be destroyed. (2)
3. Play a sound when the cube is destroyed. There are plenty of websites for free sound effects.  You should find one that you like and import it into your project. (1)
4. Create a `CubeSpawner` that creates new cubes at random time intervals between 0.5 and 2.0 seconds.  You can feel free adjust these time thresholds to reach an appropriate difficulty.  The cubes should also start at the far end of the room, fly towards the user, and should be destroyed when hit by a laser sword. *Hint: you can create a template game object in your scene that is hidden under the floor and then use `GameObject.Instantiate` to copy it.* (3)
5. The exact X and Y positions of the spawned cubes should be chosen randomly, but should always be within reach of the user's swords when standing at the start position. (2)
6. If a cube collides with a wall, it should also be destroyed, but no sound effect should be played.  We don't want to end up with infinite cubes that would slow down the Quest! (2)
7. Replace your cubes with the premade one located in the `Prefabs` folder.  This cube has an arrow texture that indicates the direction in which to swing the sword.  Modify your script so that the cube is only destroyed when hit by a sword swung in that direction.  *Hint: you can use the velocity of the rigid body to determine the direction.*  (3)
8. When a cube is spawned, it should be rotated so that the direction of the arrow is either up, down, left, or right, selected randomly.  (2)
9. Finally, modify your `CubeSpawner` so that it spawns two different types of cubes, depending on the sword the user should hit them with.  For example, blue cubes could only be destroyed by the left sword, and red cubes could only be destroyed by the right sword. Finally, adjust the colors of each laser sword to match the cubes. *Hint: you can make a copy of the cube and sword materials and then change their colors in the object inspector.* (3)

**Bonus Challenge:** Transform your project into a real rhythm game!  Find a music track that you like and add it as an audio source in the scene.  Then, modify your `CubeSpawner` so that instead of spawning cubes randomly, the user will need to hit them according to the beat of the music.  You will need to store the time for spawning each cube in a data structure that the `CubeSpawner` can iterate through.  To accomplish this, you may want to play the music track in an external application such as [Audacity](https://www.audacityteam.org/) and make a list of times for each beat.  When you start playing the music, make sure to insert a delay to account for the time for the cubes to travel to the player.  You do not need to do this for the entire song; approximately 20-30 seconds of gameplay will be sufficient for bonus credit. (2)

Make sure to document all third party assets in your readme file. ***Be aware that points will be deducted for using third party assets that are not properly documented.***

## Submission

You will need to check out and submit the project through GitHub classroom.  **Make sure your APK file is in the root folder.** Do not remove the `.gitignore` or `README.md` files.

Please test that your submission meets these requirements.  For example, after you check in your final version of the assignment to GitHub, check it out again to a new directory and make sure everything opens and runs correctly.  You can also test your APK file by installing it manually using [SideQuest](https://sidequestvr.com/).

## Acknowledgements

This project uses a modified version of the [UnityLaserSword](https://github.com/jjxtra/UnityLaserSword) asset by Jeff Johnson (jjxtra).

## License

Material for [CSCI 5619 Fall 2021](https://canvas.umn.edu/courses/268490) by [Evan Suma Rosenberg](https://illusioneering.umn.edu/) is licensed under a [Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License](http://creativecommons.org/licenses/by-nc-sa/4.0/).

The intent of choosing CC BY-NC-SA 4.0 is to allow individuals and instructors at non-profit entities to use this content.  This includes not-for-profit schools (K-12 and post-secondary). For-profit entities (or people creating courses for those sites) may not use this content without permission (this includes, but is not limited to, for-profit schools and universities and commercial education sites such as Coursera, Udacity, LinkedIn Learning, and other similar sites).   