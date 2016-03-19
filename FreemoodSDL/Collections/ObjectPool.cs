using System;
using System.Collections.Concurrent;
using System.Drawing;

using FreeMoO.Controls;

namespace FreeMoO.Collections
{
    public class ObjectPool
    {
        public class ObjectPoolImpl<T>
        {
            private ConcurrentBag<T> _objects;
            private Func<T> _objectGenerator;

            public ObjectPoolImpl(Func<T> objectGenerator)
                : this(objectGenerator, 0)
            {}

            public ObjectPoolImpl(Func<T> objectGenerator, int initialCount)
            {
                if (objectGenerator == null) throw new ArgumentNullException("objectGenerator");
                _objects = new ConcurrentBag<T>();
                _objectGenerator = objectGenerator;

                for (int i = 0; i < initialCount; i++) _objects.Add(_objectGenerator());
            }

            public T GetObject()
            {
                T item;
                if (_objects.TryTake(out item)) return item;
                return _objectGenerator();
            }

            public void PutObject(T item)
            {
                _objects.Add(item);
            }
        }
        

        public static void Initialize()
        {
            RectanglePool = new ObjectPoolImpl<Rectangle>(() => new Rectangle(), 100);
            SizeObjPool = new ObjectPoolImpl<Size>(() => new Size(), 100);
            PointObjPool = new ObjectPoolImpl<Point>(() => new Point(), 100);
            ProductionBarGroupPool = new ObjectPoolImpl<ProductionBarGroup>(() => new ProductionBarGroup(), 10);
            ProductionBarPool = new ObjectPoolImpl<ProductionBar>(() => new ProductionBar(), 10);
        }

        public static Rectangle GetRectangle(int x, int y, int w, int h)
        {
            var rect = RectanglePool.GetObject();
            rect.X = x;
            rect.Y = y;
            rect.Width = w;
            rect.Height = h;
            return rect;
        }

        public static ObjectPoolImpl<Rectangle> RectanglePool;
        public static ObjectPoolImpl<Size> SizeObjPool;
        public static ObjectPoolImpl<Point> PointObjPool;
        public static ObjectPoolImpl<ProductionBarGroup> ProductionBarGroupPool;
        public static ObjectPoolImpl<ProductionBar> ProductionBarPool;

    }
}
