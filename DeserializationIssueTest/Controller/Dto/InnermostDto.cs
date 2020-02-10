﻿using System;
using System.ComponentModel.DataAnnotations;

namespace DeserializationIssueTest.Controller.Dto
{
    public class InnermostDto
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        public string Description { get; set; }
    }
}
