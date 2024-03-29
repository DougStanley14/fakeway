﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace microsvc_userprofile.Model
{
    public class User
    {
        public User()
        {
            Orgs = new List<Organization>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public long EDIPI { get; set; }

        [MaxLength(20)]
        public string UserName { get; set; }

        //[MaxLength(50)]
        //public string FirstName { get; set; }

        //[MaxLength(50)]
        //public string LastName { get; set; }

        //[MaxLength(50)]
        //public string FullName { get; set; }

        //[MaxLength(80)]
        //public string Email { get; set; }

        public virtual ICollection<Organization> Orgs { get; set; }
    }

    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasIndex(e => e.Id);
            builder.HasIndex(e => e.EDIPI);

        }
    }
}
