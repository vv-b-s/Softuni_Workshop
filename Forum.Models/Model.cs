using System;
using System.Collections.Generic;
using System.Text;

namespace Forum.Models
{
    /// <summary>
    /// Using this constructor to make the input data suitable for reflection
    /// </summary>
    public abstract class Model
    {
        public Model(params string[] args) { }
    }
}
