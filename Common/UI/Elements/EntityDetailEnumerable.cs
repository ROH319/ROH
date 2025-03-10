using ROH.Common.UI.Elements.ProjectileDetails;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.Config.UI;

namespace ROH.Common.UI.Elements
{
    public class EntityDetailEnumerable : EntityDetail
    {
        public EntityDetailEnumerable(object obj, string name, int depth) : base(obj, name, depth)
        {
        }

        public override void BuildList(object obj, string name)
        {
            this.name = name;
            MemberList.Clear();
            MemberList.OverflowHidden = false;

            var type = obj.GetType();
            if(obj != null)
            {
                IEnumerable enumerable = obj as IEnumerable;
                int index = 0;
                foreach (var item in enumerable)
                {
                    MemberList.Add(new DetailEnumerator(item, Depth, index));
                    index++;
                }
                MemberList.UpdateOrder();
            }
        }
        public void TryAddEditor(FieldInfo f, object o, int index)
        {
            MemberList.Add(new DetailEnumerator(o, Depth, index));
        }
    }
}
