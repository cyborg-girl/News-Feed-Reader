using System;
using System.Data.Entity;

namespace NewsReader.Models
{
    public class SiteLink
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
    }

    public class SiteDBContext : DbContext
    {
        public DbSet<SiteLink> SiteDB { get; set; }
    }
}