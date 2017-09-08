using System;
namespace VSLiveToDo.Models
{
    public class ToDoItem
    {
        public string Text { get; set; }
        public string Notes { get; set; }
        public bool Complete { get; set; }

        public string Id { get; set; }
        public byte[] Version { get; set; }
    }
}
