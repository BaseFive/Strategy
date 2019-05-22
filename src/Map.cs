using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Griddle;

namespace Strategy
{
    public class Map
    {
        TileMap tilemap;
        List<Texture2D> Tilesets;
        Texture2D currentTileset;
        List<Rectangle> CollisionBoxes;

        public int Width { get { return tilemap.Width * tilemap.TileWidth; } }
        public int Height { get { return tilemap.Height * tilemap.TileHeight; } }

        int tileWidth;
        int tileHeight;
        int columns;
        int rows;
        int firstGid;

        public Map(string fileName)
        {
            tilemap = new TileMap("Content/" + fileName);
            Tilesets = new List<Texture2D>();
            CollisionBoxes = new List<Rectangle>();
        }

        public void LoadContent(ContentManager Content)
        {
            foreach (Tileset set in tilemap.Tilesets)
                Tilesets.Add(Content.Load<Texture2D>("Tilesets/" + set.Name.ToString()));

            foreach (ObjectGroup group in tilemap.ObjectGroups)
                foreach (Object box in group.Objects)
                    CollisionBoxes.Add(new Rectangle((int)box.X, (int)box.Y, (int)box.Width, (int)box.Height));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (TileLayer layer in tilemap.TileLayers)
                for (int i = 0; i < layer.Tiles.Count; i++)
                {
                    int gid = layer.Tiles[i].Gid;

                    //Loop through each tileset
                    for (int t = 0; t < tilemap.Tilesets.Count; t++)
                        if (t == tilemap.Tilesets.Count - 1 || gid < tilemap.Tilesets[t + 1].FirstGid)
                        {
                            currentTileset = Tilesets[t];
                            tileWidth = tilemap.Tilesets[t].TileWidth;
                            tileHeight = tilemap.Tilesets[t].TileHeight;
                            columns = tilemap.Tilesets[t].Columns;
                            rows = tilemap.Tilesets[t].Rows;
                            firstGid = tilemap.Tilesets[t].FirstGid;
                            break;
                        }

                    if (gid == 0)
                        continue;

                    int tileFrame = gid - firstGid;
                    int column = tileFrame % columns;
                    int row = tileFrame / columns;

                    float x = (i % tilemap.Width) * tilemap.TileWidth;
                    float y = (float)System.Math.Floor(i / (double)tilemap.Width) * tilemap.TileHeight;

                    Rectangle tilesetRec = new Rectangle(tileWidth * column, tileHeight * row, tileWidth, tileHeight);

                    spriteBatch.Draw(currentTileset, new Rectangle((int)x, (int)y, tileWidth, tileHeight), tilesetRec, Color.White);
                }
        }
    }
}