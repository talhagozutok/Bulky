﻿// <auto-generated />
using Bulky.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Bulky.DataAccess.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230728093603_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Bulky.Models.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DisplayOrder")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DisplayOrder = 1,
                            Name = "Action"
                        },
                        new
                        {
                            Id = 2,
                            DisplayOrder = 2,
                            Name = "Sci-Fi"
                        },
                        new
                        {
                            Id = 3,
                            DisplayOrder = 3,
                            Name = "History"
                        });
                });

            modelBuilder.Entity("Bulky.Models.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(550)
                        .HasColumnType("nvarchar(550)");

                    b.Property<string>("ISBN")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("ListPrice")
                        .HasColumnType("float");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<double>("PriceFifty")
                        .HasColumnType("float");

                    b.Property<double>("PriceHundredOrMore")
                        .HasColumnType("float");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Author = "Stephen King",
                            CategoryId = 1,
                            Description = "11/22/63 is a novel by Stephen King about a time traveller who attempts to prevent the assassination of United States President John F. Kennedy, which occurred on November 22, 1963.",
                            ISBN = "06A2B06C-2CC",
                            ImageUrl = "\\images\\product\\initial\\22-11-63.jpg",
                            ListPrice = 100.0,
                            Price = 80.0,
                            PriceFifty = 70.0,
                            PriceHundredOrMore = 60.0,
                            Title = "22/11/63"
                        },
                        new
                        {
                            Id = 2,
                            Author = "Vladimir Bartol",
                            CategoryId = 2,
                            Description = "Alamut is a novel by Vladimir Bartol, first published in 1938 in Slovenian, dealing with the story of Hassan-i Sabbah and the Hashshashin, and named after their Alamut fortress. The maxim of the novel is \"Nothing is an absolute reality; all is permitted\". This book was one of the inspirations for the video game series Assassin's Creed.",
                            ISBN = "BF16868D-B8D",
                            ImageUrl = "\\images\\product\\initial\\alamut.jpg",
                            ListPrice = 150.0,
                            Price = 100.0,
                            PriceFifty = 80.0,
                            PriceHundredOrMore = 70.0,
                            Title = "Alamut"
                        },
                        new
                        {
                            Id = 3,
                            Author = "Billy Spark",
                            CategoryId = 2,
                            Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                            ISBN = "SWD9999001",
                            ImageUrl = "\\images\\product\\initial\\fortune of time.jpg",
                            ListPrice = 99.0,
                            Price = 90.0,
                            PriceFifty = 85.0,
                            PriceHundredOrMore = 80.0,
                            Title = "Fortune of Time"
                        },
                        new
                        {
                            Id = 4,
                            Author = "Nancy Hoover",
                            CategoryId = 2,
                            Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                            ISBN = "CAW777777701",
                            ImageUrl = "\\images\\product\\initial\\dark skies.jpg",
                            ListPrice = 40.0,
                            Price = 30.0,
                            PriceFifty = 25.0,
                            PriceHundredOrMore = 20.0,
                            Title = "Dark Skies"
                        },
                        new
                        {
                            Id = 5,
                            Author = "Julian Button",
                            CategoryId = 2,
                            Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                            ISBN = "RITO5555501",
                            ImageUrl = "\\images\\product\\initial\\vanish in the sunset.jpg",
                            ListPrice = 55.0,
                            Price = 50.0,
                            PriceFifty = 40.0,
                            PriceHundredOrMore = 35.0,
                            Title = "Vanish in the Sunset"
                        },
                        new
                        {
                            Id = 6,
                            Author = "Abby Muscles",
                            CategoryId = 2,
                            Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                            ISBN = "WS3333333301",
                            ImageUrl = "\\images\\product\\initial\\cotton candy.jpg",
                            ListPrice = 70.0,
                            Price = 65.0,
                            PriceFifty = 60.0,
                            PriceHundredOrMore = 55.0,
                            Title = "Cotton Candy"
                        },
                        new
                        {
                            Id = 7,
                            Author = "Ron Parker",
                            CategoryId = 2,
                            Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                            ISBN = "SOTJ1111111101",
                            ImageUrl = "\\images\\product\\initial\\rock in the ocean.jpg",
                            ListPrice = 30.0,
                            Price = 27.0,
                            PriceFifty = 25.0,
                            PriceHundredOrMore = 20.0,
                            Title = "Rock in the Ocean"
                        },
                        new
                        {
                            Id = 8,
                            Author = "Laura Phantom",
                            CategoryId = 2,
                            Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                            ISBN = "FOT000000001",
                            ImageUrl = "\\images\\product\\initial\\leaves and wonders.jpg",
                            ListPrice = 25.0,
                            Price = 23.0,
                            PriceFifty = 22.0,
                            PriceHundredOrMore = 20.0,
                            Title = "Leaves and Wonders"
                        });
                });

            modelBuilder.Entity("Bulky.Models.Entities.Product", b =>
                {
                    b.HasOne("Bulky.Models.Entities.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });
#pragma warning restore 612, 618
        }
    }
}