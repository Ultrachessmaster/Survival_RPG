using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

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

        Area area = new Area();

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
            Area.AddEntity(new Player(Vector2.Zero));
            Area.AddEntity(new Kobolt(new Vector2(5f * Eng.tilesize, 5f * Eng.tilesize)));
        }

        protected override void LoadContent() {
            SpriteMap = Content.Load<Texture2D>("spritemap");
            TileMap = Content.Load<Texture2D>("tilemap");
            Arial = Content.Load<SpriteFont>("font");
            spriteBatch = new SpriteBatch(GraphicsDevice);
            SetUpGame();
        }
        protected override void UnloadContent() {
            
        }
        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //---Game Logic---//
            switch(GameSt) {
                case GameState.Normal:
                    area.Update(gameTime);
                    Physics.Update();
                    area.PostUpdate(gameTime);
                    Eng.Update(gameTime);
                    break;
                case GameState.Inventory:
                    var player = (Player)Area.GetEntity<Player>();
                    player.inventory.Update();
                    Input.Update();
                    break;
            }
            //---Game Logic---//

            
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.NonPremultiplied);

            //---Drawing---//
            switch(GameSt) {
                case GameState.Normal:
                    area.Draw(spriteBatch, Eng.pxlsize, Eng.tilesize, Color.White);
                    foreach (Text t in Texts)
                        t.Draw(spriteBatch);
                    break;
                case GameState.Inventory:
                    var player = (Player)Area.GetEntity<Player>();
                    player.inventory.Draw(spriteBatch);
                    break;
            }
            spriteBatch.End();
            //---Drawing---//

            base.Draw(gameTime);
        }
    }
}
