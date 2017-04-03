using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SpaceShooter.GameSystem
{
    public class GameObjectCollection :IDisposable
    {
        //readonly
        public List<IGameObject> Set { get; private set; }

        public GameObjectCollection()
        {
            Set = new List<IGameObject>();
        }

        public IGameObject FindGameObject(string tag)
        {
            return Set.Find(x => x.Tag == tag);
        }

        public List<T> GetAllOfType<T>() where T : IGameObject
        {
            return Set.FindAll(x => x is T).Cast<T>().ToList();
        }

        public T FindGameObject<T>(string tag) where T : IGameObject
        {
            return (T) Set.Find(x => x is T && x.Tag == tag);
        }

        public void Add(IGameObject gameObject)
        {
            Set.Add(gameObject);
        }

        public void Remove(IGameObject gameObject)
        {
            Set.Remove(gameObject);
        }

        public void Remove(string tag)
        {
            Set.RemoveAll(x => x.Tag == tag);
        }

        public int RemoveAll(string regexp)
        {
            return Set.RemoveAll(x => Regex.IsMatch(x.Tag, regexp));
        }

        public void Dispose()
        {
            Set = null;
        }
    }
}