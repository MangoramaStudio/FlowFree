using UnityEngine;

namespace Mechanics.RoboticFlows
{
    [System.Serializable]
    public class Flow
    {
        public int id;
        public Node a;
        public Node b;
        public Color color;

        public void Initialize()
        {
            a.SetId(id);
            b.SetId(id);
            a.SetColor(color);
            b.SetColor(color);
        }
    }
}