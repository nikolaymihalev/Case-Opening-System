﻿// <auto-generated />
using System;
using CaseOpener.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CaseOpener.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241224102821_AddRoles")]
    partial class AddRoles
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CaseOpener.Infrastructure.Models.Case", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("Case's identifier");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<byte[]>("Image")
                        .IsRequired()
                        .HasColumnType("varbinary(max)")
                        .HasComment("Case's image");

                    b.Property<string>("Items")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasComment("Case's items");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasComment("Case's name");

                    b.Property<decimal>("Price")
                        .HasPrecision(18, 4)
                        .HasColumnType("decimal(18,4)")
                        .HasComment("Case's price");

                    b.HasKey("Id");

                    b.ToTable("Cases", t =>
                        {
                            t.HasComment("Represents the Case");
                        });
                });

            modelBuilder.Entity("CaseOpener.Infrastructure.Models.CaseOpening", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("Opening's identifier");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CaseId")
                        .HasColumnType("int")
                        .HasComment("Case's identifier");

                    b.Property<DateTime>("DateOpened")
                        .HasColumnType("datetime2")
                        .HasComment("Opening's open date");

                    b.Property<int>("ItemId")
                        .HasColumnType("int")
                        .HasComment("Item's identifier");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasComment("User's identifier");

                    b.HasKey("Id");

                    b.HasIndex("CaseId");

                    b.HasIndex("ItemId");

                    b.HasIndex("UserId");

                    b.ToTable("CaseOpenings", t =>
                        {
                            t.HasComment("Represents case opening");
                        });
                });

            modelBuilder.Entity("CaseOpener.Infrastructure.Models.DailyReward", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("Reward's identifier");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CaseId")
                        .HasColumnType("int")
                        .HasComment("Case's identifier");

                    b.Property<DateTime>("LastClaimedDate")
                        .HasColumnType("datetime2")
                        .HasComment("Case's claimed date");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasComment("User's identifier");

                    b.HasKey("Id");

                    b.HasIndex("CaseId");

                    b.HasIndex("UserId");

                    b.ToTable("DailyRewards", t =>
                        {
                            t.HasComment("Represents the user's daily reward");
                        });
                });

            modelBuilder.Entity("CaseOpener.Infrastructure.Models.InventoryItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("Inventory item's identifier");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("AcquiredDate")
                        .HasColumnType("datetime2")
                        .HasComment("Inventory item's acquired date");

                    b.Property<int>("ItemId")
                        .HasColumnType("int")
                        .HasComment("Item's identifier");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasComment("User's identifier");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("UserId");

                    b.ToTable("InventoryItems", t =>
                        {
                            t.HasComment("Represents the user's inventory item");
                        });
                });

            modelBuilder.Entity("CaseOpener.Infrastructure.Models.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("Item's indentifier");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasPrecision(18, 4)
                        .HasColumnType("decimal(18,4)")
                        .HasComment("Item's amount");

                    b.Property<byte[]>("Image")
                        .IsRequired()
                        .HasColumnType("varbinary(max)")
                        .HasComment("Item's image");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasComment("Item's name");

                    b.Property<double>("Probability")
                        .HasColumnType("float")
                        .HasComment("Item's probability chance");

                    b.Property<string>("Rarity")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasComment("Item's rarity");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasComment("Item's type");

                    b.HasKey("Id");

                    b.ToTable("Items", t =>
                        {
                            t.HasComment("Represents the Item");
                        });
                });

            modelBuilder.Entity("CaseOpener.Infrastructure.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("Role's identifier");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasComment("Role's name");

                    b.HasKey("Id");

                    b.ToTable("Roles", t =>
                        {
                            t.HasComment("Represents the user role");
                        });
                });

            modelBuilder.Entity("CaseOpener.Infrastructure.Models.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasComment("Transaction's identifier");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasPrecision(18, 4)
                        .HasColumnType("decimal(18,4)")
                        .HasComment("Transaction's amount");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2")
                        .HasComment("Transaction's date");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasComment("Transaction's status");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasComment("Transaction's type");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasComment("User's identifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Transactions", t =>
                        {
                            t.HasComment("Represents the Transaction");
                        });
                });

            modelBuilder.Entity("CaseOpener.Infrastructure.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)")
                        .HasComment("User's identifier");

                    b.Property<decimal>("Balance")
                        .HasPrecision(18, 4)
                        .HasColumnType("decimal(18,4)")
                        .HasComment("User's balance");

                    b.Property<DateTime>("DateJoined")
                        .HasColumnType("datetime2")
                        .HasComment("The date when user joined");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasComment("User's email");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasComment("User's password");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasComment("User's username");

                    b.HasKey("Id");

                    b.ToTable("Users", t =>
                        {
                            t.HasComment("Represents the User");
                        });
                });

            modelBuilder.Entity("CaseOpener.Infrastructure.Models.UserRole", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)")
                        .HasComment("User's identifier");

                    b.Property<int>("RoleId")
                        .HasColumnType("int")
                        .HasComment("Role's identifier");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles", t =>
                        {
                            t.HasComment("Represents a mapping between users and roles");
                        });
                });

            modelBuilder.Entity("CaseOpener.Infrastructure.Models.CaseOpening", b =>
                {
                    b.HasOne("CaseOpener.Infrastructure.Models.Case", "Case")
                        .WithMany("CaseOpenings")
                        .HasForeignKey("CaseId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CaseOpener.Infrastructure.Models.Item", "Item")
                        .WithMany("CaseOpenings")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CaseOpener.Infrastructure.Models.User", "User")
                        .WithMany("CaseOpenings")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Case");

                    b.Navigation("Item");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CaseOpener.Infrastructure.Models.DailyReward", b =>
                {
                    b.HasOne("CaseOpener.Infrastructure.Models.Case", "Case")
                        .WithMany("DailyRewards")
                        .HasForeignKey("CaseId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CaseOpener.Infrastructure.Models.User", "User")
                        .WithMany("DailyRewards")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Case");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CaseOpener.Infrastructure.Models.InventoryItem", b =>
                {
                    b.HasOne("CaseOpener.Infrastructure.Models.Item", "Item")
                        .WithMany("InventoryItems")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CaseOpener.Infrastructure.Models.User", "User")
                        .WithMany("InventoryItems")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Item");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CaseOpener.Infrastructure.Models.Transaction", b =>
                {
                    b.HasOne("CaseOpener.Infrastructure.Models.User", "User")
                        .WithMany("Transactions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("CaseOpener.Infrastructure.Models.UserRole", b =>
                {
                    b.HasOne("CaseOpener.Infrastructure.Models.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CaseOpener.Infrastructure.Models.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("CaseOpener.Infrastructure.Models.Case", b =>
                {
                    b.Navigation("CaseOpenings");

                    b.Navigation("DailyRewards");
                });

            modelBuilder.Entity("CaseOpener.Infrastructure.Models.Item", b =>
                {
                    b.Navigation("CaseOpenings");

                    b.Navigation("InventoryItems");
                });

            modelBuilder.Entity("CaseOpener.Infrastructure.Models.Role", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("CaseOpener.Infrastructure.Models.User", b =>
                {
                    b.Navigation("CaseOpenings");

                    b.Navigation("DailyRewards");

                    b.Navigation("InventoryItems");

                    b.Navigation("Transactions");

                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
