﻿using NinjaTurtles.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTurtles.Core.Entities.Concrete
{
    public class OperationClaim : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
