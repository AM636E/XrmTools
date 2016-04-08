using System;
using DynamicaLabs.Tools.Logging;
using Microsoft.Xrm.Sdk;

namespace DynamicaLabs.CrmTools.Logging
{
    /// <summary>
    /// Settings for CrmLogger.
    /// </summary>
    public class CrmLoggerSettings
    {
        /// <summary>
        /// EntityLogicalName of log holder. Required.
        /// </summary>
        public string LogEntityName { get; set; }

        /// <summary>
        /// Field for subject. Required.
        /// </summary>
        public string SubjectField { get; set; }

        /// <summary>
        /// Field for message. Required.
        /// </summary>
        public string MessageField { get; set; }

        /// <summary>
        /// Action to fill log type. Optional.
        /// </summary>
        public Action<LogType, Entity> LogTypeSetter { get; set; }
    }
}