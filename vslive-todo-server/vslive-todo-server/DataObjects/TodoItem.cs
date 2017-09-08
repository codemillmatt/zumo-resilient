using Microsoft.Azure.Mobile.Server;

namespace vslive_todo_server.DataObjects
{
    public class TodoItem : EntityData
    {
        public string Text { get; set; }

        public bool Complete { get; set; }

        public string Notes { get; set; }
    }
}