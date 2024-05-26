using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TicketOnline.Model;

namespace TicketOnline.Data
{
    public class Movie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public string Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [MaxLength(10000)]
        public string Name { get; set; }
        [Required]
        public string Director { get; set; }
        [Required]
        public string Cast { get; set; }
        [Required]
        public string GenreId { get; set; }
        [ForeignKey("GenreId")]
        public virtual Genre? Genre { get; set; }
        [Required]
        public string Language { get; set; }
        [Required]
        public DateOnly ReleaseDate { get; set; }
        [Required]
        public TimeOnly RunTime { get; set; }
        public bool Adult { get; set; }
        public bool Status { get; set; }
        public int Budget { get; set; }
        public int Revenue { get; set; }
        public decimal Vote_average { get; set; }
        public int Vote_count { get; set; }
        public bool Video { get; set; }
        public decimal Popularity { get; set; }
        public string Tagline { get; set; }
        public string HomePage { get; set; }
        public string Overview { get; set; }
        public string PosterPath { get; set; }
        public string BackdropPath { get; set; }
        //one to many 
        public virtual ICollection<ShowTime>? Showtimes { get; set; }
    }
}
