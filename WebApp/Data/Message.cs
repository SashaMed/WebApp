using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Data
{
    [Table("Messages", Schema = "data")]
    public class Message
    {
        public int Id { get; set; }
        public string SenderName { get; set; }

        public string Text { get; set; }

        public DateTime SendTime { get; set; }

        public string ReciverName { get; set; }
    }
}
