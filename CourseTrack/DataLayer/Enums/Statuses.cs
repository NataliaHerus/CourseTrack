using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Enums
{
    /// <summary>
    /// Types of statuses.
    /// </summary>
    public enum Statuses
    {
        /// <summary>
        /// Represents just created task.
        /// </summary>
        New,

        /// <summary>
        /// Represents in progress task.
        /// </summary>
        InProgress,

        /// <summary>
        /// Represents a rejected task.
        /// </summary>
        Rejected,

        /// <summary>
        /// Represents done task.
        /// </summary>
        Done,
    }

}
