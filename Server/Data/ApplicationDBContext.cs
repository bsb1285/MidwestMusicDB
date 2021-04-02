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
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<ArtistsAlbum> ArtistsAlbums { get; set; }
        public DbSet<ArtistsSong> ArtistsSongs { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<Song> Songs { get; set; }
        public DbSet<SongOnAlbum> SongOnAlbums { get; set; }
        public DbSet<SongOnPlaylist> SongOnPlaylists { get; set; }

    }
}