﻿using System.ComponentModel.DataAnnotations;

namespace Filminurk.Models.UserComments
{
    public class UserCommentsIndexViewModel
    {
        [Key]
        public Guid CommentID { get; set; }
        public string? CommenterUserID { get; set; }
        public string CommentBody { get; set; }
        public int? CommentedScore { get; set; }
        public int? IsHelpful { get; set; } // 👍
        public int IsHarmful { get; set; } // 

        /* Andmebaasi jaoks vajalikud andmed */
        public DateTime CommentCreatedAt { get; set; }
        public DateTime? CommentModifiedAt { get; set; }
        public DateTime? CommentDeletedAt { get; set; }
    }
}
