using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ZealEducationSystem.Models;

namespace ZealEducationSystem.Data;

public partial class ZealEducationTestContext : DbContext
{
    public ZealEducationTestContext()
    {
    }

    public ZealEducationTestContext(DbContextOptions<ZealEducationTestContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Batchess> Batchesses { get; set; }

    public virtual DbSet<Candidate> Candidates { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Enquiry> Enquiries { get; set; }

    public virtual DbSet<Exam> Exams { get; set; }

    public virtual DbSet<ExamMark> ExamMarks { get; set; }

    public virtual DbSet<ExamType> ExamTypes { get; set; }

    public virtual DbSet<Faculty> Faculties { get; set; }

    public virtual DbSet<FeesDetail> FeesDetails { get; set; }

    public virtual DbSet<SignupDetail> SignupDetails { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Batchess>(entity =>
        {
            entity.HasKey(e => e.BatchId).HasName("PK__Batchess__78CCD75313A0F8E9");

            entity.ToTable("Batchess");

            entity.HasIndex(e => e.BatchCode, "UQ__Batchess__A014E0A54A8ED730").IsUnique();

            entity.Property(e => e.BatchId).HasColumnName("batchID");
            entity.Property(e => e.BatchCode)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("batchCode");
            entity.Property(e => e.BatchDays)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("batchDays");
            entity.Property(e => e.BatchDuration)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("batchDuration");
            entity.Property(e => e.BatchStartDate)
                .HasColumnType("date")
                .HasColumnName("batchStartDate");
            entity.Property(e => e.BatchTime)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("batchTime");
            entity.Property(e => e.CourseId).HasColumnName("courseID");
            entity.Property(e => e.FacultyId).HasColumnName("facultyID");

            entity.HasOne(d => d.Course).WithMany(p => p.Batchesses)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK__Batchess__course__56E8E7AB");

            entity.HasOne(d => d.Faculty).WithMany(p => p.Batchesses)
                .HasForeignKey(d => d.FacultyId)
                .HasConstraintName("FK__Batchess__facult__14270015");
        });

        modelBuilder.Entity<Candidate>(entity =>
        {
            entity.HasKey(e => e.CandidateId).HasName("PK__Candidat__7437A4643027AD43");

            entity.HasIndex(e => e.CandidateEmail, "UQ__Candidat__49D53067CE25A517").IsUnique();

            entity.Property(e => e.CandidateId).HasColumnName("candidateID");
            entity.Property(e => e.BatchId).HasColumnName("batchID");
            entity.Property(e => e.CandidateEmail)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("candidateEmail");
            entity.Property(e => e.CandidateImage)
                .IsUnicode(false)
                .HasColumnName("candidateImage");
            entity.Property(e => e.CandidatePassword)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("candidatePassword");
            entity.Property(e => e.CourseId).HasColumnName("courseID");
            entity.Property(e => e.FirstName)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("firstName");
            entity.Property(e => e.LastName)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("lastName");

            entity.HasOne(d => d.Batch).WithMany(p => p.Candidates)
                .HasForeignKey(d => d.BatchId)
                .HasConstraintName("FK__Candidate__batch__59C55456");

            entity.HasOne(d => d.Course).WithMany(p => p.Candidates)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK__Candidate__cours__5AB9788F");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__Course__2AA84FF1A11FD12B");

            entity.ToTable("Course");

            entity.HasIndex(e => e.CourseCode, "UQ__Course__537513F10D50B4FF").IsUnique();

            entity.Property(e => e.CourseId).HasColumnName("courseID");
            entity.Property(e => e.CourseCode)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("courseCode");
            entity.Property(e => e.CourseDescription)
                .HasMaxLength(110)
                .IsUnicode(false)
                .HasColumnName("courseDescription");
            entity.Property(e => e.CourseFees)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("courseFees");
            entity.Property(e => e.CourseLength)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("courseLength");
            entity.Property(e => e.CourseName)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("courseName");
            entity.Property(e => e.CourseThumbnail)
                .IsUnicode(false)
                .HasColumnName("courseThumbnail");
        });

        modelBuilder.Entity<Enquiry>(entity =>
        {
            entity.HasKey(e => e.EnqId).HasName("PK__Enquiry__06983F20698F7B5E");

            entity.ToTable("Enquiry");

            entity.Property(e => e.EnqId).HasColumnName("enqID");
            entity.Property(e => e.EnqEmail)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("enqEmail");
            entity.Property(e => e.EnqMessage)
                .IsUnicode(false)
                .HasColumnName("enqMessage");
            entity.Property(e => e.FirstName)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("firstName");
            entity.Property(e => e.LastName)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("lastName");
        });

        modelBuilder.Entity<Exam>(entity =>
        {
            entity.HasKey(e => e.ExamId).HasName("PK__Exam__A56D123FC02B1B42");

            entity.ToTable("Exam");

            entity.Property(e => e.ExamId).HasColumnName("examID");
            entity.Property(e => e.BatchId).HasColumnName("batchID");
            entity.Property(e => e.CourseId).HasColumnName("courseID");
            entity.Property(e => e.ExamDate)
                .HasColumnType("date")
                .HasColumnName("examDate");
            entity.Property(e => e.ExamTime).HasColumnName("examTime");
            entity.Property(e => e.ExamtypeId).HasColumnName("examtypeID");
            entity.Property(e => e.ExaxmName)
                .HasMaxLength(80)
                .IsUnicode(false);

            entity.HasOne(d => d.Batch).WithMany(p => p.Exams)
                .HasForeignKey(d => d.BatchId)
                .HasConstraintName("FK__Exam__batchID__5BAD9CC8");

            entity.HasOne(d => d.Course).WithMany(p => p.Exams)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK__Exam__courseID__5CA1C101");

            entity.HasOne(d => d.Examtype).WithMany(p => p.Exams)
                .HasForeignKey(d => d.ExamtypeId)
                .HasConstraintName("FK__Exam__examtypeID__5D95E53A");
        });

        modelBuilder.Entity<ExamMark>(entity =>
        {
            entity.HasKey(e => e.ExammarksId).HasName("PK__ExamMark__3491D9426ED284FE");

            entity.Property(e => e.ExammarksId).HasColumnName("exammarksID");
            entity.Property(e => e.CandidateId).HasColumnName("candidateID");
            entity.Property(e => e.ExamId).HasColumnName("examID");
            entity.Property(e => e.ObtainedMarks).HasColumnName("obtainedMarks");
            entity.Property(e => e.TotalMarks).HasColumnName("totalMarks");

            entity.HasOne(d => d.Candidate).WithMany(p => p.ExamMarks)
                .HasForeignKey(d => d.CandidateId)
                .HasConstraintName("FK__ExamMarks__candi__5E8A0973");

            entity.HasOne(d => d.Exam).WithMany(p => p.ExamMarks)
                .HasForeignKey(d => d.ExamId)
                .HasConstraintName("FK__ExamMarks__examI__5F7E2DAC");
        });

        modelBuilder.Entity<ExamType>(entity =>
        {
            entity.HasKey(e => e.ExamtypeId).HasName("PK__ExamType__ACCE3D6A4255C608");

            entity.ToTable("ExamType");

            entity.Property(e => e.ExamtypeId).HasColumnName("examtypeID");
            entity.Property(e => e.ExamType1)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("examType");
        });

        modelBuilder.Entity<Faculty>(entity =>
        {
            entity.HasKey(e => e.FacultyId).HasName("PK__Faculty__DBBF6FD1CA5BCAFC");

            entity.ToTable("Faculty");

            entity.Property(e => e.FacultyId).HasColumnName("facultyID");
            entity.Property(e => e.FacultyEmail)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("facultyEmail");
            entity.Property(e => e.FacultyPassword)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("facultyPassword");
            entity.Property(e => e.FirstName)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("firstName");
            entity.Property(e => e.LastName)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("lastName");
        });

        modelBuilder.Entity<FeesDetail>(entity =>
        {
            entity.HasKey(e => e.FeesId).HasName("PK__FeesDeta__CB7A4CBD935E7C37");

            entity.ToTable("FeesDetail");

            entity.Property(e => e.FeesId).HasColumnName("feesID");
            entity.Property(e => e.CandidateId).HasColumnName("candidateID");
            entity.Property(e => e.CollectedAmount)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("collectedAmount");
            entity.Property(e => e.FeeMonth)
                .HasMaxLength(80)
                .IsUnicode(false);

            entity.HasOne(d => d.Candidate).WithMany(p => p.FeesDetails)
                .HasForeignKey(d => d.CandidateId)
                .HasConstraintName("FK__FeesDetai__candi__3D2915A8");
        });

        modelBuilder.Entity<SignupDetail>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__SignupDe__CB9A1CDFA2EECAF4");

            entity.ToTable("SignupDetail");

            entity.Property(e => e.UserId).HasColumnName("userID");
            entity.Property(e => e.UserEmail)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("userEmail");
            entity.Property(e => e.UserPassword)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("userPassword");
            entity.Property(e => e.UserRole)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("userRole");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCAC33F166D2");

            entity.HasIndex(e => e.UserName, "UQ__Users__66DCF95C1876C9C2").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.UserName)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("userName");
            entity.Property(e => e.UserPassword)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("userPassword");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
