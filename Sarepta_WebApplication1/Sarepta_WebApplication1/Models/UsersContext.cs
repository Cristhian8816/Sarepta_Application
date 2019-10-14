using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Sarepta_WebApplication1.Models
{
    public partial class UsersContext : DbContext
    {
        public UsersContext()
        {
        }

        public UsersContext(DbContextOptions<UsersContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Patients> Patients { get; set; }
        public virtual DbSet<Payments> Payments { get; set; }
        public virtual DbSet<Processes> Processes { get; set; }
        public virtual DbSet<Treatments> Treatments { get; set; }
        public virtual DbSet<userAccount> userAccount { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseSqlServer("Data Source=MVMOAN175A\\SQLEXPRESS;Initial Catalog=Sarepta_Consultory;User ID=sa;Password=sa");
                optionsBuilder.UseSqlServer("Data Source = SQL5045.site4now.net; Initial Catalog = DB_A4E9E2_SareptaConsultory; User Id = DB_A4E9E2_SareptaConsultory_admin; Password = 40A39j21c14a11l");                
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patients>(entity =>
            {
                entity.HasKey(e => e.PatientId);

                entity.ToTable("patients");

                entity.Property(e => e.PatientId)
                    .HasColumnName("patient_id")
                    .ValueGeneratedNever();                

                entity.Property(e => e.Address)
                    .HasColumnName("address")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Birthdate)
                    .HasColumnName("birthdate")
                    .HasColumnType("date");

                entity.Property(e => e.CellphoneNumber)
                    .HasColumnName("cellphone_number")
                    .HasMaxLength(10);

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("date");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(50);

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasColumnName("gender")
                    .HasMaxLength(1);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.city)
                   .HasColumnName("city")
                   .HasMaxLength(50)
                   .IsUnicode(false);

                entity.Property(e => e.cedula)
                   .HasColumnName("cedula")
                   .HasMaxLength(12)
                   .IsUnicode(true);
            });

            modelBuilder.Entity<Payments>(entity =>
            {
                entity.HasKey(e => e.PaymentId);

                entity.ToTable("payments");

                entity.Property(e => e.PaymentId)
                    .HasColumnName("payment_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Discount).HasColumnName("discount");

                entity.Property(e => e.Finished).HasColumnName("finished");

                entity.Property(e => e.Pay).HasColumnName("pay");

                entity.Property(e => e.PayDate)
                    .HasColumnName("pay_date")
                    .HasColumnType("date");

                entity.Property(e => e.PayType)
                    .IsRequired()
                    .HasColumnName("pay_type")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.ProcessId).HasColumnName("process_id");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasColumnName("status")
                    .HasMaxLength(8)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Processes>(entity =>
            {
                entity.HasKey(e => e.ProcessId);

                entity.ToTable("processes");

                entity.Property(e => e.ProcessId)
                    .HasColumnName("process_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Assistant).HasColumnName("assistant");

                entity.Property(e => e.Consultory).HasColumnName("consultory");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("date");

                entity.Property(e => e.Home).HasColumnName("home");

                entity.Property(e => e.Laboratory).HasColumnName("laboratory");

                entity.Property(e => e.Materials).HasColumnName("materials");

                entity.Property(e => e.PatientId).HasColumnName("patient_id");

                entity.Property(e => e.Tooth)
                    .HasColumnName("tooth")
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.Transport).HasColumnName("transport");

                entity.Property(e => e.TreatmentsId).HasColumnName("treatments_id");

                entity.Property(e => e.real_Cost).HasColumnName("real_Cost");
            });

            modelBuilder.Entity<Treatments>(entity =>
            {
                entity.HasKey(e => e.TreatmentId);

                entity.ToTable("treatments");

                entity.Property(e => e.TreatmentId)
                    .HasColumnName("treatment_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(50);

                entity.Property(e => e.Price).HasColumnName("price");
            });

            modelBuilder.Entity<userAccount>(entity =>
            {
                entity.HasKey(e => e.UserID);

                entity.ToTable("UserAccounts");

                entity.Property(e => e.UserID)
                    .IsRequired()
                    .HasColumnName("UserID")
                    .ValueGeneratedNever();

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("FirstName")
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("LastName")
                    .HasMaxLength(50);

                entity.Property(e => e.email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(50);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnName("UserName")
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("Password")
                    .HasMaxLength(50);

                entity.Property(e => e.ConfirmPassword)
                    .IsRequired()
                    .HasColumnName("ConfirmPassword")
                    .HasMaxLength(50);
            });
        }
    }
}
