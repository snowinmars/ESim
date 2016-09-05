using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SandS.Algorithm.Library.PositionNamespace;
using SandS.Algorithm.Extensions.GraphicsDeviceExtensionNamespace;

namespace ESim.Config
{
    public static class Configuration
    {
        public static int DnaSize { get; } = 16;
        public static Position WorldSize { get; } = new Position(4, 4);
        public static Position CreatureTextureSize { get; } = new Position(80, 40);
        public static Position WorldTextureSize { get; } = new Position(400, 200);

        public static Texture2D DefaultCreatureTexture { get; private set; }
        public static Texture2D DefaultWorldTexture { get; private set; }

        public static void LoadContent(GraphicsDevice graphicsDevice)
        {
            Configuration.DefaultCreatureTexture = graphicsDevice.Generate(Configuration.CreatureTextureSize.X,
                                                                            Configuration.CreatureTextureSize.Y,
                                                                            Color.Black,
                                                                            1,
                                                                            Color.Azure);

            Configuration.DefaultWorldTexture = graphicsDevice.Generate(Configuration.WorldTextureSize.X,
                                                            Configuration.WorldTextureSize.Y,
                                                            Color.Aquamarine);
        }

        public static void UnloadContent()
        {
            Configuration.DefaultCreatureTexture.Dispose();
        }
    }
}
