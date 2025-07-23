using LMS.Domain.Chapters;
using LMS.Domain.Curriculums;
using LMS.Domain.Lessons;
using LMS.Domain.Supervisor;
using LMS.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Infrastructure.Persistence
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
       : base(options)
        {
        }

        // --- 1. تعريف الـ DbSets ---
        // لكل Aggregate Root أو كيان مستقل، نضيف DbSet هنا

        public DbSet<Supervisor> Supervisors { get; set; }
        public DbSet<Curriculum> Curriculums { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<Lesson> Lessons { get; set; }

        // نضيف DbSet للكلاس الأساسي فقط، وEF Core سيفهم الباقي
        public DbSet<LessonContent> LessonContents { get; set; }

        public DbSet<ExampleItem> ExampleItems { get; set; }


        // --- 2. تكوين الموديل باستخدام Fluent API ---
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // --- إعداد الـ Value Objects كـ Owned Types ---
            // هذا يخبر EF Core أن Title و SortOrder ليسا جدولين منفصلين،
            // بل يجب تخزين خصائصهما كأعمدة في الجدول الأصل
            builder.Entity<Curriculum>().OwnsOne(c => c.Title);
            builder.Entity<Curriculum>().OwnsOne(c => c.Introduction);
            builder.Entity<Chapter>().OwnsOne(c => c.Title);
            builder.Entity<Chapter>().OwnsOne(c => c.SortOrder);
            builder.Entity<Lesson>().OwnsOne(l => l.Title);
            builder.Entity<Lesson>().OwnsOne(l => l.SortOrder);
            builder.Entity<LessonContent>().OwnsOne(lc => lc.SortOrder);


            // --- إعداد وراثة Table-Per-Hierarchy (TPH) ---
            // هذا هو الجزء الأهم الذي يتعامل مع أنواع المحتوى المختلفة
            builder.Entity<LessonContent>()
                .ToTable("LessonContents") // يمكن تحديد اسم الجدول هنا
                .HasDiscriminator<string>("ContentType") // العمود الذي سيفرق بين الأنواع
                .HasValue<RichTextContent>("RichText")
                .HasValue<VideoContent>("Video")
                .HasValue<ImageWithCaptionContent>("ImageWithCaption")
                .HasValue<ExamplesGridContent>("ExamplesGrid");


            // --- إعداد العلاقات (Relationships) ---
            // EF Core ذكي كفاية لاكتشاف معظم العلاقات،
            // لكن من الجيد تعريفها بوضوح هنا

            // علاقة Curriculum -> Chapters
            builder.Entity<Curriculum>()
                .HasMany(c => c.Chapters)
                .WithOne()
                .HasForeignKey("CurriculumId") // اسم العمود FK في جدول Chapters
                .IsRequired();

            // علاقة Chapter -> Lessons
            builder.Entity<Chapter>()
                .HasMany(c => c.Lessons)
                .WithOne()
                .HasForeignKey("ChapterId")
                .IsRequired();

            // علاقة Lesson -> LessonContents
            builder.Entity<Lesson>()
                .HasMany(l => l.Contents)
                .WithOne()
                .HasForeignKey(lc => lc.LessonId)
                .IsRequired();

            // علاقة ExamplesGridContent -> ExampleItems
            builder.Entity<ExamplesGridContent>()
                .HasMany(g => g.ExampleItems)
                .WithOne()
                .HasForeignKey(i => i.ExamplesGridContentId)
                .IsRequired();
        }
    }
}
