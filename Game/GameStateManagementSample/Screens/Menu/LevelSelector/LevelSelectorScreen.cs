#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Threading;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
#endregion

namespace GameStateManagement
{

    class LevelSelector : GameScreen
    {
        #region Fields

        // the number of pixels to pad above and below menu entries for touch input
        const int menuEntryPadding = 10;

        List<LevelEntry> levelEntries = new List<LevelEntry>();
        int selectedEntry = 0;
        string menuTitle;

        //Number of entries each line will hold
        int entriesEachLine = 5;
        int entriLines = 3;
        ContentManager content;

        /// <summary>
        /// Gets the list of menu entries, so derived classes can add
        /// or change the menu contents.
        /// </summary>
        public IList<LevelEntry> LevelEntries
        {
            get { return levelEntries; }
        }

        public ContentManager Content { get { return content; } }

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public LevelSelector()
        {
            // menus generally only need Tap for menu selection
            EnabledGestures = GestureType.Tap | GestureType.HorizontalDrag; 

            this.menuTitle = "Select Level";

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void LoadContent()
        {
            base.LoadContent();
            if(content == null)
            content = new ContentManager(ScreenManager.Game.Services,"Content");

            LevelEntry lvl1 = new LevelEntry("Level 1", "LevelSelectionIcons_SD\\LevelIcon_SD", Stars.BRONZE, this);
            LevelEntry lvl2 = new LevelEntry("Level 2", "LevelSelectionIcons_SD\\LevelIcon_SD", Stars.GOLD, this);
            LevelEntry lvl3 = new LevelEntry("Level 3", "LevelSelectionIcons_SD\\LevelIcon_SD", Stars.SILVER, this);
            LevelEntry lvl4 = new LevelEntry("Level 4", "LevelSelectionIcons_SD\\LevelIcon_SD", Stars.NONE, this);
            LevelEntry lvl5 = new LevelEntry("Level 5", "LevelSelectionIcons_SD\\LevelIcon_SD", Stars.GOLD, this);
            LevelEntry lvl6 = new LevelEntry("Level 6", "LevelSelectionIcons_SD\\LevelIcon_SD", Stars.SILVER, this);
            LevelEntry lvl7 = new LevelEntry("Level 7", "LevelSelectionIcons_SD\\LevelIcon_SD", Stars.LOCKED, this);
            LevelEntry lvl8 = new LevelEntry("Level 8", "LevelSelectionIcons_SD\\LevelIcon_SD", Stars.LOCKED, this);
            LevelEntry lvl9 = new LevelEntry("Level 9", "LevelSelectionIcons_SD\\LevelIcon_SD", Stars.LOCKED, this);
            LevelEntry lvl10 = new LevelEntry("Level 10", "LevelSelectionIcons_SD\\LevelIcon_SD", Stars.LOCKED, this);

            lvl1.Selected += LvL1;
            lvl2.Selected += LvL2;
            lvl3.Selected += LvL3;
            lvl4.Selected += LvL4;
            lvl5.Selected += LvL5;
            lvl6.Selected += LvL6;
            lvl7.Selected += LvL7;
            lvl8.Selected += LvL8;
            lvl9.Selected += LvL9;
            lvl10.Selected += LvL10;
        }
        #endregion

        #region eventHandlers

        /// <summary>
        /// Event handler for when the levelSelector entry is selected
        /// </summary>
        void LvL1(object sender, PlayerIndexEventArgs e){
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                               new GameplayScreen());
            Debug.WriteLine("lvl1");
        }
        void LvL2(object sender, PlayerIndexEventArgs e){
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                               new GameplayScreen());
            Debug.WriteLine("lvl2");
        }
        void LvL3(object sender, PlayerIndexEventArgs e){
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                               new GameplayScreen());
            Debug.WriteLine("lvl3");
    }
        void LvL4(object sender, PlayerIndexEventArgs e){
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                               new GameplayScreen());
            Debug.WriteLine("lvl4");
        }
        void LvL5(object sender, PlayerIndexEventArgs e){
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                               new GameplayScreen());
            Debug.WriteLine("lvl5");
        }
        void LvL6(object sender, PlayerIndexEventArgs e){
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                               new GameplayScreen());
            Debug.WriteLine("lvl6");
        }
        void LvL7(object sender, PlayerIndexEventArgs e){
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                               new GameplayScreen());
            Debug.WriteLine("lvl7");
        }
        void LvL8(object sender, PlayerIndexEventArgs e){
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                               new GameplayScreen());
            Debug.WriteLine("lvl8");
        }
        void LvL9(object sender, PlayerIndexEventArgs e){
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                               new GameplayScreen());
            Debug.WriteLine("lvl9");
        }
        void LvL10(object sender, PlayerIndexEventArgs e){
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                               new GameplayScreen());
            Debug.WriteLine("lvl10");
        }
        
        #endregion

        #region Handle Input

        /// <summary>
        /// Allows the screen to create the hit bounds for a particular menu entry.
        /// </summary>
        protected virtual Rectangle GetMenuEntryHitBounds(LevelEntry entry)
        {
            // the hit bounds are the entire width of the screen, and the height of the entry
            // with some additional padding above and below.
            return new Rectangle(
                (int)entry.Position.X,
                (int)entry.Position.Y,
                entry.GetWidth(this),
                entry.GetHeight(this));
        }

        /// <summary>
        /// Responds to user input, changing the selected entry and accepting
        /// or cancelling the menu.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            // we cancel the current menu screen if the user presses the back button
            PlayerIndex player;
            if (input.IsNewButtonPress(Buttons.Back, ControllingPlayer, out player))
            {
                LoadingScreen.Load(ScreenManager, false, ControllingPlayer, new MenuBackgroundScreen("mainMenubackground"), new MainMenuScreen());
            }

            // look for any taps that occurred and select any entries that were tapped
            foreach (GestureSample gesture in input.Gestures)
            {
                if (gesture.GestureType == GestureType.Tap)
                {
                    // convert the position to a Point that we can test against a Rectangle
                    Point tapLocation = new Point((int)gesture.Position.X, (int)gesture.Position.Y);

                    // iterate the entries to see if any were tapped
                    for (int i = 0; i < levelEntries.Count; i++)
                    {
                        LevelEntry lvlEntry = levelEntries[i];

                        if (GetMenuEntryHitBounds(lvlEntry).Contains(tapLocation))
                        {
                            if (lvlEntry.starsDisplayed != Stars.LOCKED)
                            {
                                // select the entry. since gestures are only available on Windows Phone,
                                // we can safely pass PlayerIndex.One to all entries since there is only
                                // one player on Windows Phone.
                                OnSelectEntry(i, PlayerIndex.One);
                            }
                            else
                            {
                                Debug.WriteLine("Level locked");
                            }
                        }
                    }
                }
                if (gesture.GestureType == GestureType.HorizontalDrag)
                {
                    TransitionPosition = -(float)gesture.Delta.X*0.1f;
                    if (gesture.Delta.X >= 10)
                    {
                        IsExiting = true;
                    }
                    else if (gesture.Delta.X <= -10)
                    {
                        IsExiting = true;
                    }
                    Debug.WriteLine(gesture.Delta.X);
                }
            }
        }


        /// <summary>
        /// Handler for when the user has chosen a menu entry.
        /// </summary>
        protected virtual void OnSelectEntry(int entryIndex, PlayerIndex playerIndex)
        {
            levelEntries[entryIndex].OnSelectEntry(playerIndex);
        }


        /// <summary>
        /// Handler for when the user has cancelled the menu.
        /// </summary>
        protected virtual void OnCancel(PlayerIndex playerIndex)
        {
            ExitScreen();
        }


        /// <summary>
        /// Helper overload makes it easy to use OnCancel as a LevelEntry event handler.
        /// </summary>
        protected void OnCancel(object sender, PlayerIndexEventArgs e)
        {
            OnCancel(e.PlayerIndex);
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Allows the screen the chance to position the menu entries. By default
        /// all menu entries are lined up in a vertical list, centered on the screen.
        /// </summary>
        protected virtual void UpdateMenuEntryLocations()
        {
            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // start at Y = 175; each X value is generated per entry
            

            // update each menu entry's location in turn
            for (int i = 0; i < levelEntries.Count; i++)
            {
                LevelEntry lvlEntry = levelEntries[i];
                Vector2 position = new Vector2(110f, 125f);
                Vector2 entryPos = GetNewEntryPosition(i);
                position = new Vector2(position.X + entryPos.X * (lvlEntry.GetWidth(this)+50),
                                       position.Y + entryPos.Y * lvlEntry.GetHeight(this));

                if (ScreenState == ScreenState.TransitionOn)
                {
                    position.X -= transitionOffset * 256;
                    Debug.WriteLine("transitioning");
                }
                else
                {
                    position.X += transitionOffset * 512;
                    Debug.WriteLine("Not transitioning");
                }

                // set the entry's position
                lvlEntry.Position = position;

                // move down for the next entry the size of this entry plus our padding
                //position.Y += lvlEntries.GetHeight(this) + (menuEntryPadding * 2);
            }
        }

        /// <summary>
        /// Gets a position for a new entry on the screen. Position is based on count in 
        /// levelEntries and is "normalized" to position 1-numberOfEntriesEachLine
        /// </summary>
        /// <returns></returns>
        public Vector2 GetNewEntryPosition(int entryNumber)
        {
            int y = 0;
            if (entryNumber > entriesEachLine-1)
            {
                y = 1;
                if (entryNumber > (entriesEachLine) * 2)
                {
                    y = 2;
                }
            }
            return new Vector2(entryNumber % entriesEachLine, y);
        }

        /// <summary>
        /// Updates the menu.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            // Update each nested LevelEntry object.
            for (int i = 0; i < levelEntries.Count; i++)
            {
                bool isSelected = IsActive && (i == selectedEntry);

                levelEntries[i].Update(this, isSelected, gameTime);
            }
        }


        /// <summary>
        /// Draws the menu.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // make sure our entries are in the right place before we draw them
            UpdateMenuEntryLocations();

            GraphicsDevice graphics = ScreenManager.GraphicsDevice;
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            spriteBatch.Begin();

            // Draw each menu entry in turn.
            for (int i = 0; i < levelEntries.Count; i++)
            {
                LevelEntry lvlEntry = levelEntries[i];

                bool isSelected = IsActive && (i == selectedEntry);

                lvlEntry.Draw(this, isSelected, gameTime);
            }

            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // Draw the menu title centered on the screen
            Vector2 titlePosition = new Vector2(graphics.Viewport.Width / 2, 80);
            Vector2 titleOrigin = font.MeasureString(menuTitle) / 2;
            Color titleColor = new Color(192, 192, 192) * TransitionAlpha;
            float titleScale = 1.25f;

            titlePosition.Y -= transitionOffset * 100;

            spriteBatch.DrawString(font, menuTitle, titlePosition, titleColor, 0,
                                   titleOrigin, titleScale, SpriteEffects.None, 0);

            spriteBatch.End();
        }


        #endregion
    }
}
