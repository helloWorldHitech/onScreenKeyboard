

    public class JsonMenu
    {
        public string title { get; set; }
        public string src { get; set; }
        public string returnTo { get; set; }
        public JsonMenu[] children { get; set; }
        public Action action { get; set; }
    }

    public class Action
    {
        public string messageType { get; set; }
        public string type { get; set; }
        public int number { get; set; }
        public int velocity { get; set; }
        public string qwertyKey { get; set; }
    }   

