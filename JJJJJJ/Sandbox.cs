using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JJJJJJ
{
    public enum WINDOW_SIZES { HEIGHT = 768, WIDTH = 1024 }


    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Sandbox : Microsoft.Xna.Framework.Game
    {
        #region Fields and Properties

        string[] hitstrings = { "AUCH!", "DOH!", "INSERT HIT SOUND HERE", "NOOOOOO!", "AGH!", "FAQ!", "WTF!", "I DIDNT TOUCH THAT!!", "I LIKE TRAINS", "FFFFFFFUUUUUUUU-", "TROLLED", "WHY!!!", "I HATE THIS GAME", "O RLY?", "OSCAR IS A POTATO", "[UGLY COMPANY THAT CANCELLED OUR FIRST PROJECT]'S FAULT FOR SURE!", "TL;DR", "MAYBE I SHOULD STOP PLAYING NOW", "FEELS LIKE SUPER MEAT BOY", "JUANMI IS GREAT!", "TIME TO BED", "RAGEQUIT NOW?", "..." };

        string[] brakestrings = { "BRAKES!", "NOW WITH SOME MANUAL FRICTION", "LET'S SOME SQUARES PASS OVER THERE" };
        string[] ninjastrings = { "CAN'T TOUCH THIS!", "NINJA MODE!", "TRASSPASSING!", "GHOST MODE!" };
        string[] bulletstrings = { "BULLET TIME!", "MAX PAYNE!", "[UGLY COMPANY THAT CANCELLED OUR FIRST PROJECT] DO YOU LIKE THIS?", "WITH CALM" };
        string[] halfstrings = { "MINIME!", "WINRAR!", "INVERSE MARIO MUSHROOM!", "LITTLE LITTLE" };

        string[] energydepleted = { "TIRED!", "EMPTY!", "UFFF!!!", "NEED SOME REST", "ANY BATTERIES OVER THERE?" };
        string[] energyrestored = { "FULL OF ENERGY AGAIN", "I'VE RETURNED SQUARES!", "I'M FULL", "READY!", "RELOADED!" };

        string[] randomstrings = { "HISCORES HISCORES!", "SOMEDAY I WILL LIVE ON A BEACH", "I'M GETTING A LITTLE DIZZY", "GO UP AND THEN DOWN AND THEN UP...", "BUY ZEIT2 AND SUPPORT JUANMI'S ERASMUS!", "5 EUROS FOR YOUR ADVERTISMENT HERE!", "PROGRAMMING PROGRAMMING", "JUANMI IS A GREAT PERSON, YOU SHOULD TELL HIM", "I'M GETTING HUNGRY, MENSA ANYONE?", "SOMETIMES I FEEL THAT THERE IS A THIRD DIMENSION", "I'M AFRAID OF THIS", "I'VE GOT A BAD FEELING ABOUT THIS", "SOMEDAY OSCAR WILL LET ME DEPLOY ANDROID ON HIS PC", "ANY ERASMUS PARTY SOON?", "PINEAPPLES!!!", "POTATOES!", "DID YOU SEE HOW IS THE WEATHER?", "SAAAAALLY, SAAAAAAAALLY", "TOO MUCH BOUNCE AND NO BEER MAKES BALL UNHAPPY", "PLEASE DON'T SMASH ME AGAINST ONE OF THOSE SQUARES", "EHM", "????", "WHY DON'T THOSE SQUARES GO TO MINECRAFT AND LEFT ME ALONE?", "MAYBE I SHOULD BUY SOME SHIFT ATTACKS", "SPAIN IS DIFFERENT", "WHY SO SERIOUS?", "HATERS GONNA HATE" };

        float timetonextrandom = 5.0f;

        Random sr = new Random(); // Random for strings, not for game
        /// <summary>
        /// The XNA graphics device manager
        /// </summary>
        GraphicsDeviceManager graphics;

        /// <summary>
        /// The XNA sprite batch renderer
        /// </summary>
        SpriteBatch spriteBatch;

        /// <summary>
        /// The XNA primitive batch renderer. Can be used for drawing 
        /// simple points, lines and triangles.
        /// </summary>
        PrimitiveBatch primitiveBatch;

        /// <summary>
        /// The previous state of the keyboard, used for detecting key presses.
        /// </summary>
        KeyboardState keyboardState;

        /// <summary>
        /// The previous state of the gamepad.
        /// </summary>
        GamePadState gamepadState;

        // Some sprites and data for the player and other entities
        Texture2D squareSprite;
        Texture2D circleSprite;
        List<Square> squares = new List<Square>();
        List<ChangeLine> changeLines = new List<ChangeLine>();

        int maxEntitySize = 40;

        Circle player;
        bool playerIsIntersecting = false;
        int playerSize = 30;
        float playerAcceleration = 50;
        float playerMaxSpeed = 600;
        float squareRotation = 0;
        double patterntime = 0;

        float timeBetweenPatterns = 1.0f;
        float timeToNextPattern = 0.0f;

        float indicatortime = 1.0f;
        bool[] indicators = new bool[7 * 2];

        float timeForNextPowerup = 8.0f;

        int sh = 0;

        int[] positions = new int[7];

        List<List<Vector2>> squareLaunchers = new List<List<Vector2>>();

        List<Vector2> currentPattern;

        int minOverlay = (int)((float)WINDOW_SIZES.HEIGHT / 10.0f);

        int side = 0;

        enum PuType { BRAKE, BULLETTIME, HALFSIZE, NINJA };

        PuType powerup = PuType.NINJA;

        float[] decadence = { 1.50f, 2.00f, 1.00f, 5.00f };

        float timeusingpowerup = 0.0f;

        bool usingpowerup = false;

        Random random = new Random();

        float timeToDie = 1.0f;
        bool dying = false;
        float timeDying = 3.0f;

        // 1
        // 2
        // 3
        // 4
        // 5 


        Vector2[][] SidedPatterns = {
                                   new Vector2[]{ new Vector2(1, 0.0f), new Vector2(2, 1.0f), new Vector2(3, 2.0f), new Vector2(4, 3.0f), new Vector2(5, 4.0f)                         },
                                   new Vector2[]{ new Vector2(5, 0.0f), new Vector2(4, 1.0f), new Vector2(3, 2.0f), new Vector2(2, 3.0f), new Vector2(1, 4.0f)                         },
                                   new Vector2[]{ new Vector2(1, 0.0f), new Vector2(2, 0.5f), new Vector2(3, 1.0f), new Vector2(4, 1.5f), new Vector2(5, 2.0f)                         },
                                   new Vector2[]{ new Vector2(5, 0.0f), new Vector2(4, 0.5f), new Vector2(3, 1.0f), new Vector2(2, 1.5f), new Vector2(1, 2.0f)                         },


                                   new Vector2[]{ new Vector2(1, 0.0f), new Vector2(2, 0.0f), new Vector2(4, 1.0f), new Vector2(5, 1.0f), new Vector2(1, 0.0f), new Vector2(2, 0.0f)           },
                                   new Vector2[]{ new Vector2(4, 0.0f), new Vector2(5, 0.0f), new Vector2(1, 1.0f), new Vector2(2, 1.0f),  new Vector2(4, 2.0f), new Vector2(5, 2.0f)      },

                                   
                                   //new Vector2[]{ new Vector2(3, 0.0f), new Vector2(3, 1.0f), new Vector2(3, 2.0f)                                                                           },
                                   //new Vector2[]{ new Vector2(1, 0.0f), new Vector2(1, 1.0f), new Vector2(1, 2.0f)                                                                           },
                                   //new Vector2[]{ new Vector2(2, 0.0f), new Vector2(2, 1.0f), new Vector2(2, 2.0f)                                                                           },
                                   //new Vector2[]{ new Vector2(4, 0.0f), new Vector2(4, 1.0f), new Vector2(4, 2.0f)                                                                           },
                                   //new Vector2[]{ new Vector2(5, 0.0f), new Vector2(5, 1.0f), new Vector2(5, 2.0f)                                                                           },

                                   new Vector2[]{ new Vector2(2, 0.0f), new Vector2(4, 0.5f), new Vector2(3, 1.0f)                                                                           },
                                   new Vector2[]{ new Vector2(4, 0.0f), new Vector2(2, 0.5f), new Vector2(3, 1.0f)                                                                           },

                               
                               };
        Vector2[][] OneSidedPatterns = {

                                   new Vector2[]{ new Vector2(3, 0.0f), new Vector2(3, 0.2f), new Vector2(3, 0.4f)                                                                           },
                                   new Vector2[]{ new Vector2(1, 0.0f), new Vector2(1, 0.2f), new Vector2(1, 0.4f)                                                                           },
                                   new Vector2[]{ new Vector2(2, 0.0f), new Vector2(2, 0.2f), new Vector2(2, 0.4f)                                                                           },
                                   new Vector2[]{ new Vector2(4, 0.0f), new Vector2(4, 0.2f), new Vector2(4, 0.4f)                                                                           },
                                   new Vector2[]{ new Vector2(5, 0.0f), new Vector2(5, 0.2f), new Vector2(5, 0.4f)                                                                           },

                                   new Vector2[]{ new Vector2(1, 0.0f), new Vector2(3, 0.0f), new Vector2(5, 0.0f)          },
                                   new Vector2[]{ new Vector2(3, 0.0f), new Vector2(3, 0.2f), new Vector2(2, 0.2f), new Vector2(4, 0.2f), new Vector2(3, 0.4f) },

                                   new Vector2[]{ new Vector2(2, 0.0f), new Vector2(3, 0.0f), new Vector2(4, 0.0f)                                                                           },
                                   new Vector2[]{ new Vector2(2, 0.0f), new Vector2(3, 0.0f), new Vector2(1, 0.0f)                                                                           },
                                   new Vector2[]{ new Vector2(5, 0.0f), new Vector2(3, 0.0f), new Vector2(4, 0.0f)                                                                           },


                                       };


        /// <summary>
        /// The screen overlay (see ScreenOverlay.cs) for text/debug rendering.
        /// </summary>
        ScreenOverlay overlay;

        Rectangle titleSafe;
        /// <summary>
        /// The title safe rectangle used for both the game and the text overlay
        /// </summary>
        public Rectangle TitleSafe
        {
            get { return titleSafe; }
            set { titleSafe = value; }
        }

        #endregion

        #region Construction and Initialization

        /// <summary>
        /// The constructor of our game. Initializes the graphics.
        /// </summary>
        public Sandbox()
        {
            // Initialize the graphics device and viewport
            graphics = new GraphicsDeviceManager(this);
            // This is the resolution which is compatible with both windows and Xbox 360
#if XBOX
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
#else
            graphics.PreferredBackBufferHeight = (int)WINDOW_SIZES.HEIGHT;
            graphics.PreferredBackBufferWidth = (int)WINDOW_SIZES.WIDTH;
#endif
            Content.RootDirectory = "Content";
            graphics.IsFullScreen = false;

            // Add text overlay (see ScreenOverlay.cs)
            overlay = new ScreenOverlay(this);
            Components.Add(overlay);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Get title safe area (differs between windows and xbox displays)
            titleSafe = GetTitleSafeArea(0.92f);

            // Inform the screen overlay about this as well
            overlay.TitleSafe = this.TitleSafe;

            // Add some entities
            CreateEntities();

            // Call the base class
            base.Initialize();

            overlay.squarebar = this.squareSprite;
        }

        /// <summary>
        /// Creates some random squares and initializes the player.
        /// </summary>
        protected void CreateEntities()
        {
            timeusingpowerup = timeForNextPowerup;
            // Random positioning and size of some entities
            int minEntityX = TitleSafe.X;
            int minEntityY = TitleSafe.Y + minOverlay;
            int maxEntityX = TitleSafe.X + TitleSafe.Width;
            int maxEntityY = TitleSafe.Y + TitleSafe.Height;

            maxEntitySize = (maxEntityY - minEntityY) / (10 * 2);

            for (int i = 0; i < 7 * 2; i++)
            {
                squareLaunchers.Add(new List<Vector2>());
                indicators[i] = false;
            }

            // Change lines
            int egral = 10;
            ChangeLine cl = new ChangeLine(minEntityY + egral);
            changeLines.Add(cl);
            cl = new ChangeLine((int)WINDOW_SIZES.HEIGHT - (egral * 4));
            changeLines.Add(cl);

            // Add the player in the center
            player = new Circle(new Vector2(TitleSafe.X + TitleSafe.Width / 2,
                                            TitleSafe.Y + TitleSafe.Height / 2),
                                            playerSize);
            player.Speed = new Vector2(0, playerMaxSpeed);


            sh = (int)((changeLines[1].Position.Y - changeLines[0].Position.Y) / 6.0f);

            for (int i = 0; i < 7; i++)
            {
                positions[i] = (int)changeLines[0].Position.Y + (sh * (i));
            }
        }

        private void restartGame()
        {
            overlay.CurrentTime = 0;
            dying = false;
            timeDying = timeToDie;
            squares = new List<Square>();
            changeLines = new List<ChangeLine>();
            playerIsIntersecting = false;
            squareRotation = 0;
            patterntime = 0;
            timeToNextPattern = 0.0f;

            indicatortime = 1.0f;
            indicators = new bool[7 * 2];

            sh = 0;

            positions = new int[7];

            squareLaunchers = new List<List<Vector2>>();

            currentPattern = null;

            side = 0;

            powerup = PuType.NINJA;
    

            timeusingpowerup = timeForNextPowerup;

            usingpowerup = false;

            random = new Random();
            overlay.energyused = overlay.energymax;
            overlay.energyreloading = false;
            CreateEntities();

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Create a new PrimitiveBatch, which can be used to draw lines
            primitiveBatch = new PrimitiveBatch(GraphicsDevice);

            // Load player and entity sprites
            squareSprite = Content.Load<Texture2D>("Sprites\\Square");
            circleSprite = Content.Load<Texture2D>("Sprites\\Circle");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {

            // TODO: Unload any non ContentManager content here
            Content.Unload();
        }

        #endregion

        #region Game Update, Input and Collisions

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// This method is called from the framework at 60 Hz (if possible).
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            // Update text
            if (!overlay.tosay.Equals("nothing"))
            {
                overlay.stringtime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (overlay.stringtime > 1.5f)
                {
                    overlay.tosay = "nothing";
                }
            }


            // Get (and process/delegate) input

            if (dying)
            {
                timeDying -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                squareRotation += 8.0f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (squareRotation >= Math.PI)
                {
                    squareRotation = 0;
                }
                if (timeDying <= 0.0f)
                {
                    restartGame();
                }
            }
            else
            {
                timeusingpowerup += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (timeusingpowerup > timeForNextPowerup)
                {
                    timeusingpowerup -= timeForNextPowerup;
                    Random rd = new Random();
                    powerup = (PuType)rd.Next(4);
                    switch (powerup)
                    {
                        case PuType.BRAKE:
                            overlay.tosay = brakestrings[sr.Next(brakestrings.Length)];
                            break;

                        case PuType.BULLETTIME:
                            overlay.tosay = bulletstrings[sr.Next(bulletstrings.Length)];
                            break;

                        case PuType.HALFSIZE:
                            overlay.tosay = halfstrings[sr.Next(halfstrings.Length)];
                            break;

                        case PuType.NINJA:
                            overlay.tosay = ninjastrings[sr.Next(ninjastrings.Length)];
                            break;
                    }
                        
                    overlay.stringtime = 0.0f;
                }

                timetonextrandom -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (timetonextrandom <= 0.0f)
                {
                    float f = sr.Next(10) / 2.0f;
                    timetonextrandom = f + 3.0f;

                    if (overlay.tosay.Equals("nothing"))
                    {
                        overlay.tosay = randomstrings[sr.Next(randomstrings.Length)];
                        overlay.stringtime = 0.0f;
                    }
                }

                GetInput(gameTime);

                float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (powerup == PuType.BULLETTIME)
                {
                    if (usingpowerup)
                    {
                        delta *= 0.30f;
                    }
                    else
                    {
                        delta *= 1.0f;
                    }
                }

                // Control patterns
                UpdatePatterns(delta);

                UpdateLauncher(delta);

                // Move the entities and the player
                UpdateEntities(delta);
                UpdatePlayer(delta);
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Grabs and processes the user input.
        /// </summary>
        private void GetInput(GameTime gameTime)
        {
            // Grab some info from the gamepad or keyboard
            GamePadState gamepad = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboard = Keyboard.GetState();

            if ((!overlay.energyreloading) && (keyboard.IsKeyDown(Keys.A) || keyboard.IsKeyDown(Keys.Space) || gamepadState.IsButtonDown(Buttons.RightShoulder) || gamepadState.IsButtonDown(Buttons.A)))
            {
                    overlay.energyused -= (float)(decadence[(int)powerup] * gameTime.ElapsedGameTime.TotalSeconds);
                    if (overlay.energyused <= 0.0f)
                    {
                        overlay.energyreloading = true;
                        usingpowerup = false;
                        overlay.tosay = energydepleted[sr.Next(energydepleted.Length)];
                        overlay.stringtime = 0.0f;
                    }
                    else
                    {
                        usingpowerup = true;
                    }
                
            }
            else
            {
                usingpowerup = false;
                overlay.energyused += (float)(1.0f * gameTime.ElapsedGameTime.TotalSeconds);
                if (overlay.energyused >= overlay.energymax)
                {
                    overlay.energyused = overlay.energymax;
                    if (overlay.energyreloading == true)
                    {
                        overlay.tosay = energyrestored[sr.Next(energyrestored.Length)];
                        overlay.stringtime = 0.0f;
                    }
                    overlay.energyreloading = false;
                }
            }
            

            // Create some player velocity

            // Try for keyboard 1st
            Vector2 vel = new Vector2(0, 0);
            //if (keyboard.IsKeyDown(Keys.Up))
            //{
            //    vel.Y += -playerAcceleration;
            //}
            if (keyboard.IsKeyDown(Keys.Left))
            {
                vel.X += -playerAcceleration;
            }
            if (keyboard.IsKeyDown(Keys.Right))
            {
                vel.X += playerAcceleration;
            }
            //if (keyboard.iskeydown(keys.down))
            //{
            //    vel.y += playeracceleration;
            //}

            // Also check for player one gamepad thumbsticks
            if (gamepad.IsConnected)
            {
                vel.X = gamepad.ThumbSticks.Left.X * playerAcceleration;
                //vel.Y = -gamepad.ThumbSticks.Left.Y * playerAcceleration;
            }

            // Add to player speed
            player.VelX += vel.X;

            if (powerup == PuType.BRAKE)
            {
                if (usingpowerup)
                {
                    player.VelY *= 0.95f;
                    float minimum = playerMaxSpeed / 3.0f;
                    if (Math.Abs(player.VelY) < minimum)
                    {
                        if (player.VelY > 0)
                            player.VelY = minimum;
                        else
                            player.VelY = -minimum;

                    }
                }
                else
                {
                    player.VelY *= 1.05f;
                    float minimum = playerMaxSpeed;
                    if (Math.Abs(player.VelY) > minimum)
                    {
                        if (player.VelY > 0)
                            player.VelY = minimum;
                        else
                            player.VelY = -minimum;

                    }
                }
            }
            else
            {
                float minimum = playerMaxSpeed;
                if (Math.Abs(player.VelY) > minimum)
                {
                    if (player.VelY > 0)
                        player.VelY = minimum;
                    else
                        player.VelY = -minimum;

                }
            }
            // Store the previous keyboard and gamepad states
            keyboardState = keyboard;
            gamepadState = gamepad;
        }

        /// <summary>
        /// Simple entity dummy movement, plus collision detection and resolution.
        /// </summary>
        /// <param name="gameTime"></param>
        void UpdateEntities(float delta)
        {
            squareRotation += 1.0f * delta;
            if (squareRotation >= Math.PI)
            {
                squareRotation = 0;
            }

            List<Square> toRemove = new List<Square>();
            foreach (Square s in squares)
            {
                // Move the square by speed, scaled by elapsed time.
                s.Position += s.Speed * delta;
                
                // Resolve collision with the wall, with elastic bounce
                if (CheckSquarePosition(s))
                {
                    toRemove.Add(s);
                }
                //ResolveCollisionsElastic(squares[i]);

                //if (!ResolveCollisionsSquares(squares[i]))
                //{
                //    i++;
                //}
            }
            foreach (Square s in toRemove)
            {
                Console.WriteLine("Removing square!");
                squares.Remove(s);
            }
        }

        /// <summary>
        /// Simple player movement, plus collision detection and resolution.
        /// </summary>
        /// <param name="gameTime"></param>
        void UpdatePlayer(float delta)
        {
            overlay.CurrentTime += delta;

            // Check for max speed
            if (player.VelX > playerMaxSpeed)
            {
                player.VelX = playerMaxSpeed;
            }
            else if (player.VelX < -playerMaxSpeed)
            {
                player.VelX = -playerMaxSpeed;
            }

            // Move the player by speed, scaled by elapsed time. 
            player.Position += player.Speed * delta;

            if (powerup == PuType.HALFSIZE && usingpowerup)
            {
                player.HalfSize = (int)(playerSize * 0.40f);
            }
            else
            {
                player.HalfSize = (int)(playerSize * 1.0f);
            }

            // Resolve collisions
            Wrap(player);
            foreach (ChangeLine cl in changeLines)
            {
                if (cl.BoundingBox.Contains(player.BoundingSphere) == ContainmentType.Intersects)
                {
                    if ((player.PosY > cl.Position.Y && player.Speed.Y < 0) || (player.PosY < cl.Position.Y && player.Speed.Y > 0))
                        player.Speed *= new Vector2(1.0f, -1.0f);

                }
            }

            overlay.PlayerSpeed = player.Speed;

            // check for collisions with the other entities
            playerIsIntersecting = false;
            for (int i = 0; i < squares.Count; i++)
            {
                if (Intersects(player, squares[i]))
                {
                    if (powerup == PuType.NINJA)
                    {
                        if (usingpowerup)
                        {

                        }
                        else
                        {
                            playerIsIntersecting = true;
                            if (overlay.CurrentTime > overlay.RecordTime)
                            {
                                overlay.RecordTime = overlay.CurrentTime;
                            }

                            dying = true;
                            timeDying = timeToDie;
                            Random d = new Random();
                            overlay.tosay = hitstrings[d.Next(hitstrings.Length)];
                            overlay.stringtime = 0.0f;
                            break;
                        }
                    }
                    else
                    {
                        playerIsIntersecting = true;
                        if (overlay.CurrentTime > overlay.RecordTime)
                        {
                            overlay.RecordTime = overlay.CurrentTime;
                        }

                        dying = true;
                        timeDying = timeToDie;
                        Random d = new Random();
                        overlay.tosay = hitstrings[d.Next(hitstrings.Length)];
                        overlay.stringtime = 0.0f;
                        break;
                    }
                }
            }

            overlay.playerpos = player.Position;
            // Artificial damping, so the player stops after a while
            //player.Speed *= 0.95f;
        }

        private bool ResolveCollisionsSquares(Square _sq)
        {
            Square collided = null;
            foreach (Square sq in this.squares)
            {
                if (sq != _sq)
                {
                    if (Intersects(sq, _sq))
                    {
                        collided = sq;
                    }
                }
            }

            if (collided != null)
            {
                if (collided.Size >= _sq.Size)
                {
                    Absorb(collided, _sq);
                    squares.Remove(_sq);
                }
                else
                {
                    Absorb(_sq, collided);
                    squares.Remove(collided);
                }
            }
            return collided != null;
        }

        private void Absorb(Square _big, Square _small)
        {
            _big.HalfSize += _small.HalfSize / 2;
        }

        private void UpdateLauncher(float delta)
        {
            bool empty = true;
            for (int i = 0; i < squareLaunchers.Count; i++)
            {
                bool showIndicator = false;
                List<Vector2> launcher = squareLaunchers[i];
                for (int j = launcher.Count - 1; j >= 0; j--)
                {
                    Vector2 v = launcher[j];
                    v.Y -= delta;
                    launcher.RemoveAt(j);
                    if (v.Y < -indicatortime)
                    {
                        Square s = new Square(new Vector2(i < 8 ? 0 : TitleSafe.X + TitleSafe.Width, positions[(int)v.X]), maxEntitySize);
                        s.Speed = new Vector2(i < 8 ? playerMaxSpeed : -playerMaxSpeed, 0);
                        squares.Add(s);
                        Console.WriteLine("Spawning: " + v.X + " " + v.Y + " in " + s.Position + " at " + s.Speed);
                    }
                    else if (v.Y < 0.0f)
                    {
                        showIndicator = true;
                        launcher.Add(v);
                    }
                    else
                    {
                        empty = false;
                        launcher.Add(v);
                    }
                }
                if (showIndicator)
                {
                    indicators[i] = true;
                }
                else
                {
                    indicators[i] = false;
                }
            }

            if (empty)
            {
                if (timeToNextPattern <= 0.0f)
                {
                    timeToNextPattern = timeBetweenPatterns;
                }
                    
                timeToNextPattern -= delta;
                if (timeToNextPattern <= 0.0f)
                {
                    currentPattern = null;
                }
            }
        }


        private void UpdatePatterns(float delta)
        {
            if (currentPattern == null) // Generate a new pattern
            {
                side = random.Next(3);
                int i = 0;
                if (side == 2)
                {
                    i = random.Next(SidedPatterns.Length);
                }
                else
                {
                    i = random.Next(SidedPatterns.Length + OneSidedPatterns.Length);
                }

                currentPattern = new List<Vector2>();

                if (i >= SidedPatterns.Length)
                {
                    foreach (Vector2 v in OneSidedPatterns[i - SidedPatterns.Length])
                    {
                        currentPattern.Add(v);
                    }
                }
                else
                {
                    foreach (Vector2 v in SidedPatterns[i])
                    {
                        currentPattern.Add(v);
                    }
                }
                patterntime = 0;

                Console.WriteLine("New pattern! " + i + " " + side);

                //for (int i = 0; i < numSquares; i++)
                //{
                //    int halfSize = (int)random.Next(minEntitySize, maxEntitySize); ;
                //    Square square = new Square(new Vector2(random.Next(minEntityX, maxEntityX),
                //                                           random.Next(minEntityY, maxEntityY)),
                //                               halfSize);
                //    square.Speed = new Vector2(random.Next(-100, 100), random.Next(-100, 100));
                //    squares.Add(square);
                //}
            }

            // Update current pattern
            patterntime += delta;
            Console.WriteLine("PATTERNTIME: " + patterntime);

            for (int i = currentPattern.Count -1; i >= 0; --i)
            {
                Vector2 v = currentPattern[i];
                currentPattern.Remove(v);

                if (side == 0 || side == 2)
                {
                    squareLaunchers[(int)v.X].Add(v);
                }
                if (side == 1 || side == 2)
                {
                    squareLaunchers[(int)v.X + 7].Add(v);
                }
            } 
        }

        /// <summary>
        /// Resolves collisions with the boundary with some "bounce".
        /// </summary>
        /// <param name="entity">The entity for which we compute collisions.</param>
        private void ResolveCollisionsElastic(Entity entity)
        {
            // Get the boundary of the playfield
            int MaxX = TitleSafe.X + TitleSafe.Width - entity.HalfSize;
            int MinX = TitleSafe.X + entity.HalfSize;
            int MaxY = TitleSafe.Y + TitleSafe.Height - entity.HalfSize;
            int MinY = TitleSafe.Y + entity.HalfSize;

            // Check for collisions
            if (entity.PosX > MaxX)
            {
                entity.VelX *= -1;
                entity.PosX = MaxX;
            }

            else if (entity.PosX < MinX)
            {
                entity.VelX *= -1;
                entity.PosX = MinX;
            }

            if (entity.PosY > MaxY)
            {
                entity.VelY *= -1;
                entity.PosY = MaxY;
            }

            else if (entity.PosY < MinY)
            {
                entity.VelY *= -1;
                entity.PosY = MinY;
            }
        }

        private bool CheckSquarePosition(Square entity)
        {
            // Same a resolve collisions, but with fully inelastic collision and drag 
            // on the screen boundary
            // Get the boundary of the playfield
            int MaxX = TitleSafe.X + TitleSafe.Width;
            int MinX = TitleSafe.X;
            int MaxY = TitleSafe.Y + TitleSafe.Height;
            int MinY = TitleSafe.Y;

            // Check for collisions
            if (entity.PosX > MaxX)
            {
                return true;
            }

            else if (entity.PosX < MinX)
            {
                return true;
            }

            if (entity.PosY > MaxY)
            {
                return true;
            }

            else if (entity.PosY < MinY)
            {
                return true;
            }
            return false;
        }

        private void Wrap(Entity entity)
        {
            // Same a resolve collisions, but with fully inelastic collision and drag 
            // on the screen boundary
            // Get the boundary of the playfield
            int MaxX = TitleSafe.X + TitleSafe.Width;
            int MinX = TitleSafe.X;
            int MaxY = TitleSafe.Y + TitleSafe.Height;
            int MinY = TitleSafe.Y;

            // Check for collisions
            if (entity.PosX > MaxX)
            {
                entity.PosX = MinX;
            }

            else if (entity.PosX < MinX)
            {
                entity.PosX = MaxX;
            }

            if (entity.PosY > MaxY)
            {
                entity.PosY = MinY;
            }

            else if (entity.PosY < MinY)
            {
                entity.PosY = MaxY;
            }
        }

        /// <summary>
        /// Resolves collisions with the boundary with no bounce and some added friction.
        /// </summary>
        /// <param name="entity">The entity for which we compute collisions.</param>
        private void ResolveCollisionsInelastic(Entity entity)
        {
            // Same a resolve collisions, but with fully inelastic collision and drag 
            // on the screen boundary
            // Get the boundary of the playfield
            int MaxX = TitleSafe.X + TitleSafe.Width - entity.HalfSize;
            int MinX = TitleSafe.X + entity.HalfSize;
            int MaxY = TitleSafe.Y + TitleSafe.Height - entity.HalfSize;
            int MinY = TitleSafe.Y + entity.HalfSize;

            // Check for collisions
            if (entity.PosX > MaxX)
            {
                entity.VelX = 0;
                entity.VelY *= 0.9f;
                entity.PosX = MaxX;
            }

            else if (entity.PosX < MinX)
            {
                entity.VelX = 0;
                entity.VelY *= 0.9f;
                entity.PosX = MinX;
            }

            if (entity.PosY > MaxY)
            {
                entity.VelY = 0;
                entity.VelX *= 0.9f;
                entity.PosY = MaxY;
            }

            else if (entity.PosY < MinY)
            {
                entity.VelY = 0;
                entity.VelX *= 0.9f;
                entity.PosY = MinY;
            }
        }

        #endregion

        #region Rendering

        /// <summary>
        /// This is called when the game should draw itself.
        /// This method is called from the framework at 60 Hz (if possible).
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.SteelBlue);
            if (playerIsIntersecting)
                graphics.GraphicsDevice.Clear(Color.Chocolate);

            // Draw a white border around the playfield
            DrawBorder();

            // Draw the random entities
            DrawOutlinedSquares();

            DrawChangeLines();

            // draw the player
            DrawPlayer();

            DrawIndicators(gameTime);

            base.Draw(gameTime);
        }

        private void DrawIndicators(GameTime gameTime)
        {
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            spriteBatch.Begin();

            Texture2D sprite = circleSprite;

            Vector2 spriteOrigin = new Vector2(sprite.Height / 2, sprite.Height / 2);

            float indicatortime = 0.25f;
            Color color = Color.White;
            if (gameTime.TotalGameTime.TotalSeconds % (indicatortime * 2) < indicatortime)
            {
                color = Color.Red;
            }

            float rotation = 0;

            float scale = 0.25f;

            // Draw the sprite
            int indicatordistance = 40;
            for (int i = 0; i < indicators.Length; i++)
            {
                if (indicators[i])
                {
                    bool b = i < 7;
                    Vector2 position = new Vector2(b ? indicatordistance : TitleSafe.X + TitleSafe.Width - indicatordistance, positions[i % 7]);
                    Rectangle r = b ? new Rectangle(0, 0, sprite.Width / 2, sprite.Height) : new Rectangle(sprite.Width / 2, 0, sprite.Width / 2, sprite.Height);

                    spriteBatch.Draw(sprite, position, r, color, rotation, spriteOrigin, scale, SpriteEffects.None, 0f);
                }
            }
            
            spriteBatch.End();
        }

        /// <summary>
        /// Draw the player.
        /// </summary>
        private void DrawPlayer()
        {
            // Compute the sprite scale based on the actual pixel size of the sprite
            float scale = (float)player.Size / (float)circleSprite.Height;

            if (dying)
                scale *= timeDying / timeToDie;

            spriteBatch.Begin();

            Color border = Color.White;
            Color inside = Color.White;

            switch (powerup)
            {
                case PuType.BRAKE:
                    border = Color.ForestGreen;
                    break;

                case PuType.BULLETTIME:
                    border = Color.DarkBlue;
                    if (usingpowerup)
                    {
                        inside = Color.Blue;
                    }
                    break;

                case PuType.HALFSIZE:
                    border = Color.Yellow;
                    break;

                case PuType.NINJA:
                    if (usingpowerup)
                    {
                        border = Color.Purple;
                        inside = Color.MediumPurple;
                    }
                    else
                    {
                        border = Color.MediumPurple;
                        inside = Color.White;
                    }
                    break;
            }
            Color b = border;

            if (timeusingpowerup >= timeForNextPowerup - 1.0f)
            {
                if (timeusingpowerup % 0.25 >= 0.125)
                {
                    b = Color.White;
                }
            }
            else if (timeusingpowerup >= timeForNextPowerup - 3.0f)
            {
                if (timeusingpowerup % 0.5 >= 0.25)
                {
                    b = Color.White;
                }
            }

            // To get a red border on a white circle, draw sprite twice at varying scales
            DrawSprite(circleSprite, player.Position, b, scale * 1.0f, 0f);
            DrawSprite(circleSprite, player.Position, border, scale * 0.8f, 0f);
            DrawSprite(circleSprite, player.Position, inside, scale * 0.6f, 0f);

            spriteBatch.End();
        }

        private void DrawChangeLines()
        {
            spriteBatch.Begin();

            Color color = Color.DarkSlateBlue;

            foreach (ChangeLine cl in changeLines)
            {
                // Draw the sprite
                float scale = ((int)WINDOW_SIZES.WIDTH / squareSprite.Width);
                spriteBatch.Draw(squareSprite, cl.Position, null, color, 0f, Vector2.Zero, new Vector2(scale, 0.01f), SpriteEffects.None, 0f);
            }

            spriteBatch.End();
        }

        /// <summary>
        /// Draw the squares. To achieve the outline rendering effect simply render all 
        /// outlines first, follwed by the white interiors.
        /// </summary>
        private void DrawOutlinedSquares()
        {
            DrawSpriteSquares(Color.Blue, 1.0f);
            DrawSpriteSquares(Color.White, 0.85f);
        }

        /// <summary>
        /// Draw a sprite. Assumes SpriteBatch.Begin() and .End() are called from elsewhere
        /// </summary>
        /// <param name="sprite">The texture to use.</param>
        /// <param name="position">The screen space position.</param>
        /// <param name="color">The player color.</param>
        /// <param name="scale">The scale at which to draw.</param>
        private void DrawSprite(Texture2D sprite, Vector2 position, Color color, float scale, float rotation)
        {
            // Set the origin to the center of the sprite
            Vector2 spriteOrigin = new Vector2(sprite.Height / 2, sprite.Height / 2);

            // Draw the sprite
            spriteBatch.Draw(sprite, position, null,
                             color, rotation, spriteOrigin, scale,
                             SpriteEffects.None, 0f);
        }

        /// <summary>
        /// Draw simple sprite squares.
        /// </summary>
        /// <param name="color">The scale with which to draw.</param>
        /// <param name="scale">The scale at which to draw.</param>
        private void DrawSpriteSquares(Color color, float scale)
        {
            spriteBatch.Begin();

            float s;
            for (int i = 0; i < squares.Count; i++)
            {
                // Compute the sprite scale based on the actual pixel size of the sprite
                s = scale * (float)squares[i].Size / (float)squareSprite.Height;
                DrawSprite(squareSprite, squares[i].Position, color, s, squareRotation);
            }

            spriteBatch.End();
        }

        /// <summary>
        /// Called to draw a single pixel wide border around the screen. 
        /// Uses the PrimitiveBatch class.
        /// </summary>
        private void DrawBorder()
        {
            // tell the primitive batch to start drawing lines
            primitiveBatch.Begin(PrimitiveType.LineList);

            Vector2 tmp = Vector2.Zero;

            // draw borders
            tmp.X = TitleSafe.X; tmp.Y = TitleSafe.Y;
            primitiveBatch.AddVertex(tmp, Color.White);
            tmp.X += TitleSafe.Width - 1;
            primitiveBatch.AddVertex(tmp, Color.White);

            primitiveBatch.AddVertex(tmp, Color.White);
            tmp.Y += TitleSafe.Height - 1;
            primitiveBatch.AddVertex(tmp, Color.White);

            primitiveBatch.AddVertex(tmp, Color.White);
            tmp.X = TitleSafe.X;
            primitiveBatch.AddVertex(tmp, Color.White);

            primitiveBatch.AddVertex(tmp, Color.White);
            tmp.Y = TitleSafe.Y;
            primitiveBatch.AddVertex(tmp, Color.White);

            // and we're done.
            primitiveBatch.End();
        }

        #endregion

        #region Helper Functions

        /// <summary>
        /// Returns true if the two circles intersect or are contained in one another.
        /// </summary>
        /// <param name="c1">Circle one.</param>
        /// <param name="c2">Circle two.</param>
        /// <returns></returns>
        bool Intersects(Circle c1, Circle c2)
        {
            ContainmentType type = c1.BoundingSphere.Contains(c2.BoundingSphere);
            if (type == ContainmentType.Contains || type == ContainmentType.Intersects)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns true if the two squares intersect or are contained in one another.
        /// </summary>
        /// <param name="s1">Square one.</param>
        /// <param name="s2">Square two.</param>
        /// <returns></returns>
        bool Intersects(Square s1, Square s2)
        {
            ContainmentType type = s1.BoundingBox.Contains(s2.BoundingBox);
            if (type == ContainmentType.Contains || type == ContainmentType.Intersects)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns true if the square and circle intersect.
        /// </summary>
        /// <param name="s1">The square.</param>
        /// <param name="s2">The circle.</param>
        /// <returns></returns>
        bool Intersects(Square s, Circle c)
        {
            ContainmentType type = s.BoundingBox.Contains(c.BoundingSphere);
            // Note: "Contains" is impossible, since our bounding boxes have zero volume
            if (type == ContainmentType.Intersects)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Returns true if the square and circle intersect.
        /// </summary>
        /// <param name="s1">The circle.</param>
        /// <param name="s2">The square.</param>
        /// <returns></returns>
        bool Intersects(Circle c, Square s)
        {
            return Intersects(s, c);
        }

        /// <summary>
        /// Returns the viewport rectangle, scaled down by the passed in parameter if 
        /// developed on the Xbox 360.
        /// </summary>
        /// <param name="percent">The amount of visible screen space on the Xbox 360.</param>
        public Rectangle GetTitleSafeArea(float percent)
        {
            Rectangle retval = new Rectangle(graphics.GraphicsDevice.Viewport.X,
                graphics.GraphicsDevice.Viewport.Y,
                graphics.GraphicsDevice.Viewport.Width,
                graphics.GraphicsDevice.Viewport.Height);
#if XBOX
            // Find Title Safe area of Xbox 360.
            float border = (1 - percent) / 2;
            retval.X = (int)(border * retval.Width);
            retval.Y = (int)(border * retval.Height);
            retval.Width = (int)(percent * retval.Width);
            retval.Height = (int)(percent * retval.Height);
            return retval;
#else
            return retval;
#endif
        }

        #endregion
    }

    #region Dynamic Game Objects

    /// <summary>
    /// The generic Entity class. Extended by Circle and Square.
    /// </summary>
    abstract class Entity
    {
        public Entity(Vector2 position, int halfSize)
        {
            Position = position;
            HalfSize = halfSize;
        }

        private int halfSize;

        /// <summary>
        /// Halfsize doubles as half of the square edge length, 
        /// as well as the circle radius of the respective entity.
        /// </summary>
        public int HalfSize
        {
            get { return halfSize; }
            set { halfSize = value; }
        }

        /// <summary>
        /// Edge length of square, or diameter of circle.
        /// </summary>
        public int Size
        {
            get { return 2 * halfSize; }
        }

        protected Vector2 position;

        /// <summary>
        /// Screen space position of center point of this entitiy.
        /// </summary>
        public virtual Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// Used for assigning a value to the x position of entity
        /// </summary>
        public virtual float PosX
        {
            get { return position.X; }
            set { position.X = value; }
        }

        /// <summary>
        /// Used for assigning a value to the y position of entity
        /// </summary>
        public virtual float PosY
        {
            get { return position.Y; }
            set { position.Y = value; }
        }

        private Vector2 speed;

        /// <summary>
        /// This entity's velocity.
        /// </summary>
        public Vector2 Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        /// <summary>
        /// Returns a unit vector of the speed direction.
        /// </summary>
        public void NormalizeSpeed()
        {
            speed.Normalize();
        }

        /// <summary>
        /// Used for assigning a value to the x component of speed
        /// </summary>
        public float VelX
        {
            get { return speed.X; }
            set { speed.X = value; }
        }

        /// <summary>
        /// Used for assigning a value to the y component of speed
        /// </summary>
        public float VelY
        {
            get { return speed.Y; }
            set { speed.Y = value; }
        }

    }

    /// <summary>
    /// A simple 2D circle.
    /// </summary>
    class Circle : Entity
    {
        public Circle(Vector2 position, int halfSize)
            :
            base(position, halfSize)
        {
            // Vector3 needed for compatibility with (3D) BoundingSphere
            spherePosition = new Vector3(position, 0f);
            boundingSphere = new BoundingSphere(spherePosition, halfSize * 0.8f);
        }

        // Vector3 needed for compatibility with (3D) BoundingSphere
        protected Vector3 spherePosition;
        
        private BoundingSphere boundingSphere;
        /// <summary>
        /// The bounding sphere associated with this circle.
        /// </summary>
        public BoundingSphere BoundingSphere
        {
            get { return boundingSphere; }
        }

        /// <summary>
        /// Update of the bounding sphere center after a position change.
        /// </summary>
        private void UpdateBoundingSphere()
        {
            spherePosition.X = Position.X;
            spherePosition.Y = Position.Y;
            boundingSphere.Center = spherePosition;
            boundingSphere.Radius = HalfSize;
        }

        /// <summary>
        /// Screen space position of center point of this circle.
        /// </summary>
        public override Vector2 Position
        {
            set 
            { 
                position = value;
                UpdateBoundingSphere();
            }
        }

        /// <summary>
        /// Used for assigning a value to the x position of this circle.
        /// </summary>
        public override float PosX
        {
            set 
            { 
                position.X = value;
                UpdateBoundingSphere();
            }
        }

        /// <summary>
        /// Used for assigning a value to the y position of this circle.
        /// </summary>
        public override float PosY
        {
            set
            {
                position.Y = value;
                UpdateBoundingSphere();
            }
        }

    }

    /// <summary>
    /// A simple 2D Square.
    /// </summary>
    class Square : Entity
    {
        public Square(Vector2 position, int halfSize)
            :
            base(position, halfSize) 
        {
            // Vector3 needed for compatibility with (3D) BoundingBox
            min = new Vector3(position.X - halfSize, position.Y - halfSize, 0f);
            max = new Vector3(position.X + halfSize, position.Y + halfSize, 0f);
            boundingBox = new BoundingBox(min, max);
        }

        // Vector3 needed for compatibility with (3D) BoundingBox
        protected Vector3 min;
        protected Vector3 max;

        private BoundingBox boundingBox;
        /// <summary>
        /// The bounding box associated with this square.
        /// </summary>
        public BoundingBox BoundingBox
        {
            get { return boundingBox; }
        }

        /// <summary>
        /// Update of the bounding box after a position change.
        /// </summary>
        private void UpdateBoundingBox()
        {
            // Update bounding box
            min.X = Position.X - HalfSize;
            min.Y = Position.Y - HalfSize;
            max.X = Position.X + HalfSize;
            max.Y = Position.Y + HalfSize;
            boundingBox.Min = min;
            boundingBox.Max = max;
        }

        /// <summary>
        /// Screen space position of center point of this Square.
        /// </summary>
        public override Vector2 Position
        {
            set
            {
                position = value;
                UpdateBoundingBox();
            }
        }

        /// <summary>
        /// Used for assigning a value to the x position of this circle.
        /// </summary>
        public override float PosX
        {
            set
            {
                position.X = value;
                UpdateBoundingBox();
            }
        }

        /// <summary>
        /// Used for assigning a value to the y position of this circle.
        /// </summary>
        public override float PosY
        {
            set
            {
                position.Y = value;
                UpdateBoundingBox();
            }
        }

    }

    class ChangeLine
    {
        public ChangeLine(int _startPos)
        {
            position = new Vector2(0, _startPos);

            // Vector3 needed for compatibility with (3D) BoundingBox
            min = new Vector3(0, _startPos - 1, 0f);
            max = new Vector3((int)WINDOW_SIZES.WIDTH, _startPos + 1, 0f);
            boundingBox = new BoundingBox(min, max);
        }

        // Vector3 needed for compatibility with (3D) BoundingBox
        protected Vector3 min;
        protected Vector3 max;

        private Vector2 position;

        public virtual Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        private BoundingBox boundingBox;
        /// <summary>
        /// The bounding box associated with this square.
        /// </summary>
        public BoundingBox BoundingBox
        {
            get { return boundingBox; }
        }
    }

    #endregion
}
