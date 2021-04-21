using System;
using Microsoft.EntityFrameworkCore;
using MidwestMusicDB.Shared.Models;


namespace MidwestMusicDB.Server.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options):base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasKey(u => u.username);
            modelBuilder.Entity<Album>()
                    .HasKey(a => a.album_name);
            modelBuilder.Entity<Artist>()
                    .HasKey(a => a.artist_name);
            modelBuilder.Entity<ArtistsAlbum>()
                    .HasKey(aa => aa.album_name);
            modelBuilder.Entity<ArtistsAlbum>()
                    .HasAlternateKey(aa => aa.artist_name);
            modelBuilder.Entity<ArtistsSong>()
                    .HasKey(song => song.title);
            modelBuilder.Entity<ArtistsSong>()
                    .HasAlternateKey(song => song.artist_name);
            modelBuilder.Entity<Genre>()
                        .HasKey(g => g.name);
            modelBuilder.Entity<Playlist>()
                        .HasKey(playlist => playlist.id);
            modelBuilder.Entity<Song>()
                        .HasKey(song => song.title);
            modelBuilder.Entity<SongOnAlbum>()
                    .HasKey(song => song.title);
            modelBuilder.Entity<SongOnAlbum>()
                    .HasAlternateKey(song => song.album_name);
            modelBuilder.Entity<SongOnPlaylist>()
                    .HasKey(song => song.title);
            modelBuilder.Entity<SongOnPlaylist>()
                    .HasAlternateKey(song => song.id);
            modelBuilder.Entity<UsersPlaylist>()
                    .HasKey(up => up.username);
            modelBuilder.Entity<UsersPlaylist>()
                    .HasAlternateKey(up => up.id);
            modelBuilder.Entity<SongGenre>().HasKey(sg => sg.name);
            modelBuilder.Entity<SongGenre>().HasAlternateKey(sg => sg.title);
            modelBuilder.Entity<UserSong>().HasKey(sg => sg.username);
            modelBuilder.Entity<UserSong>().HasAlternateKey(sg => sg.title);
            modelBuilder.Entity<UserFollower>().HasKey(u => u.following);
            modelBuilder.Entity<UserFollower>().HasAlternateKey(u => u.follower);




        }
        public DbSet<User> Users { get; set; }
        public DbSet<Album> Album { get; set; }
        public DbSet<Artist> Artist { get; set; }
        public DbSet<ArtistsAlbum> ArtistsAlbum { get; set; }
        public DbSet<ArtistsSong> ArtistsSong { get; set; }
        public DbSet<Genre> Genre { get; set; }
        public DbSet<Playlist> Playlist { get; set; }
        public DbSet<Song> Song { get; set; }
        public DbSet<SongOnAlbum> SongOnAlbum { get; set; }
        public DbSet<SongOnPlaylist> SongOnPlaylist { get; set; }
        public DbSet<UsersPlaylist> UsersPlaylist { get; set; }
        public DbSet<SongGenre> SongGenre { get; set; }
        public DbSet<UserSong> UserSong { get; set; }
        public DbSet<UserFollower> UserFollower { get; set; }

    }
}