﻿// <auto-generated />
using System;
using APIBaseTemplate.Datamodel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace APIBaseTemplate.Migrations
{
    [DbContext(typeof(APIBaseTemplateDbContext))]
    partial class APIBaseTemplateDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("APIBaseTemplate.Datamodel.DbEntities.Airline", b =>
                {
                    b.Property<int>("AirlineId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AirlineId"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("RegionId")
                        .HasColumnType("int");

                    b.HasKey("AirlineId");

                    b.HasIndex("RegionId")
                        .HasDatabaseName("IDX_AIRLINES_REGION");

                    b.ToTable("Airlines", (string)null);
                });

            modelBuilder.Entity("APIBaseTemplate.Datamodel.DbEntities.Airport", b =>
                {
                    b.Property<int>("AirportId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AirportId"));

                    b.Property<int>("CityId")
                        .HasColumnType("int");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("AirportId");

                    b.HasIndex("CityId")
                        .HasDatabaseName("IDX_AIRPORTS_CITY");

                    b.ToTable("Airports", (string)null);
                });

            modelBuilder.Entity("APIBaseTemplate.Datamodel.DbEntities.City", b =>
                {
                    b.Property<int>("CityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CityId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("RegionId")
                        .HasColumnType("int");

                    b.HasKey("CityId");

                    b.HasIndex("RegionId")
                        .HasDatabaseName("IDX_CITIES_REGION");

                    b.ToTable("Cities", (string)null);
                });

            modelBuilder.Entity("APIBaseTemplate.Datamodel.DbEntities.Currency", b =>
                {
                    b.Property<int>("CurrencyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CurrencyId"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("CurrencyId");

                    b.ToTable("Currencies", (string)null);
                });

            modelBuilder.Entity("APIBaseTemplate.Datamodel.DbEntities.Fligth", b =>
                {
                    b.Property<int>("FligthId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FligthId"));

                    b.Property<int>("AirlineId")
                        .HasColumnType("int");

                    b.Property<int>("ArrivalAirportId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ArrivalTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)");

                    b.Property<int>("DepartureAirportId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DepartureTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Gate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Terminal")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("FligthId");

                    b.HasIndex("AirlineId")
                        .HasDatabaseName("IDX_FLIGHTS_AIRLINE");

                    b.HasIndex("ArrivalAirportId")
                        .HasDatabaseName("IDX_FLIGHTS_ARRIVAL_AIRPORT");

                    b.HasIndex("DepartureAirportId")
                        .HasDatabaseName("IDX_FLIGHTS_DEPARTURE_AIRPORT");

                    b.ToTable("Fligths", (string)null);
                });

            modelBuilder.Entity("APIBaseTemplate.Datamodel.DbEntities.FligthService", b =>
                {
                    b.Property<int>("FligthServiceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FligthServiceId"));

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<int>("CurrencyId")
                        .HasColumnType("int");

                    b.Property<string>("FlightServiceType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasComment("FlightServiceType values (Fligth = 0, HandLuggage = 1, HoldLuggage = 2, FligthInsurance = 3)");

                    b.Property<int>("FligthId")
                        .HasColumnType("int");

                    b.HasKey("FligthServiceId");

                    b.HasIndex("CurrencyId")
                        .HasDatabaseName("IDX_FLIGTHSERVICES_CURRENCY");

                    b.HasIndex("FligthId")
                        .HasDatabaseName("IDX_FLIGTHSERVICES_FLIGTH");

                    b.ToTable("FligthServices", (string)null);
                });

            modelBuilder.Entity("APIBaseTemplate.Datamodel.DbEntities.Region", b =>
                {
                    b.Property<int>("RegionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RegionId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("RegionId");

                    b.ToTable("Regions", (string)null);
                });

            modelBuilder.Entity("APIBaseTemplate.Datamodel.DbEntities.Airline", b =>
                {
                    b.HasOne("APIBaseTemplate.Datamodel.DbEntities.Region", "Region")
                        .WithMany()
                        .HasForeignKey("RegionId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_AIRLINES_REGION");

                    b.Navigation("Region");
                });

            modelBuilder.Entity("APIBaseTemplate.Datamodel.DbEntities.Airport", b =>
                {
                    b.HasOne("APIBaseTemplate.Datamodel.DbEntities.City", "City")
                        .WithMany()
                        .HasForeignKey("CityId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_AIRPORTS_CITY");

                    b.Navigation("City");
                });

            modelBuilder.Entity("APIBaseTemplate.Datamodel.DbEntities.City", b =>
                {
                    b.HasOne("APIBaseTemplate.Datamodel.DbEntities.Region", "Region")
                        .WithMany()
                        .HasForeignKey("RegionId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_CITIES_REGION");

                    b.Navigation("Region");
                });

            modelBuilder.Entity("APIBaseTemplate.Datamodel.DbEntities.Fligth", b =>
                {
                    b.HasOne("APIBaseTemplate.Datamodel.DbEntities.Airline", "Airline")
                        .WithMany()
                        .HasForeignKey("AirlineId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_FLIGHTS_AIRLINE");

                    b.HasOne("APIBaseTemplate.Datamodel.DbEntities.Airport", "ArrivalAirport")
                        .WithMany()
                        .HasForeignKey("ArrivalAirportId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_FLIGHTS_ARRIVAL_AIRPORT");

                    b.HasOne("APIBaseTemplate.Datamodel.DbEntities.Airport", "DepartureAirport")
                        .WithMany()
                        .HasForeignKey("DepartureAirportId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_FLIGHTS_DEPARTURE_AIPORT");

                    b.Navigation("Airline");

                    b.Navigation("ArrivalAirport");

                    b.Navigation("DepartureAirport");
                });

            modelBuilder.Entity("APIBaseTemplate.Datamodel.DbEntities.FligthService", b =>
                {
                    b.HasOne("APIBaseTemplate.Datamodel.DbEntities.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_FLIGTHSERVICES_CURRENCY");

                    b.HasOne("APIBaseTemplate.Datamodel.DbEntities.Fligth", "Fligth")
                        .WithMany("FligthServices")
                        .HasForeignKey("FligthId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_FLIGTHSERVICES_FLIGTH");

                    b.Navigation("Currency");

                    b.Navigation("Fligth");
                });

            modelBuilder.Entity("APIBaseTemplate.Datamodel.DbEntities.Fligth", b =>
                {
                    b.Navigation("FligthServices");
                });
#pragma warning restore 612, 618
        }
    }
}
