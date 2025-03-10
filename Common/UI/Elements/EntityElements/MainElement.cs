using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace ROH.Common.UI.Elements.EntityElements
{
    public class MainElement : EntityElement
    {
        public override bool IsActive() => true;
        public override object GetObject() => Main.instance;
        public override string GetObjectName() => "Main";
        public MainElement(float width, int index) : base(width, index)
        {
            Height.Set(22, 0);
        }
    }
}
