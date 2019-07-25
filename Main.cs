using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StarshipTycoon.InfoMenus;
using StarshipTycoon.Utils;
using System;
using System.Collections.Generic;

namespace StarshipTycoon {
    public class Main : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        Texture2D whiteSquare;
        Texture2D mouseTexture;

        HeaderBar headerBar;
        InputHandler inputHandler;
        Random random = new Random();

        Player human;
        //TODO: Pass in Content to manager and let it load its own textures
        private Texture2D hoverPlanetErrorText;
        private Texture2D hoverPlanetSuccessText;

        ComputerPlayer ai = new ComputerPlayer();

        List<Planet> planets = new List<Planet>();
        int planetSize = 50;
        int planetNum = 50;

        bool isPaused = false;
        bool isFullScreen = false;
        private bool shouldShowFourCornerPlanets = true;

        public Main() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize() {
            //this.IsMouseVisible = true;

            this.graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            this.graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            //graphics.ToggleFullScreen();

            whiteSquare = Content.Load<Texture2D>("WhiteSquare");
            font = Content.Load<SpriteFont>("text"); 
            
            inputHandler = InputHandler.Instance;
            mouseTexture = whiteSquare;
            inputHandler.mouseTexture = mouseTexture;

            hoverPlanetErrorText = whiteSquare;
            hoverPlanetSuccessText = whiteSquare;
            human = new Player(hoverPlanetErrorText, hoverPlanetSuccessText);

            Globals.screenHeight = GraphicsDevice.Viewport.Height;
            Globals.screenWidth = GraphicsDevice.Viewport.Width;
            Globals.camera = new Camera(GraphicsDevice.Viewport);
            Globals.camera.CenterOn(new Vector2(Globals.screenWidth / 2, Globals.screenHeight / 2));

            for (int i = 0; i < planetNum; i++) {
                Color randColor = new Color(random.Next(256), random.Next(256), random.Next(256));
                Planet planet = new Planet(whiteSquare, 
                    random.Next(-Globals.screenWidth / 2, Globals.screenWidth * 3 / 2 - planetSize),
                    //+30 for height of HeaderBar
                    random.Next(-Globals.screenHeight / 2 + 30, Globals.screenHeight * 3 / 2 - planetSize),
                    planetSize, planetSize, randColor, i.ToString());
                planets.Add(planet);
            }

            if (shouldShowFourCornerPlanets) {
                Planet topLeft = new Planet(whiteSquare,
                    -Globals.screenWidth / 2,
                    //+30 for height of HeaderBar
                    -Globals.screenHeight / 2 + 30,
                    planetSize, planetSize, Color.Black, "Top Left");
                Planet topRight = new Planet(whiteSquare,
                     Globals.screenWidth * 3 / 2 - planetSize,
                    //+30 for height of HeaderBar
                    -Globals.screenHeight / 2 + 30,
                    planetSize, planetSize, Color.Black, "Top Right");
                Planet bottomLeft = new Planet(whiteSquare,
                    -Globals.screenWidth / 2,
                    Globals.screenHeight * 3 / 2 - planetSize,
                    planetSize, planetSize, Color.Black, "Bottom Left");
                Planet bottomRight = new Planet(whiteSquare,
                    Globals.screenWidth * 3 / 2 - planetSize,
                    Globals.screenHeight * 3 / 2 - planetSize,
                    planetSize, planetSize, Color.Black, "Bottom Right");

                planets.Add(topLeft);
                planets.Add(topRight);
                planets.Add(bottomLeft);
                planets.Add(bottomRight);
            }

            HeaderBar.init(human);
            headerBar = HeaderBar.Instance;

            PlanetUtil.init(ref planets);
            DrawUtil.init(whiteSquare);

            BaseInfo.init(whiteSquare, font);

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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape)) {
                Exit();
            }

            inputHandler.update();
            Globals.camera.update();

            //TODO: This stupid thing still isn't working 100% right
            if (inputHandler.wasKeyPressedAndReleased(Keys.F)) {
                //isFullScreen = !isFullScreen;
                graphics.ToggleFullScreen();
                if (!graphics.IsFullScreen) {
                    this.graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                    this.graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                } else {
                    graphics.PreferredBackBufferWidth = 640;
                    graphics.PreferredBackBufferHeight = 480;
                }

                graphics.ApplyChanges();

                Globals.screenHeight = GraphicsDevice.Viewport.Height;
                Globals.screenWidth = GraphicsDevice.Viewport.Width;
            }

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
                //if (inputHandler.wasLeftButtonClicked()) {
                //    foreach (Planet planet in planets) {
                //        if (planet.rectangle.Intersects(inputHandler.rectangle)) {
                //            Point planetCenter = planet.rectangle.Center;
                //            Ship ship = new Ship("AI " + ai.ships.Count, whiteSquare, planetCenter.X, planetCenter.Y, 3, 5, 2, 1000, Color.White);

                //            //Well this is weird. Have to set init planet as the one we clicked so that our
                //            //new destination isn't set to the same planet.
                //            //TODO: Make this better
                //            ship.dest = planet;
                //            ship.dest = PlanetUtil.getDestination(ship);
                //            ship.updateAngle(); //TODO: I don't like how this is stuck out here instead of being a private method

                //            ai.ships.Add(ship);
                //        }
                //    }
                //}

                //Human ship
                if (inputHandler.wasRightButtonClicked()) {
                    foreach (Planet planet in planets) {
                        if (planet.getCollisionRectangle().Intersects(inputHandler.rectangle)) {
                            Ship ship = new Ship("Human " + human.ships.Count, whiteSquare, planet, 7, 15, 2, 1000, Color.Black);

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
            spriteBatch.Begin(SpriteSortMode.Deferred, null,
                null, null, null, null, Globals.camera.TranslationMatrix);

            //TODO: I guess make some states
            if (isPaused) {
                for (int p = 0; p < planets.Count; p++) {
                    spriteBatch.DrawString(font, planets[p].name + " visited: " + planets[p].timesVisited, new Vector2(100 * (int)(p / 30), p * 12 % Globals.screenHeight), Color.Pink);
                }
                spriteBatch.DrawString(font, "Human money: " + human.money, new Vector2(400, 40), Color.Green);
                spriteBatch.DrawString(font, "AI money: " + ai.money, new Vector2(400, 80), Color.Green);
            } else {

                foreach (Planet planet in planets) {
                    planet.Draw(spriteBatch);
                    Rectangle planetRect = planet.getCollisionRectangle();
                    if (inputHandler.rectangle.Intersects(planet.getCollisionRectangle())) {
                        spriteBatch.DrawString(font, planetRect.X + ", " + planetRect.Y, planetRect.Location.ToVector2(), Color.Green);
                    }
                }

                ai.draw(spriteBatch);
                human.draw(spriteBatch);

                human.ships.ForEach(ship => {
                    if (inputHandler.rectangle.Intersects(ship.getCollisionRectangle())) {
                        spriteBatch.DrawString(font, "Fuel: " + ship.fuelRemaining, ship.getCollisionRectangle().Center.ToVector2(), Color.Black);
                    }
                });
            }
            spriteBatch.End();

            spriteBatch.Begin();
            human.drawNoTransform(spriteBatch);
            headerBar.draw(spriteBatch);
            inputHandler.draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}