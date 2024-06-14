﻿// <auto-generated />
using System;
using LibraryMan.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LibraryMan.Migrations
{
    [DbContext(typeof(LibraryManContext))]
    partial class LibraryManContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.1");

            modelBuilder.Entity("LibraryMan.Models.KsiazkaModel", b =>
                {
                    b.Property<string>("BookName")
                        .HasColumnType("TEXT");

                    b.Property<double>("AverageRating")
                        .HasColumnType("REAL");

                    b.Property<string>("Genre")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PublisherName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("BookName");

                    b.HasIndex("PublisherName");

                    b.ToTable("KsiazkaModel");
                });

            modelBuilder.Entity("LibraryMan.Models.RecenzjaModel", b =>
                {
                    b.Property<int>("ReviewID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("BookName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Rating")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ReviewDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("UserID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ReviewID");

                    b.HasIndex("BookName");

                    b.HasIndex("UserID");

                    b.ToTable("RecenzjaModel");
                });

            modelBuilder.Entity("LibraryMan.Models.UzytkownikModel", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("UserID");

                    b.ToTable("UzytkownikModel");
                });

            modelBuilder.Entity("LibraryMan.Models.WydawnictwoModel", b =>
                {
                    b.Property<string>("PublisherName")
                        .HasColumnType("TEXT");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Founded")
                        .HasColumnType("INTEGER");

                    b.HasKey("PublisherName");

                    b.ToTable("WydawnictwoModel");
                });

            modelBuilder.Entity("LibraryMan.Models.KsiazkaModel", b =>
                {
                    b.HasOne("LibraryMan.Models.WydawnictwoModel", "WydawnictwoModel")
                        .WithMany()
                        .HasForeignKey("PublisherName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WydawnictwoModel");
                });

            modelBuilder.Entity("LibraryMan.Models.RecenzjaModel", b =>
                {
                    b.HasOne("LibraryMan.Models.KsiazkaModel", "KsiazkaModel")
                        .WithMany()
                        .HasForeignKey("BookName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LibraryMan.Models.UzytkownikModel", "UzytkownikModel")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("KsiazkaModel");

                    b.Navigation("UzytkownikModel");
                });
#pragma warning restore 612, 618
        }
    }
}
