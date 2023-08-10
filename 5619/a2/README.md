# Assignment 2: Create a Virtual Environment

**Due: Thursday, September 30, 10:00pm CDT**

The purpose of this assignment is for you to gain experience creating virtual environments in Unity.  Your goal is to create your own virtual environment using free 3D art assets found on the web.  You will also implement some basic locomotion using the [XR Interaction Toolkit](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@1.0/manual/index.html).  If you get stuck on any of the steps in this assignment, you should go back and watch the video for [lecture 5](https://github.com/CSCI-5619-Fall-2021/Lecture-5).

Creativity is encouraged!

## Submission Information

You should fill out this information before submitting your assignment.  Make sure to document the name and source of any third party assets such as 3D models, textures, or any other content used that was not solely written by you.  Include sufficient detail for the instructor or TA to easily find them, such as a download link.

Name: Jon-Michael Hoang

UMN Email: hoang339@umn.edu

Third Party Assets (All from Unity):
- [WorldSkies Free Skybox Pack](https://assetstore.unity.com/packages/2d/textures-materials/sky/worldskies-free-skybox-pack-86517)
- [Rocks and Boulders 2](https://assetstore.unity.com/packages/3d/props/exterior/rock-and-boulders-2-6947)
- [Japanese Street Lamp](https://assetstore.unity.com/packages/3d/props/exterior/japanese-street-lamp-68184)
- [Japanese Garden Pack](https://assetstore.unity.com/packages/3d/vegetation/trees/japanese-garden-pack-179492)
- [Flying Tree](https://assetstore.unity.com/packages/3d/environments/fantasy/flying-tree-33053)
- [Water Shader](https://assetstore.unity.com/packages/2d/textures-materials/water/simple-water-shader-urp-191449)
- [Low-Poly Models](https://assetstore.unity.com/packages/3d/props/15-low-poly-models-202061)
- [Destroyed City](https://assetstore.unity.com/packages/3d/environments/sci-fi/destroyed-city-free-6459)

Music and Sounds:
- [Ocean SFX](https://www.zapsplat.com/?s=gentle+waves&post_type=music&sound-effect-category-id=)
- [Aidan Clare - Caterpillar In A Mol For Piano](https://www.youtube.com/watch?v=IJAu2RDuAAA)

6+ unique objects downloaded from the internet used in this project:
- The rocks and boulders for the wall behind the camera on start
- The street lamps
- The torii gate from [Japanese Garden Pack]
- The flying tree
- The water shader you see on the ground
- The piano from [Low-Poly Models]
- The ruined skyscraper from [Destroyed City]

***I also did the extra credit portion, where I'd have to implement teleport funcitonality. Places you can teleport to/on are the
buildings, the torii gate, the flying tree, the rock walls, and of course -- the ground.***

## Getting Started

Clone the assignment using GitHub Classroom.  The project has been configured for the Oculus Quest, and the [XR Interaction Toolkit](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@1.0/manual/index.html) package has already been imported.  However, the project does not contain a scene file.  Your goal is to create a new scene using free 3D art assets found on the web, with the following requirements:

- The objects in the scene should be imported art assets.  You cannot use the basic 3D objects in Unity, such as cubes, spheres, cylinders, etc. 
- Do not simply use an already completed demo scene from an asset package.  The goal here is for you to construct your own scene.
- The environment should be an outdoor scene.  
- The assets should display smoothly on the Oculus Quest.  This means you should specifically look for **low poly** assets that are appropriate for rending on a mobile graphics processor.  Make sure to test them to make sure everything runs well!
- You are free to use any legal sources for third party assets, including the [Unity Asset Store](https://assetstore.unity.com/).  Alternatively, you can also locate and export game assets from websites such as [Clara.io](https://clara.io/).  One student in class recommended [Kenney](https://www.kenney.nl/) as a great source of free low poly game assets.

## Rubric

Graded out of 10 points. 

1. Create a new scene. The ground can either be a textured plane or an imported mesh with more complex geometry. (1)
2. Now, add an assortment of 3D objects to the scene.  The environment should also contain *at least* six unique objects that were downloaded on the web. These objects should be placed in the environment to form a plausible scene. (3)
3. Add a [skybox](https://medium.com/nerd-for-tech/tip-of-the-day-skybox-101-in-unity3d-d0b043ece592) to the scene. You can find plenty of skybox textures on the asset store. (1)
4. Add an `XR Rig` to the scene and place it so that the floor is at the correct height when running on the Oculus Quest. Remember to use the **device-based** rig instead of the action-based version. (2)
5. The default `XR Rig` does not include models for the controllers.  Download the [Quest controller models](https://developer.oculus.com/downloads/package/oculus-controller-art/) and add them to the rig.  You can then disable the `XR Interactor Line Visual` components on the left and right controllers. (1)
6. Add a `Locomotion System` to the scene.  Disable the `Teleportation Provider` and then configure the `Snap Turn Provider` to work with both the left and right controllers. (1)
7. Add a `Continuous Move Provider` to the `Locomotion System` and configure it to work with both controllers. (1)

**Bonus Challenge:** Re-enable the `Teleportation Provider` and add set up your scene to also work with teleportation.  The user should be able to teleport by using the grip button on either controller. To accomplish this, you will need to add an invisible `Teleportation Area` at ground height.  Then, add an object to the scene with a flat surface on top (e.g., a raised platform).  Set up a second invisible `Teleportation Area` so that the user can teleport to the top of the object and then back to the ground. You will also need to re-enable the `XR Interactor Line Visual` components on the controllers. Feel free to experiment with different settings to customize their appearance.  (1)

Make sure to document all third party assets in your readme file. ***Be aware that points will be deducted for using third party assets that are not properly documented.***

## Submission

You will need to check out and submit the project through GitHub classroom.  **Make sure your APK file is in the root folder.** Do not remove the `.gitignore` or `README.md` files.

Please test that your submission meets these requirements.  For example, after you check in your final version of the assignment to GitHub, check it out again to a new directory and make sure everything opens and runs correctly.  You can also test your APK file by installing it manually using [SideQuest](https://sidequestvr.com/).

## License

Material for [CSCI 5619 Fall 2021](https://canvas.umn.edu/courses/268490) by [Evan Suma Rosenberg](https://illusioneering.umn.edu/) is licensed under a [Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License](http://creativecommons.org/licenses/by-nc-sa/4.0/).

The intent of choosing CC BY-NC-SA 4.0 is to allow individuals and instructors at non-profit entities to use this content.  This includes not-for-profit schools (K-12 and post-secondary). For-profit entities (or people creating courses for those sites) may not use this content without permission (this includes, but is not limited to, for-profit schools and universities and commercial education sites such as Coursera, Udacity, LinkedIn Learning, and other similar sites).   
