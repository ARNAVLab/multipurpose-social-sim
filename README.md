# Project Installation / Setup Guide
## Requirements
- [Unity 2022.1.23](https://unity.com/releases/editor/whats-new/2022.1.23#release-notes)
- [MongoDB Community Server](https://www.mongodb.com/try/download/community)
  - By default, the SimManager tool expects to find a MongoDB server running on local port 27017, which is also the default port that new MongoDB servers start on. If you want to change this, you can adjust the port the SimManager is listening for in Assets/Scripts/SimManager/SimulationManager/HistoryManager/MongoHM.cs by changing the string used to assign DbClient near the top of the file.
  - Additionally, the SimManager tool expects a database named "SimManager" to exist on the server. All collections without the database will be generated at runtime if they don't yet exist. Setting this up is very easy in Mongo Compass, so it is highly recommended to use that if possible.
## Recommendations
- [Unity Hub](https://unity.com/unity-hub)
- [Mongo Compass](https://www.mongodb.com/try/download/compass)

## Installation
1. Download the latest stable version of the Social Sim (stored on the 'main' branch).
2. Extract the .zip file.
3. Open the resulting folder with Unity 2022.1.23f1; Unity Hub will select the version automatically.
## Setting up the Simulation
1. Inside the project files, navigate to Assets/Scripts/SimManager/Data.
2. Edit the Actions.json, Agents.json, and Locations.json based on desired Simulation parameters.
3. Ensure that the Paths.json file has references to the .json files you wish to use.
4. In the Unity Editor, navigate to Assets/Scenes/Production.
5. Click the 'Play' button to start the simulation.
## Using the Simulation
![The in-game HUD interface.](https://github.com/ARNAVLab/multipurpose-social-sim/blob/develop/Media/sim_wiki_1.png?raw=true)
- Pan the camera view with WASD, or by clicking and dragging with the MIDDLE mouse button
- Zoom the camera view by scrolling
- Select Agents or Locations by clicking and dragging with the LEFT mouse button
  - A rectangular selection box will appear -- all Agents or Locations in this box when the mouse button is released will be selected as a group
  - When a group is selected, use Left and Right arrow keys to cycle between individual members of the group
- In AGENT MODE (default), only Agents are selectable
  - LOCATION MODE can be enabled / disabled in the bottom-left corner of the screen
- Control the Simulation's speed with the Time Controls in the bottom-center
![The in-game Time Control interface.](https://github.com/ARNAVLab/multipurpose-social-sim/blob/develop/Media/sim_wiki_3.png?raw=true)
![The in-game Agents Panel interface.](https://github.com/ARNAVLab/multipurpose-social-sim/blob/develop/Media/sim_wiki_2.png?raw=true)
