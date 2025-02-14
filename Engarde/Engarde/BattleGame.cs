﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.MediaFoundation;

namespace Engarde
{
    public class BattleGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SpriteFont _font;

        private Character player1;
        private Character player2;
        private Character activePlayer;
        private Character opponent;
        private bool gameOver;
        private string gameMessage = "Welcome to Battle Simulator!";

        private KeyboardState currentKeyState, previousKeyState;

        public BattleGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
            player1 = new Tank("Tanky");
            player2 = new Character("Player 2", 100, 0);
            activePlayer = player1;
            opponent = player2;
            gameOver = false;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _font = Content.Load<SpriteFont>("GameFont");
        }

        protected override void Update(GameTime gameTime)
        {
            if (gameOver) return;

            previousKeyState = currentKeyState;
            currentKeyState = Keyboard.GetState();

            if (IsKeyPressed(Keys.D1))
            {
                activePlayer.Attack(opponent);
                if (opponent.Armor <= 0 && opponent.armorJustBroken)
                {
                    gameMessage = $"{activePlayer.Name} attacked {opponent.Name}! {opponent.Name}'s armor broke!";
                    if (opponent.countered)
                    {
                        gameMessage = $"{activePlayer.Name} attacked {opponent.Name}! {opponent.Name}'s armor broke! But {opponent.Name} is defending, and countered!";
                        opponent.countered = false ;
                    }
                    opponent.armorJustBroken = false;
                }
                else
                {
                    gameMessage = $"{activePlayer.Name} attacked {opponent.Name}!";
                    if (opponent.countered)
                    {
                        gameMessage = $"{activePlayer.Name} attacked {opponent.Name}! But {opponent.Name} is defending, and countered! {activePlayer.Name} lost HP instead!";
                        opponent.countered = false;
                    }
                }
                EndTurn();
            }
            else if (IsKeyPressed(Keys.D2))
            {
                activePlayer.Defend();
                gameMessage = $"{activePlayer.Name} is defending!";
                EndTurn();
            }
            else if (IsKeyPressed(Keys.D3))
            {
                if (activePlayer.TryBuff())
                {
                    if (activePlayer.buffEffect == "heal")
                        gameMessage = $"{activePlayer.Name} used a buff! Healed for 15 HP!";
                    else if (activePlayer.buffEffect == "armor")
                        gameMessage = $"{activePlayer.Name} used a buff! Gained 15 armor!";
                    else if (activePlayer.buffEffect == "nothing")
                        gameMessage = $"{activePlayer.Name} used a buff! ...but nothing happened!";
                    EndTurn();
                }
                else
                {
                    gameMessage = "Used buff last turn! Choose another action.";
                }
            }
            base.Update(gameTime);
        }

        private bool IsKeyPressed(Keys key)
        {
            return currentKeyState.IsKeyDown(key) && previousKeyState.IsKeyUp(key);
        }

        private void EndTurn()
        {
            if (opponent.IsDefeated)
            {
                gameMessage = $"{activePlayer.Name} wins!";
                gameOver = true;
                return;
            }

            (activePlayer, opponent) = (opponent, activePlayer); // Swap turns
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteBatch.DrawString(_font, gameMessage, new Vector2(50, 50), Color.White);
            _spriteBatch.DrawString(_font, $"{player1.Name}: {player1.HP} HP - {player1.Armor} ARM", new Vector2(50, 100), Color.White);
            _spriteBatch.DrawString(_font, $"{player2.Name}: {player2.HP} HP - {player2.Armor} ARM", new Vector2(50, 130), Color.White);
            _spriteBatch.DrawString(_font, "Press 1 to Attack, 2 to Defend, 3 to Buff", new Vector2(50, 180), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

