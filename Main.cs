using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StarshipTycoon {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Main : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont text;
        Texture2D whiteSquare;
        List<Planet> planets = new List<Planet>();
        List<Ship> ships = new List<Ship>();
        int screenHeight, screenWidth;
        int planetSize = 15;
        int planetNum = 5;
        InputHandler inputHander = InputHandler.Instance;
        bool isPaused = false;

        public Main() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize() {
            this.IsMouseVisible = true;
            graphics.ToggleFullScreen();
            whiteSquare = Content.Load<Texture2D>("WhiteSquare");
            text = Content.Load<SpriteFont>("text");
            screenHeight = GraphicsDevice.Viewport.Height;
            screenWidth = GraphicsDevice.Viewport.Width;

            Random rand = new Random();

            for (int i = 0; i < planetNum; i++) {
                Color randColor = new Color(rand.Next(256), rand.Next(256), rand.Next(256));
                Planet planet = new Planet(whiteSquare, rand.Next(screenWidth - planetSize), rand.Next(screenHeight - planetSize),
                    planetSize, planetSize, randColor, i.ToString());
                planets.Add(planet);
            }

            base.Initialize();
        }
        
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }
        
        protected override void UnloadContent() {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            inputHander.update();

            //TODO: I guess make some states
            if (isPaused) {
                if (inputHander.wasKeyPressedAndReleased(Keys.Space)) {
                    isPaused = false;
                }
            } else {
                if (inputHander.wasKeyPressedAndReleased(Keys.Space)) {
                    isPaused = true;
                }

                if (inputHander.wasLeftButtonClicked()) {
                    foreach (Planet planet in planets) {
                        if (planet.rectangle.Intersects(inputHander.rectangle)) { 
                            List<Planet> validPlanets = planets.FindAll(p => p != planet);
                            int index = new Random().Next(validPlanets.Count);

                            Planet dest = validPlanets[index];
                            Point planetCenter = planet.rectangle.Center;

                            ships.Add(new Ship(whiteSquare, planetCenter.X, planetCenter.Y, 3, 5, 2, dest, 1000));
                        }
                    }
                }

                if (inputHander.wasKeyPressedAndReleased(Keys.Down)) {
                    foreach(Ship ship in ships) {
                        ship.speed--;
                    }
                } else if (inputHander.wasKeyPressedAndReleased(Keys.Up)) {
                    foreach (Ship ship in ships) {
                        ship.speed++;
                    }
                }

                foreach (Ship ship in ships) {
                    ship.update(planets);
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            //TODO: I guess make some states
            if (isPaused) {
                for (int p = 0; p < planets.Count; p++) {
                    spriteBatch.DrawString(text, planets[p].name + " visited: " + planets[p].timesVisited, new Vector2(100 * (int)(p / 30), p * 12 % screenHeight), Color.Pink);
                }
            } else {

                foreach (Planet planet in planets) {
                    planet.Draw(spriteBatch);
                    if (inputHander.rectangle.Intersects(planet.rectangle)) {
                        spriteBatch.DrawString(text, planet.rectangle.X + ", " + planet.rectangle.Y, planet.rectangle.Location.ToVector2(), Color.Green);
                    }
                }
                foreach (Ship ship in ships) {
                    ship.draw(spriteBatch);
                }
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}