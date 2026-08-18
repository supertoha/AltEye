//using Microsoft.Graphics.Canvas;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Collections.Immutable;
//using System.Linq;
//using Windows.UI;

//namespace AltEye.Views.Controls.Render
//{
//    internal class RenderCollection : ICollection<IRender>
//    {
//        public void Init(ICanvasResourceCreator canvasResourceCreator)
//        {
//            this.canvasResourceCreator = canvasResourceCreator;
//        }

//        private ICanvasResourceCreator canvasResourceCreator;
//        private readonly List<IRender> _children = new();
//        private readonly Dictionary<Color, List<IRender>> _colors = new();

//        public int Count => this._children.Count;

//        public bool IsReadOnly => false;

//        public IRender[] FindByColor(Color color)
//        {
//            if (this._colors.TryGetValue(color, out List<IRender> outList))
//                return outList.ToArray();

//            return [];
//        }

//        public void Add(IRender item)
//        {
//            this._children.Add(item);
//            item.CreateResources(this.canvasResourceCreator, this._children.Count);
//            foreach (var itemColor in item.IndexColors)
//            {
//                if (!this._colors.TryAdd(itemColor, [item]))
//                    this._colors[itemColor].Add(item);
//            }        
//        }

//        public void Clear()
//        {
//            this._children.Clear();
//        }

//        public bool Contains(IRender item)
//        {
//            return this._children.Contains(item);
//        }

//        public void CopyTo(IRender[] array, int arrayIndex)
//        {
//            this._children.CopyTo(array, arrayIndex);
//        }

//        public IEnumerator<IRender> GetEnumerator()
//        {
//            return this._children.GetEnumerator();
//        }

//        public bool Remove(IRender item)
//        {
//            return this._children.Remove(item);
//        }

//        IEnumerator IEnumerable.GetEnumerator()
//        {
//            return GetEnumerator();
//        }
//    }
//}
