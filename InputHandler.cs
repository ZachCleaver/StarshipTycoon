﻿using System;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StarshipTycoon {
    /// <summary>
    /// Used to handle mouse inputs.
    /// </summary>
    public class InputHandler {
        private static InputHandler instance;
        public Rectangle rectangle { get; set; }
        public Texture2D mouseTexture { get; set; }
        public Texture2D tempMouseTexture { get; set; }

        private InputHandler() { }

        /// <summary>
        /// Singleton instance to access the mouse from anywhere
        /// without duplicating mouse instances across classes.
        /// </summary>
        public static InputHandler Instance {
            get {
                if (instance == null) {
                    instance = new InputHandler();
                }
                return instance;
            }
        }

        private MouseState mouse, oldMouse;
        private KeyboardState board, oldBoard;
        public int X, Y;
        public Vector2 pos;
        public bool didMouseMove { get; private set; }

        /// <summary>
        /// Updates the current state of the mouse and keyboard.
        /// </summary>
        public void update() {
            didMouseMove = !(oldMouse.X == mouse.X && oldMouse.Y == mouse.Y);

            Instance.oldMouse = Instance.mouse;
            Instance.mouse = Mouse.GetState();

            oldBoard = board;
            board = Keyboard.GetState();

            if (didMouseMove) {
                Instance.X = Instance.mouse.X;
                Instance.Y = Instance.mouse.Y;

                Instance.rectangle = new Rectangle(Instance.X, Instance.Y, 1, 1);

                Instance.pos = Instance.mouse.Position.ToVector2();
            }
        }

        public void draw(SpriteBatch sb) {
            if (tempMouseTexture != null) {
                sb.Draw(tempMouseTexture, new Rectangle(pos.ToPoint(), new Point(15, 15)), Color.PapayaWhip);
            } else {
                sb.Draw(mouseTexture, new Rectangle(pos.ToPoint(), new Point(15, 15)), Color.OrangeRed);
            }

            tempMouseTexture = null;
        }

        /// <summary>
        /// Returns whether the left mouse button was pressed and released.
        /// </summary>
        /// <returns>True if left mouse button is clicked.</returns>
        public bool wasLeftButtonClicked() {
            return (Instance.mouse.LeftButton == ButtonState.Released && Instance.oldMouse.LeftButton == ButtonState.Pressed);
        }

        public bool wasLeftButtonClickedAndHeld() {
            return (Instance.mouse.LeftButton == ButtonState.Pressed && Instance.oldMouse.LeftButton == ButtonState.Pressed);
        }

        public bool wasRightButtonClicked() {
            return (Instance.mouse.RightButton == ButtonState.Released && Instance.oldMouse.RightButton == ButtonState.Pressed);
        }

        /// <summary>
        /// Returns true if the provided key was pressed and released.
        /// </summary>
        /// <param name="key">Key to check for.</param>
        /// <returns></returns>
        public bool wasKeyPressedAndReleased(Keys key) {
            return (oldBoard.IsKeyDown(key) && board.IsKeyUp(key));
        }

        /// <summary>
        /// Returns true if the provided key was pressed and held.
        /// </summary>
        /// <param name="key">Key to check for.</param>
        /// <returns></returns>
        public bool wasKeyPressedAndHeld(Keys key) {
            return (oldBoard.IsKeyDown(key) && board.IsKeyDown(key));
        }
    }
}