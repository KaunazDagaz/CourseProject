using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CourseProject.Entities
{
    public class ApplicationDbContext : IdentityDbContext<User> 
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Template> Templates { get; set; }
        public DbSet<Form> Forms { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionOption> QuestionOptions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<AnswerOption> AnswerOptions { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TemplateTag> TemplateTags { get; set; }
        public DbSet<TemplateStats> TemplateStats { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Template>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(t => t.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Form>()
                .HasOne<Template>()
                .WithMany()
                .HasForeignKey(f => f.TemplateId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Question>()
               .HasOne<Form>()
               .WithOne()
               .HasForeignKey<Question>(q => q.FormId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<QuestionOption>()
                .HasOne<Question>()
                .WithMany()
                .HasForeignKey(q => q.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Answer>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(a => a.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Answer>()
                .HasOne<Form>()
                .WithMany()
                .HasForeignKey(a => a.FormId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<AnswerOption>()
                .HasOne<Answer>()
                .WithMany()
                .HasForeignKey(a => a.AnswerId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<AnswerOption>()
                .HasOne<QuestionOption>()
                .WithMany()
                .HasForeignKey(a => a.OptionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
               .HasOne<Template>()
               .WithMany()
               .HasForeignKey(c => c.TemplateId)
               .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Comment>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(c => c.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Like>()
               .HasOne<User>()
               .WithMany()
               .HasForeignKey(l => l.AuthorId)
               .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Like>()
                .HasOne<Template>()
                .WithMany()
                .HasForeignKey(l => l.TemplateId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TemplateTag>()
                .HasKey(tt => new { tt.TemplateId, tt.TagId });
            modelBuilder.Entity<TemplateTag>()
                .HasOne<Template>()
                .WithMany()
                .HasForeignKey(tt => tt.TemplateId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<TemplateTag>()
                .HasOne<Tag>()
                .WithMany()
                .HasForeignKey(tt => tt.TagId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TemplateStats>()
                .HasOne<Template>()
                .WithOne()
                .HasForeignKey<TemplateStats>(ts => ts.TemplateId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Template>()
                .HasIndex(t => t.Title);
            modelBuilder.Entity<Template>()
                .HasIndex(t => t.Description);
            modelBuilder.Entity<Template>()
                .HasIndex(t => t.Topic);

            modelBuilder.Entity<Question>()
                .HasIndex(q => q.Title);
            modelBuilder.Entity<Question>()
                .HasIndex(q => q.Description);

            modelBuilder.Entity<QuestionOption>()
                .HasIndex(qo => qo.Text);

            modelBuilder.Entity<Comment>()
                .HasIndex(c => c.Content);

            modelBuilder.Entity<Tag>()
                .HasIndex(t => t.Name);
        }
    }
}
