﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StarshipTycoon {
    public class Main : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont text;
        Random random = new Random();

        Texture2D whiteSquare;

        Player human = new Player();
        Player ai = new Player();
        List<Planet> planets = new List<Planet>();
        int screenHeight, screenWidth;
        int planetSize = 15;
        int planetNum = 12;
        InputHandler inputHandler = InputHandler.Instance;
        bool isPaused = false;

        public Main() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize() {
            this.IsMouseVisible = true;
            //graphics.ToggleFullScreen();
            whiteSquare = Content.Load<Texture2D>("WhiteSquare");
            text = Content.Load<SpriteFont>("text");
            screenHeight = GraphicsDevice.Viewport.Height;
            screenWidth = GraphicsDevice.Viewport.Width;

            for (int i = 0; i < planetNum; i++) {
                Color randColor = new Color(random.Next(256), random.Next(256), random.Next(256));
                Planet planet = new Planet(whiteSquare, random.Next(screenWidth - planetSize), random.Next(screenHeight - planetSize),
                    planetSize, planetSize, randColor, i.ToString());
                planets.Add(planet);
            }

            PlanetUtil.init(ref planets);

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

            inputHandler.update();

            //TODO: I guess make some states
            if (isPaused) {
                if (inputHandler.wasKeyPressedAndReleased(Keys.Space)) {
                    isPaused = false;
                }
            } else {
                if (inputHandler.wasKeyPressedAndReleased(Keys.Space)) {
                    isPaused = true;
                }

                //AI Ship
                if (inputHandler.wasLeftButtonClicked()) {
                    foreach (Planet planet in planets) {
                        if (planet.rectangle.Intersects(inputHandler.rectangle)) {
                            Point planetCenter = planet.rectangle.Center;
                            Ship ship = new Ship("AI " + ai.ships.Count, whiteSquare, planetCenter.X, planetCenter.Y, 3, 5, 2, 1000, Color.White);

                            //Well this is weird. Have to set init planet as the one we clicked so that our
                            //new destination isn't set to the same planet.
                            //TODO: Make this better
                            ship.dest = planet;
                            ship.dest = PlanetUtil.getDestination(ship);
                            ship.updateAngle(); //TODO: I don't like how this is stuck out here instead of being a private method

                            ai.ships.Add(ship);
                        }
                    }
                }

                //Human ship
                if (inputHandler.wasRightButtonClicked()) {
                    foreach (Planet planet in planets) {
                        if (planet.rectangle.Intersects(inputHandler.rectangle)) {
                            Point planetCenter = planet.rectangle.Center;
                            Ship ship = new Ship("Human " + human.ships.Count, whiteSquare, planetCenter.X, planetCenter.Y, 3, 5, 2, 1000, Color.Black);

                            //Well this is weird. Have to set init planet as the one we clicked so that our
                            //new destination isn't set to the same planet.
                            //TODO: Make this better
                            ship.dest = planet;
                            ship.dest = PlanetUtil.getDestination(ship);
                            ship.updateAngle(); //TODO: I don't like how this is stuck out here instead of being a private method

                            human.ships.Add(ship);
                        }
                    }
                }

                if (inputHandler.wasKeyPressedAndReleased(Keys.Down)) {
                    ai.ships.ForEach(ship => ship.speed--);
                    human.ships.ForEach(ship => ship.speed--);
                } else if (inputHandler.wasKeyPressedAndReleased(Keys.Up)) {
                    ai.ships.ForEach(ship => ship.speed++);
                    human.ships.ForEach(ship => ship.speed++);
                }

                ai.update();
                human.update();
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
                spriteBatch.DrawString(text, "Human money: " + human.money, new Vector2(400, 40), Color.Green);
                spriteBatch.DrawString(text, "AI money: " + ai.money, new Vector2(400, 80), Color.Green);
            } else {

                foreach (Planet planet in planets) {
                    planet.Draw(spriteBatch);
                    if (inputHandler.rectangle.Intersects(planet.rectangle)) {
                        spriteBatch.DrawString(text, planet.rectangle.X + ", " + planet.rectangle.Y, planet.rectangle.Location.ToVector2(), Color.Green);
                    }
                }

                ai.draw(spriteBatch);
                human.draw(spriteBatch);

                human.ships.ForEach(ship => {
                    if (inputHandler.rectangle.Intersects(ship.rect)) {
                        spriteBatch.DrawString(text, "Fuel: " + ship.fuelRemaining, ship.rect.Center.ToVector2(), Color.Black);
                    }
                });
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}