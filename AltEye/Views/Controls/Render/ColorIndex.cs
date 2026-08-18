using System;
using System.Collections.Generic;
using Windows.UI;

namespace AltEye.Views.Controls.Render
{
    internal class ColorIndex<T> : IDisposable where T : class
    {
        private Dictionary<Color, List<T>> _dictionary = new();

        public void Add(Color color, T item)
        {
            if (!this._dictionary.TryAdd(color, [item]))
                this._dictionary[color].Add(item);
        }

        public T[] FindByColor(Color color)
        {
            if (this._dictionary.TryGetValue(color, out List<T> outList))
                return outList.ToArray();

            return [];
        }

        public void Dispose()
        {
            this._dictionary.Clear();
        }
    }
}
