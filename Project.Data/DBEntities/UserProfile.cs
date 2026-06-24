//  <copyright file="tblTransactions.cs" company="PlaceholderCompany">
//  Copyright (c) PlaceholderCompany. All rights reserved.
//  </copyright>

namespace Project.Data.DBEntities
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Project.Data.ExtendedDBEntities;

    public class UserProfile
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [ForeignKey(nameof(User))]
        public Guid? UserId { get; set; }
        
        public string FCMToken { get; set; }
        public string DeviceType { get; set; }
        
        // Navigation Property
        public virtual DerivedIdentityUser User { get; set; }
    }
}