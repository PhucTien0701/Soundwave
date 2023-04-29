using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace Soundwave_Music.Models
{
    [Table("Song")]
    public partial class Song
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Song()
        {
            Song_Comment = new HashSet<Song_Comment>();
            Song_Love_React = new HashSet<Song_Love_React>();
        }

        //Song ID
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Song ID")]
        public int Song_id { get; set; }

        //Image
        [Required(ErrorMessage = "You have to upload an image for song.")]
        [StringLength(500)]
        public string Image { get; set; }

        //Song Name
        [Required(ErrorMessage = "Please input song name.")]
        [Display(Name = "Song Name")]
        [StringLength(100)]
        public string Song_name { get; set; }

        //Song Count View
        public int View_count { get; set; }

        //Song File
        [StringLength(500)]
        public string Music_File_Upload { get; set; }

        //Release Date
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Release Date")]
        public DateTime Release_date { get; set; }

        //Song Lyric
        public string Lyric { get; set; }

        //Song Status
        [StringLength(1)]
        public string Song_status { get; set; }

        //Create by
        [Required]
        [Display(Name = "Create By")]
        [StringLength(100)]
        public string Create_by { get; set; }

        public int Album_id { get; set; }

        public int Singer_id { get; set; }

        public int Composer_id { get; set; }

        public int Genre_id { get; set; }

        public int Supplier_id { get; set; }

        public int Area_id { get; set; }

        public virtual Album Album { get; set; }

        public virtual Area Area { get; set; }

        public virtual Composer Composer { get; set; }

        public virtual Genre Genre { get; set; }

        public virtual Singer Singer { get; set; }

        public virtual Supplier Supplier { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Song_Comment> Song_Comment { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Song_Love_React> Song_Love_React { get; set; }

        public virtual ICollection<PlaylistSong> PlaylistSong { get; set; }
    }
}
