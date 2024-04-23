--- LAYERS ---

Layers are not exported so you have to:
- set up a new User Layer named "Hovering" in Edit/Project Settings/Tags and Layers;
- set up a new User Layer named "Player" if it does not already exists;
- open AppartRV01 in Assets/Scenes;
- select Player at the top of the Hierarchy and change its Layer to "Player" in the inspector if needed;
- select KitchenManager in Managers' children, right click and select properties (it will open a free tab, it will be useful as you will need to change several objects' Layer);
- under Hovering Objects in Kitchen Manager (Script) Component, under each Element select the Kitchen Object and change its Layer to "Hovering".


--- RENDER SETTINGS ---

Enable URP for your projet if it is not already the case, then:
- go to Edit/Project Settings/XR Plug-in Management/OpenVR and set Stereo Rendering Mode to Single Pass Instanced;
- go to Edit/Project Settings/Quality;
- in Render Pipeline Asset select UniversalRenderPipelineAsset located in Assets/Rendering.

--- VR SETTINGS ---

If you want to enable (resp. disable) VR for development:
- open AppartRV01 in Assets/Scenes;
- select RV_GameManager in Managers's children and check (resp. uncheck) the box enable VR in the inspector.
