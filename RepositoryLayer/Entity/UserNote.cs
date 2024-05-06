using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RepositoryLayer.Entity
{
    public class UserNote
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NoteId { get; set; }

        public string Description { get; set; }
        public string Title { get; set; }
        public string Colour { get; set; } = "";
        public bool IsArchived { get; set; } = false;

        public bool IsDeleted { get; set; } = false;


        [ForeignKey("User")]
        public int UserId { get; set; }

        [JsonIgnore]
        public virtual UserEntity User { get; set; }
    }
}
