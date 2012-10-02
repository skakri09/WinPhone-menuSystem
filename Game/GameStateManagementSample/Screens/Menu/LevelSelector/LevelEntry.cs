#region File Description
//-----------------------------------------------------------------------------
// MenuEntry.cs
//
// XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
#endregion

public enum Stars { NONE, BRONZE, SILVER, GOLD, LOCKED}

namespace GameStateManagement
{
    /// <summary>
    /// Helper class represents a single entry in a MenuScreen. By default this
    /// just draws the entry text string, but it can be customized to display menu
    /// entries in different ways. This also provides an event that will be raised
    /// when the menu entry is selected.
    /// </summary>
    class LevelEntry
    {
        #region Fields

        /// <summary>
        /// The name of the level
        /// </summary>
        string levelName { get; set; }

        /// <summary>
        /// The levelIcon rendered for this entry.
        /// </summary>
        Texture2D levelIcon { get; set; }

        /// <summary>
        /// The star texture rendered for this entry.
        /// </summary>
        Texture2D starsTexture;

        /// <summary>
        /// The locked icon texture
        /// </summary>
        Texture2D LockedTexture;

        /// <summary>
        /// Tracks a fading selection effect on the entry.
        /// </summary>
        /// <remarks>
        /// The entries transition out of the selection effect when they are deselected.
        /// </remarks>
        float selectionFade;

        /// <summary>
        /// The position at which the entry is drawn. This is set by the MenuScreen
        /// each frame in Update.
        /// </summary>
        Vector2 position;

        int entryContentPadding = 3;
        /// <summary>
        /// The collision box of this entry. It will be a rectangle surrounding the 
        /// levelIcon, stars and level name, but nothing more.
        /// </summary>
        public Rectangle EntryCollisionBox { get; protected set; }

        public Stars starsDisplayed { get; set; }

        #endregion

        #region Properties


        /// <summary>
        /// Gets or sets the position at which to draw this menu entry.
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }


        #endregion

        #region Events


        /// <summary>
        /// Event raised when the menu entry is selected.
        /// </summary>
        public event EventHandler<PlayerIndexEventArgs> Selected;


        /// <summary>
        /// Method for raising the Selected event.
        /// </summary>
        protected internal virtual void OnSelectEntry(PlayerIndex playerIndex)
        {
            if (Selected != null)
                Selected(this, new PlayerIndexEventArgs(playerIndex));
        }


        #endregion

        #region Initialization


        /// <summary>
        /// Constructs a new menu entry with the specified text.
        /// </summary>
        public LevelEntry(string levelName, string lvlIconToLoad, Stars stars, LevelSelector screen)
        {
            this.starsDisplayed = stars;
            this.levelName = levelName;
            this.levelIcon = screen.Content.Load<Texture2D>(lvlIconToLoad);
            LoadStars(screen, stars);

            screen.LevelEntries.Add(this);
        }

        private void LoadStars(LevelSelector screen, Stars stars)
        {
            switch (stars)
            {
                case Stars.NONE:
                    this.starsTexture = screen.Content.Load<Texture2D>("LevelSelectionIcons_SD\\NoMedal_SD");
                    break;
                case Stars.BRONZE:
                    this.starsTexture = screen.Content.Load<Texture2D>("LevelSelectionIcons_SD\\Bronze_SD");
                    break;
                case Stars.SILVER:
                    this.starsTexture = screen.Content.Load<Texture2D>("LevelSelectionIcons_SD\\Silver_SD");
                    break;
                case Stars.GOLD:
                    this.starsTexture = screen.Content.Load<Texture2D>("LevelSelectionIcons_SD\\Gold_SD");
                    break;
                case Stars.LOCKED:
                    this.LockedTexture = screen.Content.Load<Texture2D>("LevelSelectionIcons_SD\\LockedLevel_SD");
                    this.starsTexture = screen.Content.Load<Texture2D>("LevelSelectionIcons_SD\\NoMedal_SD");
                    break;
            }
        }
        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the menu entry.
        /// </summary>
        public virtual void Update(LevelSelector screen, bool isSelected, GameTime gameTime)
        {
            // there is no such thing as a selected item on Windows Phone, so we always
            // force isSelected to be false
#if WINDOWS_PHONE
            isSelected = false;
#endif

            // When the menu selection changes, entries gradually fade between
            // their selected and deselected appearance, rather than instantly
            // popping to the new state.
            float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 4;

            if (isSelected)
                selectionFade = Math.Min(selectionFade + fadeSpeed, 1);
            else
                selectionFade = Math.Max(selectionFade - fadeSpeed, 0);
        }


        /// <summary>
        /// Draws the menu entry. This can be overridden to customize the appearance.
        /// </summary>
        public virtual void Draw(LevelSelector screen, bool isSelected, GameTime gameTime)
        {
            // there is no such thing as a selected item on Windows Phone, so we always
            // force isSelected to be false
#if WINDOWS_PHONE
            isSelected = false;
#endif

            // Draw the selected entry in yellow, otherwise white.
            Color color = isSelected ? Color.Yellow : Color.White;

            screen.ScreenManager.SpriteBatch.DrawString(screen.ScreenManager.Font, levelName, position, color);

            screen.ScreenManager.SpriteBatch.Draw(levelIcon, new Rectangle((int)position.X,
                                                      (int)position.Y + (int)screen.ScreenManager.Font.MeasureString(levelName).Y,
                                                      levelIcon.Width,
                                                      levelIcon.Height),
                                                      Color.White);


            screen.ScreenManager.SpriteBatch.Draw(starsTexture, new Rectangle((int)position.X,
                                                      (int)position.Y +
                                                            ((int)screen.ScreenManager.Font.MeasureString(levelName).Y +
                                                            levelIcon.Height),
                                                      starsTexture.Width,
                                                      starsTexture.Height),
                                                      Color.White);

            if (starsDisplayed == Stars.LOCKED)
            {
                screen.ScreenManager.SpriteBatch.Draw(LockedTexture, new Rectangle((int)position.X - 15,
                                                      (int)position.Y+15,
                                                      LockedTexture.Width,
                                                      LockedTexture.Height),
                                                      Color.White);
            }

        }


        /// <summary>
        /// Queries how much space this menu entry requires.
        /// </summary>
        public virtual int GetHeight(LevelSelector screen)
        {
            int height = (int)screen.ScreenManager.Font.MeasureString(levelName).Y;
            height += entryContentPadding*2;
            height += levelIcon.Height;
            height += starsTexture.Height;
            return height;
        }


        /// <summary>
        /// Queries how wide the entry is, used for centering on the screen.
        /// </summary>
        public virtual int GetWidth(LevelSelector screen)
        {
            int width = (int)screen.ScreenManager.Font.MeasureString(levelName).Y;
            
            if(width < levelIcon.Width)
                width = levelIcon.Width;

            if (width < starsTexture.Width)
                width = starsTexture.Width;

            return width;
        }


        #endregion
    }
}
