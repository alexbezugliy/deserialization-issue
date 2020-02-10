﻿using System.Collections.Generic;
 using System.ComponentModel.DataAnnotations;

namespace DeserializationIssueTest.Controller.Dto
{
    public class Dto
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        public List<InnerDto> InnerDtos { get; set; } = new List<InnerDto>();
    }
}
