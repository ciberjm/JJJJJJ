using System;
//using System.Collections.Generic;
using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Input;
//using Microsoft.Xna.Framework.Net;
//using Microsoft.Xna.Framework.Storage;

namespace JJJJJJ
{
    /// <summary>
    /// game component for a simple text overlay
    /// </summary>
    public class ScreenOverlay : DrawableGameComponent
    {
        #region Fields and Properties

        private ContentManager content;
        private SpriteBatch spriteBatch;

        public Texture2D squarebar;

        // For font rendering
        private int fontSpacing = 3;
        private SpriteFont font;
        private SpriteFont smallFont;

        public string tosay = "nothing";
        public float stringtime = 0.0f;

        // Some strings and positioning data
        string xnaString = "JJJJJJ by Juanmi";
        string fpsString = "FPS: {0}";
        string playerSpeedString = "Speed: {0} {1}";
        string currentTimeString = "{0:000.00}";
        string recordTimeString = "Best: {0:000.00}";
        Vector2 xnaStringPos;
        Vector2 xnaStringOrigin;
        Vector2 titleSafeUpperLeft;
        Vector2 playerSpeedPos;

        // For the frame rate counter
        int frameRate = 0;
        int frameCounter = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;

        // for detecting the visible region on the xbox 360 (this is set from Sandbox.cs)
        Rectangle titleSafe;
        /// <summary>
        /// The title safe rectangle used for both the game and the text overlay
        /// </summary>
        public Rectangle TitleSafe
        {
            get { return titleSafe; }
            set { titleSafe = value; }
        }

        public float energymax = 5.0f;
        public float energyused = 5.0f;
        public bool energyreloading = false;

        Vector2 playerSpeed;
        /// <summary>
        /// The player speed display on the overlay. 
        /// Add any other things you want to monitor during gameplay.
        /// </summary>
        public Vector2 PlayerSpeed
        {
            get { return playerSpeed; }
            set { playerSpeed = value; }
        }

        public Vector2 playerpos = Vector2.Zero;

        public float CurrentTime = 0;

        public float RecordTime = 0;

        #endregion

        #region Construction and Initialization

        public ScreenOverlay(Game game)
            : base(game)
        {
            content = new ContentManager(game.Services);
            content.RootDirectory = "Content";
        }

        public override void Initialize()
        {            
            // The center of the string
            xnaStringPos = new Vector2(TitleSafe.Width - 10, TitleSafe.Height);
            titleSafeUpperLeft = new Vector2(TitleSafe.X + 10, TitleSafe.Y + 5);
            playerSpeedPos = new Vector2(TitleSafe.X + 10, TitleSafe.Y + 50);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // Load font and set spacing
            font = content.Load<SpriteFont>("Fonts\\Font");
            font.Spacing = fontSpacing;
            xnaStringOrigin = font.MeasureString(xnaString);
            smallFont = content.Load<SpriteFont>("Fonts\\FontSmall");
        }

        protected override void UnloadContent()
        {
            content.Unload();
        }

        #endregion

        #region Update

        public override void Update(GameTime gameTime)
        {
            // Update the framerate timer
            elapsedTime += gameTime.ElapsedGameTime;

            // Update the frame counter every second
            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }
        }

        #endregion

        #region Rendering

        public override void Draw(GameTime gameTime)
        {
            // Draw text
            spriteBatch.Begin();

            // Font color
            Color fontColor = Color.DarkBlue;

            // Draw the lower right string
            spriteBatch.DrawString(font, xnaString, xnaStringPos, fontColor, 0, xnaStringOrigin, 1.0f, SpriteEffects.None, 0.5f);

            // Update the framerate counter
            frameCounter++;

            // Format data strings
            string fps = string.Format(fpsString, frameRate);
            string speed = string.Format(playerSpeedString, (int)playerSpeed.X, (int)playerSpeed.Y);
            string time = string.Format(currentTimeString, CurrentTime);
            string record = string.Format(recordTimeString, RecordTime);

            // Draw data strings
            spriteBatch.DrawString(font, fps, titleSafeUpperLeft, fontColor, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.5f);

            //spriteBatch.DrawString(smallFont, speed, playerSpeedPos, fontColor, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.5f);

            spriteBatch.DrawString(font, time, new Vector2(-(font.MeasureString(time).X / 2) + TitleSafe.X + TitleSafe.Width / 2, TitleSafe.Y + 20), fontColor, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.5f);

            spriteBatch.DrawString(font, record, new Vector2(TitleSafe.X + TitleSafe.Width - font.MeasureString(record).X - 20, TitleSafe.Y + 20), fontColor, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.5f);


            Vector2 barPos = new Vector2(TitleSafe.X + 20, TitleSafe.Y + 25);

            if (squarebar != null)
            {
                Color o = Color.DarkBlue;
                if (energyreloading)
                {
                    o = Color.Tomato;
                }
                
                spriteBatch.Draw(squarebar, new Vector2(barPos.X - 10, barPos.Y - 10), null, o, 0.0f, new Vector2(0, 0), new Vector2(1.58f, 0.21f), SpriteEffects.None, 0f);
                spriteBatch.Draw(squarebar, barPos, null, Color.DimGray, 0.0f, new Vector2(0.0f, 0.0f), new Vector2(1.50f, 0.12f), SpriteEffects.None, 0f);

                float scale = energyused / energymax;
                Color c = Color.Green;

                if (scale >= 1.0f)
                {
                    c = Color.ForestGreen;
                }
                else if (scale > 0.80f)
                {
                    c = Color.YellowGreen;
                }
                else if (scale > 0.60f)
                {
                    c = Color.GreenYellow;
                }
                else if (scale > 0.40f)
                {
                    c = Color.Yellow;
                }
                else if (scale > 0.20f)
                {
                    c = Color.OrangeRed;
                }
                else if (scale > 0.00f)
                {
                    c = Color.Red;
                }

                spriteBatch.Draw(squarebar, barPos, null, c, 0.0f, new Vector2(0.0f, 0.0f), new Vector2(scale * 1.50f, 0.12f), SpriteEffects.None, 0f);
            }

            spriteBatch.DrawString(font, string.Format("Energy: {0:0.00}", energyused), new Vector2(TitleSafe.X + 20, TitleSafe.Y + 20), fontColor, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.5f);


            if (!tosay.Equals("nothing"))
            {
                Vector2 pos = new Vector2(playerpos.X - font.MeasureString(tosay).X / 2, playerpos.Y - 50);
                spriteBatch.DrawString(font, tosay, pos, Color.PaleTurquoise, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.5f);
            }

            spriteBatch.End();
        }

        #endregion
    }
}