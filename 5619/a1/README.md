# Assignment 1: Getting Started with Unity

**Due: Tuesday, September 21, 10:00pm CDT**

The purpose of the assignment is twofold:

1. Complete the setup of the virtual reality hardware and software that we will be using in this course.
2. Do a little work with Unity.

## Submission Information

You should fill out this information before submitting your assignment.  Make sure to document the name and source of any third party assets such as 3D models, textures, or any other content used that was not solely written by you.  Include sufficient detail for the instructor or TA to easily find them, such as asset store or download links.

Name: 

UMN Email:

Third Party Assets:

## Step 1: Oculus Account Setup

In order to build VR applications on the Oculus quest, you will need to create an Oculus developer account at: https://developer.oculus.com/ 

Unfortunately, Facebook has increased their integration with Oculus software, and by default they want you to log in with your Facebook account.  This is the easiest option.  However, I realize that some of you may not comfortable with this.  Fortunately, developers can use the following workaround:

1. Create an unmerged Oculus developer account.
2. Create an [Oculus Test User](https://skarredghost.com/2020/11/07/how-to-oculus-test-users/).  This will let you log in to the headset for development purposes.  Note that you will not be able to access the Oculus Store or social media functions without using a real Facebook account (you will not need to do either of those in this course).

For some reason, Oculus will ask you to enter a credit card number for verification of your developer account.  (Rest assured that if I ever meet the person that came up with that idea, I will give them a piece of my mind.)  We have confirmed that you do **not** need to enter this information to activate developer mode on your headset.  When you see that prompt, you can just close to the window and go on to step 2.

## Step 2: Oculus Organization Setup

Because Facebook likes to make things unnecessarily complicated, your Oculus developer account also needs to be part of an organization.  You can either create one yourself or join the existing CSCI 5619 organization by emailing your Oculus user name to: csci5619-admin@umn.edu

We then add your Oculus user name to the organization, which will enable you to activate developer mode on your headset in a later step.   Note that your Oculus user name is **not** the same as the email address associated with the account! 

We have to add people manually, so it is recommended that you perform this step early.

## Step 3: Confirm Hardware Prerequisites

The Oculus Quest will also require that have access to the following:
1.	An Android or Apple mobile device for initial setup.  If you do not have a smartphone or tablet, it is possible to complete this step by borrowing one from another student.  The TA can also provide assistance if you need help with completing this step.
2.	A cable for connecting the headset to a computer.  Please note that the Quest comes with a USB-C to USB-C cable only.  If your computer does not have a USB-C port or you are using a CSE Labs computer, then you may need to obtain a USB-C to USB cable.  If you have a newer smartphone with a USB-C charging port, then may already have a cable that will work.  However, if you are purchasing one, I recommend buying one that is advertised to work with Oculus Link.
3.	Spare batteries for the controllers.  Two AA batteries are already included in the box, so you will be able to run everything now.  However, it is highly recommended that you always have spare batteries ready in case they run out while you are in the middle of working on an assignment.

## Step 4: Headset Checkout

The Quest headsets are available to check out from the CSE-IT help desk in Keller Hall 1-201.

Please note that you will need to bring your UCard.  You can keep the headset for the duration of the class and return it at the end of the semester.  

## Step 5: Reset the Headset

If you turn on the headset and it appears that someone else is logged in, then the previous user most likely forgot to reset the headset after they were done.  In that case, I strongly recommend that you complete a factory reset before continuing further.  You can find instructions here: https://support.oculus.com/articles/fix-a-problem/troubleshoot-headsets-and-accessories/troubleshooting-factory-reset-quest-2/

After you reset the headset, you can follow the instructions in the box to complete the setup.  Please note that this will require installing the Oculus app on an Android or Apple mobile device and logging in with your Oculus account.  During this process, the headset will download the newest updates. 

## Step 6: Activate Developer Mode

1.	After you complete the setup process, you will need to enable developer mode.  On your mobile device, open the Oculus app and select the Quest device.  If you completed steps 1 and 2 of this assignment, then you will find a Developer Mode option under Headset Settings.
2.	After this step is completed, you will be able to build VR applications by connecting the Quest to a computer, and the mobile app is no longer necessary.
3.	If this is your first time using an Oculus device, it is recommended that you spend some time familiarizing yourself with using the controllers and navigating the menus.  There are also several free demo games in the Oculus store that you can download and try.

## Step 7: Unity Setup

In this class, we will be learning to develop VR applications using Unity.  The software is available on all CSE Lab machines and can also be installed for free on your own personal computer (Windows, Mac, and some distributions of Linux).

**Option 1: Installing on your own computer** 

Download and install Unity Hub, available at the following link: https://unity3d.com/get-unity/download

The Hub application is useful for creating projects and managing installations of Unity.  For compatibility reasons, you will be **required** to use only the 2020 LTS (Long Term Support) version of Unity in this class.  You will also need to install the following modules, which can be added through Unity Hub:

1. Android Build Support
2. Android SDK & NDK Tools
3. OpenJDK

At the time of this writing, the most recent LTS version number is **2020.3.18f1**.  Note that the minor version number (18f1) may change during the semester if Unity releases an update.  Unfortunately, the projects are not backwards compatible.  If you install Unity at a later point in time, please make sure you install the *exact* version we are using in class.

**Option 2: Using the CSE Labs computers**

Unity 2020.3.18f1 should already be installed with Android support on CSE Labs computers in the Keller 1-254 lab.  Note that access to this lab is available *only* for students in this class, so no other students should be working there this semester.

## Step  8: Unity Tutorials (Optional)

Unity has a very large developer community, and there are extensive tutorials online at: https://learn.unity.com/

If you are unfamiliar with Unity, then you may want to get started by completing a basic tutorial such as [Unity Core Concepts](https://learn.unity.com/tutorial/learn-the-unity-core-concepts).  Note that these tutorials are completely optional and are not part of the assignment grade.  

## Step 9: Build and Deploy a VR Application

To complete this assignment, you are going to create a simple virtual environment and then deploy the application on the Oculus Quest.   If you get stuck on any of these steps, please watch the Lecture 3 video that walks through these steps.

1. Check out this assignment using GitHub classroom.  An empty Unity project has already been created for you.  (Note: this project was created using the VR Project template as shown in class.  If you want to create new projects using this template, you will need to install version 3.0 of Unity Hub.  The VR templates are not present in previous versions.)
2. Configure the project platform settings for Android with support for the Oculus Quest.  You should also give the project a well-structured package name, such as `edu.csci5619.username.assignment1`, where `username` is your x500 ID.
3. Build the APK file and deploy the project to your Quest headset.  You should save it as `Assignment-1.apk` in the same folder as the `README.md` file.  
4. The first time you try to build on a new computer, you may get an error that USB debugging is not enabled.  Put on the headset, and you will see a popup to authorize the computer.  
5. After the build process is complete, you can unplug the headset and put it on.  Your scene should be loaded automatically.  If not, your application is also installed on the Quest and will show up in the Library under Unknown Sources.
6. Test the application on your Oculus Quest.  If it works, then congratulations... you have a working VR development pipeline!

## Step 10: Add Some Objects to the Scene

1.	Now, go back to Unity and add a few 3D objects to the scene (cubes, cylinders, spheres, etc.).
2.	Move each object to different location around the user.
3.	When you have added at least four objects to your scene, *Build and Run* the project again.  

## Step 11: Animate an Object

In this step, you are going to add a C# script that animates one of the 3D objects you created in the previous step.  You can make the object do anything you want, with the following requirements:

1. The object should use the `Update()` function to continuously move in every frame.  You can change the object's translation, rotation, scale, or any combination of the three.
2. The object should never move outside the user's view or disappear.  This means that you may need to make the animation cycle between two end points.

## Rubric

Graded out of 10 points. 

1. Platform settings configured correctly (1)
2. Submission includes APK file (1)
3. Project runs on the Quest (4)
4. Scene contains at least four new objects (2)
5. One of the objects is animated (2)

## Submission

You will need to check out and submit the project through GitHub classroom.  Make sure your APK file is in the root folder. Do not remove the `.gitignore` or `README.md` files.

Please test that your submission meets these requirements.  For example, after you check in your final version of the assignment to GitHub, check it out again to a new directory and make sure everything opens and runs correctly.  You can also test your APK file by installing it manually using [SideQuest](https://sidequestvr.com/).

## License

Material for [CSCI 5619 Fall 2021](https://canvas.umn.edu/courses/268490) by [Evan Suma Rosenberg](https://illusioneering.umn.edu/) is licensed under a [Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International License](http://creativecommons.org/licenses/by-nc-sa/4.0/).

The intent of choosing CC BY-NC-SA 4.0 is to allow individuals and instructors at non-profit entities to use this content.  This includes not-for-profit schools (K-12 and post-secondary). For-profit entities (or people creating courses for those sites) may not use this content without permission (this includes, but is not limited to, for-profit schools and universities and commercial education sites such as Coursera, Udacity, LinkedIn Learning, and other similar sites).   

