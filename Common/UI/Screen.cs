using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.UI;

namespace ROH.Common.UI
{
    public class Screen : UIState
    {
        public override void OnInitialize()
        {
            Vector2 size = new Vector2(Main.screenWidth, Main.screenHeight);
            Width.Set(size.X, 0);
            Height.Set(size.Y, 0);
            base.OnInitialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Elements.RemoveAll(ui => !ROHUIManager.EntityDetails.Contains(ui));
        }
    }
}
