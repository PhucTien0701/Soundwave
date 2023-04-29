using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Soundwave_Music.Models
{
    public partial class SoundwaveDbContext : DbContext
    {
        public SoundwaveDbContext()
            : base("name=SoundwaveDbContext")
        {
        }

        public virtual DbSet<Album> Albums { get; set; }
        public virtual DbSet<Album_Comment> Album_Comment { get; set; }
        public virtual DbSet<Album_Love_React> Album_Love_React { get; set; }
        public virtual DbSet<API_Key> API_Key { get; set; }
        public virtual DbSet<Area> Areas { get; set; }
        public virtual DbSet<Composer> Composers { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Like_Album_Comment> Like_Album_Comment { get; set; }
        public virtual DbSet<Like_News_Comment> Like_News_Comment { get; set; }
        public virtual DbSet<Like_Reply_News_Comment> Like_Reply_News_Comment { get; set; }
        public virtual DbSet<Like_Song_Comment> Like_Song_Comment { get; set; }
        public virtual DbSet<Like_Video_Comment> Like_Video_Comment { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<News_Comment> News_Comment { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Order_Detail> Order_Detail { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<Playlist> Playlists { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Reply_News_Comment> Reply_News_Comment { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Singer> Singers { get; set; }
        public virtual DbSet<Song> Songs { get; set; }
        public virtual DbSet<Song_Comment> Song_Comment { get; set; }
        public virtual DbSet<Song_Love_React> Song_Love_React { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Video> Videos { get; set; }
        public virtual DbSet<Video_Comment> Video_Comment { get; set; }
        public virtual DbSet<Video_Love_React> Video_Love_React { get; set; }
        public virtual DbSet<Role_Permission> Role_Permissions { get; set; }
        public virtual DbSet<PlaylistSong> PlaylistSongs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Album>()
                .Property(e => e.Album_status)
                .IsUnicode(false);

            modelBuilder.Entity<Album>()
                .HasMany(e => e.Album_Comment)
                .WithRequired(e => e.Album)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Album>()
                .HasMany(e => e.Album_Love_React)
                .WithRequired(e => e.Album)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Album_Comment>()
                .HasMany(e => e.Like_Album_Comment)
                .WithRequired(e => e.Album_Comment)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Area>()
                .Property(e => e.Area_status)
                .IsUnicode(false);

            modelBuilder.Entity<Area>()
                .HasMany(e => e.Albums)
                .WithRequired(e => e.Area)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Area>()
                .HasMany(e => e.Composers)
                .WithRequired(e => e.Area)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Area>()
                .HasMany(e => e.Genres)
                .WithRequired(e => e.Area)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Area>()
                .HasMany(e => e.Singers)
                .WithRequired(e => e.Area)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Area>()
                .HasMany(e => e.Songs)
                .WithRequired(e => e.Area)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Area>()
                .HasMany(e => e.Videos)
                .WithRequired(e => e.Area)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Composer>()
                .Property(e => e.Composer_status)
                .IsUnicode(false);

            modelBuilder.Entity<Composer>()
                .HasMany(e => e.Songs)
                .WithRequired(e => e.Composer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Composer>()
                .HasMany(e => e.Videos)
                .WithRequired(e => e.Composer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Genre>()
                .Property(e => e.Genre_status)
                .IsUnicode(false);

            modelBuilder.Entity<Genre>()
                .HasMany(e => e.Albums)
                .WithRequired(e => e.Genre)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Genre>()
                .HasMany(e => e.Songs)
                .WithRequired(e => e.Genre)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Genre>()
                .HasMany(e => e.Videos)
                .WithRequired(e => e.Genre)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<News_Comment>()
                .HasMany(e => e.Like_News_Comment)
                .WithRequired(e => e.News_Comment)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<News_Comment>()
                .HasMany(e => e.Reply_News_Comment)
                .WithRequired(e => e.News_Comment)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Order>()
                .Property(e => e.Status)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.Order_Detail)
                .WithRequired(e => e.Order)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Order_Detail>()
                .Property(e => e.Status)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Payment>()
                .Property(e => e.Status)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Payment>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.Payment)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Playlist>()
                .HasMany(e => e.PlaylistSong)
                .WithRequired(e => e.Playlist)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Song>()
                .HasMany(e => e.PlaylistSong)
                .WithRequired(e => e.Song)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.quantity)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.status)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Order_Detail)
                .WithRequired(e => e.Product)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Reply_News_Comment>()
                .HasMany(e => e.Like_Reply_News_Comment)
                .WithRequired(e => e.Reply_News_Comment)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Singer>()
                .Property(e => e.Singer_status)
                .IsUnicode(false);

            modelBuilder.Entity<Singer>()
                .HasMany(e => e.Albums)
                .WithRequired(e => e.Singer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Singer>()
                .HasMany(e => e.Songs)
                .WithRequired(e => e.Singer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Singer>()
                .HasMany(e => e.Videos)
                .WithRequired(e => e.Singer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Song>()
                .Property(e => e.Music_File_Upload)
                .IsUnicode(false);

            modelBuilder.Entity<Song>()
                .Property(e => e.Song_status)
                .IsUnicode(false);

            modelBuilder.Entity<Song>()
                .HasMany(e => e.Song_Comment)
                .WithRequired(e => e.Song)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Song>()
                .HasMany(e => e.Song_Love_React)
                .WithRequired(e => e.Song)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Song_Comment>()
                .HasMany(e => e.Like_Song_Comment)
                .WithRequired(e => e.Song_Comment)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Supplier>()
                .Property(e => e.Supplier_status)
                .IsUnicode(false);

            modelBuilder.Entity<Supplier>()
                .HasMany(e => e.Songs)
                .WithRequired(e => e.Supplier)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Supplier>()
                .HasMany(e => e.Videos)
                .WithRequired(e => e.Supplier)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Phone_number)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Gender)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.User_Status)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Album_Comment)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Album_Love_React)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Like_Album_Comment)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Like_News_Comment)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Like_Reply_News_Comment)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Like_Song_Comment)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Like_Video_Comment)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.News)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.News_Comment)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Playlists)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Reply_News_Comment)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Song_Comment)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Song_Love_React)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Video_Comment)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Video_Love_React)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Video>()
                .Property(e => e.Video_File_Upload)
                .IsUnicode(false);

            modelBuilder.Entity<Video>()
                .Property(e => e.Video_status)
                .IsUnicode(false);

            modelBuilder.Entity<Video>()
                .HasMany(e => e.Video_Comment)
                .WithRequired(e => e.Video)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Video>()
                .HasMany(e => e.Video_Love_React)
                .WithRequired(e => e.Video)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Video_Comment>()
                .HasMany(e => e.Like_Video_Comment)
                .WithRequired(e => e.Video_Comment)
                .WillCascadeOnDelete(false);
        }
    }
}
