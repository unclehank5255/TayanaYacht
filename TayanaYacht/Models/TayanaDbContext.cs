using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace TayanaYacht.Models
{
    public partial class TayanaDbContext : DbContext
    {
        public TayanaDbContext()
            : base("name=TayanaDbContext")
        {
        }
        public virtual DbSet<Yacht.Yacht> Yachts { get; set; }
        public virtual DbSet<Yacht.YachtFile> YachtFiles { get; set; }
        public virtual DbSet<Yacht.YachtPicture> YachtPictures { get; set; }
        public virtual DbSet<News.News> News { get; set; }
        public virtual DbSet<News.NewsFile> NewsFiles { get; set; }
        public virtual DbSet<News.NewsPage> NewsPages { get; set; }
        public virtual DbSet<News.NewsDetailImage> NewsDetailImages { get; set; }
        public virtual DbSet<Dealer.Dealer> Dealers { get; set; }
        public virtual DbSet<Dealer.DealerPage> DealerPages { get; set; }
        public virtual DbSet<Contact.Contact> Contacts { get; set; }
        public virtual DbSet<Contact.ContactMessage> ContactMessages { get; set; }
        public virtual DbSet<Contact.PrivacyVersion> PrivacyVersions { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            // 刪除 Yacht 時，不刪除歷史聯絡訊息
            modelBuilder.Entity<Contact.ContactMessage>()
                .HasRequired(message => message.Yacht)
                .WithMany(yacht => yacht.ContactMessages)
                .HasForeignKey(message => message.YachtId)
                .WillCascadeOnDelete(false);

            // 刪除 PrivacyVersion 時，不刪除歷史聯絡訊息
            modelBuilder.Entity<Contact.ContactMessage>()
                .HasRequired(message => message.PrivacyVersion)
                .WithMany(version => version.ContactMessages)
                .HasForeignKey(message => message.PrivacyVersionId)
                .WillCascadeOnDelete(false);

            // 刪除 PrivacyVersion 時，不刪除 Contact 頁面設定
            modelBuilder.Entity<Contact.Contact>()
                .HasRequired(contact => contact.PrivacyVersion)
                .WithMany(version => version.Contacts)
                .HasForeignKey(contact => contact.PrivacyVersionId)
                .WillCascadeOnDelete(false);

            // 執行 EF 原本的 Model 建立流程
            base.OnModelCreating(modelBuilder);
        }
    }
}
