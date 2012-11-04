
namespace IstLight.Simulation
{
    public enum SimulationEndReason
    {
        /// <summary>
        /// Simulation finished successfully
        /// </summary>
        Completion,
        /// <summary>
        /// Simulation was stopped because error occurred.
        /// </summary>
        Error,
        /// <summary>
        /// Simulation was canceled by user.
        /// </summary>
        Cancellation
    }
}
