﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria;

namespace ROH.Common.UI.Elements
{
    public class UIListImproved : UIList
    {
        public readonly static FieldInfo field_innerList;
        public readonly static MethodInfo method_uiElementAppend;
        public readonly static MethodInfo method_uiElementRecalcuate;
        public readonly static MethodInfo method_uiElementRemoveChild;

        static UIListImproved()
        {
            field_innerList = typeof(UIList).GetField("_innerList", BindingFlags.Instance | BindingFlags.NonPublic);
            method_uiElementAppend = typeof(UIElement).GetMethod("Append", BindingFlags.Instance | BindingFlags.Public);
            method_uiElementRecalcuate = typeof(UIElement).GetMethod("Recalculate", BindingFlags.Instance | BindingFlags.Public);
            method_uiElementRemoveChild = typeof(UIElement).GetMethod("RemoveChild", BindingFlags.Instance | BindingFlags.Public);
        }

        public override void Add(UIElement item)
        {
            _items.Add(item);
            method_uiElementAppend.Invoke(field_innerList.GetValue(this), [item]);
            method_uiElementRecalcuate.Invoke(field_innerList.GetValue(this), null);
        }

        public override bool Remove(UIElement item)
        {
            method_uiElementRemoveChild.Invoke(field_innerList.GetValue(this), [item]);
            return _items.Remove(item);
        }
    }
}
