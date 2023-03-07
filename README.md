# words-world
mobile game

Welcome!
This git repository contains the source code for a Unity project that implements a crossword puzzle game with word-forming mechanics. The development process was split into several stages, which are documented in the commit history.
When starting a new project, I like to think it as hole. Knowing the game would have several levels I though right away of how the level were going to work and how the flow of the game would happen. For example, I thought of doing a scene for each level, but the levels are very similar to each other, therefore I choose to have just one scene for the game play and one for the main menu, and make the levels generate itself using the information from a scriptable object. 
Having already decided the architecture of the code I started implementing the base that it would hold them together. The first set of commits focused on defining the basic game structure and mechanics, including the game manager, UI manager, and input manager. 
Them I’ve started implementing the main game mechanics:
1.	the ability to form words by a continuous touch on the screen was implemented. 
2.	A word dictionary loader was also added to the game, enabling it to load words from Google Sheets.
3.	Validating word input, which involved creating a binary search algorithm to check if the input word was in the word data. 
4.	Setting up the letter controller that receives the level letter that will be used and instantiating it on the appropriate place for the player to use.
5.	Game score related function.
Once the game architecture and mechanics were made I started to think of how the scriptable object would setup the level. 
My initial intention was to make as easier as possible for game designers.  A custom editor for level setup was created, allowing users to define the number of rows and columns for the crossword and initialize/edit the puzzle's grid of characters in the Unity Editor. 
Previously I’ve used packages that can serialize 2D arrays, but since I didn’t know if it was ok for the test, I try to do it myself. Reached a good point but I am not entirely used to make editor changes, and, in the end, it wasn’t working so I decided to change for another step for the scriptable object. 
These new scriptable object lets the user add word and setup a grid position for each word, which is similar with the previous one buy on a different layout that is not as straight forward as the initial idea but still a quick and easy way for the designer to use, plus it makes the crossword generating quicker and easier. 
Each level the game will instantiate the crossword capsules, taking the scriptable object into consideration.
The level gameplay messages were added to the game, including a UI message for invalid and bonus words found. 
The save manager was implemented, which handles the saving using binary serialization. 
The journey score and the last completed level are data saved in the file.

A decision was made to remove the level selection options, which was added to let players search for bonus words and replay levels, but it increased the scope of the project and didn't leave much time to work on it
Finally, the letter line renderer connection was added, though it appears on the scene, it is not being rendered by the camera. I did not have the time to figure out why, so I left the code for the review but could fix the bug in time.
Hope you like my work!
