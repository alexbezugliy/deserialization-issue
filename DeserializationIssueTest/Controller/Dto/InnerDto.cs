﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

 namespace DeserializationIssueTest.Controller.Dto
{
    public class InnerDto
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public List<InnermostDto> InnermostDtos { get; set; } = new List<InnermostDto>();
    }
}
