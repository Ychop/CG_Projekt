# Which module does what?

### Content:
Here you find the embedded Sounds and Textures used for the Game

### Control:
In this folder, there are 2 classes:
1. Controller.cs: 
    This includes the code for controlling all the other classes like calling the functions to interact with the player and the environment and how the Game objects interact with each other (the Intersections).
2. Intersection.cs:
    In this class the intersections between the game objects are handled. the funtions get called in the controller class as mentioned above.
### Framework:
In this folder you will find several modules where different "Managers" are implemented.
1. Soundmanager.cs:
    Here the sound handling is implemented. There are some helper classes as well to make the loading and unloading of the soundeffects easier.
    The only function we need call from this class is public void PlaySound(CachedSound sound)
    #### Important Notice !!!
    The sample rate of all used sound files must be 44100Khz.
    The helper Classes are:
        - CachedSound.cs
            Here we load the soundeffect file into the memory once so we don`t need to reload it every time the sound gets played which saves much computation time!
        - CachedSoundSampleProvider.cs
        The CachedSoundSampleProvider takes the loaded CachedSound and transforms it into a Sample the mixer can actually play!
        Before calling the PlaySound(CachedSound sound) just create an object of type Cachedsound sound and pass it to the function.
2. Ressource.cs:
    Helper class to load images into Memory as Bytestream
    Only functtion to use is public static Stream LoadStream(string name). the "name" is the desired files path as a String. Find an example below in the <Texture.cs> explanation

3. Texture.cs:
    Here is a helper class to easily load textures into the memory without the need to set the repeating values over and over again for each Texture. (Like setting the pixelformat and the TexParameters, generating a MipMap...) Simply call Texture.Load(Stream stream)
    #### Example       
        private int texImage;
        texImage = Texture.Load(Resource.LoadStream(<FilePath>));

4. The Rect.cs and IReadOnlyRectangle.cs Interface:
    A helper class to define the rectangle and simply draw it in any desired size for any Gameobject.
5. SpriteSheetTools.cs:
    The Helperclass to calculate the Tecxture Coordinates for Animation Spritesheets 
    (internal static Rect CalcTexCoords(uint spriteId, uint columns, uint rows))
    or to convert the TextFonts into Ascii signs 
    (internal static IEnumerable<uint> StringToSpriteIds(string text, uint firstCharacter))

### Models:
In the Models folder all interactable and non-interactable gameobjects are implemented with their own unique functions for interacting with the environment.
Every model has the same base object from which it inherits wich is called  <GameObject.cs>.
Also the gamesounds are added in the respective gameobject class.
Classes:
1. Gameobject.cs:
    The Gameobject.cs class contents the blueprint for all other gameobjects that are created.
    - public GameObject(Vector2 position_, float radiusDraw_, float radiusColl_, float velocity_, float hitpoints_, int id_)
    When creating a Gameobject it is necessary to pass at least:
    - <Vector2 position_>   The position of the obj. on the map 
    - <float radiusDraw_>   The Drawsize of the obj.
    - <float radiusColl_>   The radius size of the circle collider for each obj.
    - <float velocity_>     The velocity of the obj. (if it has any, otherwise pass 0f)
    - <float hitpoints_>    The amount of hitpoints of the obj. (if it has any, otherwise pass 0f)
    - <int id_>             The unique id of the obj. to identify it in the List in which it is added
        
    #### Notice:
    Some gameobjects can and might have more values that need to be passed! (e.g. the enemy.cs object)

2. Player.cs:
    Here the player is implemented with the functions to move the player (internal void MovePlayer(Player player, float deltaTime)) and  to align the players facing direction to the mouse cursor (internal void AglignPlayer(Vector2 mousePosition))
3. Enemy.cs:
    Here the enemy is implemented with its function to make the enemies chase towards the player when entering the "Aggro Range". The function gets called in the update()function of the controller.cs class (internal void EnemyAI(Enemy enemy, Player player, float deltaTime)
4. Obstacle.cs:
    Here lies the obstacle "blueprint" for creating obstacles of a desired size.
5. Bullet.cs:
    Here the bullets are created and the Movement function is implemented for moving the bullets after beeing fired.
6. LevelGrid.cs:
    Here the Levelgrid is defined for creating the Level Grid and determining the Level Size.
7. Particle.cs:
     Here the particles are created
8. Pickup.cs:
    Here the different pickups are created
9. Weapon.cs:
    Here the different weapons are created
    Each weapon is of a unique type which is used to identify the weapon and determine the stats like how many rounds per minute can be fired (float RPM), how much damage the weapon deals (float Damage) and how fast the bullet speed is (float Velocity).
10. Transformation.cs:
    This class is used to transform several matrices from their origin to a new coordinate system for example.
    It is a helper class to easily and fast transform different matrices. 
    #### Example:
    It is used in the view.cs view for resizing the aspect ratio of the picture size to the window size with public void Camera.Resize(int width_, int height_)
11. Model.cs:
    In here we create every gameobject and add it to the Gameobject list which is then drawn in the view.cs class. 

### Views:
1. Camera.cs:
    The camera class handles the zoom function of the game with the Scale function and focuses thhe viewpoint always on the player
2. View.cs:
    Here all the gameobjects are Drawn after beeing created in the Model.cs and all the textures and Animations are added.