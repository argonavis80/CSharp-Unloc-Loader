namespace UnlocLoader.Model
{
    /// <summary>
    /// Functions of location according to UN/LOCODE MANUAL.
    /// </summary>
    public enum Function
    {
        /// <summary>
        /// Function not known, to be specified
        /// </summary>
        Unknown,

        /// <summary>
        /// port, as defined in Rec. 16
        /// </summary>
        Port,

        /// <summary>
        /// Rail terminal
        /// </summary>
        RailTerminal,

        /// <summary>
        /// Road terminal
        /// </summary>
        RoadTerminal,

        /// <summary>
        /// Airport
        /// </summary>
        Airport,

        /// <summary>
        /// Postal exchange office
        /// </summary>
        PostalExchangeOffice,

        /// <summary>
        /// Reserved for multimodal functions, ICD's, etc.
        /// </summary>
        MultimodalFunction,

        /// <summary>
        /// Reserved for fixed transport functions (e.g. oil platform)
        /// </summary>
        FixedTransportFunction,

        /// <summary>
        /// Border crossing
        /// </summary>
        BorderCrossing
    }
}