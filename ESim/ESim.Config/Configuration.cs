using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using SandS.Algorithm.Library.PositionNamespace;
using SandS.Algorithm.Extensions.GraphicsDeviceExtensionNamespace;

namespace ESim.Config
{

    public static class Configuration
    {
        public class ConfigurationInstance
        {
            public int DnaSize { get; set; }
            public Position WorldSize { get; set; }
            public Position CreatureTextureSize { get; set; }
            public Position WorldTextureSize { get; set; }
            public int KillCount { get; set; }
            public TimeSpan UpdateTime { get; set; }
            public int WillNucleotideMutateMax { get; set; }
            public int WillOrganismMutateMax { get; set; }
            public int WillHaveChildMax { get; set; }
            public int HowManyChildren { get; set; }
            public Color WorldColor { get; set; }
        }

        static Configuration()
        {
            using (StreamReader reader = new StreamReader(@"cfg\esim.cfg"))
            {
                string str = reader.ReadToEnd();
                ConfigurationInstance c = JsonConvert.DeserializeObject<ConfigurationInstance>(str);
                MapConfigurationInstance(c);
            }
        }

        private static void MapConfigurationInstance(ConfigurationInstance c)
        {
            Configuration.DnaSize = c.DnaSize;
            Configuration.WorldSize = c.WorldSize;
            Configuration.CreatureTextureSize = c.CreatureTextureSize;
            Configuration.WorldTextureSize = c.WorldTextureSize;
            Configuration.KillCount = c.KillCount;
            Configuration.UpdateTime = c.UpdateTime;
            Configuration.WillNucleotideMutateMax = c.WillNucleotideMutateMax;
            Configuration.WillOrganismMutateMax = c.WillOrganismMutateMax;
            Configuration.WillHaveChildMax = c.WillHaveChildMax;
            Configuration.HowManyChildren = c.HowManyChildren;
            Configuration.WorldColor = c.WorldColor;
        }

        public static Color WorldColor { get; set; }
        public static Position WorldSize { get; set; } 
        public static int DnaSize { get; private set; }
        public static Position CreatureTextureSize { get; private set; } 
        public static Position WorldTextureSize { get; private set; } 
        public static int KillCount { get; set; } 
        public static TimeSpan UpdateTime { get; set; }
        public static int WillNucleotideMutateMax { get; set; } 
        public static int WillOrganismMutateMax { get; set; }
        public static int WillHaveChildMax { get; set; } 
        public static int HowManyChildren { get; set; }
        public static Texture2D DefaultCreatureTexture { get; private set; }
        public static Texture2D DefaultWorldTexture { get; private set; }
        public static SpriteFont DefaultApplicationFont { get; private set; }

        public static void LoadContent(GraphicsDevice graphicsDevice, ContentManager content)
        {
            Configuration.DefaultCreatureTexture = graphicsDevice.Generate(Configuration.CreatureTextureSize.X,
                                                                            Configuration.CreatureTextureSize.Y,
                                                                            Color.White);

            Configuration.DefaultWorldTexture = graphicsDevice.Generate(Configuration.WorldTextureSize.X,
                                                                        Configuration.WorldTextureSize.Y,
                                                                        Color.Aquamarine);

            Configuration.DefaultApplicationFont = content.Load<SpriteFont>("fonts/PTSans10");
        }

        public static void UnloadContent()
        {
            Configuration.DefaultCreatureTexture.Dispose();
        }
    }
}
