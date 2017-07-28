using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;

namespace Survival {
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class SRPG : Game {

        public static Texture2D SpriteMap { get; private set; }
        public static Texture2D tilemap { get; private set; }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static SpriteFont Arial { get; private set; }
        public static ContentManager CM;

        public static List<Text> Texts = new List<Text>();

        static Area area = new Area();
        static List<Room> rooms;
        public static int RoomID;

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
            Generator g = new Generator();
            rooms = g.GenerateFloor();
            RoomID = 0;
            LoadRoom(RoomID);
        }

        protected override void LoadContent() {
            SpriteMap = Content.Load<Texture2D>("spritemap");
            tilemap = Content.Load<Texture2D>("tilemap");
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
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.NonPremultiplied);

            //---Drawing---//
            switch(GameSt) {
                case GameState.Normal:
                    area.Draw(spriteBatch, Eng.pxlsize, Eng.tilesize, Color.White, tilemap);
                    foreach (Text text in Texts)
                        text.Draw(spriteBatch);
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

        public static void LoadRoom(int id) {
            foreach(Entity e in Area.entities) {
                e.Enabled = false;
            }
            Room room = rooms[id];
            room.SetEnabledAllEntities(true);
            area.tm = new Tilemap(room.tiles);
        }
    }
}
