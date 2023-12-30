using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Sales_Dash_Board.Models
{
    public partial class SalesDashboard_dbContext : DbContext
    {
        public SalesDashboard_dbContext()
        {
        }

        public SalesDashboard_dbContext(DbContextOptions<SalesDashboard_dbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<LoginUser> LoginUser { get; set; }
        public virtual DbSet<SalesPerson> SalesPerson { get; set; }
        public virtual DbSet<SalesTrack> SalesTrack { get; set; }
        public virtual DbSet<SourceId> SourceId { get; set; }
        public virtual DbSet<SourcePerson> SourcePerson { get; set; }
        public virtual DbSet<StateId> StateId { get; set; }
        public virtual DbSet<Version> Version { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=13.126.28.255,3336;Initial Catalog=SalesDashboard_db;User Id=SalesDashboard_db_Admin;Password=Bourn@2023");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LoginUser>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Password)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SalesPerson>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Designation)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Mobile)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SalesPersonName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Sid)
                    .HasColumnName("SID")
                    .ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<SalesTrack>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address)
                    .HasMaxLength(800)
                    .IsUnicode(false);

                entity.Property(e => e.Attachments).HasMaxLength(200);

                entity.Property(e => e.Comment).HasMaxLength(800);

                entity.Property(e => e.Createddate).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Followupdate).HasColumnType("datetime");

                entity.Property(e => e.LabName)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Mobile)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.OwnerName)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ProjectedAmount).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.SalesPersonId).HasColumnName("SalesPersonID");

                entity.Property(e => e.SourceId).HasColumnName("SourceID");

                entity.Property(e => e.StateId).HasColumnName("StateID");

                entity.Property(e => e.VersionId).HasColumnName("VersionID");
            });

            modelBuilder.Entity<SourceId>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SourceID");

                entity.Property(e => e.Sid)
                    .HasColumnName("SID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.SourceName)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SourcePerson>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.CompanyName)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Mobile)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SourceId).HasColumnName("SourceID");

                entity.Property(e => e.SourcePersonName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Spid)
                    .HasColumnName("SPID")
                    .ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<StateId>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("StateID");

                entity.Property(e => e.StateName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Stid)
                    .HasColumnName("STID")
                    .ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Version>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.EstimatedCost).HasColumnType("numeric(18, 2)");

                entity.Property(e => e.VesionName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Vid)
                    .HasColumnName("VID")
                    .ValueGeneratedOnAdd();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
        public virtual DataSet GetSalesDashboard(string FromDate, string ToDate, int SalesPersonID = 0)
        {
            using (var connection = Database.GetDbConnection())
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SalesDashboard";
                    command.CommandType = CommandType.StoredProcedure;

                    var parameter1 = new SqlParameter("@FromDate", FromDate);
                    var parameter2 = new SqlParameter("@ToDate", ToDate);
                    var parameter3 = new SqlParameter("@SalesPersonID", SalesPersonID);

                    command.Parameters.Add(parameter1);
                    command.Parameters.Add(parameter2);
                    command.Parameters.Add(parameter3);

                    using (var adapter = new SqlDataAdapter((SqlCommand)command))
                    {
                        var dataSet = new DataSet();
                        adapter.Fill(dataSet);
                        return dataSet;
                    }
                }
            }
        }

        public virtual IEnumerable<SalesTrack> SalesList(string FromDate, string ToDate, int SalesPersonID = 0, int VersionID = 0, int StateID = 0)
        {
            var result = Set<SalesTrack>().FromSqlRaw("EXEC SalesList @FromDate, @ToDate,@SalesPersonID,@VersionID,@StateID",
                  new SqlParameter("@FromDate", FromDate),
                  new SqlParameter("@ToDate", ToDate),
                  new SqlParameter("@SalesPersonID", SalesPersonID),
                  new SqlParameter("@VersionID", VersionID),
                  new SqlParameter("@StateID", StateID)
            ).ToList();
            return result;
        }

    }
}
