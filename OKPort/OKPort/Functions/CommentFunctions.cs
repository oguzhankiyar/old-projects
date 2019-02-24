using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OKPort.Models;

namespace OKPort.Functions
{
    public class CommentFunctions
    {
        public static int GetPopularPoint(Comment comment)
        {
            return comment.ID;
        }
    }
}