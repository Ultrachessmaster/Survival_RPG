using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework.Media;

namespace Suvival_RPG {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class SRPG : Game {

        public static Texture2D SpriteMap { get; private set; }
        public static Texture2D TileMap { get; private set; }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static SpriteFont Arial { get; private set; }
        public static ContentManager CM;

        public static List<Text> Texts = new List<Text>();

        Tilemap tm;
        ERegistry er = new ERegistry();
        public static World World;

        public static GameState GameSt = GameState.Normal;

        public SRPG() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            CM = Content;
        }
        
        protected override void Initialize() {
            
            base.Initialize();
        }

        void SetUpGame() {
            World = new World(Vector2.Zero);

            Generator g = new Generator();
            tm = g.GenerateFloor();
            //ERegistry.AddEntity(new Player(new Vector2((tm.Width / 2) * Eng.tilesize, (tm.Height / 2) * Eng.tilesize)));
            //ERegistry.AddRangeE(g.GenerateEnemies(tm));
        }

        protected override void LoadContent() {
            SpriteMap = Content.Load<Texture2D>("spritemap");
            TileMap = Content.Load<Texture2D>("tilemap");
            Arial = Content.Load<SpriteFont>("font");
            var song = Content.Load<Song>("dungeon crawling");
            MediaPlayer.Volume = 0.0f;//0.2f
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(song);
            
            spriteBatch = new SpriteBatch(GraphicsDevice);
            SetUpGame();
        }
        protected override void UnloadContent() {
            
        }
        public static int times = 0;
        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            //---Game Logic---//
            switch(GameSt) {
                case GameState.Normal:
                    er.Update(gameTime);
                    World.Step(gameTime.ElapsedGameTime.Milliseconds * 0.001f);
                    er.PostUpdate(gameTime);
                    Eng.Update(gameTime);
                    break;
                case GameState.Inventory:
                    var player = (Player)ERegistry.GetEntity<Player>();
                    player.inventory.Update();
                    Input.Update();
                    break;
            }
            //---Game Logic---//

            
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.NonPremultiplied);

            //---Drawing---//
            switch(GameSt) {
                case GameState.Normal:
                    tm.Draw(spriteBatch, TileMap);
                    er.Draw(spriteBatch, Eng.pxlsize, Eng.tilesize, Color.White);
                    foreach (Text text in Texts)
                        text.Draw(spriteBatch);
                    break;
                case GameState.Inventory:
                    var player = (Player)ERegistry.GetEntity<Player>();
                    player.inventory.Draw(spriteBatch);
                    break;
            }
            spriteBatch.End();
            //---Drawing---//

            base.Draw(gameTime);
        }
    }
}
