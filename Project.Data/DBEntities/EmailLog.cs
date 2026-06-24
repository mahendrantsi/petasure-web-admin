// <copyright file="tblTransactions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Project.Data.DBEntities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class EmailLog
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public string FromMail { get; set; }

        [Required]
        public string ToMail { get; set; }

        public string CcMail { get; set; }

        public string BccMail { get; set; }

        [Required]
        public string Subject { get; set; }

        public string Description { get; set; }

        [Required]
        public string MailStatus { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public string SendResult { get; set; }

        public string SendResultId { get; set; }
    }
}