using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IstLight.Domain.Simulation
{
    /// <summary>
    /// Contains simulation end data.
    /// </summary>
    public class SimulationEndEventArgs : EventArgs
    {
        /// <summary>
        /// Explains why simulation ended.
        /// </summary>
        public SimulationEndReason EndReason { get; set; }

        /// <summary>
        /// Simulation result. Null if EndReason is not equal to Completion.
        /// </summary>
        public SimulationResult Result { get; set; }

        /// <summary>
        /// If EndReason is equal to Error, this property describes it.
        /// </summary>
        public string Error { get; set; }
    }
}
