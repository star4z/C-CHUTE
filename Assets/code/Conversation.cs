using System.Collections.Generic;

public class Conversation
    {
        public string Trigger { get; set; }
        public List<MyEvent> Events { get; set; }

        public Conversation(string trigger)
        {
            Trigger = trigger;
            Events = new List<MyEvent>();
        }
    }