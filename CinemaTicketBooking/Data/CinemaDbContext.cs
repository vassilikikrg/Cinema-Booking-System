using System;
using System.Collections.Generic;
using CinemaTicketBooking.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaTicketBooking.Data;

public partial class CinemaDbContext : DbContext
{
    public CinemaDbContext()
    {
    }

    public CinemaDbContext(DbContextOptions<CinemaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Cinema> Cinemas { get; set; }

    public virtual DbSet<ContentAdmin> ContentAdmins { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<Screening> Screenings { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_admin_1");

            entity.HasOne(d => d.User).WithMany(p => p.Admins)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("admins_fk0");
        });

        modelBuilder.Entity<Cinema>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_cinema_1");
        });

        modelBuilder.Entity<ContentAdmin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_contentAdmin_1");

            entity.HasOne(d => d.User).WithMany(p => p.ContentAdmins)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("content_admin_fk0");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_customer_1");

            entity.HasOne(d => d.User).WithMany(p => p.Customers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("customers_fk0");
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_movie_1");

            entity.HasOne(d => d.ContentAdmin).WithMany(p => p.Movies)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("movies_fk0");
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_reservation_1");

            entity.HasOne(d => d.Customer).WithMany(p => p.Reservations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("reservations_fk1");

            entity.HasOne(d => d.Screening).WithMany(p => p.Reservations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("reservations_fk0");
        });

        modelBuilder.Entity<Screening>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_screening_1");

            entity.HasOne(d => d.Cinema).WithMany(p => p.Screenings)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("screenings_fk1");

            entity.HasOne(d => d.Movie).WithMany(p => p.Screenings)
                .OnDelete(DeleteBehavior.Cascade) 
                .HasConstraintName("screenings_fk0");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_user_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
